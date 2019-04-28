<template>
  <s-modal
    ref="modal"
    title="重置管理员密码"
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
        <a-form-item label="管理员密码" v-bind="layout">
          <a-input placeholder="请输入您的账户登录密码" type="password" v-decorator="formDecorator.adminPassword"/>
        </a-form-item>
        <a-form-item label="登录密码" v-bind="layout">
          <a-input placeholder="请输入登录密码" type="password" v-decorator="formDecorator.password"/>
        </a-form-item>
        <a-form-item label="确认密码" v-bind="layout">
          <a-input placeholder="请再次输入登录密码进行确认" type="password" v-decorator="formDecorator.confirmPassword"/>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>
<script>
import md5 from 'md5'
import { SModal } from '@/components'
import { resetPassword } from '@/api/control'
export default {
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
        adminPassword: ['adminPassword', {
          rules: [
            { required: true, message: '管理员密码不能为空' },
            { min: 2, message: '管理员密码字符长度不能少于 6 个字符' },
            { max: 32, message: '管理员密码字符长度不能大于 32 个字符' }
          ]
        }],
        password: ['password', {
          rules: [
            { required: true, message: '登录密码不能为空' },
            { min: 2, message: '登录密码字符长度不能少于 6 个字符' },
            { max: 32, message: '登录密码字符长度不能大于 32 个字符' }
          ]
        }],
        confirmPassword: ['confirmPassword', {
          rules: [
            { required: true, message: '确认密码不能为空' },
            { validator: this.compareConfirmPassword }
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
      }
    }
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
     * @param {Number} id 管理员账户ID
     * @param {String} username 用户名
     */
    show (id, username) {
      this.visible = true
      this.accountId = id
      this.$nextTick(() => {
        this.form.setFieldsValue({ username: username })
      })
    },
    /**
     *提交后端
     */
    submit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        values.id = this.accountId
        values.password = md5(values.password)
        values.adminPassword = md5(values.adminPassword)
        values.confirmPassword = ''

        this.confirmLoading = true
        resetPassword(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`重置 ${values.username} 账户密码成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('修改账户密码失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    },
    /**
     * 验证确认密码是否一致
     */
    compareConfirmPassword  (rule, value, callback) {
      const form = this.form
      if (value && value !== form.getFieldValue('password')) {
        // eslint-disable-next-line standard/no-callback-literal
        callback('您输入的两个密码不一致!')
      } else {
        callback()
      }
    }
  }
}
</script>
