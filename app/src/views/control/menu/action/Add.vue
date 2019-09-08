<template>
  <s-modal
    ref="modal"
    title="新增操作功能"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="550"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form" >
        <a-form-item label="父级菜单" v-bind="layout">
          <a-tag color="#108ee9">{{ menuId }}</a-tag> <span>{{ menuName }}</span>
        </a-form-item>
        <a-form-item label="功能标识" v-bind="layout">
          <a-input placeholder="请输入功能的标识，如 “add” " v-decorator="formDecorator.code" style="width: 60%"/>
        </a-form-item>
        <a-form-item label="功能名称" v-bind="layout">
          <a-input placeholder="请输入功能的名称 " v-decorator="formDecorator.name" style="width: 60%"/>
        </a-form-item>
        <a-form-item label="是否记录日志" v-bind="layout">
          <a-switch checkedChildren="记录" unCheckedChildren="不记录" v-decorator="formDecorator.isLogger"/>
        </a-form-item>
        <a-form-item label="API 地址" v-bind="layout">
          <a-input placeholder="请输入功能请求的 API 地址，动态参数通过正则表达式匹配" v-decorator="formDecorator.apiTemplate" />
        </a-form-item>
        <a-form-item label="功能描述" v-bind="layout" >
          <a-textarea placeholder="请输入功能的描述" v-decorator="formDecorator.description">
          </a-textarea>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { SModal } from '@/components'
import { createAction } from '@/api/control'
export default {
  name: 'ActionAdd',
  components: { SModal },
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),

      menuId: '',
      menuName: '',
      // 表单描述
      formDecorator: {
        isLogger: ['isLogger', { initialValue: true, valuePropName: 'checked' }],
        code: ['code', {
          rules: [
            { required: true, message: '功能标识不能为空' },
            { max: 30 }
          ]
        }],
        name: ['name', {
          rules: [
            { required: true, message: '功能名称不能为空' },
            { max: 100 }
          ]
        }],
        apiTemplate: ['apiTemplate', {
          rules: [
            { required: true, message: '功能请求的 API 地址不能为空' },
            { max: 500 }
          ]
        }],
        description: ['description', {
          rules: [
            { required: true, message: '功能不能为空' },
            { max: 200, message: '功能不能超过 200 个字符' }
          ]
        }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 19 }
        }
      }
    }
  },

  methods: {
    /**
     *显示添加对话框
     */
    show (menuId, menuName) {
      this.menuId = menuId
      this.menuName = menuName
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
        this.confirmLoading = true
        values.parentId = this.menuId
        createAction(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`创建 ${values.name} 操作功能成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('创建操作功能失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
