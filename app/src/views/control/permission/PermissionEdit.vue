<template>
  <a-modal
    title="编辑导航权限"
    :width="640"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleOk"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限编码" hasFeedback>
          <a-input placeholder="权限编码" :disabled="!isAdd" v-decorator="[ 'id', {rules: [{ required: true, message: '请输入权限编码' }]} ]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限名称" hasFeedback>
          <a-input placeholder="权限名称" v-decorator="['name',{rules: [{ required: true, message: '请输入权限名称' }]}]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="上级权限编码">
          <a-dropdown>
            <span class="ant-dropdown-link" :trigger="['click']">
              点击选择父级
              <a-icon type="down" />
            </span>
            <a-menu slot="overlay">
              <a-sub-menu v-for="(items,index) in permissionAll" :key="index" :title="items.name" @titleClick="nevigateTrigger(index, items.id)">
                <a-menu-item v-for="(list,index1) in items.children" :key="index1" @click="nevigateTrigger(index1, list.id)">{{ list.name }}</a-menu-item>
              </a-sub-menu>
            </a-menu>
          </a-dropdown>
          <!-- <a-select>
            <a-select-option v-for="(item,index) in permissionAll" :key="index">{{ item.name }}</a-select-option>
          </a-select> -->
          您的选择：
          <span>{{ parentId }}</span>
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="状态">
          <a-switch checkedChildren="启用" unCheckedChildren="停用" v-decorator="[ 'enable', { valuePropName: 'checked' }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序">
          <a-input-number :min="1" :max="99" placeholder="排序" v-decorator="['sort',{rules: [{ required: true, message: '请选择排序' }]}]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="描述" hasFeedback>
          <a-textarea :rows="5" placeholder="权限描述" v-decorator="[ 'description', {rules: [{ required: false, message: '请描述权限信息' }] }]" />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
/* eslint-disable */
import { getActionList, getDetails, getMenuDetails, modifyMenu, getPermissionAll, CreateMenu } from '@/api/manage'
import { actionToObject } from '@/utils/permissions'
import pick from 'lodash.pick'

export default {
  name: 'PermissionModal',
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
      visible: false,
      confirmLoading: false,
      mdl: {},
      title:'',
      form: this.$form.createForm(this),
      permissionAll: [],
      menuId: 0,
      parentId: '',
      isAdd: true
    }
  },
  created() {
    this.loadPermissionAll()
  },
  methods: {
    add() {
      this.edit({ id: 0 })
    },
    edit(record) {
      this.menuId = record.id
      if (record.id === 0) {
        this.isAdd = true
        this.visible = true
        this.title='添加导航权限'
        this.$nextTick(() => {
          this.form.setFieldsValue({ enable: true })
        })
      } else {
        this.isAdd = false
        this.visible = true
        this.title='编辑导航权限'
        getMenuDetails(record.id).then(res => {
          if (res.status == 200) {
            this.mdl = Object.assign({}, res.result)
            let actions = []
            if (res.result.actions) {
              for (let i in res.result.actions) {
                actions.push(res.result.actions[i])
              }
            }
            this.parentId = this.mdl.parentId
            this.mdl.opers = actions
            this.$nextTick(() => {
              this.form.setFieldsValue(pick(this.mdl, 'id', 'name', 'enable', 'sort', 'description'))
            })
          } else {
          }
        })
      }
    },
    close() {
      this.form = this.$form.createForm(this)
      this.mdl = {}
      this.parentId = ''
      this.$emit('close')
      this.visible = false
      this.confirmLoading = false
    },
    handleOk() {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          values.parentId = _this.parentId
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              // Do something
              if (_this.isAdd) {
                values.code = values.id
                console.log('IS-add:', values)
                CreateMenu(values).then(res => {
                  if (res.status === 200) {
                    // this.$refs.table.refresh(true)
                    _this.visible = false
                    _this.confirmLoading = false
                    _this.$message.success('提交成功')
                    _this.$emit('ok')
                  } else {
                    this.$notification['error']({
                      message: '错误',
                      description: res.message,
                      duration: 4
                    })
                  }
                })
              } else {
                values.id = this.menuId
                modifyMenu(values).then(res => {
                  if (res.status == 200) {
                    _this.visible = false
                    _this.confirmLoading = false
                    _this.$message.success('提交成功')
                    _this.$emit('ok')
                  } else {
                    _this.confirmLoading = false
                    _this.$message.error(res.message)
                  }
                })
              }
            })
            .catch(() => {
              // Do something
            })
            .finally(() => {
              _this.confirmLoading = false
              _this.close()
            })
        }
      })
    },
    handleCancel() {
      this.close()
    },
    //select选择事件
    nevigateTrigger(index, id) {
      this.parentId = id
    },
    //加载获取所有的权限功能
    loadPermissionAll() {
      getPermissionAll().then(res => {
        this.permissionAll = res.result
      })
    }
  }
}
</script>

<style scoped>
</style>
