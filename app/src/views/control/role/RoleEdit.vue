<template>
  <a-modal
    title="操作"
    :width="800"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleOk"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="角色名称"
          hasFeedback>
          <a-input
            placeholder="角色名称"
            v-decorator="[ 'name', {rules: [{ required: true, message: '请输入角色名称' }] }]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="状态">
          <a-switch
            checkedChildren="启用"
            unCheckedChildren="禁用"
            v-decorator="[ 'enable', { valuePropName: 'checked' }]" />
        </a-form-item>

        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="描述"
          hasFeedback>
          <a-textarea
            :rows="5"
            placeholder="角色描述"
            v-decorator="[ 'description', {rules: [{ required: false, message: '请描述角色信息' }] }]" />
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          :colon="false"
          hasFeedback>
          <span slot="label">
            拥有权限&nbsp;
            <a-tooltip>
              <a-icon type="check-square" />
            </a-tooltip>
          </span>
        </a-form-item>
        <a-divider style="margin:0" />

        <a-row
          :gutter="16"
          class="row"
          v-for="(permission, index) in permissions"
          :key="index"
          type="flex"
          justify="center">
          <a-col
            :span="6"
            class="col-text-right col-backgroud-color1">
            {{ permission.name }}：
          </a-col>
          <a-col
            :span="18"
            class="col-backgroud-color2">
            <a-checkbox
              :indeterminate="permission.indeterminate"
              :checked="permission.checkedAll"
              @change="onChangeCheckAll($event, permission, index)">
              全选
            </a-checkbox>
            <a-checkbox-group
              :options="permission.actionsOptions"
              v-model="permission.selected"
              @change="onChangeCheck($event,permission,index)" />
          </a-col>
        </a-row>

      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { getPermissionAll, addRole, editRole, getRoleDetail } from '@/api/manage'
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
      isChecked: false,
      visible: false, // 弹框是否显示
      confirmLoading: false, // 弹框中的提交按钮是否加载中
      mdl: {}, // 修改对象
      form: this.$form.createForm(this), // 表单对象
      permissions: [], // 全部权限集合
      checkpermissions: [], // 选择后的权限集合
      isAdd: false, // 是否添加
      roleId: 0, // 角色ID
      tagList: [], // checkbox选择后的权限集合（中间键）
      actionKeys: [] // 编辑对象的权限集
    }
  },
  created () {
    this.loadPermissions()
  },
  methods: {
    // 添加
    add () {
      this.edit({ id: 0 })
    },
    // 编辑
    edit (record) {
      this.visible = true
      if (record.id === 0) {
        this.isChecked = true
        this.isAdd = true
        this.$nextTick(() => {
          this.form.setFieldsValue({ enable: true })
        })
      } else {
        this.isAdd = false
        getRoleDetail(record.id)
          .then(res => {
            if (res.status === 200) {
              this.mdl = Object.assign({}, res.result)
              this.roleId = record.id
              // 有权限表，处理勾选
              if (this.mdl.permissions && this.permissions) {
                // console.log(this.permissions)
                // 把权限表遍历一遍，设定要勾选的权限 action
                this.permissions.forEach(permission => {
                  for (const key in this.mdl.permissions) {
                    if (Object.keys(permission.actions).includes(key)) {
                      permission.selected.push(key)
                      permission.indeterminate = true
                    }
                  }
                })
              }
              this.$nextTick(() => {
                this.form.setFieldsValue(pick(this.mdl, 'name', 'enable', 'description'))
              })
            } else {
              this.$message.success(res.message)
            }
          })
          .catch(() => {})
      }
    },
    // 关闭方法
    close () {
      this.form = this.$form.createForm(this)
      this.mdl = {}
      this.tagList = []
      this.actionKeys = []
      this.checkpermissions = []
      this.loadPermissions()
      this.$emit('close')
      this.visible = false
      this.isChecked = false
    },
    // 弹框提交方法
    handleOk () {
      const _this = this
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          values.roleId = this.roleId
          this.loadCheck()
          values.permissions = this.checkpermissions
          if (!this.checkHaveRoles(values)) {
            return false
          }
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              // Do something
              if (this.isAdd) {
                addRole(values)
                  .then(res => {
                    if (res.status === 200) {
                      _this.confirmLoading = false
                      this.checkpermissions = []
                      _this.$message.success('保存成功')
                      _this.$emit('ok')
                    } else {
                      _this.confirmLoading = false
                      _this.$message.error(res.message)
                    }
                  }, error => {
                    if (error) {
                      _this.$message.error('服务器超时，请重新再试。')
                    }
                  })
              } else {
                editRole(values)
                  .then(res => {
                    if (res.status === 200) {
                      _this.confirmLoading = false
                      this.checkpermissions = []
                      _this.$message.success('保存成功')
                      _this.$emit('ok')
                    } else {
                      _this.confirmLoading = false
                      _this.$message.error(res.message)
                    }
                  }, error => {
                    if (error) {
                      _this.$error({ title: '错误提示', content: '服务器超时，请重新再试。' })
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
        } else {
          this.tagList = []
          this.loadPermissions()
        }
      })
    },
    // 检测是否选择了权限（添加/编辑的时候）
    checkHaveRoles (values) {
      if (this.isAdd) {
        if (values.permissions.length <= 0) {
          this.$message.warning('请给角色赋予权限，请勾选权限')
          return false
        } else {
          return true
        }
      } else {
        if (this.mdl && Object.keys(this.mdl.permissions).length === 0 && values.permissions.length <= 0) {
          this.$message.warning('请给角色赋予权限，请勾选权限')
          return false
        } else {
          return true
        }
      }
    },
    // 弹框关闭
    handleCancel () {
      this.close()
    },
    // 子级复选框check事件
    onChangeCheck (e, permission, index) {
      permission.indeterminate =
        !!permission.selected.length && permission.selected.length < permission.actionsOptions.length
      permission.checkedAll = permission.selected.length === permission.actionsOptions.length
    },
    // 全选复选框check事件
    onChangeCheckAll (e, permission, index) {
      Object.assign(permission, {
        selected: e.target.checked ? permission.actionsOptions.map(obj => obj.value) : [],
        indeterminate: false,
        checkedAll: e.target.checked
      })
    },
    // 选择后的权限
    loadCheck () {
      this.permissions.forEach(ele => {
        ele.selected.forEach(sel => {
          this.checkpermissions.push(sel)
        })
      })
    },
    // 加载全部页面权限
    loadPermissions () {
      getPermissionAll().then(res => {
        const result = res.result
        this.permissions = result.map(permission => {
          permission.checkedAll = false
          permission.selected = []
          permission.indeterminate = false
          permission.actionsOptions = this.loadOptions(permission.actions)
          return permission
        })
        // 移除没有子权限的页面权限（用途：筛选仪表盘、控制面板等顶级权限不显示于添加和编辑中）
        for (const i in this.permissions) {
          if (Object.keys(this.permissions[i].actions).length === 0) {
            this.permissions.splice(i, 1)
          }
        }
      })
    },
    // 遍历键值对，返回包含了key与value为值的对象集
    loadOptions (actions) {
      const options = []
      for (var key in actions) {
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
.row {
  padding: 10px;
}
.col-text-right {
  text-align: right;
}
/* .col-backgroud-color1{
   background: rgba(0, 160, 233, 0.7);
}
.col-backgroud-color2{
   background: #00a0e9;
} */
</style>
