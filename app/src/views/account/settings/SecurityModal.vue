<template>
    <a-modal
    title="操作"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleOk"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">        
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          :label="lable"
          hasFeedback >
          <a-input :type="inputtype" :placeholder="text" v-decorator="[ property, {rules: [{ required: true, message: text }] }]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="新登陆密码"
          v-if="isModifyPwd"
          hasFeedback >
          <a-input type="password" placeholder="请输入新登陆密码" v-decorator="[ 'password', {rules: [{ required: true, message: '请输入新登陆密码' }] }]" />
        </a-form-item>        
       
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import md5 from 'md5'
import { mapActions } from 'vuex'
import { modifyNameOrAvatar, modifyTelephone, modifyEmail, modifyPassword } from '@/api/manage'
import pick from 'lodash.pick'
export default {
  name: 'SecurityModal',
  data() {
    return {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 5 }
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 16 }
      },
      id: 0,
      lable: '',
      text: '',
      property: '',
      inputtype:'text',
      visible: false,
      confirmLoading: false,
      isModifyPwd: false,
      mdl: {}, //修改对象
      form: this.$form.createForm(this) //表单对象
    }
  },
  created() {},
  methods: {
    ...mapActions(['Logout']),
    //编辑
    edit(record, property, lable, text, isModifyPwd) {
      this.visible = true
      this.mdl = Object.assign({}, record)
      this.mdl.number = this.mdl.telephone==null || this.mdl.telephone==""?"": this.mdl.telephone.substring(this.mdl.telephone.indexOf('+') + 1, this.mdl.telephone.lenght)
      this.isModifyPwd = isModifyPwd
      if(isModifyPwd){
        this.inputtype='password'
      }else{
        this.inputtype='text'
      }
      this.property = property
      this.lable = lable
      this.text = text
      this.id = record.id
      this.$nextTick(() => {
        this.form.setFieldsValue(pick(this.mdl, this.property))
      })
    },
    //关闭方法
    close() {
      this.form = this.$form.createForm(this)
      this.mdl = {}
      this.property = ''
      this.lable = ''
      this.text = ''
      this.$emit('close')
      this.visible = false
    },
    //弹框提交方法
    handleOk() {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          console.log('form->values:', values)
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              //执行方法
              this.checkFunction(values)
            })
            .catch(() => {
              // Do something
            })
            .finally(() => {
              _this.confirmLoading = false
              _this.close()
            })
        } else {
          _this.$error({ title: '错误提示', content: err })
        }
      })
    },
    //弹框关闭
    handleCancel() {
      this.close()
    },
    //修改密码框成功后的跳转的点击方法
    pwdModalClose() {
      const _this = this
      _this.Logout({}).then(() => {
          window.location.reload()
        })
        .catch(err => {
          _this.$message.error({
            title: '错误',
            description: err.message
          })
        })
    },
    //检查修改方法并执行
    checkFunction(values) {
      const _this = this      
      switch (this.property) {
        case 'name':
          modifyNameOrAvatar(values)
            .then(res => {
              console.log(res)
              if (res.status == 200) {
                _this.confirmLoading = false
                _this.$message.success('保存成功')
                _this.$emit('ok')
              } else {
                _this.confirmLoading = false
                _this.$message.error(res.message)
              }
            })
            .catch(error => {
              _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
            })
          break
        case 'number':
          modifyTelephone(values)
            .then(res => {
              if (res.status == 200) {
                _this.confirmLoading = false
                _this.$message.success('保存成功')
                _this.$emit('ok')
              } else {
                _this.confirmLoading = false
                _this.$message.error(res.message)
              }
            })
            .catch(error => {
              _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
            })
          break
        case 'email':
          modifyEmail(values)
            .then(res => {
              if (res.status == 200) {
                _this.confirmLoading = false
                _this.$message.success('保存成功')
                _this.$emit('ok')
              } else {
                _this.confirmLoading = false
                _this.$message.error(res.message)
              }
            })
            .catch(error => {
              _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
            })
          break
        case 'oldPassword':
          values.oldPassword = md5(values.oldPassword)
          values.password = md5(values.password)
          modifyPassword(values)
            .then(res => {
              if (res.status == 200) {
                _this.confirmLoading = false
                this.$success({
                  title: '温馨提示',
                  // JSX support
                  content: (
                    <div>
                      <p>修改密码成功，需要重新登录！</p>
                    </div>
                  ),
                  onOk: this.pwdModalClose
                })
                //_this.$emit('ok')
              } else {
                _this.confirmLoading = false
                _this.$message.error(res.message)
              }
            })
            .catch(error => {
              _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
            })
          break
      }
    }
  }
}
</script>
