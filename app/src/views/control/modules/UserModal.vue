<template>
  <a-modal
    title="操作"
    :width="800"
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
          label="登录名"
          hasFeedback
        >
          <a-input placeholder="登录名" disabled="disabled" v-decorator="[ 'username', {rules: []} ]"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="昵称"       
          hasFeedback    
        >
          <a-input placeholder="昵称"  v-decorator="['name',{rules: [{ required: true, message: '请输入昵称' }]}]"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态"        
          hasFeedback   
        >
          <a-select  v-decorator="['status',{rules: [{ required: true, message: '请选择状态' }]}]">
            <a-select-option :value="1">正常</a-select-option>
            <a-select-option :value="2">禁用</a-select-option>
          </a-select>
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="赋予角色" hasFeedback>
          <a-select style="width: 100%" mode="multiple" :allowClear="true"   v-decorator="['roles',{rules: [{ required: true, message: '请赋予角色' }]}]">
            <a-select-option
              v-for="(role, index) in roleList"
              :key="index"
              :value="role.id"
            >{{ role.name }}</a-select-option>
          </a-select>
        </a-form-item>
        
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { getPermissions ,getRoleListAll,modifyAccount,getAccountDetail} from '@/api/manage'
import { actionToObject } from '@/utils/permissions'
import pick from 'lodash.pick'

export default {
  name: 'UserModal',
  data () {
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
      permissions: [],
      roleList:[]
    }
  },
  created () {
    this.loadPermissions()
    this.loadRoles()
  },
  methods: {
    add () {
      this.edit({ id: 0 })
    },
    edit (record) {
        console.log('record:', record)
        getAccountDetail(record.id).then(res=>{

            this.mdl = Object.assign({}, res.result)
     
      this.visible = true
      this.$nextTick(() => {
        this.form.setFieldsValue(pick(this.mdl, 'username', 'name', 'status', 'roles'))
      })
      console.log('this.mdl', this.mdl)
        })
      
    },
    close () {
      this.$emit('close')
      this.visible = false
    },
    handleOk () {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          console.log('form values', values)

          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise((resolve) => {
            setTimeout(() => resolve(), 2000)
          }).then(() => {
            // Do something
            modifyAccount(values).then(res=>{
                console.log('form res', res)
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
           
          }).catch(() => {
            // Do something
          }).finally(() => {
            _this.confirmLoading = false
            _this.close()
          })
        }
      })
    },
    handleCancel () {
      this.close()
    },
    onChangeCheck (permission) {
      permission.indeterminate = !!permission.selected.length && (permission.selected.length < permission.actionsOptions.length)
      permission.checkedAll = permission.selected.length === permission.actionsOptions.length
    },
    onChangeCheckAll (e, permission) {
      Object.assign(permission, {
        selected: e.target.checked ? permission.actionsOptions.map(obj => obj.value) : [],
        indeterminate: false,
        checkedAll: e.target.checked
      })
    },
    loadPermissions () {
      getPermissions().then(res => {
        const result = res.result
        this.permissions = result.map(permission => {
          const options = actionToObject(permission.actionData)
          permission.checkedAll = false
          permission.selected = []
          permission.indeterminate = false
          permission.actionsOptions = options.map(option => {
            return {
              label: option.describe,
              value: option.action
            }
          })
          return permission
        })
      })
    },
    //加载角色
    loadRoles(){
     getRoleListAll().then(res => {      
        this.roleList = res.result        
      })
    }

  }
}
</script>

<style scoped>

</style>
