<template>
  <a-table
    ref="table"
    :columns="tables.columns"
    :dataSource="tables.dataSource"
    :expandedRowKeys="tables.expandedRowKeys"
    :pagination="false"
    :loading="loading"
    @expand="onTableExpand">
    <span slot="allSelect" slot-scope="text, record" >
      <a-checkbox
        v-if="Object.keys(record.actions).length>0 "
        :checked="record.checkAll"
        :indeterminate="record.checkboxIndeterminate"
        @change="onCheckAllChange($event,record)"/>
    </span>
    <span slot="assign" slot-scope="text, record" >
      <a-checkbox-group
        :options="record.actions | ConvertCheckboxGroupValue"
        v-model="record.selectOption"
        @change="onChange(record)" />
    </span>
  </a-table>
</template>

<script>
import { getMenuSimpleAll } from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      loading: false,
      tables: {
        dataSource: [],
        expandedRowKeys: [],
        // 表头
        columns: [
          {
            title: '导航',
            dataIndex: 'name',
            key: 'name',
            width: '20%'
          },
          {
            title: '全选',
            align: 'center',
            width: '60px',
            scopedSlots: {
              customRender: 'allSelect'
            }
          },
          {
            title: '权限',
            scopedSlots: {
              customRender: 'assign'
            }
          }
        ]
      },
      selectValue: {}
    }
  },
  props: {
    // eslint-disable-next-line vue/require-default-prop
    defaultValue: Array,
    createdLoad: {
      type: Boolean,
      default: true
    }
  },
  created () {
    if (this.createdLoad) {
      this.loadData()
    }
  },
  filters: {
    ConvertCheckboxGroupValue (data) {
      const newData = []
      for (const key in data) {
        newData.push({
          label: data[key],
          'value': key
        })
      }
      return newData
    }
  },
  methods: {
    /**
     *加载列表数据
     */
    loadData () {
      this.loading = true
      getMenuSimpleAll().then(res => {
        this.loading = false
        if (res.status === 200) {
          this.$nextTick(() => {
            this.tables.dataSource = this.buildTreeData(res.result)
          })
        } else {
          this.$notification.error({ message: `获取需要分配权限失败`, description: res.message })
        }
      }).catch(() => { this.loading = false })
    },
    /**
     *全选 checkbox
     */
    onCheckAllChange (e, record) {
      record.checkboxIndeterminate = false
      if (e.target.checked) {
        record.checkAll = true
        record.selectOption = Object.keys(record.actions)
      } else {
        record.checkAll = false
        record.selectOption = []
      }
      this.selectValue[record.key] = record.selectOption
    },
    /**
     * 选择 checkbox
     */
    onChange (record) {
      if (record.selectOption.length === Object.keys(record.actions).length) {
        record.checkboxIndeterminate = false
        record.checkAll = true
      } else {
        record.checkAll = false
        record.checkboxIndeterminate = record.selectOption.length > 0
      }
      this.selectValue[record.key] = record.selectOption
    },
    /**
     * 展开列表树形数据
     */
    onTableExpand (expanded, record) {
      if (expanded) {
        this.tables.expandedRowKeys.push(record.key)
      } else {
        this.tables.expandedRowKeys.forEach((key, index) => {
          if (key === record.key) {
            this.tables.expandedRowKeys.splice(index, 1)
          }
        })
      }
    },
    /**
     * 组装数据源
     * @param {Array} data 数据
     * @param {String} parentId 父级编号
     */
    buildTreeData (data, parentId = '') {
      const childrens = []
      data.forEach(value => {
        value.parentId = value.parentId || ''
        if (value.parentId === parentId) {
          const children = this.buildTreeData(data, value.id)
          value.key = value.id

          value.children = children.length > 0 ? children : null
          value.selectOption = []

          // 默认值赋值
          this.selectValue[value.id] = []
          Object.keys(value.actions).forEach(key => {
            if (this.defaultValue && this.defaultValue.includes(key)) {
              this.selectValue[value.id].push(key)
              value.selectOption.push(key)
            }
          })
          value.checkAll = Object.keys(value.actions).length === value.selectOption.length
          if (!value.checkAll) {
            value.checkboxIndeterminate = value.selectOption.length > 0
          }
          this.tables.expandedRowKeys.push(value.key)
          childrens.push(value)
        }
      })
      return childrens
    }
  }
}
</script>
