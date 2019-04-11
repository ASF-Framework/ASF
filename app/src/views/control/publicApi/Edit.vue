<template>
  <s-modal
    ref="modal"
    title="编辑公共 API"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form" >
        <a-form-item label="编号" v-bind="layout" hasFeedback>
          <a-input disabled v-decorator="formDecorator.id" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="API 名称" v-bind="layout" hasFeedback>
          <a-input placeholder="请输入公共 API 名称 " v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="API 地址" v-bind="layout" hasFeedback>
          <a-input placeholder="API 地址" v-decorator="formDecorator.apiTemplate"/>
        </a-form-item>
        <a-form-item label="状态" v-bind="layout">
          <a-switch checkedChildren="启用" unCheckedChildren="停止" v-decorator="formDecorator.enable"/>
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
import pick from 'lodash.pick'
import { modifyPublicApi } from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this), // 表单描述
      formDecorator: {
        id: ['id'],
        enable: ['enable', { valuePropName: 'checked' }],
        name: ['name', {
          rules: [
            { required: true, message: '公共 API 名称不能为空' },
            { max: 100 }
          ]
        }],
        apiTemplate: ['apiTemplate',
          {
            rules: [
              { required: true, message: '公共 API 地址不能为空' },
              { max: 500 }
            ]
          }],
        description: ['description', {
          rules: [
            { max: 200, message: '公共 API 描述不能超过 200 个字符' }
          ]
        }]
      },
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
     * 显示添加对话框
     * @param {object} data 编辑数据
     */
    show (data) {
      this.visible = true
      this.$nextTick(() => {
        this.form.setFieldsValue(pick(data, 'id', 'name', 'apiTemplate', 'enable', 'description'))
      })
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
        modifyPublicApi(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`修改公共API 成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('修改失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
