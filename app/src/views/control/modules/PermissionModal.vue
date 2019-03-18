<template>
  <a-modal title="编辑导航权限" :width="640" :visible="visible" :confirmLoading="confirmLoading" @ok="handleOk" @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限编码" hasFeedback>
          <a-input placeholder="权限编码" disabled="disabled" v-decorator="[ 'id', {rules: []} ]" />
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
          <span>{{parentId}}</span>
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="状态">
          <a-switch checkedChildren="启用" unCheckedChildren="停用" v-decorator="[ 'enable', { valuePropName: 'checked' }]" />
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
import { getActionList, getDetails, getMenuDetails, modifyMenu, getPermissionAll } from '@/api/manage'
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

      form: this.$form.createForm(this),
      permissionAll: [],
      menuId: 0,
      parentId: ''
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
          this.visible = true
          this.$nextTick(() => {
            this.form.setFieldsValue(pick(this.mdl, 'id', 'name', 'enable', 'description'))
          })
        } else {
        }
      })
    },
    close() {
      this.$emit('close')
      this.visible = false
    },
    handleOk() {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          values.id = this.menuId
          values.parentId = this.parentId
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              // Do something
              modifyMenu(values).then(res=>{
                  if(res.status==200){
                      this.visible = false
                      this.confirmLoading=false
                      _this.$message.success('提交成功')
                      _this.$emit('ok')
                  }else{
                      this.confirmLoading=false
                      this.$message.error(res.message)
                  }
              })
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
