<template>
  <a-card :bordered="false">
    <detail-list title="操作权限详情">
      <detail-list-item term="名称">{{ menuDetails.name }}</detail-list-item>
      <detail-list-item term="状态">{{ menuDetails.enable | statusFilter }}</detail-list-item>
      <detail-list-item term="权限ID">{{ menuDetails.id }}</detail-list-item>
      <detail-list-item term="创建时间">{{ menuDetails.createTime | dayFormat }}</detail-list-item>
      <detail-list-item term="父级权限ID">{{ menuDetails.parentId }}</detail-list-item>
      <detail-list-item term="排序">{{ menuDetails.sort }}</detail-list-item>
      <detail-list-item term="描述">{{ menuDetails.description }}</detail-list-item>
    </detail-list>
    <a-divider style="margin-bottom: 32px"/>
    <a-table
      :columns="DetalisColumns"
      :dataSource="tableDetails"
      :pagination="false"
      ref="table"
    >
      <span slot="isSystem" slot-scope="text"> {{ text | statusIsSystem }}</span>
      <span slot="isLogger" slot-scope="text"> {{ text | statusIsSystem }}</span>
      <span slot="enable" slot-scope="text">{{ text | statusFilter }}</span>
      <span slot="sort" slot-scope="text, record">
        <editable-cell :text="text" ref="sortDom" @handleChange="editSortInput" @change="handerChange" @editBlurInput="handerChange( record, text)"/>
      </span>
      <span slot="action" slot-scope="text, record, index">
        <!--<a @click="handleEdit(record)">编辑</a>-->
        <a @click="handerContrl('222', record.id)" v-if="!record.isSystem">编辑</a>
        <a v-else disabled>编辑</a>
        <a-divider type="vertical"/>
        <a @click="editDelete(record.id, index)" v-if="!record.isSystem">删除</a>
        <a v-else disabled>删除</a>
      </span>
    </a-table>
    <a-modal
      title="操作权限编辑"
      :width="800"
      v-model="visibleContrl"
      @cancel="editModalCancel()"
      :centered="true"
      @ok="handleSubmit">
      <a-form :form="form">
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="父级权限"
          hasFeedback
        >
          <a-input placeholder="父级权限" v-model="controlFrom.parentId" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="标识"
          hasFeedback
        >
          <a-input placeholder="标识" v-model="controlFrom.id" disabled="disabled"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="API地址模板"
          hasFeedback
          validateStatus="success"
        >
          <a-input
            placeholder="请输入API地址模板"
            v-model="controlFrom.apiTemplate"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="权限名称"
          hasFeedback
          validateStatus="success"
        >
          <a-input
            placeholder="权限描述"
            v-model="controlFrom.name"/>
        </a-form-item>
        <a-form-item
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
          label="添加时间"
          hasFeedback
        >
          <a-input placeholder="添加时间" v-model="controlFrom.createTime" disabled="disabled"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="系统权限">
          <a-switch :checked="controlFrom.isSystem" :disabled="true"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="记录日志">
          <a-switch :checked="controlFrom.isLogger"/>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="是否启用">
          <a-switch :checked="controlFrom.enable"/>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>
<script>
import EditableCell from '@/components/ASF/EditCell/EditableCell'
import STable from '@/components/Table/'
import DetailList from '@/components/tools/DetailList'
import AFormItem from 'ant-design-vue/es/form/FormItem'
import { getActionDetails, modifyAction, getMenuDetails, getActionList, deleteAction, modifySort } from '@/api/manage'
const DetailListItem = DetailList.Item
const DetalisColumns = [
  {
    title: '权限名称',
    dataIndex: 'name',
    key: '1'
  },
  {
    title: '是否日志记录',
    dataIndex: 'isLogger',
    scopedSlots: {
      customRender: 'isLogger'
    },
    key: '2'
  },
  {
    title: '状态',
    dataIndex: 'enable',
    scopedSlots: {
      customRender: 'enable'
    },
    key: '3'
  },
  {
    title: '添加时间',
    dataIndex: 'createTime',
    key: '4'
  },
  {
    title: '描述',
    dataIndex: 'description',
    key: '5'
  },
  {
    title: '排序',
    dataIndex: 'sort',
    scopedSlots: {
      customRender: 'sort'
    },
    key: '6'
  },
  {
    title: '操作',
    dataIndex: 'action',
    key: '7',
    scopedSlots: {
      customRender: 'action'
    }
  }
]
export default {
  name: 'Detalis',
  components: {
    AFormItem,
    DetailList,
    DetailListItem,
    STable,
    EditableCell
  },
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
      DetalisColumns,
      menuDetails: [],
      visibleContrl: false,
      tableDetails: [],
      controlFrom: {
        id: '',
        parentId: '',
        name: '',
        apiTemplate: '',
        isSystem: '',
        isLogger: '',
        enable: '',
        createTime: ''
      },
      form: this.$form.createForm(this)
    }
  },
  created () {
    this.getData()
  },
  computed: {
    title () {
      return this.$route.meta.title
    }
  },
  methods: {
    getData () {
      getMenuDetails(this.$route.query.data).then(res => {
        this.menuDetails = res.result
        const obj = {
          vague: '',
          enable: '-1',
          paramId: res.result.id
        }
        getActionList(obj).then(data => {
          const list = data.result
          for (const i in data.result) {
            list[i].key = list[i].id
          }
          this.tableDetails = list
        })
      })
    },
    // 修改排序input，input失去焦点或者按Enter键触发此函数
    handerChange (record, text) {
      const parameter = {
        id: record.id,
        sort: this.editSort
      }
      modifySort(parameter).then(res => {
        if (res.status === 200) {
          // 重新请求数据
          this.getData()
        }
      })
    },
    editSortInput (e) {
      this.editSort = e
    },
    handerContrl (action, key) {
      this.visibleContrl = true
      const id = key
      getActionDetails(id).then(res => {
        if (res.status === 200) {
          this.controlFrom = res.result
        }
      })
    },
    handleSubmit (e) {
      e.preventDefault()
      const obj = this.controlFrom
      modifyAction(obj).then(res => {
        if (res.status === 200) {
          this.visibleContrl = !this.visibleContrl
        } else {
          this.$notification['error']({
            message: '错误',
            description: res.message,
            duration: 4
          })
        }
      })
    },
    editDelete (id, index) {
      const that = this
      this.$confirm({
        title: '您确定要删除此权限？',
        content: '删除权限！',
        okText: '确定删除',
        okType: 'danger',
        cancelText: '取消',
        onOk () {
          deleteAction(id).then(res => {
            if (res.message === 'Success') {
              // this.$notification.open({
              //   message: '温馨提醒！',
              //   description: '您的内容删除成功！',
              //   onClick: () => {
              //   }
              // })
              that.getData()
            }
          })
        },
        onCancel () {
        }
      })
    },
    // 编辑权限弹窗关闭回调函数
    editModalCancel () {
      const obj = {
        id: '',
        parentId: '',
        name: '',
        apiTemplate: '',
        isSystem: '',
        isLogger: '',
        enable: '',
        createTime: ''
      }
      this.controlFrom = obj
    }
  }
}
</script>

<style lang="less" scoped>
  .title {
    color: rgba(0,0,0,.85);
    font-size: 16px;
    font-weight: 500;
    margin-bottom: 16px;
  }
</style>
