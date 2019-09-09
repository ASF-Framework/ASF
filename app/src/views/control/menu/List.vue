<template>
  <a-card :bordered="false">
    <!--搜索-->
    <div class="table-page-search-wrapper">
      <a-row type="flex" justify="space-around" :gutter="48">
        <a-col :md="18" :sm="24">
          <a-button type="primary" @click="$refs.add.show()" icon="plus" class="right10">新增</a-button>
          <a-button-group class="right10">
            <a-button type="primary" @click="handleExport">导出</a-button>
            <a-button type="primary" @click="$refs.immodal.import()">导入</a-button>
          </a-button-group>
          <a-select placeholder="请选择" v-model="queryParam.enable" style="width:100px;" @change="loadDataing">
            <a-select-option :value="-1">API 状态</a-select-option>
            <a-select-option :value="1">启用</a-select-option>
            <a-select-option :value="0">禁用</a-select-option>
          </a-select>
        </a-col>
        <a-col :md="6" :sm="24">
          <div class="table-page-search-submitButtons" >
            <a-input-search
              placeholder="菜单名称和菜单标识"
              v-model="queryParam.vague"
              enterButton="查询"
              @search="loadDataing" >
            </a-input-search>
          </div>
        </a-col>
      </a-row>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      rowKey="id"
      size="middle"
      :rowSelection="{selectedRowKeys: table.selectedRowKeys, onChange: onSelectChange}"
      :pagination="false"
      :loading="table.loading"
      :columns="table.columns"
      :expandedRowKeys="table.expandedRowKeys"

      :dataSource="table.dataSource"
      @expandedRowsChange="(e)=>{this.table.expandedRowKeys=e}">
      <span slot="id" slot-scope="text">
        <router-link :to="{ name: 'asf_permission_details', query: {data: text }}" >{{ text }}</router-link>
      </span>
      <span slot="icon" slot-scope="text">
        <a-icon v-if="text" :type="text" />
      </span>
      <span slot="enable" slot-scope="text">
        <a-badge v-if="text" status="success" text="启用"/>
        <a-badge v-else status="error" text="禁用"/>
      </span>
      <span slot="hidden" slot-scope="text">
        <a-badge v-if="!text" status="success" text="显示"/>
        <a-badge v-else status="error" text="隐藏"/>
      </span>
      <span slot="actions" slot-scope="text, record">
        <a-tag v-for="(val,key) in record.actions" :key="key">{{ val }}</a-tag>
      </span>
      <span slot="createTime" slot-scope="text">
        {{ text | dayFormat('YYYY-MM-DD HH:mm') }}
      </span>
      <span slot="sort" slot-scope="text, record">
        <modify-sort :text="text" :id="record.id" @complete="loadDataing" type="menu"></modify-sort>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>
          <router-link :to="{ name: 'asf_permission_details', query: {data: record.id }}" >详情</router-link>
          <a-divider type="vertical" />
          <a-dropdown>
            <a class="ant-dropdown-link">更多
              <a-icon type="down" />
            </a>
            <a-menu slot="overlay">
              <a-menu-item :disabled="record.isSystem" @click="$refs.edit.show(record)">
                编辑
              </a-menu-item>
              <a-menu-item :disabled="record.isSystem" @click="handleDelete(record.id,record.name)" >
                删除
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </template>
      </span>
    </a-table>

    <!--功能模块-->
    <import-modal ref="immodal" @ok="loadDataing"></import-modal>
    <menu-add ref="add" @complete="loadDataing"></menu-add>
    <menu-edit ref="edit" @complete="loadDataing"></menu-edit>
  </a-card>
</template>

<script>
import { getMenuList, deleteMenu, exportMenu } from '@/api/control'
import ModifySort from './modules/ModifySort'
import ImportModal from './modules/ImportModal'
import MenuAdd from './Add'
import MenuEdit from './Edit'
export default {
  name: 'MenuList',
  components: { ModifySort, MenuAdd, MenuEdit, ImportModal },
  created () { this.loadDataing() },
  data () {
    return {
      // 查询参数
      queryParam: {
        search: false,
        vague: '',
        enable: -1
      },
      // 列表属性
      table: {
        selectedRowKeys: [],
        loading: false,
        expandedRowKeys: [],
        dataSource: [],
        // 表头
        columns: [
          {
            title: '标识',
            dataIndex: 'id',
            scopedSlots: {
              customRender: 'id'
            }
          },
          {
            title: '图标',
            width: 60,
            align: 'right',
            dataIndex: 'icon',
            scopedSlots: {
              customRender: 'icon'
            }
          },
          {
            title: '名称',
            dataIndex: 'name'

          },
          {
            title: '操作权限',
            dataIndex: 'actions',
            scopedSlots: {
              customRender: 'actions'
            }
          },
          {
            title: '状态',
            dataIndex: 'enable',
            scopedSlots: {
              customRender: 'enable'
            }
          },
          {
            title: '是否隐藏',
            dataIndex: 'hidden',
            scopedSlots: {
              customRender: 'hidden'
            }
          },

          {
            title: '排序',
            dataIndex: 'sort',
            scopedSlots: {
              customRender: 'sort'
            }
          },
          {
            title: '操作',
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      },
      selectRows: []
    }
  },
  methods: {
    // 表格勾选事件
    onSelectChange (selectedRowKeys, selectedRows) {
      const _this = this
      _this.table.selectedRowKeys = selectedRowKeys
      _this.selectRows = selectedRows
    },

    /**
     * 加载列表
     */
    loadDataing () {
      this.table.loading = true
      this.queryParam.search = !(!this.queryParam.vague && this.queryParam.enable === -1)
      getMenuList(this.queryParam).then(res => {
        this.table.loading = false
        if (res.status === 200) {
          this.table.dataSource = this.queryParam.search ? res.result : this.buildTreeData(res.result)
        } else {
          this.$notification.error({ message: '加载失败', description: res.message })
        }
      }).catch(() => {
        this.table.loading = false
      })
    },
    /**
     * 导出菜单
     */
    handleExport () {
      const _this = this
      const param = { list: _this.selectRows }
      if (_this.selectRows === undefined || _this.selectRows.length === 0) {
        _this.$notification['warning']({
          message: '提示',
          description: '请勾选你要导出的对象',
          duration: 4
        })
        return false
      }
      this.$confirm({ title: '导出菜单',
        content: `是否导出菜单？`,
        onOk: () => {
          _this.table.loading = true
          exportMenu(param).then(res => {
            if (res.status === 200) {
              _this.table.loading = false
              _this.table.expandedRowKeys = []
              const json = JSON.stringify(res.result)
              _this.exportRaw('menu.json', json)
              _this.loadDataing()
              _this.$message.success(`导出菜单成功`)
            } else {
              _this.table.loading = false
              _this.table.expandedRowKeys = []
              _this.$message.error('导出失败;' + res.message)
            }
          })
        }
      })
    },

    // 保存txt\json文档到本地
    exportRaw (name, data) {
      const urlObject = window.URL || window.webkitURL || window
      const exportBlob = new Blob([data])
      const saveLink = document.createElementNS('http://www.w3.org/1999/xhtml', 'a')
      saveLink.href = urlObject.createObjectURL(exportBlob)
      saveLink.download = name

      const ev = document.createEvent('MouseEvents')
      ev.initMouseEvent('click', true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null)
      saveLink.dispatchEvent(ev)
    },

    /**
     * 删除菜单
     *@param {String} id 编号
     *@param {String} name 名称
     */
    handleDelete (id, name) {
      const _this = this
      this.$confirm({ title: '删除菜单',
        content: `是否删除 ${name} 菜单？`,
        onOk: () => {
          deleteMenu(id).then(res => {
            if (res.status === 200) {
              this.loadDataing()
              _this.$message.success(`删除 ${name} 菜单成功`)
            } else {
              _this.$message.error('删除失败;' + res.message)
            }
          })
        }
      })
    },
    /**
     * 组装数据源
     * @param {Array} data 菜单数据
     * @param {String} parentId 父级编号
     */
    buildTreeData (data, parentId = '') {
      return data.filter(value => {
        value.parentId = value.parentId || ''
        if (value.parentId === parentId) {
          value.key = value.id
          const children = this.buildTreeData(data, value.id)
          if (children.length > 0) {
            value.children = children
          }
          this.table.expandedRowKeys.push(value.key)
          return true
        } else {
          return false
        }
      })
    }
  }

}
</script>

<style lang="less" scoped>
.right10 {
   margin-right:10px;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold { width: 100%;}
}
</style>
