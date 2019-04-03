<template>
  <a-modal
    :title="title"
    :width="640"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleOk"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="父级权限" >
          <a-input placeholder="父级权限" disabled="disabled" v-decorator="[ 'parentId', {rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="标识" >
          <a-input placeholder="标识" disabled="disabled" v-decorator="[ 'id', {rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="API地址模板" hasFeedback validateStatus="success">
          <a-input placeholder="请输入API地址模板" v-decorator="[ 'apiTemplate', {rules: [{ required: true, message: '请输入API地址' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限名称" hasFeedback validateStatus="success">
          <a-input placeholder="权限名称" v-decorator="[ 'name', {rules: [{ required: true, message: '请输入权限名称' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="添加时间" >
          <a-input placeholder="添加时间" disabled="disabled" v-decorator="[ 'createTime', {rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="系统权限">
          <a-switch checkedChildren="是" unCheckedChildren="否" :disabled="true" v-decorator="[ 'isSystem', { valuePropName: 'checked' }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="记录日志">
          <a-switch checkedChildren="是" unCheckedChildren="否" v-decorator="[ 'isLogger', { valuePropName: 'checked' }]"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否启用">
          <a-switch checkedChildren="是" unCheckedChildren="否" v-decorator="[ 'enable', { valuePropName: 'checked' }]" />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'
import { modifyAction, getActionDetails } from '@/api/manage'
import pick from 'lodash.pick'
export default {
  name: 'ActionEdit',
  data () {
    return {
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
      title: '编辑',
      visible: false, // 弹框是否显示
      confirmLoading: false, // 弹框中的提交按钮是否加载中
      mdl: {}, // 修改对象
      form: this.$form.createForm(this), // 表单对象
      actionKey: ''
    }
  },
  created () {},
  methods: {
    // 编辑
    edit (action, key) {
      this.visible = true
      this.title = '编辑'
      getActionDetails(key).then(res => {
        if (res.status === 200) {
          this.actionKey = key
          console.log('res', res.result)
          this.mdl = Object.assign({}, res.result)
          this.mdl.createTime = this.formatTimespan(this.mdl.createTime)
          this.$nextTick(() => {
            this.form.setFieldsValue(pick(this.mdl, 'parentId', 'id', 'apiTemplate', 'isSystem', 'name', 'isLogger', 'enable', 'createTime'))
          })
        } else {

        }
      })
    },
    // 关闭方法
    close () {
      this.form = this.$form.createForm(this)
      this.mdl = {}
      this.$emit('close')
      this.visible = false
    },
    // 弹框提交方法
    handleOk () {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              // Do something

              values.id = this.actionKey
              console.log(values)
              // 修改
              modifyAction(values).then(
                res => {
                  if (res.status === 200) {
                    _this.confirmLoading = false
                    _this.$message.success('保存成功')
                    _this.$emit('ok')
                  } else {
                    _this.confirmLoading = false
                    _this.$message.error(res.message)
                  }
                },
                error => {
                  if (error) {
                    _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
                  }
                }
              )
            })
            .catch(() => {
              // Do something
            })
            .finally(() => {
              _this.confirmLoading = false
              _this.close()
            })
        } else {
        }
      })
    },
    // 弹框关闭
    handleCancel () {
      this.close()
    },
    // 格式化时间戳
    formatTimespan (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
      if (!dataStr) return ''
      const date = dataStr.toString().length
      if (date === 10) {
        return moment(dataStr * 1000).format(pattern)
      } else {
        return moment(dataStr).format(pattern)
      }
    }
  }
}
</script>

<style scoped>
.row {
  padding: 10px;
}
.col-text-right {
  text-align: right;
}
/* .col-backgroud-color1{
       background: rgba(0, 160, 233, 0.7);
    }
    .col-backgroud-color2{
       background: #00a0e9;
    } */
</style>
