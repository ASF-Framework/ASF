<template>
  <a-card :bordered="false">
    <!--搜索-->
    <div class="table-page-search-wrapper">
      <a-row type="flex" justify="space-around" :gutter="48">
        <a-col :md="18" :sm="24">
          <a-tooltip>
            <template slot="title">新增公共API</template>
            <a-button type="primary" @click="$refs.PermissionModal.add()" icon="plus" class="right10"></a-button>
          </a-tooltip>
          <a-select placeholder="请选择" v-model="queryParam.enable" style="width:100px;" @change="LoadDataing">
            <a-select-option :value="-1">API 状态</a-select-option>
            <a-select-option :value="1">启用</a-select-option>
            <a-select-option :value="2">禁用</a-select-option>
          </a-select>
        </a-col>
        <a-col :md="6" :sm="24">
          <div class="table-page-search-submitButtons" >
            <a-input-search
              placeholder="公共API标识、名称"
              v-model="queryParam.vague"
              enterButton="查询"
              @search="LoadDataing" >
            </a-input-search>
          </div>
        </a-col>
      </a-row>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      :pagination="false"
      :loading="table.loading"
      :columns="table.columns"
      :dataSource="table.dataSource">
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.detail.show(record)">详情</a>
      </span>
    </a-table>

  </a-card>
</template>

<script>
import { STable } from '@/components'
import { getPublicApiList } from '@/api/control'
export default {
  name: 'PublicApiList',
  data () {
    return {
      queryParam: {
        vague: '',
        enable: -1
      },
      table: {
        loading: false,
        dataSource: [],
        columns: [
          {
            title: '日志编号',
            dataIndex: 'id'
          },
          {
            title: '名称',
            dataIndex: 'name'
          },
          {
            title: 'Api 地址',
            dataIndex: 'apiTemplate'
          },
          {
            title: '状态',
            dataIndex: 'enable'
          },
          {
            title: '描述',
            dataIndex: 'description'
          },
          {
            title: '创建时间',
            dataIndex: 'createTime'
          }, {
            title: '操作',
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      }
    }
  },
  components: {
    STable
  },
  methods: {
    /**
     * 加载数据方法
     */
    LoadDataing () {
      this.table.loading = true
      getPublicApiList(this.queryParam).then(res => {
        this.table.loading = false
        if (res.status === 200) {
          this.table.dataSource = res.result
        } else {
          this.$notification('加载失败', res.message)
        }
      }).catch(() => {
        this.table.loading = false
      })
    }
  },
  created () {
    this.LoadDataing()
  }
}
</script>

<style lang="less" scoped>
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
