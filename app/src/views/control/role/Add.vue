<template>
  <a-drawer
    title="新增角色"
    width="50%"
    :closable="false"
    @close="onClose"
    :destroyOnClose="true"
    :visible="visible">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-bind="layout" label="角色名称" >
          <a-input placeholder="请输入角色名称" v-decorator="formDecorator.name" style="width:200px" />
        </a-form-item>
        <a-form-item v-bind="layout" label="角色描述" >
          <a-textarea :rows="3" placeholder="请输入角色的描述" v-decorator="formDecorator.description" style="width:50%" />
        </a-form-item>
      </a-form>
    </a-spin>
    <a-divider> 分配权限</a-divider>
    <role-assign-permissions ref="assign"></role-assign-permissions>
    <div
      :style="{
        position: 'absolute',
        left: 0,
        bottom: 0,
        width: '100%',
        borderTop: '1px solid #e9e9e9',
        padding: '10px 16px',
        background: '#fff',
        textAlign: 'right',
      }" >
      <a-button
        :style="{marginRight: '8px'}"
        @click="onClose" >
        取消
      </a-button>
      <a-button @click="onSubmit" type="primary">确定</a-button>
    </div>
  </a-drawer>
</template>
<script>
import RoleAssignPermissions from './AssignPermissions'
import { createRole } from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 表单描述
      formDecorator: {
        name: [ 'name', {
          rules: [
            { required: true, message: '角色名称不能为空' },
            { max: 20, message: '角色名称字符长度不能大于 20 个字符' }
          ]
        }],
        description: [ 'description', {
          rules: [
            { required: true, message: '角色描述不能为空' },
            { max: 200, message: '角色描述字符长度不能大于 200 个字符' }
          ]
        }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: { span: 4 }
        },
        wrapperCol: {
          xs: { span: 20 }
        }
      }
    }
  },
  components: {
    RoleAssignPermissions
  },
  methods: {
    show () {
      this.visible = true
    },
    onClose () {
      this.visible = false
    },
    /**
     *处理完成
     */
    complete () {
      this.onClose()
      this.$emit('complete')
    },
    /**
     *提交后端
     */
    onSubmit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        values.permissions = []
        Object.keys(this.$refs.assign.selectValue).forEach((key, index, arr) => {
          this.$refs.assign.selectValue[key].forEach(item => {
            values.permissions.push(item)
          })
        })
        if (values.permissions.length === 0) {
          this.$error({ title: '创建角色失败', content: `必须给 ${values.name} 这个角色分配权限` })
          return
        }
        this.confirmLoading = true
        createRole(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$notification.success({ message: `创建 ${values.name} 角色成功` })
            this.complete()
          } else {
            this.$error({ title: '创建角色失败', content: res.message })
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
