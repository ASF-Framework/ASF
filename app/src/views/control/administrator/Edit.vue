<template>
  <s-modal
    title="编辑管理员"
    ref="modal"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-bind="layout" label="用户名" >
          <a-input disabled="disabled" v-decorator="formDecorator.username" style="width: 50%"/>
        </a-form-item>
        <a-form-item v-bind="layout" label="昵称" hasFeedback >
          <a-input placeholder="请输入昵称" v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item>
        <a-form-item v-bind="layout" label="状态" >
          <a-radio-group buttonStyle="solid" v-decorator="formDecorator.status" >
            <a-radio-button :value="1">正常</a-radio-button>
            <a-radio-button :value="2">禁用</a-radio-button>
          </a-radio-group>
        </a-form-item>
        <a-form-item v-bind="layout" label="角色" hasFeedback >
          <a-select mode="multiple" :allowClear="true" v-decorator="formDecorator.roles">
            <a-select-option v-for="(role, index) in roleList" :key="index" :value="role.id">{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { getRoleSimpleList, modifyAccount, getAccountDetail } from '@/api/control'
import { SModal } from '@/components'
import pick from 'lodash.pick'

export default {
  name: 'UserModal',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 账户ID
      accountId: 0,
      // 表单描述
      formDecorator: {
        username: ['username'],
        status: ['status'],
        name: ['name', {
          rules: [
            { required: true, message: '昵称不能为空' },
            { max: 20 }
          ]
        }],
        roles: ['roles', {
          rules: [
            { required: true, message: '请给此管理员赋予角色' }
          ]
        }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 18 }
        }
      },
      // 角色集合
      roleList: []
    }
  },
  created () {
    this.loadRoleList()
  },
  components: {
    SModal
  },
  methods: {
    /**
     * 关闭当前窗口
     */
    close () {
      this.confirmLoading = false
      this.visible = false
    },
    /**
     * 显示添加对话框
     * @param {Number} id 管理账户ID
     */
    show (id) {
      this.visible = true
      this.loadAccountDetail(id)
    },
    /**
     *提交后端
     */
    submit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        values.accountId = this.accountId

        this.confirmLoading = true
        modifyAccount(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`修改 ${values.username} 管理员账户成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('修改管理员账户失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    },
    /**
     * 加载管理账户信息
     * @param {Number} id 管理账户ID
     */
    loadAccountDetail (id) {
      this.confirmLoading = true
      getAccountDetail(id).then(res => {
        this.confirmLoading = false
        if (res.status === 200) {
          this.accountId = res.result.id
          this.$nextTick(() => {
            this.form.setFieldsValue(pick(res.result, 'username', 'status', 'name', 'roles'))
          })
        } else {
          this.$refs.modal.error('获取管理账户详情失败', res.message)
        }
      }).catch(() => { this.confirmLoading = false })
    },
    /**
     *加载角色数据
     */
    loadRoleList () {
      this.confirmLoading = true
      getRoleSimpleList().then(res => {
        this.confirmLoading = false
        if (res.status === 200) {
          this.roleList = res.result
        } else {
          this.$refs.modal.error('获取角色失败', res.message)
        }
      }).catch(() => { this.confirmLoading = false })
    }

  }
}
</script>
