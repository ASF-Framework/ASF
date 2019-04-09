<template>
  <a-modal
    title="日志详情"
    :width="800"
    :visible="visible"
    :destroyOnClose="true"
    :footer="null"
    @cancel="this.close">
    <a-spin :spinning="loading">
      <a-card :bordered="false">
        <detail-list title="基本信息">
          <detail-list-item term="操作名称" style="width:50%">{{ detailData.subject }}</detail-list-item>
          <detail-list-item term="日志类型" style="width:50%">{{ detailData.logType | loggerType }}</detail-list-item>
          <detail-list-item term="操作账户" style="width:50%">
            <span>{{ detailData.accountName }}</span>
            <a-tag color="blue">{{ "ID:"+detailData.accountId }}</a-tag>
          </detail-list-item>
          <detail-list-item term="客户端IP" style="width:50%">{{ detailData.clientIp | replaceA(/::ffff:/, "") }}</detail-list-item>
          <detail-list-item v-show="detailData.permissionId!==null" term="权限标识" style="width:50%">
            {{ detailData.permissionId }}
          </detail-list-item>
          <detail-list-item term="记录时间" style="width:50%">
            {{ detailData.addTime | dayFormat('YYYY-MM-DD HH:mm:ss') }}
          </detail-list-item>
          <detail-list-item v-show="detailData.remark!==null" term="备注" >
            {{ detailData.remark }}
          </detail-list-item>
        </detail-list>
        <a-divider />
        <detail-list title="操作API" v-show="detailData.apiAddress!==null">
          <detail-list-item>
            {{ (detailData.apiAddress===null ? '--':detailData.apiAddress) }}
          </detail-list-item>
        </detail-list>
        <detail-list title="请求数据">
          <detail-list-item >
            <a-row>
              <p v-if="!requestJson.isJson">{{ requestJson.data }}</p>
              <JsonView v-else :hidden="!requestJson.isJson" :json="requestJson.data" :closed="true"></JsonView>
            </a-row>
          </detail-list-item>
        </detail-list>
        <detail-list title="响应数据">
          <detail-list-item >
            <a-row>
              <p v-if="!responseJson.isJson">{{ responseJson.data }}</p>
              <JsonView v-else :hidden="!responseJson.isJson" :json="responseJson.data" :closed="true"></JsonView>
            </a-row>
          </detail-list-item>
        </detail-list>
      </a-card>
    </a-spin>
  </a-modal>
</template>

<script>
import JsonView from '@/components/JsonView/JsonView'
import DetailList from '@/components/tools/DetailList'
const DetailListItem = DetailList.Item

export default {
  name: 'AuditDetail',
  components: {
    DetailList,
    DetailListItem,
    JsonView
  },
  data () {
    return {
      visible: false, // 弹框是否显示
      loading: true, // 是否加载中

      detailData: {}, // 对象
      requestJson: {},
      responseJson: {}
    }
  },
  filters: {
    loggerType (status) {
      const statusMap = {
        1: '登录日志',
        2: '操作审计'
      }
      return statusMap[status === 1 ? 1 : 2]
    },
    replaceA (text, substr, replacement) {
      if (text) {
        return text.replace(substr, replacement)
      } else {
        return text
      }
    }
  },
  methods: {
    /**
     * 显示详情
     */
    show (record) {
      this.visible = true
      this.detailData = record
      this.loading = true
      this.requestJson = this.makeJsonData(this.detailData.apiRequest)
      this.responseJson = this.makeJsonData(this.detailData.apiResponse)
      this.loading = false
    },

    // 关闭方法
    close () {
      this.visible = false
    },
    // 构建响应json数据
    makeJsonData (value) {
      const jsonData = {
        isJson: false,
        data: value
      }
      if (value) {
        try {
          if (this.isJson(value)) {
            jsonData.isJson = true
            jsonData.data = JSON.parse(value)
          }
        } catch (err) {
        }
      }
      return jsonData
    },
    isJson (value) {
      if (typeof value === 'string') {
        try {
          var obj = JSON.parse(value)
          if (typeof obj === 'object' && obj) {
            return true
          } else {
            return false
          }
        } catch (e) {
          return false
        }
      }
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
</style>
