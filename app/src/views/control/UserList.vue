<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline" :model="filters">
        <a-row :gutter="0">
          <a-col :md="12" :sm="24">
            <a-tooltip>
              <template slot="title">新建角色</template>
              <a-button type="primary" icon="plus" @click="handleAdd" style="margin-right:10px"></a-button>
            </a-tooltip>
            <a-select placeholder="请选择状态" style="width:120px">
              <a-select-option value="0">全部</a-select-option>
              <a-select-option value="1">正常</a-select-option>
              <a-select-option value="2">禁用</a-select-option>
            </a-select>
          </a-col>

          <a-col :md="12" :sm="24">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input
                placeholder="请输入登录名"
                v-model="filters.name"
                style="width:auto;margin-right:10px"
              />
              <a-button-group>
                <a-tooltip>
                  <template slot="title">查询</template>
                  <a-button type="primary" icon="search" @click="loadData"></a-button>
                </a-tooltip>
                <a-tooltip>
                  <template slot="title">清除查询条件</template>
                  <a-button icon="undo"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <div class="table-operator"></div>
    <s-table size="default" :columns="columns" :data="loadData">
      <span slot="roles" slot-scope="text, record">
        <a-tag v-for="(role, index) in record.roles" :key="index">{{ role.roleName}}</a-tag>
      </span>
      <span slot="action" slot-scope="text, record">
        <a @click="handleEdit(record)">编辑</a>
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
              <a href="javascript:;" @click="handleDisables(record)">禁用</a>
            </a-menu-item>
            <a-menu-item>
              <a href="javascript:;" @click="handleDelete(record)">删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </s-table>
    <a-modal title="编辑" style="top: 20px;" :width="800" v-model="visible" @ok="handleOk">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录名"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="登录名" v-model="mdl.name" id="no" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="角色名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="角色名称" v-model="mdl.roleName" id="role_name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态"
          hasFeedback
          validateStatus="warning"
        >
          <a-select v-model="mdl.status">
            <a-select-option value="1">正常</a-select-option>
            <a-select-option value="2">禁用</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="描述" hasFeedback>
          <a-textarea :rows="5" v-model="mdl.describe" placeholder="..." id="describe"/>
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予角色" hasFeedback>
          <a-select style="width: 100%" mode="multiple" :allowClear="true">
            <a-select-option
              v-for="(role, index) in permissionList"
              :key="index"
              :value="role.name"
            >{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
    <a-modal title="添加" style="top: 20px;" :width="800" v-model="visibleAdd" @ok="handleOk">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录名"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="登录名" id="no"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态"
          hasFeedback
          validateStatus="warning"
        >
          <a-select>
            <a-select-option value="1">正常</a-select-option>
            <a-select-option value="2">禁用</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="描述" hasFeedback>
          <a-textarea :rows="5" placeholder="..." id="describe"/>
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予角色" hasFeedback>
          <a-select style="width: 100%" mode="multiple" :allowClear="true">
            <a-select-option
              v-for="(role, index) in permissionList"
              :key="index"
              :value="role.name"
            >{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
import STable from '@/components/table/'
import { getRoleList, getServiceList, getAdminList } from '@/api/manage'

export default {
  name: 'TableList',
  components: {
    STable
  },
  data() {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visible: false,
      visibleAdd: false,
      filters: {
        name: ''
      },
      labelCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 5
        }
      },
      wrapperCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 16
        }
      },
      form: null,
      mdl: {},
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {},
      // 表头
      columns: [
        {
          title: '登录名',
          dataIndex: 'name'
        },
        {
          title: '角色集',
          dataIndex: 'roles',
          scopedSlots: {
            customRender: 'roles'
          }
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
      permissionList: null,
      // 加载数据方法 必须为 Promise 对象
      loadData: parameter => {
        return getAdminList(parameter).then(res => {
          return res.result
        })
      },
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  created() {
    getServiceList().then(res => {
      console.log('getServiceList.call()', res)
    })
    getRoleList().then(res => {
      this.permissionList = res.result.data
      console.log('getRoleList.call()', res)
    })
    getAdminList(this.filters).then(res => {
      console.log('getAdminList.call()', res)
    })
  },
  methods: {
    //禁用
    handleDisables(record) {
      const that = this
      this.$confirm({
        title: '提示',
        content: '确定要禁用吗 ?',
        onOk() {
          // that.$message.info({
          //   title: '提示',
          //   description: '功能正在升级当中'
          // })
          that.$message.success('功能正在升级当中')
        },
        onCancel() {}
      })
    },
    handleDelete(record) {
      const that = this
      this.$confirm({
        title: '提示',
        content: '确定要删除吗 ?',
        onOk() {
          // that.$message.info({
          //   title: '提示',
          //   description: '功能正在升级当中'
          // })
          that.$message.success('功能正在升级当中')
        },
        onCancel() {}
      })
    },
    handleAdd() {
      this.visibleAdd = true
    },
    handleEdit(record) {
      this.mdl = Object.assign({}, record)
      // this.mdl.permissions.forEach(permission => {
      //   permission.actionsOptions = permission.actionEntitySet.map(action => {
      //     return {
      //       label: action.describe,
      //       value: action.action,
      //       defaultCheck: action.defaultCheck
      //     }
      //   })
      // })
      this.visible = true
    },
    handleOk() {

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
