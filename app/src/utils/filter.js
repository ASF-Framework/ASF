import Vue from 'vue'
import moment from 'moment'
import 'moment/locale/zh-cn'
moment.locale('zh-cn')

Vue.filter('NumberFormat', function (value) {
  if (!value) {
    return '0'
  }
  const intPartFormat = value.toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,') // 将整数部分逢三一断
  return intPartFormat
})

Vue.filter('dayjs', function (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
  return moment(dataStr).format(pattern)
})

Vue.filter('moment', function (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
  return moment(dataStr).format(pattern)
})

// 时间戳格式化
Vue.filter('dayFormat', function (dataStr, pattern) {
  if (!dataStr) return ''
  const date = dataStr.toString().length
  if (date === 10) {
    return moment(dataStr * 1000).format(pattern)
  } else {
    return moment(dataStr).format(pattern)
  }
})
// 转换是否启用文字
Vue.filter('statusFilter', function (value) {
  if (value === null) return ''
  const statusMap = {
    1: '启用',
    0: '停止'
  }
  if (typeof value === 'boolean') {
    value = value ? 1 : 0
  }
  return statusMap[value ? 1 : 0]
})

Vue.filter('statusIsFilter', function (value) {
  if (value === null) return ''
  const statusMap = {
    1: '是',
    0: '否'
  }
  if (typeof value === 'boolean') {
    value = value ? 1 : 0
  }
  return statusMap[value ? 1 : 0]
})
