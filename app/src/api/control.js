import { axios } from '@/utils/request'

const api = {
  getPublicApiList: '/asf/permission/getOpenApiList',
  getAuditList: '/asf/Logger/GetList',
  auditDelete: 'asf/Logger/Delete'
}
export default api

// 公共API列表
export function getPublicApiList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.getPublicApiList, parameter, { errorRedirect: true }).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 获取审计集合
export function getAuditList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.getAuditList, parameter, { errorRedirect: true }).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 根据日期删除日志
export function auditDelete (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.auditDelete, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
