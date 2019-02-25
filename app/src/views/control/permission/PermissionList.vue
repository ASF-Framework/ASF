<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row type="flex" justify="space-around">
          <a-col :md="8" :sm="24">
            <a-tooltip>
              <template slot="title">新增权限</template>
              <a-button type="primary" @click="NevigateView" icon="plus" style="margin-right:10px"></a-button>
            </a-tooltip>
            <a-radio-group
              defaultValue="-1"
              v-model="queryParam.Enable"
              buttonStyle="solid"
              @change="$refs.table.refresh(true)">
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
                  <a-button
                    icon="undo"
                    @click="() => queryParam = {Vague:'', Enable:-1,PagedCount:10,SkipPage:1}"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <s-table
      ref="table"
      :columns="columns"
      :data="loadData"
      size="small"
    >
      <span slot="actions" slot-scope="text, record">
        <a-tag
          v-for="(val,key) in record.actions"
          :key="key"
          @click="handerContrl(val,key)"
        >{{ val }}</a-tag>
      </span>
      <span slot="enable" slot-scope="text">{{ text | statusFilter }}</span>
      <span slot="sort" slot-scope="text, record">
        <editable-cell :text="text" ref="sortDom" @handleChange="editSortInput" @change="handerChange" @editBlurInput="handerChange(record)"/>
      </span>
      <span slot="action" slot-scope="text, record">
        <!--<a @click="handleEdit(record)">编辑</a>-->
        <a @click="handleEdit(record)" v-if="!record.isSystem">编辑</a>
        <a v-else disabled>编辑</a>
        <a-divider type="vertical"/>
        <a-tooltip>
          <template slot="title">添加操作权限</template>
          <a @click="handleAdd">添加</a>
        </a-tooltip>
        <a-divider type="vertical"/>
        <a-dropdown>
          <a class="ant-dropdown-link">更多
            <a-icon type="down"/>
          </a>
          <a-menu slot="overlay">
            <a-menu-item>
              <a href="javascript:;" @click="pushDetalis(record)">详情</a>
            </a-menu-item>
            <a-menu-item :disabled="record.isSystem">
              <!--<a href="javascript:;">禁用</a>-->
              <a @click="handleEdit(record)" v-if="!record.isSystem">禁用</a>
              <a v-else disabled>禁用</a>
            </a-menu-item>
            <a-menu-item :disabled="record.isSystem">
              <!--<a href="javascript:;">删除</a>-->
              <a @click="handleEdit(record)" v-if="!record.isSystem">删除</a>
              <a v-else disabled>删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </s-table>

    <!--点击操作权限弹出详情编辑框-->
    <a-modal
      title="操作权限编辑"
      :width="800"
      v-model="visibleControl"
      @cancel="editModalCancel()"
      :centered="true"
      @ok="handleSubmit">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="父级权限"
          hasFeedback
        >
          <a-input placeholder="父级权限" v-model="controlFrom.parentId" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="标识"
          hasFeedback
        >
          <a-input placeholder="标识" v-model="controlFrom.id" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="API地址模板"
          hasFeedback
          validateStatus="success"
        >
          <a-input
            placeholder="请输入API地址模板"
            v-model="controlFrom.apiTemplate"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input
            placeholder="权限描述"
            v-model="controlFrom.name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="添加时间"
          hasFeedback
        >
          <a-input placeholder="添加时间" v-model="controlFrom.createTime" disabled="disabled"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="系统权限">
          <a-switch :checked="controlFrom.isSystem" :disabled="true"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="记录日志">
          <a-switch :checked="controlFrom.isLogger"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否启用">
          <a-switch :checked="controlFrom.enable"/>
        </a-form-item>
      </a-form>
    </a-modal>
    <!--编辑弹窗-->
    <a-modal title="编辑" :width="800" v-model="visible" @ok="handleOk">
      <a-form>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限编码" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="起一个名字"/>
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
          <a-textarea :rows="5" v-model="mdl.describe" placeholder="..."/>
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予权限" hasFeedback>
          <a-select style="width: 100%" mode="multiple" v-model="mdl.actions" :allowClear="true">
            <a-select-option
              v-for="(action, index) in permissionList"
              :key="index"
              :value="action.value"
            >{{ action.label }}
            </a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
    <!--添加操作权限弹窗-->
    <a-modal
      title="添加操作权限"
      :width="800"
      :centered="true"
      v-model="visibleAdd"
      @ok="addAction"
    >
      <a-form :autoFormCreate="(form)=>{this.form = form}">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限编码,默认是code" v-model="addActine.code" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="起一个名字" v-model="addActine.name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="上级权限编码"
        >
          <!--<a-input placeholder="上级权限编码" v-model="addActine.parentId"/>-->
          <a-dropdown>
            <span class="ant-dropdown-link" :trigger="['click']" href="#">
              点击选择父级 <a-icon type="down" />
            </span>
            <a-menu slot="overlay">
              <a-sub-menu v-for="(items,index) in dataLoad.result" :key="index" :title="items.name">
                <a-menu-item v-for="(list,index) in items.children" :key="index" @click="actionTrigger(index, list.id)">{{ list.name }}</a-menu-item>
              </a-sub-menu>
            </a-menu>
          </a-dropdown>
          您的选择： {{ addActine.parentId }}
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限服务地址"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限服务地址" v-model="addActine.apiTemplate"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否日志记录">
          <a-switch :checked="addActine.isLogger"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序" hasFeedback>
          <a-input-number :min="1" :max="10" v-model="addActine.sort"/>
        </a-form-item>
        <a-divider/>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="描述"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="描述" v-model="addActine.description"/>
        </a-form-item>
      </a-form>
    </a-modal>

    <!--添加导航权限弹窗-->

    <a-modal
      title="添加导航权限"
      :width="800"
      :centered="true"
      v-model="addNevigate"
      @ok="addNevigateView"
    >
      <a-form>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限编码"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="权限编码,默认是code" v-model="addNevigateData.code" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="起一个名字" v-model="addNevigateData.name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="上级权限编码"
        >
          <!--<a-input placeholder="上级权限编码" v-model="addActine.parentId"/>-->
          <a-dropdown>
            <span class="ant-dropdown-link" :trigger="['click']">
              点击选择父级 <a-icon type="down" />
            </span>
            <a-menu slot="overlay">
              <a-sub-menu v-for="(items,index) in dataLoad.result" :key="index" :title="items.name">
                <a-menu-item v-for="(list,index) in items.children" :key="index" @click="nevigateTrigger(index, list.id)">{{ list.name }}</a-menu-item>
              </a-sub-menu>
            </a-menu>
          </a-dropdown>
          您的选择： {{ addNevigateData.parentId }}
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序" hasFeedback>
          <a-input-number :min="1" :max="10" v-model="addNevigateData.sort"/>
        </a-form-item>
        <a-divider/>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="描述"
          hasFeedback
          validateStatus="success"
        >
          <a-input placeholder="描述" v-model="addNevigateData.description"/>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
import EditableCell from '@/components/EditCell/EditableCell'
import STable from '@/components/table/'
import { getPermissions, getActionDetails, modifyAction, modifySort, CreateAction, CreateMenu } from '@/api/manage'
import AFormItem from 'ant-design-vue/es/form/FormItem'
export default {
  name: 'TableList',
  components: {
    AFormItem,
    STable,
    EditableCell
  },
  data () {
    return {
      editSort: '',
      addNevigate: false,
      addNevigateData: {
        code: 'code',
        parentId: '',
        name: '',
        sort: '',
        description: ''
      },
      addActine: {
        code: 'code',
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
      description: '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
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
      loadData: parameter => {
        return getPermissions(this.queryParam).then(res => {
          this.addActine.parentId = res.result[0].id
          //           if(this.queryParam.Enable!="0")   {
          // this.queryParam.totalCount=5
          //           }else{
          //             this.queryParam.totalCount=0
          //           }
          return this.makePermissionList(res)
        })
      },
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  created () {
    this.loadPermissionList()
  },
  methods: {
    actionTrigger (index, id) {
      this.addActine.parentId = id
    },
    nevigateTrigger (index, id) {
      this.addNevigateData.parentId = id
    },
    NevigateView () {
      this.addNevigate = true
    },
    addNevigateView () {
      CreateMenu(this.addNevigateData).then(res => {
        if (res.status === 200) {
          // this.$refs.table.refresh(true)
          this.addNevigate = false
        } else {
          this.$notification['error']({
            message: '错误',
            description: res.message,
            duration: 4
          })
        }
      })
    },
    addAction () {
      CreateAction(this.addActine).then(res => {
        if (res.status === 200) {
          this.$refs.table.refresh(true)
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
    pushDetalis (record) {
      this.$router.push({
        name: 'Details',
        query: {
          data: record.id
        }
      })
    },
    // 编辑权限弹窗关闭回调函数
    editModalCancel () {
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
    handleSubmit (e) {
      e.preventDefault()
      // this.visibleControl = true
      const obj = this.controlFrom
      modifyAction(obj).then(res => {
        // console.log(res)
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
    makePermissionList (res) {
      const result = []
      const data = Object.assign(res, this.queryParam)
      if (this.queryParam.Vague === '' && this.queryParam.Enable === -1) {
        data.result.forEach((element, index) => {
          if (element.parentId === '' || element.parentId === ' ') {
            result.push(element)
            result[index].children = []
            data.result.forEach(ele => {
              if (element.id === ele.parentId) {
                result[index].children.push(ele)
              }
            })
          }
        })
        data.result = result
      }
      this.dataLoad = data
      return data
    },
    handleBtn () {
      this.makePermissionList()
    },
    handerContrl (action, key) {
      // this.controlFrom = action
      // console.log(222222222, action, key)
      console.log(key)
      this.visibleControl = true
      const id = key
      getActionDetails(id).then(res => {
        console.log(res)
        if (res.status === 200) {
          this.controlFrom = res.result
        }
      })
    },
    loadGetData () {
      return getPermissions(this.queryParam).then(res => {
        return this.makePermissionList(res)
      })
    },
    // 修改排序input，input失去焦点或者按Enter键触发此函数
    handerChange (record) {
      console.log(record)
      const parameter = {
        id: record.id,
        sort: this.editSort
      }
      modifySort(parameter).then(res => {
        if (res.status === 200) {
          // 重新请求数据
          this.$refs.table.refresh(true)
        }
      })
    },
    editSortInput (e) {
      this.editSort = e
    },
    handleAdd () {
      this.visibleAdd = true
    },
    loadPermissionList () {
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
    handleEdit (record, text) {
      this.mdl = Object.assign({}, record)
      console.log(this.mdl, record, text)
      this.visible = true
    },
    handleOk () {
    },
    onChange (selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    toggleAdvanced () {
      this.advanced = !this.advanced
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
