<template>
  <a-drawer
    title="修改角色"
    width="50%"
    :closable="false"
    @close="onClose"
    :destroyOnClose="true"
    :visible="visible">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-bind="layout" label="角色名称" hasFeedback>
          <a-input placeholder="请输入角色名称" v-decorator="formDecorator.name" style="width:200px" />
        </a-form-item>
        <a-form-item v-bind="layout" label="状态">
          <a-switch checkedChildren="启用" unCheckedChildren="禁用" v-decorator="formDecorator.enable" />
        </a-form-item>
        <a-form-item v-bind="layout" label="角色描述" hasFeedback>
          <a-textarea :rows="3" placeholder="请输入角色的描述" v-decorator="formDecorator.description" style="width:50%" />
        </a-form-item>
      </a-form>

      <a-divider> 分配权限</a-divider>
      <role-assign-permissions ref="assign" :defaultValue="defaultPermissions" :createdLoad="false"></role-assign-permissions>
    </a-spin>
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
import pick from 'lodash.pick'
import RoleAssignPermissions from './AssignPermissions'
import { modifyRole, getRoleDetail } from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),

      // 详情数据
      defaultPermissions: [],
      roleId: 0, // 角色id
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
        }],
        enable: [ 'enable',
          { valuePropName: 'checked' }
        ]
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
    /**
     * 显示添加对话框
     * @param {Number} id 角色ID
     */
    show (id) {
      this.roleId = id
      this.visible = true
      this.loadData(id)
    },
    onClose () {
      this.visible = false
      this.confirmLoading = false
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
        values.roleId = this.roleId
        values.permissions = []
        Object.keys(this.$refs.assign.selectValue).forEach((key, index, arr) => {
          this.$refs.assign.selectValue[key].forEach(item => {
            values.permissions.push(item)
          })
        })
        if (values.permissions.length === 0) {
          this.$error({ title: '修改角色失败', content: `必须给 ${values.name} 这个角色分配权限` })
          return
        }
        this.confirmLoading = true
        modifyRole(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$notification.success({ message: `修改 ${values.name} 角色成功` })
            this.complete()
          } else {
            this.$error({ title: '修改角色失败', content: res.message })
          }
        }).catch(() => { this.onClose() })
      })
    },
    /**
     * 加载详情数据
     * @param {Number} id 角色ID
     */
    loadData (id) {
      this.confirmLoading = true
      getRoleDetail(id).then(res => {
        if (res.status === 200) {
          this.defaultPermissions = Object.keys(res.result.permissions)
          this.$refs.assign.loadData()
          this.$nextTick(() => {
            this.form.setFieldsValue(pick(res.result, 'description', 'enable', 'name'))
          })
        } else {
          this.$notification.error({ message: '获取管理账户详情失败', description: res.message })
        }
        this.confirmLoading = false
      }).catch(() => { this.onClose() })
    }
  }
}
</script>
