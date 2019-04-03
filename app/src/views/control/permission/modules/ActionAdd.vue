<template>
  <a-modal
    :title="title"
    :width="640"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleOk"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限编码" hasFeedback >
          <a-input placeholder="权限编码" v-decorator="[ 'code', {rules: [{ required: true, message: '请输入权限编码' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限名称" hasFeedback >
          <a-input placeholder="权限名称" v-decorator="[ 'name', {rules: [{ required: true, message: '请输入权限名称' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="上级权限编码">
          <a-dropdown>
            <span class="ant-dropdown-link" :trigger="['click']" href="#">
              点击选择父级
              <a-icon type="down" />
            </span>
            <a-menu slot="overlay">
              <a-sub-menu v-for="(items,index) in dataLoad.result" :key="index" :title="items.name">
                <a-menu-item v-for="(list,index1) in items.children" :key="index1" @click="actionTrigger(index1, list.id)">{{ list.name }}</a-menu-item>
              </a-sub-menu>
            </a-menu>
          </a-dropdown>
          您的选择：{{ parentId }}
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="权限服务地址" hasFeedback>
          <a-input placeholder="权限服务地址" v-decorator="[ 'apiTemplate', {rules: [{ required: true, message: '请输入API地址' }] }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否日志记录">
          <a-switch checkedChildren="是" unCheckedChildren="否" v-decorator="[ 'isLogger', { valuePropName: 'checked' }]" />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序" hasFeedback>
          <a-input-number :min="1" :max="99" v-decorator="[ 'sort', {rules: [{ required: true, message: '请输入排序' }] }]" />
        </a-form-item>
        <a-divider/>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="描述" hasFeedback validateStatus="success">
          <a-textarea placeholder="描述" type="" v-decorator="[ 'description', {rules: [{ required: true, message: '请输入描述' }] }]" />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { CreateAction } from '@/api/manage'
export default {
  name: 'ActionAdd',
  data () {
    return {
      labelCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 5
        }
      },
      wrapperCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 16
        }
      },
      title: '添加',
      visible: false, // 弹框是否显示
      confirmLoading: false, // 弹框中的提交按钮是否加载中
      mdl: {}, // 修改对象
      form: this.$form.createForm(this), // 表单对象
      actionKey: '',
      parentId: '',
      dataLoad: '' // 父节点数据
    }
  },
  created () {},
  methods: {
    // 添加
    add (data) {
      this.dataLoad = data
      this.visible = true
    },
    // 关闭方法
    close () {
      this.form = this.$form.createForm(this)
      this.mdl = {}
      this.parentId = ''
      this.$emit('close')
      this.visible = false
    },
    // 添加操作权限的父级选择触发事件
    actionTrigger (index, id) {
      this.parentId = id
    },
    // 弹框提交方法
    handleOk () {
      const _this = this
      if (_this.parentId === '') {
        _this.$notification['warning']({
          message: '温馨提醒',
          description: '请选择父级',
          duration: 4
        })
        return false
      }
      // 触发表单验证
      this.form.validateFields((err, values) => {
        // 验证表单没错误
        if (!err) {
          _this.confirmLoading = true
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              // Do something
              values.parentId = _this.parentId
              console.log('values', values)
              // 添加
              CreateAction(values).then(res => {
                if (res.status === 200) {
                  _this.confirmLoading = false
                  _this.$message.success('添加成功')
                  _this.$emit('ok')
                } else {
                  _this.confirmLoading = false
                  _this.$message.error(res.message)
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
        } else {
        }
      })
    },
    // 弹框关闭
    handleCancel () {
      this.close()
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
