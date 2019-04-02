<template>
  <a-card :bordered="false" >
    <router-view></router-view>
    <div class="table-page-search-wrapper" >
      <a-form layout="inline">
        <a-row type="flex" justify="space-around">
          <a-col :md="8" :sm="24">
            <a-tooltip>
              <template slot="title">新增权限</template>
              <a-button type="primary" @click="$refs.PermissionModal.add()" icon="plus" style="margin-right:10px"></a-button>
            </a-tooltip>
            <a-radio-group defaultValue="-1" v-model="queryParam.enable" buttonStyle="solid" @change="loadDataing">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">启用</a-radio-button>
              <a-radio-button value="0">停用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :span="8" :md="{span:12,offset:4}" :sm="{span:24,offset:0}" :xs="{span:24,offset:0}" :offset="8">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input-search placeholder="权限标识、名称" v-model="queryParam.Vague" enterButton="查询" @search="loadDataing" style="width:300px;margin-right:10px">
              </a-input-search>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table
      ref="table"
      :columns="columns"
      :dataSource="loadData"
      size="small"
      :loading="loading"
      :pagination="false"
      :rowKey="record => record.id">
      <span slot="actions" slot-scope="text, record">
        <a-tag v-for="(val,key) in record.actions" :key="key" @click="handerContrl(val,key)">{{ val }}</a-tag>
      </span>
      <span slot="enable" slot-scope="text">{{ formatEnable(text) }}</span>
      <span slot="sort" slot-scope="text, record">
        <modify-sort :text="text" :id="record.id" @modifyComplete="loadDataing"></modify-sort>
      </span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.PermissionModal.edit(record)" v-if="!record.isSystem">编辑</a>
        <a v-else disabled>编辑</a>
        <a-divider type="vertical" />
        <a-tooltip>
          <template slot="title">添加操作权限</template>
          <a @click="handleAdd">添加</a>
        </a-tooltip>
        <a-divider type="vertical" />
        <a-dropdown>
          <a class="ant-dropdown-link">更多
            <a-icon type="down" />
          </a>
          <a-menu slot="overlay">
            <a-menu-item>
              <router-link :to="{ name: 'permissionDetail', query: {data: record.id }}" >详情</router-link>
            </a-menu-item>
            <a-menu-item :disabled="record.isSystem">
              <a @click="handleNavigationDelete(record.id)" v-if="!record.isSystem">删除</a>
              <a v-else disabled>删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </a-table>

    <!--点击操作权限弹出详情编辑框-->
    <a-modal
      title="操作权限编辑"
      :width="640"
      v-model="visibleControl"
      @cancel="editModalCancel()"
      :centered="true"
      @ok="handleSubmit">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="父级权限" hasFeedback>
          <a-input placeholder="父级权限" v-model="controlFrom.parentId" disabled="disabled" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="标识" hasFeedback>
          <a-input placeholder="标识" v-model="controlFrom.id" disabled="disabled" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="API地址模板" hasFeedback validateStatus="success">
          <a-input placeholder="请输入API地址模板" v-model="controlFrom.apiTemplate" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限名称" hasFeedback validateStatus="success">
          <a-input placeholder="权限描述" v-model="controlFrom.name" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="添加时间" hasFeedback>
          <a-input placeholder="添加时间" v-model="controlFrom.createTime" disabled="disabled" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="系统权限">
          <a-switch :checked="controlFrom.isSystem" :disabled="true" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="记录日志">
          <a-switch :checked="controlFrom.isLogger" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否启用">
          <a-switch :checked="controlFrom.enable" />
        </a-form-item>
      </a-form>
    </a-modal>

    <!--添加操作权限弹窗-->
    <a-modal title="添加操作权限" :width="640" :centered="true" v-model="visibleAdd" @ok="addAction">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限编码" hasFeedback validateStatus="success">
          <a-input placeholder="权限编码" v-model="addActine.code" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限名称" hasFeedback validateStatus="success">
          <a-input placeholder="起一个名字" v-model="addActine.name" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="上级权限编码">
          <!--<a-input placeholder="上级权限编码" v-model="addActine.parentId"/>-->
          <a-dropdown>
            <span class="ant-dropdown-link" :trigger="['click']" href="#">
              点击选择父级
              <a-icon type="down" />
            </span>
            <a-menu slot="overlay">
              <a-sub-menu v-for="(items,index) in dataLoad.result" :key="index" :title="items.name">
                <a-menu-item v-for="(list,index1) in items.children" :key="index1" @click="actionTrigger(index1, list.id)">{{ list.name }}</a-menu-item>
              </a-sub-menu>
            </a-menu>
          </a-dropdown>
          您的选择： {{ addActine.parentId }}
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限服务地址" hasFeedback validateStatus="success">
          <a-input placeholder="权限服务地址" v-model="addActine.apiTemplate" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否日志记录">
          <a-switch :checked="addActine.isLogger" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序" hasFeedback>
          <a-input-number :min="1" :max="10" v-model="addActine.sort" />
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="描述" hasFeedback validateStatus="success">
          <a-input placeholder="描述" v-model="addActine.description" />
        </a-form-item>
      </a-form>
    </a-modal>
    <!--添加/编辑导航权限弹窗-->
    <permission-edit ref="PermissionModal" @ok="handleEditSubmit"></permission-edit></a-card>
</template>

<script>
/* eslint-disable */
import ModifySort from './modules/ModifySortModule'
import STable from '@/components/Table/'
import {RouteView} from '@/layouts/'
import PermissionEdit from './PermissionEdit'
import { getPermissions,getActionDetails,modifyAction,CreateAction,CreateMenu, deleteMenu} from '@/api/manage'
import AFormItem from 'ant-design-vue/es/form/FormItem'
export default {
  name: 'TableList',
  components: {
    PermissionEdit,
    AFormItem,
    STable,
    ModifySort,
    RouteView
  },
  data() {
    return {
      pagination: {
        hideOnSinglePage: true,
        pageSizeOptions: [],
        onChange: page => {}
      }, 
      loading: false,
      addActine: {
        code: '',
        parentId: '',
        name: '',
        apiTemplate: '',
        isLogger: true,
        sort: '',
        description: ''
      },
      formLayout: 'horizontal',
      form: this.$form.createForm(this),
      defaultExpandAllRows: true,
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visible: false,
      visibleAdd: false,
      visibleControl: false,
      menuDetails: [],
      dataLoad: '',
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
      alert: {
        show: true,
        clear: true
      },
      controlFrom: {
        id: '',
        parentId: '',
        name: '',
        apiTemplate: '',
        isSystem: '',
        isLogger: '',
        enable: '',
        createTime: ''
      },
      // form: null,
      mdl: {},
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {
        Vague: '',
        Enable: -1,
        PagedCount: 10,
        SkipPage: 1,
        IsLoad: true,
        totalCount: 0
      },
      // 表头
      columns: [
        {
          title: '权限编码',
          dataIndex: 'id'
        },
        {
          title: '权限名称',
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
      ],
      // 向后端拉取可以用的操作列表
      permissionList: null,
      // 权限子级权限children:[]
      permissionchildren: {},
      // 加载数据方法 必须为 Promise 对象
      loadData: [],
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  created() {
    this.loadPermissionList()
    this.loadDataing()
  },
  methods: {
    //格式化状态
    formatEnable(value) {
      return value ? '启用' : '停止'
    },
    //删除导航权限
    handleNavigationDelete(id) {
      const that = this
      this.$confirm({
        title: '提示',
        content: '确定要删除吗 ?',
        onOk() {
          deleteMenu(id).then(res => {
            if (res.status === 200) {
              that.loadDataing()
              that.$message.success('删除成功')
            } else {
              that.$message.error(res.message)
            }
          })
        },
        onCancel() {}
      })
    },
    //加载数据
    loadDataing() {
      this.loading = true
      getPermissions(this.queryParam).then(res => {
        if (res.result.length > 0) this.addActine.parentId = res.result[0].id
        if (res.status === 200) {
          this.loading = false
          this.loadData = this.makePermissionList(res)
        } else {
          this.loading = false
        }
      })
    },
    //添加操作权限的父级选择触发事件
    actionTrigger(index, id) {
      this.addActine.parentId = id
    },
    //添加操作权限事件
    addAction() {
      CreateAction(this.addActine).then(res => {
        if (res.status === 200) {
          this.loadDataing()
          this.visibleAdd = false
        } else {
          this.$notification['error']({
            message: '错误',
            description: res.message,
            duration: 4
          })
        }
      })
    },

    // 编辑权限弹窗关闭回调函数
    editModalCancel() {
      const obj = {
        id: '',
        parentId: '',
        name: '',
        apiTemplate: '',
        isSystem: '',
        isLogger: '',
        enable: '',
        createTime: ''
      }
      this.controlFrom = obj
    },
    //编辑操作权限提交方法
    handleSubmit(e) {
      e.preventDefault()
      // this.visibleControl = true
      const obj = this.controlFrom
      modifyAction(obj).then(res => {
        if (res.status === 200) {
          this.visibleControl = !this.visibleControl
        } else {
          this.$notification['error']({
            message: '错误',
            description: res.message,
            duration: 4
          })
        }
      })
    },
    //加载导航权限数据
    makePermissionList(res) {
      const result1 = []
      const data = Object.assign(res, this.queryParam)
      data.result.forEach((element, index) => {
        if (element.parentId === '') {
          result1.push(element)
          element.children = []
          for (const i in data.result) {
            if (element.id === data.result[i].parentId) {
              element.children.push(data.result[i])
            }
          }
        }
      })
      data.result = result1
      this.dataLoad = data
      return data.result
    },
    //点击操作权限tag标签事件
    handerContrl(action, key) {
      this.visibleControl = true
      const id = key
      getActionDetails(id).then(res => {
        if (res.status === 200) {
          this.controlFrom = res.result
        }
      })
    },
   
    //添加操作权限弹框
    handleAdd() {
      this.visibleAdd = true
    },
    //加载导航权限
    loadPermissionList() {
      // permissionList
      new Promise(resolve => {
        const data = [
          {
            label: '新增',
            value: 'add',
            defaultChecked: false
          },
          {
            label: '查询',
            value: 'get',
            defaultChecked: false
          },
          {
            label: '修改',
            value: 'update',
            defaultChecked: false
          },
          {
            label: '列表',
            value: 'query',
            defaultChecked: false
          },
          {
            label: '删除',
            value: 'delete',
            defaultChecked: false
          },
          {
            label: '导入',
            value: 'import',
            defaultChecked: false
          },
          {
            label: '导出',
            value: 'export',
            defaultChecked: false
          }
        ]
        setTimeout(resolve(data), 1500)
      }).then(res => {
        this.permissionList = res
      })
    },
    //分页change事件
    onChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    // 添加/编辑导航权限完成后的刷新列表
    handleEditSubmit() {
      this.loadDataing()
    }
  }
}
</script>
<style>
/* .rownew:before,
              .rownew:after {
                content: '';
                display: none;
              }
              .rownew {
                display: flex;
                justify-content: space-between;
              } */
.editable-cell {
  position: relative;
}

.editable-add-btn {
  margin-bottom: 8px;
}

.editable-cell .ant-input {
  width: 50px;
}
</style>
