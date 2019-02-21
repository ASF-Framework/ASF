<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row type="flex" justify="space-around">
          <a-col :md="8" :sm="24">
            <a-tooltip>
              <template slot="title">新增角色</template>
              <a-button type="primary" icon="plus" @click="handleAdd" style="margin-right:10px"></a-button>
            </a-tooltip>
             <a-radio-group defaultValue="-1" v-model="queryParam.Enable" buttonStyle="solid" @change="$refs.table.refresh(true)">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">启用</a-radio-button>
              <a-radio-button value="2">停用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col
            :span="8"
            :md="{span:12,offset:4}"
            :sm="{span:24,offset:0}"
            :xs="{span:24,offset:0}"
            :offset="8"
          >
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input placeholder="角色标识、名称" style="width:300px;margin-right:10px" v-model="queryParam.Vague"/>
              <a-button-group>
                <a-tooltip>
                  <template slot="title">查询</template>
                  <a-button type="primary" icon="search" @click="$refs.table.refresh(true)"></a-button>
                </a-tooltip>
                <a-tooltip>
                  <template slot="title">清除查询条件</template>
                  <a-button icon="undo" @click="() => queryParam = {Vague:'', Enable:-1,PagedCount:10,SkipPage:1}"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <s-table ref="table" size="default" :columns="columns" :data="loadData">     
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.modal.edit(record)">编辑</a>
        <a-divider type="vertical"/>
        <a-dropdown>
          <a class="ant-dropdown-link">更多
            <a-icon type="down"/>
          </a>
          <a-menu slot="overlay">
            <a-menu-item>
              <a href="javascript:;">详情</a>
            </a-menu-item>
            <a-menu-item>
              <a href="javascript:;">禁用</a>
            </a-menu-item>
            <a-menu-item>
              <a href="javascript:;">删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </s-table>
    <role-modal ref="modal" @ok="handleOk"></role-modal>
  </a-card>
</template>

<script>
import STable from '@/components/table/'
import RoleModal from './modules/RoleModal'
  import {    getRoleList  } from '@/api/manage'
export default {
  name: 'TableList',
  components: {
    STable,
    RoleModal
  },
  data() {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visible: false,
      form: null,
      mdl: {},
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {
        Vague:"",
        Enable:-1,
        PagedCount:10,
        SkipPage:1
      },
      // 表头
      columns: [
        {
          title: '唯一识别码',
          dataIndex: 'id'
        },
        {
          title: '角色名称',
          dataIndex: 'name'
        },
        {
          title: '状态',
          dataIndex: 'status'
        },
        {
          title: '创建时间',
          dataIndex: 'createTime',
          sorter: true
        },
        {
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        }
      ],
      // 加载数据方法 必须为 Promise 对象
      loadData: () => {
        return getRoleList(this.queryParam).then(res=>{
           let data =Object.assign(res,this.queryParam)
          return data
        })
      },
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  methods: {
    handleAdd() {},
    handleEdit(record) {
      this.mdl = Object.assign({}, record)
      this.mdl.permissions.forEach(permission => {
        permission.actionsOptions = permission.actionEntitySet.map(action => {
          return {
            label: action.describe,
            value: action.action,
            defaultCheck: action.defaultCheck
          }
        })
      })
      console.log(this.mdl)
      this.visible = true
    },
    handleOk() {
      // 新增/修改 成功时，重载列表
      this.$refs.table.refresh()
    },
    onChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    toggleAdvanced() {
      this.advanced = !this.advanced
    }
  },
  watch: {
    /*
      'selectedRows': function (selectedRows) {
        this.needTotalList = this.needTotalList.map(item => {
          return {
            ...item,
            total: selectedRows.reduce( (sum, val) => {
              return sum + val[item.dataIndex]
            }, 0)
          }
        })
      }
      */
  }
}
</script>