<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline" :model="filters">
        <a-row :gutter="0">
          <a-col :md="12" :sm="24">
            <a-tooltip>
              <template slot="title">新建管理员
</template>
              <a-button type="primary" icon="plus" @click="handleAdd" style="margin-right:10px"></a-button>
            </a-tooltip>
             <a-radio-group defaultValue="-1" v-model="queryParam.status" buttonStyle="solid" @change="handleSearch">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">正常</a-radio-button>
              <a-radio-button value="2">禁用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :md="12" :sm="24">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input
                placeholder="请输入ID/昵称/用户名"
                v-model="queryParam.Vague"
                style="width:300px;margin-right:10px"  
              />
              <a-button-group>
                <a-tooltip>
<template slot="title">
   查询
</template>
                  <a-button type="primary" icon="search" @click="handleSearch"></a-button>
                </a-tooltip>
                <a-tooltip>
<template slot="title">
   重置查询条件
</template>
                  <a-button icon="undo" @click="() => queryParam = {Vague:'', Status:-1,PagedCount:10,SkipPage:1}"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-divider style="margin:0;"></a-divider>
    <a-list size="large" :pagination="{showSizeChanger: true, showQuickJumper: true, pageSize: queryParam.PagedCount, total: total}" >
        <a-list-item :key="index" v-for="(item, index) in userList">
          <a-list-item-meta>
            <a slot="description"><a-tag v-for="(role, index) in item.roles" :key="index">{{ role}}</a-tag></a>
            <a-avatar slot="avatar" size="large" shape="square" :src="item.avatar"/>
            <a slot="title">{{item.name+'（'+item.id+'）'}}</a>
          </a-list-item-meta>
          <div slot="actions"  >
            <a @click="$refs.modal.edit(item)" v-if="!item.isSystem">编辑</a>
            <a v-else disabled >编辑</a>
          </div>
          <div slot="actions" >
            <a-dropdown v-if="!item.isSystem" >
              <a-menu slot="overlay">
                <a-menu-item><a @click="handleStatus(item)">{{ShowStatus(item.status)}}</a></a-menu-item>
                <a-menu-item><a @click="handleDelete(item.id)">删除</a></a-menu-item>
              </a-menu>
              <a>更多<a-icon type="down"/></a>
            </a-dropdown>
            <a v-else disabled >更多<a-icon type="down"/></a>
          </div>
          <div class="list-content">
            <div class="list-content-item">
              <span>登录名</span>
              <p>{{ item.username }}</p>
            </div>
            <div class="list-content-item">
              <span>手机号码</span>
              <p>{{ item.telephone==null?"--":item.telephone }}</p>
            </div>
            <div class="list-content-item">
              <span>邮箱</span>
              <p>{{ item.email==null?"--":item.email }}</p>
            </div>
            <div class="list-content-item">
              <span>状态</span>
              <p>{{ formatStatus(item.status) }}</p>
            </div>
            <div class="list-content-item">
              <span>创建</span>
              <p>{{ item.createTime }}</p>
            </div>
            <div class="list-content-item">
              <span>登录</span>
              <p>{{ item.loginTime }}</p>
            </div>          
          </div>
        </a-list-item>
      </a-list>
    <user-modal ref="modal" @ok="handleEditSubmit"></user-modal>
    <a-modal title="添加管理员" style="top: 20px;" :width="800" v-model="visibleAdd" :confirmLoading="loadingAdd" @ok="handleAddSubmit" @cancel="handleAddCancel">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录名"
        >
          <a-input placeholder="登录名"  v-decorator="['username',{rules: [{ required: true, message: '请输入登录名' }]}]"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="昵称"
        >
          <a-input placeholder="昵称" v-decorator="['name',{rules: [{ required: true, message: '请输入昵称' }]}]"/>
        </a-form-item>
         <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录密码"
        >
          <a-input placeholder="登录密码"  type="password" v-decorator="['password',{rules: [{ required: true, message: '请输入密码' }]}]"/>
        </a-form-item>
        </a-form-item>
         <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="确认登录密码"
        >
          <a-input placeholder="确认登录密码" type="password" v-decorator="['confPassword',{rules: [{ required: true, message: '请确认密码' }]}]"/>
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予角色">
          <a-select style="width: 100%" mode="multiple" :allowClear="true" v-decorator="['roles',{rules: [{ required: true, message: '请赋予角色' }]}]">
            <a-select-option
              v-for="(role, index) in roleList"
              :key="index"
              :value="role.id"
            >{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
  import STable from '@/components/table/'
  import UserModal from './modules/UserModal'
  import {
    getRoleListAll,
    getServiceList,
    getAdminList,
    createAccount,
    modifyStatusAccount,
    deleteAccount
  } from '@/api/manage'
  import pick from 'lodash.pick'
  export default {
    name: 'TableList',
    components: {
      STable,
      UserModal
    },
    data() {
      return {
        description: '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
        visibleEdit: false,
        visibleAdd: false,
        loadingAdd: false,
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
        //formEdit:this.$form.createForm(this),
        //mdl:this.$form.createForm(this),
        form: this.$form.createForm(this),
        mdl: {},
        // 高级搜索 展开/关闭
        advanced: false,
        // 查询参数
        queryParam: {
          Vague: "",
          Status: -1,
          IsDeleted: false,
          PagedCount: 10,
          SkipPage: 1
        },
        roleList: [],
        selectedRowKeys: [],
        selectedRows: [],
        userList: [],
        total: 0,
        status: [{
            value: -1,
            label: "全部"
          },
          {
            value: 1,
            label: "正常"
          },
          {
            value: 2,
            label: "禁用"
          }
        ]
      }
    },
    filters: {
    statusFilter(status) {
      const statusMap = {        
        1: '正常',
        0: '禁用'
      }
      return statusMap[status?1:0]
    }
    },
    created() {
      getServiceList().then(res => {
      })
      getRoleListAll().then(res => {      
        this.roleList = res.result        
      })
      this.makeList()
    },
    methods: {
      //中文表示角色
      formatRole(rid){
        let retValue=""
         this.roleList.forEach(element => {
            if(element.id==rid){
                 retValue = element.name;
            }
         });
         return retValue
      },
      //中文表示状态
      formatStatus(value) {
        let retValue = "";
        this.status.forEach(function(opt) {
          if (opt.value == value) {
            retValue = opt.label;
          }
        });
        return retValue;
      },
      //显示状态
      ShowStatus(value) {
        let retValue = "";
        this.status.forEach(function(opt) {
          if (opt.value != value) {
            retValue = opt.label;
          }
        });
        return retValue;
      },
      //返回item状态的其它状态
      ReturnStatus(value){
        let retValue = "";
        this.status.forEach(function(opt) {
          if (opt.value != value) {
            retValue = opt.value;
          }
        });
        return retValue;
      },
      //改变状态
      handleStatus(item){
        const that = this
        let par={};
        par.id=item.id
        par.status=this.ReturnStatus(item.status)
        let text=this.ShowStatus(item.status)
        console.log(par)
        that.$confirm({
          title: '提示',
          content: '确定要修改该管理员状态为'+text+'吗 ?',
          onOk() {
            modifyStatusAccount(par).then(res=>{
              if(res.status==200){
                that.makeList()
                 that.$message.success('修改成功')
              }else{
                that.$message.error(res.message)
              }
            })
          },
          onCancel() {}
        })
      },
      //查询
      handleSearch(){
         this.makeList()
      },
      //获取列表
      makeList(){
         getAdminList(this.queryParam).then(res => {
            let data = Object.assign(res, this.queryParam)
            this.total = res.totalCount
            this.userList = res.result
            console.log("this.userList:",this.userList)
        })
      },
      formatIsDelete(value) {
        return value == true ? "启用" : "禁用";
      },
      //删除
      handleDelete(id) {
        const that = this
        this.$confirm({
          title: '提示',
          content: '确定要删除吗 ?',
          onOk() {
           deleteAccount(id).then(res=>{
             if(res.status==200){
               that.makeList()
               that.$message.success('删除成功')
             }else{
               that.$message.error(res.message)
             }
           })
            
          },
          onCancel() {}
        })
      },
      //添加弹出框
      handleAdd() {
        this.visibleAdd = true
      },
      handleEdit(record) {
        this.mdl = Object.assign({}, record)
        this.$nextTick(() => {
        this.form.setFieldsValue(pick(this.mdl, 'username', 'name'))
        })
        console.log(11111,this.mdl)
        // this.mdl.permissions.forEach(permission => {
        //   permission.actionsOptions = permission.actionEntitySet.map(action => {
        //     return {
        //       label: action.describe,
        //       value: action.action,
        //       defaultCheck: action.defaultCheck
        //     }
        //   })
        // })
        this.visibleEdit = true
      },
      handleEditSubmit(){
       this.makeList()
      },
      
      //添加框点击取消
      handleAddCancel(){
        this.visibleAdd = false
         this.loadingAdd=false
        this.form=this.$form.createForm(this)
      },
      //添加提交
      handleAddSubmit() {
        this.loadingAdd=true
         this.form.validateFields((err, values) => {
           if (!err) {            
             if(values.password!=values.confPassword){
               this.$message.error("输入的密码不一致，请重新输入");
               this.loadingAdd=false
              return false;
             }            
             createAccount(values).then(res=>{               
               if(res.status==200){
                  setTimeout(() => {
                      this.visibleAdd = false
                      this.loadingAdd=false
                   }, 2000);
                 this.makeList()
                  this.$message.success('提交成功')
               }else{
                 this.loadingAdd=false
                 this.$message.error(res.message)
               }              
             })
           }else{
             this.loadingAdd=false
           }
         })
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
<style lang="less" scoped>
  .ant-avatar-lg {
    width: 48px;
    height: 48px;
    line-height: 48px;
  }
  .list-content-item {
    color: rgba(0, 0, 0, .45);
    display: inline-block;
    vertical-align: middle;
    font-size: 14px;
    margin-left: 40px;
    span {
      line-height: 20px;
    }
    p {
      margin-top: 4px;
      margin-bottom: 0;
      line-height: 22px;
    }
  }
</style>