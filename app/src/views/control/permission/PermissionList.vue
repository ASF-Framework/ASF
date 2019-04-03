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
        <a-tag v-for="(val,key) in record.actions" :key="key" @click="$refs.ActionEdit.edit(val,key)">{{ val }}</a-tag>
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
          <a @click="$refs.ActionAdd.add(dataLoad)">添加</a>
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

    <!--添加/编辑导航权限弹窗-->
    <permission-edit ref="PermissionModal" @ok="handleEditSubmit"></permission-edit>
    <!--点击操作权限弹出详情编辑框-->
    <action-edit ref="ActionEdit" @ok="handleEditSubmit"></action-edit>
    <!--添加操作权限弹窗-->
    <action-add ref="ActionAdd" @ok="handleEditSubmit"></action-add>
  </a-card>
</template>

<script>
/* eslint-disable */
import ModifySort from './modules/ModifySortModule'
import STable from '@/components/Table/'
import {RouteView} from '@/layouts/'
import PermissionEdit from './PermissionEdit'
import ActionEdit from './modules/ActionEdit'
import ActionAdd from './modules/ActionAdd'
import { getPermissions,getActionDetails,CreateAction,CreateMenu, deleteMenu} from '@/api/manage'
import AFormItem from 'ant-design-vue/es/form/FormItem'
export default {
  name: 'TableList',
  components: {
    PermissionEdit,
    AFormItem,
    STable,
    ModifySort,
    RouteView,
    ActionEdit,
    ActionAdd
  },
  data() {
    return {
      pagination: {
        hideOnSinglePage: true,
        pageSizeOptions: [],
        onChange: page => {}
      }, 
      confirmLoading:false,
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
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visible: false,
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
