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
             <a-radio-group defaultValue="-1" buttonStyle="solid" @change="onRadioChange">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">正常</a-radio-button>
              <a-radio-button value="2">不可登录</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :md="12" :sm="24">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input
                placeholder="请输入ID/昵称/用户名"
                v-model="filters.name"
                style="width:300px;margin-right:10px"  
              />
              <a-button-group>
                <a-tooltip>
                <template slot="title">
                  查询
                </template>
                  <a-button type="primary" icon="search"></a-button>
                </a-tooltip>
                <a-tooltip>
                <template slot="title">
                  重置查询条件
                </template>
                  <a-button icon="undo"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-divider style="margin:0;"></a-divider>
    <a-list size="large" :pagination="{showSizeChanger: true, showQuickJumper: true, pageSize: queryParam.PagedCount, total: total}">
        <a-list-item :key="index" v-for="(item, index) in userList">
          <a-list-item-meta>
            <a slot="description"><a-tag v-for="(role, index) in item.roles" :key="index">{{ role}}</a-tag></a>
            <a-avatar slot="avatar" size="large" shape="square" :src="item.avatar"/>
            <a slot="title">{{item.name+'（'+item.id+'）'}}</a>
          </a-list-item-meta>
          <div slot="actions">
            <a>编辑</a>
          </div>
          <div slot="actions">
            <a-dropdown>
              <a-menu slot="overlay">
                <a-menu-item><a>编辑</a></a-menu-item>
                <a-menu-item><a>删除</a></a-menu-item>
              </a-menu>
              <a>更多<a-icon type="down"/></a>
            </a-dropdown>
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
  import {
    getRoleList,
    getServiceList,
    getAdminList
  } from '@/api/manage'
  export default {
    name: 'TableList',
    components: {
      STable
    },
    data() {
      return {
        description: '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
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
        queryParam: {
          Vague:"",
          Status:-1,
          IsDeleted:false,
          PagedCount:10,
          SkipPage:1
        },       
        permissionList: null,
        
        selectedRowKeys: [],
        selectedRows: [],

        userList:[],
        total:0,
        status:[
          {
          value: -1,
          label: "全部"
        },
        {
          value: 1,
          label: "正常"
        },
        {
          value: 2,
          label: "不可登录"
        }
        ]
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
      getAdminList(this.queryParam).then(res => {
        let data =Object.assign(res,this.queryParam)
        this.total=res.totalCount
        console.log(1111111,data)
        this.userList=res.result
      })
    },
    methods: {
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
    onRadioChange(e){
      this.$message.success('你选择的是：'+e.target.value)
    },
      formatIsDelete(value) {
      return value == true ? "启用" : "禁用";
        },
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