<template>
  <a-card :bordered="false">
    <detail-list title="退款申请">
      <detail-list-item term="取货单号">1000000000</detail-list-item>
      <detail-list-item term="状态">已取货</detail-list-item>
      <detail-list-item term="销售单号">1234123421</detail-list-item>
      <detail-list-item term="子订单">3214321432</detail-list-item>
    </detail-list>
    <a-divider style="margin-bottom: 32px"/>
    <detail-list title="用户信息">
      <detail-list-item term="用户姓名">付小小</detail-list-item>
      <detail-list-item term="联系电话">18100000000</detail-list-item>
      <detail-list-item term="常用快递">菜鸟仓储</detail-list-item>
      <detail-list-item term="取货地址">浙江省杭州市西湖区万塘路18号</detail-list-item>
      <detail-list-item term="备注">	无</detail-list-item>
    </detail-list>
    <a-divider style="margin-bottom: 32px"/>

    <!--<div class="title">退货商品</div>-->
    <a-table
      :columns="DetalisColumns"
      :dataSource="tableDetails"
      :pagination="false"
    >
      <span slot="isSystem" slot-scope="text"> {{ text | statusIsSystem }}</span>
      <span slot="isLogger" slot-scope="text"> {{ text | statusIsSystem }}</span>
      <span slot="enable" slot-scope="text">{{ text | statusFilter }}</span>
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
    <!--<div class="title">退货进度</div>-->
    <!--<s-table-->
    <!--style="margin-bottom: 24px"-->
    <!--:columns="scheduleColumns"-->
    <!--:data="loadScheduleData">-->

    <!--<template-->
    <!--slot="status"-->
    <!--slot-scope="status">-->
    <!--<a-badge :status="status" :text="status | statusFilter"/>-->
    <!--</template>-->
    <!--</s-table>-->
  </a-card>
</template>

<script>
import STable from '@/components/table/'
import DetailList from '@/components/tools/DetailList'
import ABadge from 'ant-design-vue/es/badge/Badge'
import AFormItem from 'ant-design-vue/es/form/FormItem'
import { getPermissions, getActionDetails, modifyAction, modifySort, getMenuDetails, getActionList, deleteAction, CreateAction, CreateMenu } from '@/api/manage'
const DetailListItem = DetailList.Item
const DetalisColumns = [
  {
    title: '权限名称',
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: '是否日志记录',
    dataIndex: 'isLogger',
    scopedSlots: {
      customRender: 'isLogger'
    },
    key: 'isLogger'
  },
  {
    title: '状态',
    dataIndex: 'enable',
    scopedSlots: {
      customRender: 'enable'
    },
    key: 'enable'
  },
  {
    title: '添加时间',
    dataIndex: 'createTime',
    key: 'createTime'
  },
  {
    title: '描述',
    dataIndex: 'description',
    key: 'description'
  },
  {
    title: '排序',
    dataIndex: 'sort',
    scopedSlots: {
      customRender: 'sort'
    }
  },
  {
    title: '操作',
    dataIndex: 'action',
    key: 'action',
    scopedSlots: {
      customRender: 'action'
    }
  }
]
export default {
  name: 'Detalis',
  components: {
    ABadge,
    AFormItem,
    DetailList,
    DetailListItem,
    STable
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
      goodsColumns: [
        {
          title: '商品编号',
          dataIndex: 'id',
          key: 'id'
        },
        {
          title: '商品名称',
          dataIndex: 'name',
          key: 'name'
        },
        {
          title: '商品条码',
          dataIndex: 'barcode',
          key: 'barcode'
        },
        {
          title: '单价',
          dataIndex: 'price',
          key: 'price',
          align: 'right'
        },
        {
          title: '数量（件）',
          dataIndex: 'num',
          key: 'num',
          align: 'right'
        },
        {
          title: '金额',
          dataIndex: 'amount',
          key: 'amount',
          align: 'right'
        }
      ],
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
      // 加载数据方法 必须为 Promise 对象
      loadGoodsData: () => {
        return new Promise(resolve => {
          resolve({
            data: [
              {
                id: '1234561',
                name: '矿泉水 550ml',
                barcode: '12421432143214321',
                price: '2.00',
                num: '1',
                amount: '2.00'
              },
              {
                id: '1234562',
                name: '凉茶 300ml',
                barcode: '12421432143214322',
                price: '3.00',
                num: '2',
                amount: '6.00'
              },
              {
                id: '1234563',
                name: '好吃的薯片',
                barcode: '12421432143214323',
                price: '7.00',
                num: '4',
                amount: '28.00'
              },
              {
                id: '1234564',
                name: '特别好吃的蛋卷',
                barcode: '12421432143214324',
                price: '8.50',
                num: '3',
                amount: '25.50'
              }
            ],
            pageSize: 10,
            pageNo: 1,
            totalPage: 1,
            totalCount: 10
          })
        }).then(res => {
          return res
        })
      },

      scheduleColumns: [
        {
          title: '时间',
          dataIndex: 'time',
          key: 'time'
        },
        {
          title: '当前进度',
          dataIndex: 'rate',
          key: 'rate'
        },
        {
          title: '状态',
          dataIndex: 'status',
          key: 'status',
          scopedSlots: { customRender: 'status' }
        },
        {
          title: '操作员ID',
          dataIndex: 'operator',
          key: 'operator'
        },
        {
          title: '耗时',
          dataIndex: 'cost',
          key: 'cost'
        }
      ],
      form: this.$form.createForm(this),
      loadScheduleData: () => {
        return new Promise(resolve => {
          resolve({
            data: [
              {
                key: '1',
                time: '2017-10-01 14:10',
                rate: '联系客户',
                status: 'processing',
                operator: '取货员 ID1234',
                cost: '5mins'
              },
              {
                key: '2',
                time: '2017-10-01 14:05',
                rate: '取货员出发',
                status: 'success',
                operator: '取货员 ID1234',
                cost: '1h'
              },
              {
                key: '3',
                time: '2017-10-01 13:05',
                rate: '取货员接单',
                status: 'success',
                operator: '取货员 ID1234',
                cost: '5mins'
              },
              {
                key: '4',
                time: '2017-10-01 13:00',
                rate: '申请审批通过',
                status: 'success',
                operator: '系统',
                cost: '1h'
              },
              {
                key: '5',
                time: '2017-10-01 12:00',
                rate: '发起退货申请',
                status: 'success',
                operator: '用户',
                cost: '5mins'
              }
            ],
            pageSize: 10,
            pageNo: 1,
            totalPage: 1,
            totalCount: 10
          })
        }).then(res => {
          return res
        })
      }
    }
  },
  created () {
    getMenuDetails(this.$route.query.data).then(res => {
      this.menuDetails = res.result
      const obj = {
        vague: '',
        enable: '-1',
        paramId: res.result.id
      }
      getActionList(obj).then(data => {
        this.tableDetails = data.result
        console.log(data.result)
      })
    })
  },
  filters: {
    // statusFilter (status) {
    //   const statusMap = {
    //     'processing': '进行中',
    //     'success': '完成',
    //     'failed': '失败'
    //   }
    //   return statusMap[status]
    // },
    statusFilter (status) {
      const statusMap = {
        1: '启用',
        0: '停止'
      }
      return statusMap[status ? 1 : 0]
    },
    statusIsSystem (status) {
      const statusMap = {
        1: '是',
        0: '否'
      }
      return statusMap[status ? 1 : 0]
    }
  },
  computed: {
    title () {
      return this.$route.meta.title
    }
  },
  methods: {
    handerContrl (action, key) {
      // this.controlFrom = action
      // console.log(222222222, action, key)
      console.log(key)
      this.visibleContrl = true
      const id = key
      getActionDetails(id).then(res => {
        console.log(res)
        if (res.status === 200) {
          this.controlFrom = res.result
        }
      })
    },
    handleSubmit (e) {
      e.preventDefault()
      // this.visibleContrl = true
      const obj = this.controlFrom
      modifyAction(obj).then(res => {
      // console.log(res)
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
      console.log(index)
      deleteAction(id).then(res => {
        console.log(res)
        if (res.status === 200) {
        // 重新请求数据
        // this.$refs.table.refresh(true)
          this.tableDetails.splice(index, 1)
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
