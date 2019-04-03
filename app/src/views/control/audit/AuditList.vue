/* eslint-disable vue/no-unused-vars */
<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form :form="form" @submit="handleSearch" layout="inline">
        <a-row type="flex" justify="space-around">
          <a-col :md="{span:8}" :sm="24">
            <a-tooltip>
              <template slot="title">删除审计
              </template>
              <a-button type="primary" icon="delete" style="margin-right:10px" @click="deleteList"></a-button>
            </a-tooltip>
            <a-radio-group :defaultValue="1" buttonStyle="solid" v-model="tableParam.Type" @change="handleChanges">
              <a-radio-button :value="1">登录审计</a-radio-button>
              <a-radio-button :value="2">操作审计</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :md="{span:16,offset:0}" :sm="{span:24,offset:0}" :xs="{span:24,offset:0}" :offset="0">

            <div class="table-page-search-submitButtons" style="float:right">
              <a-range-picker name="buildTime" @change="onChangeBenginEndTime" />
              <a-input-search placeholder="请输入操作名称，比如'修改权限排序'" v-model="tableParam.subject" enterButton="查询" html-type="submit" style="width:350px;margin:0px 10px 0px 10px">
              </a-input-search>
              <a-button type="primary" @click="toggleAdvanced">
                <a-icon :type="advanced ? 'up' : 'down'" /></a-button>
            </div>
          </a-col>
        </a-row>
        <a-row v-if="advanced" type="flex" justify="end" :gutter="8">
          <a-col :md="{span:5,offset:0}">
            <a-form-item label="客户端IP">
              <a-input placeholder="请输入客户端IP" v-model="tableParam.clientIp" />
            </a-form-item>
          </a-col>
          <a-col :md="{span:5,offset:0}">
            <a-form-item label="操作账号">
              <a-input placeholder="请输入操作账号" v-model="tableParam.account" />
            </a-form-item>
          </a-col>

          <a-col :md="{span:5}">
            <a-form-item label="权限ID">
              <a-input placeholder="请输入权限ID" v-model="tableParam.permissionId" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table
      ref="table"
      size="default"
      :columns="columns"
      :dataSource="loadData"
      :loading="loading"
      :pagination="pagination"
      :alert="{ show: true, clear: true }"
      @change="handleTableChange">
      <span slot="accountName" slot-scope="text,record">{{ text +"&nbsp;&nbsp;" }}
        <a-tag color="blue">{{ "ID："+record.accountId }}</a-tag>
      </span>
      <span slot="permissionId" slot-scope="text">{{ (text===null?'--':text) }}</span>
      <span slot="addTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
      <span slot="logType" slot-scope="text">{{ text | loggerType }}</span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.modal.showModal(record)">详情</a>
      </span>
    </a-table>
    <a-modal title="删除审计" :width="500" v-model="visible" @ok="handleOk">
      <a-form layout="inline" :form="deleteForm" hideRequiredMark>
        <a-row :gutter="48">
          <a-col>
            <a-form-item label="删除范围">
              <a-range-picker name="buildTime" @change="deleteRangeTime" style="width: 100%" :disabledDate="disabledDate" v-decorator="['buildTime', {rules: [{ required: true, message: '请选择删除日期' }]}]" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
      <div style="margin-top:20px">
        <p>提示：</p>
        <p style="font-size:10px">&nbsp;&nbsp;&nbsp;&nbsp;三个月之内的审计日志不能删除</p>
      </div>
    </a-modal>
    <audit-detail ref="modal"></audit-detail>
  </a-card>
</template>

<script>
import moment from 'moment'
import AuditDetail from './AuditDetail'
import { getLogger, loggerDelete } from '@/api/manage'

export default {
  name: 'AuditList',
  components: {
    AuditDetail
  },
  data () {
    return {
      // 高级搜索 展开/关闭
      advanced: false,
      visible: false,
      loggerData: false,
      jsonSource: [],
      jsonArray: '',
      form: this.$form.createForm(this),
      deleteForm: this.$form.createForm(this),
      // 查询参数
      queryParam: {},
      tableParam: {
        subject: '',
        account: '',
        type: 1,
        beginTime: '',
        endTime: '',
        permissionId: '',
        clientIp: '',
        PagedCount: 10,
        skipPage: 1
      },
      // 表头
      columns: [
        {
          title: '日志编号',
          dataIndex: 'id',
          key: 'id'
        },
        {
          title: '操作名称',
          dataIndex: 'subject',
          key: 'subject'
        },
        {
          title: '操作账户',
          dataIndex: 'accountName',
          key: 'accountName',
          scopedSlots: {
            customRender: 'accountName'
          }
        },
        {
          title: '日志类型',
          dataIndex: 'logType',
          key: 'logType',
          scopedSlots: {
            customRender: 'logType'
          }
        },
        {
          title: '权限标识',
          dataIndex: 'permissionId',
          key: 'permissionId',
          scopedSlots: {
            customRender: 'permissionId'
          }
        },
        {
          title: '日志记录时间',
          dataIndex: 'addTime',
          scopedSlots: {
            customRender: 'addTime'
          },
          key: 'addTime'
        },
        {
          title: '客户端IP',
          dataIndex: 'clientIp',
          key: 'clientIp'
        },
        {
          title: '操作',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        }
      ],
      loadData: [],
      pagination: {
        current: 1,
        pageSize: 10,
        pageSizeOptions: ['10', '20', '30', '40', '50'],
        showSizeChanger: true
      },
      deleteDate: {
        beginTime: '',
        endTime: ''
      },
      apiAddress: '',
      // page: ['10', '20', '30', '40'],
      loading: false,
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  filters: {
    loggerType (status) {
      const statusMap = {
        1: '登陆日志',
        2: '操作日志'
      }
      return statusMap[status === 1 ? 1 : 2]
    }
  },
  created () {
    this.loadDataing()
  },
  methods: {
    // 开始时间录入
    onChangebeginTime (date, dateString) {
      console.log(date, dateString)
      this.tableParam.beginTime = dateString
    },
    // 结束时间录入
    onChangeEndTime (date, dateString) {
      console.log(date, dateString)
      this.tableParam.endTime = dateString
    },
    // 查询条件开始和结束时间录入
    onChangeBenginEndTime (date, dateStrings) {
      this.tableParam.beginTime = dateStrings[0]
      this.tableParam.endTime = dateStrings[1]
    },

    // 列表change事件
    handleTableChange (pagination, filters, sorter) {
      console.log(pagination)
      const pager = {
        ...this.pagination
      }
      pager.current = pagination.current
      this.pagination = pager
      this.tableParam.pagedCount = pagination.pageSize
      this.tableParam.skipPage = pagination.current
      this.loadDataing()
    },
    // 日志类型改变事件
    handleChanges (value) {
      this.loadDataing()
    },
    // 删除日志点击确认事件
    handleOk () {
      this.deleteForm.validateFields((err, values) => {
        if (!err) {
          if (this.deleteDate.beginTime !== '' && this.deleteDate.endTime !== '') {
            loggerDelete(this.deleteDate).then(res => {
              console.log(res)
              if (res.status === 200) {
                this.loadDataing()
                this.visible = !this.visible
                this.$notification['success']({
                  message: '温馨提醒',
                  description: '删除成功',
                  duration: 4
                })
              } else {
                this.$notification['error']({
                  message: '温馨提醒',
                  description: res.message,
                  duration: 4
                })
              }
            })
          } else {
            this.$notification['error']({
              message: '温馨提醒',
              description: '请选择您的开始时间和结束时间',
              duration: 4
            })
          }
        }
      })
    },
    // 删除日志显示框
    deleteList () {
      this.visible = true
    },
    // 范围时间控件的change事件
    deleteRangeTime (date, dateStrings) {
      this.deleteDate.beginTime = dateStrings[0]
      this.deleteDate.endTime = dateStrings[1]
    },
    // 筛选出3个月之后的日子的不能选择
    disabledDate (current) {
      return current && current > moment().subtract(3, 'month')
    },
    // 加载列表集合事件
    loadDataing () {
      const _this = this
      _this.loading = true
      getLogger(_this.tableParam).then(data => {
        // this.loadData = res.result
        const pagination = {
          ..._this.pagination
        }
        // Read total count from server
        // pagination.total = data.totalCount;
        pagination.total = data.totalCount
        _this.loading = false
        const list = data.result
        for (const i in data.result) {
          list[i].key = list[i].id
          list[i].logType = list[i].type
        }
        console.log('LogList:', list)
        _this.loadData = list
        _this.pagination = pagination
      })
    },
    // 条件搜索事件
    handleSearch (e) {
      e.preventDefault()
      // this.form.validateFields((error, values) => {
      //   console.log('error', error)
      //   console.log('Received values of form: ', values)
      // })
      if (this.tableParam.skipPage > 1) {
        this.tableParam.skipPage = 1
      }
      this.loadDataing()
    },
    // 多条件展开事件
    toggleAdvanced () {
      this.advanced = !this.advanced
    }
  },
  watch: {}
}
</script>

<style lang="less" scoped>
.search {
  margin-bottom: 54px;
}
.fold {
  width: calc(100% - 216px);
  display: inline-block;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold {
    width: 100%;
  }
}
</style>
