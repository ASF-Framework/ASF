<template>
  <a-card :bordered="false">
    <!--搜索-->
    <div class="table-page-search-wrapper">
      <a-row type="flex" justify="space-around" :gutter="48">
        <a-col :md="18" :sm="24">
          <a-tooltip>
            <template slot="title">新增角色</template>
            <a-button type="primary" icon="plus" @click="$refs.add.show()" style="margin-right:10px" v-action:create></a-button>
          </a-tooltip>
          <a-radio-group defaultValue="-1" v-model="queryParam.Enable" buttonStyle="solid" @change="search()">
            <a-radio-button value="-1">全部</a-radio-button>
            <a-radio-button value="1">启用</a-radio-button>
            <a-radio-button value="0">停用</a-radio-button>
          </a-radio-group>
        </a-col>
        <a-col :md="6" :sm="24">
          <span class="table-page-search-submitButtons" style="float:right">
            <a-input-search placeholder="角色标识、名称" v-model="queryParam.vague" enterButton="查询" @search="search()" style="width:300px;margin-right:10px">
            </a-input-search>
          </span>
        </a-col>
      </a-row>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      rowKey="id"
      :pagination="tables.pagination"
      :columns="tables.columns"
      :loading="tables.loading"
      :dataSource="tables.dataSource"
      @change="loadDataing">
      <span slot="enable" slot-scope="text">
        <a-badge v-if="text" status="success" text="正常"/>
        <a-badge v-else status="error" text="禁用"/>
      </span>
      <span slot="createTime" slot-scope="text">{{ text*1000 | moment }}</span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.edit.show(record.id)" v-action:modify>编辑</a>
        <a-divider type="vertical" />
        <a-dropdown>
          <a class="ant-dropdown-link">更多
            <a-icon type="down" />
          </a>
          <a-menu slot="overlay">
            <a-menu-item v-action:modify_status @click="handleModifyStatus(record.id,record.name,record.enable)">
              <span v-if="record.enable"> 禁用</span>
              <span v-else>启用</span>
            </a-menu-item>
            <a-menu-item v-action:delete @click="handleDelete(record.id,record.name)">
              删除
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </a-table>

    <!--功能模块-->
    <role-edit ref="edit" @complete="loadDataing"></role-edit>
    <role-add ref="add" @complete="loadDataing"></role-add>
  </a-card>
</template>

<script>
import RoleEdit from './Edit'
import RoleAdd from './Add'
import { getRoleList, modifyRoleStatus, deleteRole } from '@/api/control'
export default {
  name: 'TableList',
  components: {
    RoleEdit, RoleAdd
  },
  data () {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      // 查询参数
      queryParam: {
        vague: '',
        enable: -1,
        pagedCount: 10,
        skipPage: 1
      },
      tables: {
        loading: false,
        dataSource: [],
        // 分页对象
        pagination: {
          showTotal: (total) => `总计 ${total} 条`,
          hideOnSinglePage: true,
          showSizeChanger: true,
          current: 1,
          total: 0
        },
        // 表头
        columns: [
          {
            title: '标识',
            dataIndex: 'id'
          },
          {
            title: '角色名称',
            dataIndex: 'name'
          },
          {
            title: '状态',
            dataIndex: 'enable',
            scopedSlots: {
              customRender: 'enable'
            }
          },
          {
            title: '描述',
            dataIndex: 'description'
          },
          {
            title: '创建时间',
            dataIndex: 'createTime',
            sorter: false,
            scopedSlots: {
              customRender: 'createTime'
            }
          },
          {
            title: '操作',
            width: '150px',
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]

      }
    }
  },
  created () {
    this.loadDataing()
  },
  methods: {
    /**
    *搜索 查询列表
    */
    search () {
      this.queryParam.skipPage = 1
      this.loadDataing()
    },
    /**
     * 加载数据方法
     * @param {Object} pagination 分页选项器
     * @param {Object} filters 过滤条件
     * @param {Object} sorter 排序条件
     */
    loadDataing (pagination, filters, sorter) {
      this.tables.loading = true
      if (pagination) {
        this.queryParam.skipPage = pagination.current
        this.queryParam.pagedCount = pagination.pageSize
      }
      getRoleList(this.queryParam).then(res => {
        this.tables.loading = false
        if (res.status === 200) {
          this.tables.pagination.total = res.totalCount
          this.tables.dataSource = res.result
        } else {
          this.$notification.error({ message: '加载失败', description: res.message })
        }
      }).catch(() => {
        this.tables.loading = false
      })
    },
    /**
     * 禁用/启用角色
     * @param {String} id 编号
     * @param {String} name 名称
     * @param {Boolean} enable 状态
     */
    handleModifyStatus (id, name, enable) {
      const _this = this
      var confirmTitle = enable ? '禁用角色？' : '启用角色？'
      var confirmContent = enable ? `禁用 ${name} 角色` : `启用 ${name} 角色`
      var reqData = {
        roleId: id,
        enable: enable ? 0 : 1
      }
      this.$confirm({ title: confirmTitle,
        content: `是否${confirmContent} ？`,
        onOk: () => {
          modifyRoleStatus(reqData).then(res => {
            if (res.status === 200) {
              this.loadDataing()
              _this.$message.success(`${confirmContent}成功`)
            } else {
              _this.$message.error(`${confirmContent}失败`, res.message)
            }
          })
        }
      })
    },
    /**
     * 删除 角色
     *@param {String} id 编号
     *@param {String} name 名称
     */
    handleDelete (id, name) {
      const _this = this
      this.$confirm({ title: '删除角色',
        content: `是否删除 ${name} 角色？`,
        onOk: () => {
          deleteRole(id).then(res => {
            if (res.status === 200) {
              this.loadDataing()
              _this.$message.success(`删除 ${name} 管理员成功`)
            } else {
              _this.$message.error('删除失败;' + res.message)
            }
          })
        }
      })
    }

  }
}
</script>
