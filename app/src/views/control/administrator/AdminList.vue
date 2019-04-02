<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form
        layout="inline"
        :model="queryParam">
        <a-row :gutter="0">
          <a-col
            :md="12"
            :sm="24">
            <a-tooltip>
              <template slot="title">新建管理员
              </template>
              <a-button
                type="primary"
                icon="plus"
                @click="handleAdd"
                style="margin-right:10px"
                v-action:create></a-button>
            </a-tooltip>
            <a-radio-group
              :defaultValue="0"
              v-model="queryParam.isDeleted"
              buttonStyle="solid"
              style="margin-right:10px"
              @change="handleSearch">
              <a-radio-button :value="0">正常</a-radio-button>
              <a-radio-button :value="1">已删除</a-radio-button>
            </a-radio-group>
            <a-select
              placeholder="请选择"
              v-model="queryParam.status"
              style="width:100px;"
              @change="handleSearch">
              <a-select-option value="-1">全部状态</a-select-option>
              <a-select-option value="1">启用</a-select-option>
              <a-select-option value="2">禁用</a-select-option>
            </a-select>

          </a-col>
          <a-col
            :md="12"
            :sm="24">
            <span
              class="table-page-search-submitButtons"
              style="float:right">
              <a-input-search
                placeholder="请输入ID/昵称/用户名"
                v-model="queryParam.Vague"
                enterButton="查询"
                @search="handleSearch"
                style="width:300px;margin-right:10px">
              </a-input-search>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-divider style="margin:0;"></a-divider>
    <a-list
      size="large"
      :loading="loading"
      :pagination="pagination">
      <a-list-item
        :key="index"
        v-for="(item, index) in userList">
        <a-list-item-meta>
          <a slot="description">
            <a-tag
              v-for="(role, index1) in item.roles"
              :key="index1">{{ role }}</a-tag>
          </a>
          <a-avatar
            slot="avatar"
            size="large"
            shape="square"
            :src="item.avatar" />
          <a slot="title">{{ item.name+'（'+item.username+'/'+item.id+'）' }}</a>
        </a-list-item-meta>
        <div slot="actions">
          <a
            @click="$refs.modal.edit(item)"
            v-if="!item.isSystem"
            v-action:modify_info>编辑</a>
          <a
            v-else
            disabled>编辑</a>
        </div>
        <div slot="actions">
          <a-dropdown v-if="!item.isSystem">
            <a-menu slot="overlay">
              <a-menu-item>
                <a
                  @click="handleStatus(item)"
                  v-action:modify_status>{{ ShowStatus(item.status) }}</a>
              </a-menu-item>
              <a-menu-item>
                <a
                  @click="handleDelete(item.id)"
                  v-action:delete>删除</a>
              </a-menu-item>
              <a-menu-item>
                <a
                  @click="openModalSetPwd(item.id)"
                  v-action:set_password>重置密码</a>
              </a-menu-item>
            </a-menu>
            <a>更多
              <a-icon type="down" /></a>
          </a-dropdown>
          <a
            v-else
            disabled>更多
            <a-icon type="down" /></a>
        </div>
        <div class="list-content">
          <!-- <div class="list-content-item">
              <span>登录名</span>
              <p>{{ item.username }}</p>
            </div> -->
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
            <p>{{ item.createTime*1000 | moment('YYYY-MM-DD') }}</p>
          </div>
          <div class="list-content-item">
            <span>登录</span>
            <p>{{ item.loginTime*1000 | moment }}</p>
          </div>
        </div>
      </a-list-item>
    </a-list>
    <admin-edit
      ref="modal"
      @ok="handleEditSubmit"></admin-edit>
    <a-modal
      title="添加管理员"
      style="top: 20px;"
      :width="640"
      v-model="visibleAdd"
      :confirmLoading="loadingAdd"
      @ok="handleAddSubmit"
      @cancel="handleAddCancel">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录名"
          hasFeedback>
          <a-input
            placeholder="登录名"
            v-decorator="['username',{rules: [{ required: true, message: '请输入登录名' }]}]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="昵称"
          hasFeedback>
          <a-input
            placeholder="昵称"
            v-decorator="['name',{rules: [{ required: true, message: '请输入昵称' }]}]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="登录密码"
          hasFeedback>
          <a-input
            placeholder="登录密码"
            type="password"
            v-decorator="['password',{rules: [{ required: true, message: '请输入密码' }]}]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="确认登录密码"
          hasFeedback>
          <a-input
            placeholder="确认登录密码"
            type="password"
            v-decorator="['confPassword',{rules: [{ required: true, message: '请确认密码' }]}]" />
        </a-form-item>
        <a-divider/>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="赋予角色"
          hasFeedback>
          <a-select
            style="width: 100%"
            mode="multiple"
            :allowClear="true"
            v-decorator="['roles',{rules: [{ required: true, message: '请赋予角色' }]}]">
            <a-select-option
              v-for="(role, index) in roleList"
              :key="index"
              :value="role.id">{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
    <a-modal
      title="重置密码"
      v-model="visibleSetPwd"
      :confirmLoading="loadingSetPwd"
      @ok="handleSetSubmit"
      @cancel="handleSetCancel">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="管理员密码"
          hasFeedback>
          <a-input
            type="password"
            placeholder="管理员密码"
            v-decorator="['adminPassword',{rules: [{ required: true, message: '验证管理员密码' }]}]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="新密码"
          hasFeedback>
          <a-input
            type="password"
            placeholder="新密码"
            v-decorator="['password',{rules: [{ required: true, message: '请输入新密码' }]}]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="确认新密码"
          hasFeedback>
          <a-input
            type="password"
            placeholder="确认新密码"
            v-decorator="['confimpassword',{rules: [{ required: true, message: '请输入新密码' }]}]" />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
import md5 from 'md5'
import STable from '@/components/Table/'
import AdminEdit from './AdminEdit'
import {
  getRoleListAll,
  getAdminList,
  createAccount,
  modifyStatusAccount,
  deleteAccount,
  resetPassword
} from '@/api/manage'
export default {
  name: 'TableList',
  components: {
    STable,
    AdminEdit
  },
  data () {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      visibleEdit: false,
      visibleAdd: false,
      loadingAdd: false,
      visibleSetPwd: false,
      loadingSetPwd: false,
      setPwdAdminId: 0,
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
      loading: false,
      form: this.$form.createForm(this),
      mdl: {},
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {
        vague: '',
        status: '-1',
        isDeleted: 0,
        pagedCount: 10,
        skipPage: 1
      },
      roleList: [],
      selectedRowKeys: [],
      selectedRows: [],
      userList: [],
      status: [
        {
          value: -1,
          label: '全部'
        },
        {
          value: 1,
          label: '启用'
        },
        {
          value: 2,
          label: '禁用'
        }
      ],
      // 分页对象
      pagination: {
        onChange: page => {
          this.handleChange(page)
        },
        onShowSizeChange: (current, pageSize) => {
          this.handleChangePageSize(current, pageSize)
        },
        showSizeChanger: true,
        pageSizeOptions: ['10', '20', '40', '50'],
        current: 1,
        pageSize: 10,
        total: 0
      }
    }
  },
  filters: {
    statusFilter (status) {
      const statusMap = {
        1: '启用',
        0: '禁用'
      }
      return statusMap[status ? 1 : 0]
    }
  },
  created () {
    // 获取全部角色集合
    getRoleListAll().then(res => {
      this.roleList = res.result
    })
    this.makeList()
  },
  methods: {
    // 中文表示角色
    formatRole (rid) {
      let retValue = ''
      this.roleList.forEach(element => {
        if (element.id === rid) {
          retValue = element.name
        }
      })
      return retValue
    },
    // 中文表示状态
    formatStatus (value) {
      let retValue = ''
      this.status.forEach(function (opt) {
        if (opt.value === value) {
          retValue = opt.label
        }
      })
      return retValue
    },
    // 显示状态
    ShowStatus (value) {
      let retValue = ''
      this.status.forEach(function (opt) {
        if (opt.value !== value) {
          retValue = opt.label
        }
      })
      return retValue
    },
    // 返回item状态的其它状态
    ReturnStatus (value) {
      let retValue = ''
      this.status.forEach(function (opt) {
        if (opt.value !== value) {
          retValue = opt.value
        }
      })
      return retValue
    },
    // 改变状态
    handleStatus (item) {
      const that = this
      const par = {}
      par.id = item.id
      par.status = this.ReturnStatus(item.status)
      const text = this.ShowStatus(item.status)
      that.$confirm({
        title: '提示',
        content: '确定要修改该管理员状态为' + text + '吗 ?',
        onOk () {
          modifyStatusAccount(par).then(res => {
            if (res.status === 200) {
              that.makeList()
              that.$message.success('修改成功')
            } else {
              that.$message.error(res.message)
            }
          })
        },
        onCancel () {}
      })
    },
    // 查询
    handleSearch () {
      // 当有条件查询时查询页码必须等于1，后端才会从第一页数据开始查询并返回数据
      if (this.queryParam.vague !== '' || this.queryParam.status !== -1 || this.queryParam.isDeleted !== 0) {
        this.queryParam.skipPage = 1
      }
      this.makeList()
    },
    // 获取列表K
    makeList () {
      const _this = this
      this.loading = true
      getAdminList(this.queryParam).then(res => {
        if (res.status === 200) {
          this.loading = false
          _this.userList = res.result
          const pager = {
            ..._this.pagination
          }
          pager.total = res.totalCount
          _this.pagination = pager
        } else {
          this.loading = false
          _this.$message.success(res.message)
        }
      })
    },
    // 删除
    handleDelete (id) {
      const that = this
      this.$confirm({
        title: '提示',
        content: '确定要删除吗 ?',
        onOk () {
          deleteAccount(id).then(res => {
            if (res.status === 200) {
              that.makeList()
              that.$message.success('删除成功')
            } else {
              that.$message.error(res.message)
            }
          })
        },
        onCancel () {}
      })
    },
    // 重置密码
    handleSetSubmit () {
      this.loadingSetPwd = true
      const param = {}
      this.form.validateFields((err, values) => {
        if (!err) {
          if (values.password !== values.confimpassword) {
            this.$message.error('输入的新密码不一致，请重新输入')
            this.loadingSetPwd = false
            return false
          }
          param.id = this.setPwdAdminId
          param.password = md5(values.password)
          param.adminPassword = md5(values.adminPassword)
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              resetPassword(param).then(res => {
                if (res.status === 200) {
                  this.visibleSetPwd = false
                  this.loadingSetPwd = false
                  this.form = this.$form.createForm(this)
                  this.$message.success('设置成功')
                  this.makeList()
                } else {
                  this.loadingSetPwd = false
                  this.$message.error(res.message)
                }
              })
            })
            .catch(() => {
              // Do something
            })
            .finally(() => {
              this.loadingSetPwd = false
              this.handleSetCancel()
            })
        } else {
          this.loadingSetPwd = false
        }
      })
    },
    // 关闭设置密码弹框
    handleSetCancel () {
      this.setPwdAdminId = 0
      this.visibleSetPwd = false
      this.loadingSetPwd = false
      this.form = this.$form.createForm(this)
    },
    // 打开设置密码弹框
    openModalSetPwd (id) {
      this.setPwdAdminId = id
      this.visibleSetPwd = true
    },
    // 添加弹出框
    handleAdd () {
      this.visibleAdd = true
    },
    // 编辑提交方法
    handleEditSubmit () {
      this.makeList()
    },
    // 添加框点击取消
    handleAddCancel () {
      this.visibleAdd = false
      this.loadingAdd = false
      this.form = this.$form.createForm(this)
    },
    // 添加提交
    handleAddSubmit () {
      this.loadingAdd = true
      this.form.validateFields((err, values) => {
        if (!err) {
          if (values.password !== values.confPassword) {
            this.$message.error('输入的密码不一致，请重新输入')
            this.loadingAdd = false
            return false
          }
          values.password = md5(values.password)
          createAccount(values).then(res => {
            if (res.status === 200) {
              setTimeout(() => {
                this.visibleAdd = false
                this.loadingAdd = false
              }, 2000)
              this.makeList()
              this.form = this.$form.createForm(this)
              this.$message.success('提交成功')
            } else {
              this.loadingAdd = false
              this.$message.error(res.message)
            }
          })
        } else {
          this.loadingAdd = false
        }
      })
    },
    // 改变显示条数加载数据
    handleChangePageSize (current, pageSize) {
      this.pagination.current = current
      this.pagination.pageSize = pageSize
      this.queryParam.skipPage = current
      this.queryParam.pagedCount = pageSize
      this.makeList()
    },
    // 点击页码加载数据
    handleChange (page) {
      this.pagination.current = page
      this.queryParam.skipPage = page
      this.makeList()
    }
  },
  watch: {}
}
</script>
<style lang="less" scoped>
.ant-avatar-lg {
  width: 48px;
  height: 48px;
  line-height: 48px;
}
.list-content-item {
  color: rgba(0, 0, 0, 0.45);
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
