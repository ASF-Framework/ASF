<template>
  <s-modal
    ref="modal"
    title="新增管理员"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="昵称" v-bind="layout">
          <a-input placeholder="请输入昵称" v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="用户名" v-bind="layout">
          <a-input placeholder="请输入用于登录的用户名" v-decorator="formDecorator.username" />
        </a-form-item>
        <a-form-item label="登录密码" v-bind="layout">
          <a-input placeholder="请输入登录密码" type="password" v-decorator="formDecorator.password"/>
        </a-form-item>
        <a-form-item label="确认密码" v-bind="layout">
          <a-input placeholder="请再次输入登录密码进行确认" type="password" v-decorator="formDecorator.confirmPassword"/>
        </a-form-item>
        <a-divider/>
        <a-form-item label="赋予角色" v-bind="layout">
          <a-select placeholder="请给此管理员赋予角色" mode="multiple" :allowClear="true" v-decorator="formDecorator.roles">
            <a-select-option v-for="(role, index) in roleList" :key="index" :value="role.id">
              {{ role.name }}
            </a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import md5 from 'md5'
import { SModal } from '@/components'
import { getRoleSimpleList, createAccount } from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 角色集合
      roleList: [],
      // 表单描述
      formDecorator: {
        name: ['name', {
          rules: [
            { required: true, message: '昵称不能为空' },
            { max: 20, message: '昵称字符长度不能大于 20 个字符' }
          ]
        }],
        username: ['username', {
          rules: [
            { required: true, message: '用户名不能为空' },
            { min: 2, message: '用户名字符长度不能少于 2 个字符' },
            { max: 32, message: '用户名字符长度不能大于 32 个字符' }
          ]
        }],
        password: ['password', {
          rules: [
            { required: true, message: '登录密码不能为空' },
            { min: 2, message: '登录密码字符长度不能少于 2 个字符' },
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
  created () {
    this.loadRoleList()
  },
  methods: {
    /**
     *显示添加对话框
     */
    show () {
      this.visible = true
    },
    /**
     * 关闭当前窗口
     */
    close () {
      this.confirmLoading = false
      this.visible = false
    },
    /**
     *提交后端
     */
    submit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        // 登录需要MD5加密
        values.password = md5(values.password)
        values.confirmPassword = ''

        this.confirmLoading = true
        createAccount(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`创建 ${values.username} 管理员账号成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('创建管理员账号失败', res.message)
          }
        }).catch(() => { this.close() })
      })
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
          this.$notification.error({ message: '获取角色失败', description: res.message })
        }
      }).catch(() => { this.confirmLoading = false })
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
