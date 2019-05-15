<template>
  <s-modal
    ref="modal"
    title="新增公共 API"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form" >
        <a-form-item label="API 名称" v-bind="layout">
          <a-input placeholder="请输入公共 API 名称 " v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="API 地址" v-bind="layout">
          <a-input placeholder="API 地址" v-decorator="formDecorator.apiTemplate"/>
        </a-form-item>
        <a-form-item label="描述" v-bind="layout" >
          <a-textarea placeholder="请输入公共 API 描述" v-decorator="formDecorator.description">
          </a-textarea>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { SModal } from '@/components'
import { createPublicApi } from '@/api/control'
export default {
  name: 'PublicApiAdd',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 表单描述
      formDecorator: {
        name: ['name', {
          rules: [
            { required: true, message: '名称不能为空' },
            { max: 100 }
          ]
        }],
        apiTemplate: ['apiTemplate',
          {
            rules: [
              { required: true, message: 'API 地址不能为空' },
              { max: 500 }
            ]
          }],
        description: ['description', {
          rules: [
            { required: true, message: '描述不能为空' },
            { max: 200, message: '描述不能超过 200 个字符' }
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
        this.confirmLoading = true
        createPublicApi(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`创建 ${values.name} 公共 API 成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('创建失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
