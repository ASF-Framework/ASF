import { TreeSelect } from 'ant-design-vue'
import { getMenuSimpleAll } from '@/api/control'
export default {
  name: 'MenuTreeSelect',
  props: Object.assign({}, TreeSelect.props),
  model: { props: 'value', event: 'change' },
  data () {
    return {
      menuList: [],
      treeExpandedKeyList: []
    }
  },
  created () {
    this.loadDataing()
  },
  methods: {
    /**
     * 加载菜单数据
     */
    loadDataing () {
      getMenuSimpleAll().then(res => {
        if (res.status === 200) {
          if (res.result.length < 15) {
            this.treeExpandedKeyList = res.result.map(f => f.id)
          }
          this.menuList = this.buildTreeData(res.result)
        } else {
          this.$notification.error({ message: '加载菜单失败', description: res.message })
        }
      })
    },
    /**
     * 组装树形数据
     * @param {Array} data 菜单数据
     * @param {String} parentId 父级标识
     */
    buildTreeData (data, parentId = '') {
      const treeDatas = []
      data.forEach(d => {
        if (d.parentId !== parentId) {
          return
        }
        const td = { title: d.name, value: d.id, key: d.id }
        const children = this.buildTreeData(data, d.id)
        td.children = children
        treeDatas.push(td)
      })
      return treeDatas
    }
  },
  render () {
    const _this = this
    const props = {}
    Object.keys(TreeSelect.props).forEach(k => {
      if (_this[k] === undefined) {
        return
      }
      props[k] = _this[k]
    })
    props['treeData'] = _this.menuList
    props['treeExpandedKeys'] = _this.treeExpandedKeyList
    const on = {
      treeExpand: (expandedKeys) => {
        _this.treeExpandedKeyList = expandedKeys
      },
      change: (value, label, extra) => {
        this.$emit('change', value, label, extra)
      }
    }
    return (
      <TreeSelect {...{ props, on: on }}></TreeSelect>
    )
  }
}
