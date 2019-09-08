<template>
  <a-card :bordered="false">

    <!--列表搜索-->
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row type="flex" justify="space-around" :gutter="48">
          <a-col :md="6" :sm="24">
            <a-form-item label="权限ID">
              <a-input placeholder="请输入权限ID" v-model="queryParam.permissionId" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-form-item label="记录时间">
              <a-range-picker name="buildTime" @change="ChangeBenginEndTime" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-form-item label="操作账号">
              <a-input placeholder="请输入操作账号" v-model="queryParam.account" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <div class="table-page-search-submitButtons" >
              <a-input-search
                placeholder="请输入操作名称，比如'修改权限排序'"
                v-model="queryParam.subject"
                enterButton="查询"
                @search="Search" >
              </a-input-search>
            </div>
          </a-col></a-row>
        <a-row v-if="advanced" justify="end" :gutter="48">
          <a-col :md="6" :sm="24">
            <a-form-item label="客户端IP">
              <a-input placeholder="请输入客户端IP" v-model="queryParam.clientIp" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <!--列表操作-->
    <div class="table-operator" >
      <a-tooltip>
        <template slot="title">批量删除
        </template>
        <a-button v-action:delete type="primary" icon="delete" class="right10" @click="$refs.delete.show()"></a-button>
      </a-tooltip>
      <a-radio-group :defaultValue="2" buttonStyle="solid" class="right10" v-model="queryParam.Type" @change="Search">
        <a-radio-button :value="2">操作审计</a-radio-button>
        <a-radio-button :value="1">登录日志</a-radio-button>
      </a-radio-group>

      <a-tooltip>
        <template slot="title">高级查询
        </template>
        <a-button type="primary" @click="advanced= !advanced" style="float:right;margin-right:0;margin-left:10;" >
          <a-icon :type="advanced ? 'up' : 'down'" /></a-button>
      </a-tooltip>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      rowKey="id"
      size="middle"
      :pagination="table.pagination"
      :columns="table.columns"
      :loading="table.loading"
      :dataSource="table.dataSource"
      @change="LoadDataing">
      <span slot="accountName" slot-scope="text, record">
        {{ text }}
        <a-badge :count="record.accountId" :overflowCount="999999" :numberStyle="{backgroundColor: '#52c41a'} "/>
      </span>
      <span slot="permissionId" slot-scope="text">{{ (text===null?'--':text) }}</span>
      <span slot="addTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
      <span slot="type" slot-scope="text">{{ text | loggerType }}</span>
      <span slot="clientIp" slot-scope="text">{{ text | replaceA(/::ffff:/, "") }}</span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.detail.show(record)" v-action:query>详情</a>
      </span>
    </a-table>

    <!--批量删除-->
    <audit-delete ref="delete" @complete="LoadDataing"></audit-delete>
    <!--详情-->
    <audit-detail ref="detail"></audit-detail>
  </a-card>
</template>

<script>
import { STable } from '@/components'
import { getAuditList } from '@/api/control'
import AuditDetail from './Detail'
import AuditDelete from './Delete'

export default {
  name: 'AuditList',
  components: {
    AuditDetail,
    AuditDelete,
    STable
  },
  data () {
    return {
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      queryParam: {
        subject: '',
        account: '',
        type: 2,
        beginTime: null,
        endTime: null,
        permissionId: '',
        clientIp: '',
        pagedCount: 10,
        skipPage: 1
      },
      table: {
        loading: false,
        dataSource: [],
        pagination: {
          showTotal: (total) => `总计 ${total} 条`,
          hideOnSinglePage: true,
          showSizeChanger: true,
          current: 1,
          total: 0
        },
        // 表头
        columns: [
          {
            title: '编号',
            dataIndex: 'id',
            width: 80
          },
          {
            title: '操作名称',
            dataIndex: 'subject',
            width: 200
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
            title: '日志类型',
            align: 'center',
            dataIndex: 'type',
            width: 100,
            scopedSlots: {
              customRender: 'type'
            }
          },
          {
            title: '客户端IP',
            dataIndex: 'clientIp',
            width: 150,
            scopedSlots: {
              customRender: 'clientIp'
            }
          },

          {
            title: '操作账户',
            dataIndex: 'accountName',
            width: 150,
            scopedSlots: {
              customRender: 'accountName'
            }
          },
          {
            title: '记录时间',
            dataIndex: 'addTime',
            align: 'center',
            width: 180,
            scopedSlots: {
              customRender: 'addTime'
            }
          },
          {
            title: '操作',
            align: 'center',
            width: 70,
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      }
    }
  },
  filters: {
    replaceA (text, substr, replacement) {
      return text.replace(substr, replacement)
    },
    loggerType (status) {
      const statusMap = {
        1: '登录日志',
        2: '操作审计'
      }
      return statusMap[status === 1 ? 1 : 2]
    }
  },
  watch: {
    'queryParam.skipPage' () {
      this.table.pagination.current = this.queryParam.skipPage
    }
  },
  methods: {
    /**
     * 条件搜索事件
     */
    Search () {
      this.queryParam.skipPage = 1
      this.LoadDataing()
    },
    /**
     * 加载数据方法
     * @param {Object} pagination 分页选项器
     * @param {Object} filters 过滤条件
     * @param {Object} sorter 排序条件
     */
    LoadDataing (pagination, filters, sorter) {
      this.table.loading = true
      if (pagination) {
        this.queryParam.skipPage = pagination.current
        this.queryParam.pagedCount = pagination.pageSize
      }
      getAuditList(this.queryParam).then(res => {
        this.table.loading = false
        if (res.status === 200) {
          this.table.pagination.total = res.totalCount
          this.table.dataSource = res.result
        } else {
          this.$notification.error({ message: '加载失败', description: res.message })
        }
      }).catch(() => {
        this.table.loading = false
      })
    },
    // 查询条件开始和结束时间录入
    ChangeBenginEndTime (date, dateStrings) {
      this.queryParam.beginTime = dateStrings[0]
      this.queryParam.endTime = dateStrings[1]
    }
  },
  created () {
    this.LoadDataing()
  }

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
.right10 {
   margin-right:10px;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold { width: 100%;}
}
</style>
