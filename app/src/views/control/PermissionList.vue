<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row type="flex" justify="space-around">
          <a-col :md="8" :sm="24">
            <a-tooltip>
              <template slot="title">新增权限</template>
              <a-button type="primary" icon="plus" @click="handleAdd" style="margin-right:10px"></a-button>
            </a-tooltip>
            <a-radio-group  defaultValue="-1" v-model="queryParam.Enable"  buttonStyle="solid" @change="$refs.table.refresh(true)" >
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">启用</a-radio-button>
              <a-radio-button value="0">停用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :span="8" :md="{span:12,offset:4}" :sm="{span:24,offset:0}" :xs="{span:24,offset:0}" :offset="8">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input placeholder="权限标识、名称" v-model="queryParam.Vague" style="width:300px;margin-right:10px"/>
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
    <s-table  ref="table" :columns="columns" :data="loadData" size="small" :defaultExpandAllRows="true">     
      <span slot="actions" slot-scope="text, record">
        <a-tag
          v-for="(val,key) in record.actions"
          :key="key"
          @click="handerContrl(val)"
        >{{ val }}</a-tag>
      </span>
      <span slot="enable" slot-scope="text">{{ text | statusFilter }}</span>
      <span slot="sort" slot-scope="text">
        <editable-cell :text="text" @change="handerChange"/>
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
              <a href="javascript:;">禁用</a>
            </a-menu-item>
            <a-menu-item>
              <a href="javascript:;">删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </s-table>
    <a-modal title="操作权限编辑" :width="800" v-model="visibleContrl">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="操作编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="操作编码" v-model="controlFrom.action" id="no" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限描述"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限描述" v-model="controlFrom.describe"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="操作地址"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="操作地址" v-model="controlFrom.adderss"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否可用">
          <a-switch :defaultChecked="controlFrom.defaultCheck"/>
        </a-form-item>
      </a-form>
    </a-modal>
    <a-modal title="编辑" :width="800" v-model="visible" @ok="handleOk">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限编码" v-model="mdl.id" id="no" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="起一个名字" v-model="mdl.name" id="permission_name"/>
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
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予权限" hasFeedback>
          <a-select style="width: 100%" mode="multiple" v-model="mdl.actions" :allowClear="true">
            <a-select-option
              v-for="(action, index) in permissionList"
              :key="index"
              :value="action.value"
            >{{ action.label }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
    <a-modal title="添加" :width="800" v-model="visibleAdd" @ok="handleOk">
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限编码" v-model="form.id"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="起一个名字" v-model="form.name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态"
          hasFeedback
          validateStatus="warning"
        >
          <a-select v-model="form.enable">
            <a-select-option value="1">启用</a-select-option>
            <a-select-option value="0">停止</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序" hasFeedback>
          <a-input-number :min="1" :max="10"  v-model="form.sort"  />
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予操作权限" hasFeedback>
          <a-select style="width: 100%" mode="multiple" v-model="mdl.actions" :allowClear="true">
            <a-select-option
              v-for="(action, index) in permissionList"
              :key="index"
              :value="action.value"
            >{{ action.label }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
import EditableCell from '@/components/EditCell/EditableCell'
import STable from '@/components/table/'
import { getPermissions  } from '@/api/manage'
export default {
  name: 'TableList',
  components: {
    STable,
    EditableCell
  },
  data() {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visible: false,
      visibleAdd: false,
      visibleContrl: false,
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
      controlFrom: {},
      form: null,
      mdl: {},
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {
        Vague:"",
        Enable:-1,
        PagedCount:10,
        SkipPage:1,
        IsLoad:true
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
          width: '150px',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        }
      ],
      // 向后端拉取可以用的操作列表
      permissionList: null,
      //权限子级权限children:[]
      permissionchildren:{
        
      },
      // 加载数据方法 必须为 Promise 对象
      loadData: parameter => {    
        return getPermissions(this.queryParam).then(res=>{           
               return this.makePermissionList(res)
        })
      },
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {        
        1: '启用',
        0: '停止'
      }
      return statusMap[status?1:0]
    }
  },
  created() {
    this.loadPermissionList()
  },
  methods: {
    makePermissionList(res){      
      let result= []
      let data =Object.assign(res,this.queryParam)
      if(this.queryParam.Vague=="" && this.queryParam.Enable==-1){
         data.result.forEach((element,index) => {
           if(element.parentId=="" || element.parentId==" "){
            result.push(element)
            result[index].children=[]
            data.result.forEach(ele => {
              if(element.id==ele.parentId){
                 result[index].children.push(ele)
              }
            });
          }        
        });
         data.result=result;
        console.log("Table-resource:",data)
      }    
      return data
    },
    handleBtn(){
      this.makePermissionList()
    },
    handerContrl(action) {
      this.controlFrom = action
      console.log(222222222, this.controlFrom)
      this.visibleContrl = true
    },
    handerChange() {},
    handleAdd() {
      this.visibleAdd = true
    },
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
    handleEdit(record) {
      this.mdl = Object.assign({}, record)
      console.log(this.mdl)
      this.visible = true
    },
    handleOk() {},
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