<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form
        :form="form"
        @submit="handleSearch"
        layout="inline"
      >
        <a-row :gutter="48">
          <a-col :md="8" :sm="24">
            <a-form-item label="操作名称">
              <a-input placeholder="请输入操作名称，比如'修改权限排序'" v-model="tableParam.subject"/>
            </a-form-item>
          </a-col>
          <a-col :md="8" :sm="24">
            <a-form-item label="操作账号">
              <a-input placeholder="请输入操作账号" v-model="tableParam.account" />
            </a-form-item>
          </a-col>
          <template v-if="advanced">
            <!--<a-col :md="8" :sm="24">-->
            <!--<a-form-item label="日志类型">-->
            <!--<a-select placeholder="请选择" default-value="0">-->
            <!--<a-select-option value="0">全部</a-select-option>-->
            <!--<a-select-option value="1">登录日志</a-select-option>-->
            <!--<a-select-option value="2">操作日志</a-select-option>-->
            <!--</a-select>-->
            <!--</a-form-item>-->
            <!--</a-col>-->
            <!--<a-col :md="8" :sm="24">-->
            <!--<a-form-item label="调用次数">-->
            <!--<a-input-number style="width: 100%"/>-->
            <!--</a-form-item>-->
            <!--</a-col>-->
            <a-col :md="8" :sm="24">
              <a-form-item label="开始时间">
                <a-date-picker style="width: 100%" placeholder="请输入开始时间" @change="onChangebeginTime"/>
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item label="结束时间">
                <a-date-picker style="width: 100%" placeholder="请输入结束时间" @change="onChangeEndTime"/>
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item label="使用状态">
                <a-select placeholder="请选择" default-value="0">
                  <a-select-option value="0">全部</a-select-option>
                  <a-select-option value="1">关闭</a-select-option>
                  <a-select-option value="2">运行中</a-select-option>
                </a-select>
              </a-form-item>
            </a-col>
            <!--<a-col :md="8" :sm="24">-->
            <!--<a-form-item label="使用状态">-->
            <!--<a-select placeholder="请选择" default-value="0">-->
            <!--<a-select-option value="0">全部</a-select-option>-->
            <!--<a-select-option value="1">关闭</a-select-option>-->
            <!--<a-select-option value="2">运行中</a-select-option>-->
            <!--</a-select>-->
            <!--</a-form-item>-->
            <!--</a-col>-->
          </template>
          <a-col :md="!advanced && 8 || 24" :sm="24">
            <span class="table-page-search-submitButtons" :style="advanced && { float: 'right', overflow: 'hidden' } || {} ">
              <a-button
                type="primary"
                html-type="submit"
              >查询</a-button>
              <a-button style="margin-left: 8px" @click="handleReset">重置</a-button>
              <a @click="toggleAdvanced" style="margin-left: 8px">
                {{ advanced ? '收起' : '展开' }}
                <a-icon :type="advanced ? 'up' : 'down'"/>
              </a>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <div class="table-operator">
      <a-button type="primary" icon="delete" @click="deleteList">删除</a-button>
      <!--<a-dropdown v-if="selectedRowKeys.length > 0">-->
      <!--<a-menu slot="overlay">-->
      <!--<a-menu-item key="1"><a-icon type="delete" />删除</a-menu-item>-->
      <!--&lt;!&ndash; lock | unlock &ndash;&gt;-->
      <!--<a-menu-item key="2"><a-icon type="lock" />锁定</a-menu-item>-->
      <!--</a-menu>-->
      <!--<a-button style="margin-left: 8px">-->
      <!--批量操作 <a-icon type="down" />-->
      <!--</a-button>-->
      <!--</a-dropdown>-->
    </div>

    <a-table
      ref="table"
      size="default"
      :columns="columns"
      :dataSource="loadData"
      :loading="loading"
      :pagination="pagination"
      :alert="{ show: true, clear: true }"
      @change="handleTableChange"
    >
      <span slot="addTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
    </a-table>
    <a-modal title="编辑" :width="1000" v-model="visible" @ok="handleOk">
      <a-form
        layout="inline"
      >
        <a-row :gutter="48">
          <a-col :md="12" :sm="24">
            <a-form-item label="开始时间">
              <a-date-picker style="width: 100%" placeholder="请输入开始时间" @change="deleteTime"/>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="结束时间">
              <a-date-picker style="width: 100%" placeholder="请输入结束时间" @change="deleteEndTime"/>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script>
import STable from '@/components/table/'
import { getLogger, loggerDelete } from '@/api/manage'
export default {
  name: 'TableList',
  components: {
    STable
  },
  data () {
    return {
      // 高级搜索 展开/关闭
      advanced: false,
      visible: false,
      form: this.$form.createForm(this),
      // 查询参数
      queryParam: {},
      tableParam: {
        subject: '',
        account: '',
        type: -1,
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
          title: '操作账户昵称',
          dataIndex: 'accountName',
          key: 'accountName'
        },
        // {
        //   title: '日志类型',
        //   dataIndex: 'type'
        // },
        {
          title: '日志类型',
          dataIndex: 'type',
          key: 'type'
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
          title: 'API请求数据',
          dataIndex: 'apiAddress',
          key: 'apiAddress'
        },
        {
          title: '备注',
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
          key: 'remark'
        }
      ],
      loadData: [],
      pagination: {
        pageSizeOptions: ['10', '20', '30', '40', '50'],
        showSizeChanger: true
      },
      deleteDate: {
        beginTime: '',
        endTime: ''
      },
      // page: ['10', '20', '30', '40'],
      loading: false,
      selectedRowKeys: [],
      selectedRows: []
    }
  },
  created () {
    this.loadDataing()
  },
  methods: {
    onChangebeginTime (date, dateString) {
      console.log(date, dateString)
      this.tableParam.beginTime = dateString
    },
    onChangeEndTime (date, dateString) {
      console.log(date, dateString)
      this.tableParam.endTime = dateString
    },
    handleTableChange (pagination, filters, sorter) {
      console.log(pagination)
      const pager = { ...this.pagination }
      pager.current = pagination.current
      this.pagination = pager
      // this.fetch({
      //   results: pagination.pageSize,
      //   page: pagination.current,
      //   sortField: sorter.field,
      //   sortOrder: sorter.order,
      //   ...filters
      // })
      this.tableParam.pagedCount = pagination.pageSize
      this.tableParam.skipPage = pagination.current
      this.loadDataing()
    },
    handleOk () {
      if (this.deleteDate.beginTime !== '' && this.deleteDate.endTime !== '') {
        loggerDelete(this.deleteDate).then(res => {
          console.log(res)
          if (res.status === 200) {
            this.loadDataing()
            this.visible = !this.visible
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
    },
    deleteList () {
      this.visible = true
    },
    deleteEndTime (date, dateString) {
      this.deleteDate.endTime = dateString
    },
    deleteTime (date, dateString) {
      this.deleteDate.beginTime = dateString
    },
    loadDataing () {
      getLogger(this.tableParam).then(data => {
        // this.loadData = res.result
        const pagination = { ...this.pagination }
        // Read total count from server
        // pagination.total = data.totalCount;
        pagination.total = data.totalCount
        this.loading = false
        const list = data.result
        for (const i in data.result) {
          list[i].key = list[i].id
        }
        console.log(list)
        this.loadData = list
        this.pagination = pagination
      })
    },
    handleSearch  (e) {
      e.preventDefault()
      // this.form.validateFields((error, values) => {
      //   console.log('error', error)
      //   console.log('Received values of form: ', values)
      // })
      this.loadDataing()
    },
    handleChange (value, key, column, record) {
      console.log(value, key, column)
      record[column.dataIndex] = value
    },
    handleReset () {
      this.form.resetFields()
    },
    edit (row) {
      row.editable = true
      // row = Object.assign({}, row)
    },
    // eslint-disable-next-line
      del (row) {
      this.$confirm({
        title: '警告',
        content: `真的要删除 ${row.no} 吗?`,
        okText: '删除',
        okType: 'danger',
        cancelText: '取消',
        onOk () {
          console.log('OK')
          // 在这里调用删除接口
          return new Promise((resolve, reject) => {
            setTimeout(Math.random() > 0.5 ? resolve : reject, 1000)
          }).catch(() => console.log('Oops errors!'))
        },
        onCancel () {
          console.log('Cancel')
        }
      })
    },
    save (row) {
      row.editable = false
    },
    cancel (row) {
      row.editable = false
    },
    onSelectChange (selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
      console.log(selectedRowKeys, selectedRows)
    },
    toggleAdvanced () {
      this.advanced = !this.advanced
    }
  },
  watch: {
    /*
        'selectedRows': function (selectedRows) {
          this.needTotalList = this.needTotalList.map(item => {
            return {
              ...item,
              total: selectedRows.reduce( (sum, val) => {
                return sum + val[item.dataIndex]
              }, 0)
            }
          })
        }
        */
  }
}
</script>

<style lang="less" scoped>
  .search {
    margin-bottom: 54px;
  }
  .fold {
    width: calc(100% - 216px);
    display: inline-block
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
