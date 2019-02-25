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
          label="角色名称"
          hasFeedback >
          <a-input placeholder="角色名称" v-decorator="[ 'name', {rules: [{ required: true, message: '请输入角色名称' }] }]" />
        </a-form-item>      
        <!-- <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态"
          hasFeedback >
          <a-select v-decorator="[ 'status', {rules: []} ]">
            <a-select-option :value="1">正常</a-select-option>
            <a-select-option :value="2">禁用</a-select-option>
          </a-select>
        </a-form-item> -->

        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="描述"
          hasFeedback
        >
          <a-textarea :rows="5" placeholder="角色描述" v-decorator="[ 'description', {rules: [{ required: false, message: '请描述角色信息' }] }]"  />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="拥有权限"
          :colon="false"
          hasFeedback
        ></a-form-item>        
        <a-divider style="margin:0"/>      
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol"  style="margin-left:135px">
           <a-row :gutter="18"  v-for="(permission, index) in permissions" :key="index">
            <a-col :span="6">
              {{ permission.name }}：
            </a-col>
            <a-col :span="18">
              <a-checkbox
                :indeterminate="permission.indeterminate"
                :checked="permission.checkedAll"
                @change="onChangeCheckAll($event, permission, index)" >
                全选
              </a-checkbox>
              <a-checkbox-group :options="permission.actionsOptions" v-model="permission.selected" @change="onChangeCheck($event,permission,index)" />
            </a-col>
          </a-row>
        </a-form-item>
       
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { getPermissionAll,addRole,editRole,getRoleDetail } from '@/api/manage'
import { actionToObject } from '@/utils/permissions'
import pick from 'lodash.pick'

export default {
  name: 'RoleModal',
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
      //form:null,
      form: this.$form.createForm(this),
      permissions: [],
      checkpermissions:[],
      isAdd:false,
      roleId:0,
      tagList:[],
    }
  },
  created () {
    this.loadPermissions()
  },
  methods: {
    add () {
      this.edit({ id: 0 })
    },
    edit (record) {
       this.visible = true
      if(record.id==0){
        this.isAdd=true
        this.$nextTick(() => {
          this.form.setFieldsValue(pick(this.mdl,'name', 'description'))
      })
      }else{
         this.isAdd=false
         getRoleDetail(record.id).then(res=>{
           console.log(res)
            if(res.status==200){
                this.mdl = Object.assign({}, res.result)            
                this.roleId=record.id 
                 // 有权限表，处理勾选
                if (this.mdl.permissions && this.permissions) {
                  // 先处理要勾选的权限结构
                  let permissionsAction = {}

                  permissionsAction=this.loadOptions(this.mdl.permissions)
                  //console.log("permissionsAction:",permissionsAction)
                  // this.mdl.permissions.forEach(permission => {
                  //   console.log(permission)
                  //   permissionsAction[permission.permissionId] = permission.actionEntitySet.map(entity => entity.action)
                  // })
                  // 把权限表遍历一遍，设定要勾选的权限 action
                  this.permissions.forEach(permission => {
                    //console.log("permission",permission)
                    //permission.selected = permissionsAction[permission.id]
                  })
                }              
                // console.log("edit--this.mdl:",this.mdl)
                // console.log("edit--this.permissions:",this.permissions) 
                this.$nextTick(() => {
                  this.form.setFieldsValue(pick(this.mdl,'name', 'description'))
                })
            }else{
              this.$message.success(res.message)
            }
        })      
      }
      
     
     
      
    },
    close () {
      this.form=this.$form.createForm(this)
      this.mdl={}
      this.tagList=[]
      this.loadPermissions()
      this.$emit('close')
      this.visible = false
    },
    handleOk () {
      const _this = this
      console.log(this.form)
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
           values.roleId=this.roleId
           this.loadCheck()
          values.permissions=this.checkpermissions
          console.log('form values', values)
           _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise((resolve) => {
            setTimeout(() => resolve(), 2000)
          }).then(() => {
            // Do something
            console.log('form add/edit',)
            if(this.isAdd){
              console.log('form add')
              addRole(values).then(res=>{
                if(res.status==200){
                
                  _this.confirmLoading = false
                  _this.$message.success('保存成功')
                  _this.$emit('ok')
                }else{
                  _this.confirmLoading = false
                  _this.$message.success(res.message)
                }
              })
            }else{
              console.log('form edit')
              editRole(values).then(res=>{
                if(res.status==200){
                
                  _this.confirmLoading = false
                  _this.$message.success('保存成功')
                  _this.$emit('ok')
                }else{
                  _this.confirmLoading = false
                  _this.$message.success(res.message)
                }
              })
            }
          
          }).catch(() => {
            // Do something
          }).finally(() => {
            _this.confirmLoading = false
            _this.close()
          })
        }else{
          this.tagList=[]
          this.loadPermissions()
        }
      })
    },
    handleCancel () {
      this.close()
    },
    //子级复选框check事件
    onChangeCheck (e,permission,index) {
      permission.indeterminate = !!permission.selected.length && (permission.selected.length < permission.actionsOptions.length)
      permission.checkedAll = permission.selected.length === permission.actionsOptions.length
      // 选择后的权限结果集
      if(!this.tagList.includes(permission)){
        this.tagList.push(permission)    
      }      
      if(!permission.indeterminate){
        if(this.tagList.length!==0){
          var index1 = this.tagList.indexOf(permission);
          this.tagList.splice(index1, 1);
        }
      }     
      
       console.log("tagList---------",this.tagList)
    },
    //全选复选框check事件
    onChangeCheckAll (e, permission,index) {
      const checkPermission= Object.assign(permission, {
        selected: e.target.checked ? permission.actionsOptions.map(obj => obj.value) : [],
        indeterminate: false,
        checkedAll:e.target.checked
        
      })
       if(!this.tagList.includes(permission)){
        this.tagList.push(permission)    
      }    
      if(!checkPermission.checkedAll){
        if(this.tagList.length!==0){
          var index1 = this.tagList.indexOf(permission);
          this.tagList.splice(index1, 1);
        }
      }      
      console.log("tagList---------",this.tagList)
    },
    loadCheck(){
      this.tagList.forEach(ele=>{
        this.checkpermissions.push(ele.id)
        ele.selected.forEach(sel=>{
          this.checkpermissions.push(sel)
        })
      })
    },
    loadPermissions () {
      getPermissionAll().then(res => {
        console.log("权限：",res.result)
        const result = res.result
        this.permissions = result.map(permission => {
          permission.checkedAll = false
          permission.selected = []
          permission.indeterminate = false
          permission.actionsOptions=this.loadOptions(permission.actions)
          return permission
        })
      })
    },
    loadOptions(actions){
      const options=[]
      for(var key in actions){
               options.push({
              label: actions[key],
              value: key
            })
      }
      return options
    }

  }
}
</script>

<style scoped>

</style>
