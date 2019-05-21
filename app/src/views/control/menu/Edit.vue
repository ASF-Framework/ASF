<template>
  <s-modal
    ref="modal"
    title="编辑菜单"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="550"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form" >
        <a-form-item label="菜单标识" v-bind="layout">
          <a-input disabled :value="menuCode" style="width: 60%"/>
        </a-form-item>
        <a-form-item label="菜单名称" v-bind="layout" hasFeedback>
          <a-input placeholder="请输入菜单的名称 " v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="菜单图标" v-bind="layout" >
          <a-input placeholder="请输入菜单的图标 " v-decorator="formDecorator.icon" @change="e=>{this.iconType=e.target.value}" style="width: 40%"/>
          <a-icon :type="iconType" style="font-size: 20px; margin-left: 5px;"></a-icon>
        </a-form-item>
        <a-form-item label="父级菜单" v-bind="layout" hasFeedback>
          <menu-tree-select :allowClear="true" placeholder="请选择父级菜单，不选默认顶级菜单" v-decorator="formDecorator.parentId"></menu-tree-select>
        </a-form-item>
        <a-form-item label="状态" v-bind="layout">
          <a-switch checkedChildren="启用" unCheckedChildren="禁用" v-decorator="formDecorator.enable"/>
        </a-form-item>
        <a-form-item label="是否隐藏" v-bind="layout">
          <a-switch checkedChildren="显示" unCheckedChildren="隐藏" v-decorator="formDecorator.hidden"/>
        </a-form-item>
        <a-form-item label="跳转地址" v-bind="layout" hasFeedback>
          <a-input placeholder="请输入菜单的跳转地址" v-decorator="formDecorator.redirect" />
        </a-form-item>
        <a-form-item label="菜单描述" v-bind="layout" hasFeedback>
          <a-textarea placeholder="请输入菜单的描述" v-decorator="formDecorator.description">
          </a-textarea>
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { SModal } from '@/components'
import { modifyMenu } from '@/api/control'
import MenuTreeSelect from './modules/MenuTreeSelect'
import pick from 'lodash.pick'

export default {
  name: 'MenuEdit',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),

      iconType: '',
      menuCode: '',
      menuId: 0,
      // 表单描述
      formDecorator: {
        hidden: ['show', { initialValue: false, valuePropName: 'checked' }],
        icon: ['icon'],
        enable: ['enable', { initialValue: true, valuePropName: 'checked' }],
        parentId: ['parentId'],
        name: ['name', {
          rules: [
            { required: true, message: '菜单名称不能为空' },
            { max: 100 }
          ]
        }],
        redirect: ['redirect', { rules: [ { max: 300 } ] }],
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
    SModal, MenuTreeSelect
  },
  methods: {
    /**
     *显示添加对话框
     */
    show (data) {
      this.visible = true
      this.menuId = data.id
      this.menuCode = data.code
      this.iconType = data.icon
      this.$nextTick(() => {
        data.show = !data.hidden
        this.form.setFieldsValue(pick(data, 'show', 'icon', 'enable', 'parentId', 'name', 'redirect', 'description'))
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
        values.id = this.menuId
        values.hidden = !values.show
        this.confirmLoading = true
        modifyMenu(values).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success(`修改 ${values.name} 菜单成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('修改菜单失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
