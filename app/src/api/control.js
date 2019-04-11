import { post } from '@/utils/request'

const api = {
  getPublicApiList: '/asf/permission/GetOpenApiList',
  createPublicApi: '/asf/permission/CreateOpenApi',
  editPublicApi: '/asf/permission/ModifyOpenApi',
  deletePublicApi: '/asf/permission/Delete',

  getAuditList: '/asf/Logger/GetList',
  deleteAudit: 'asf/Logger/Delete'
}
export default api

// 公共API列表
export function getPublicApiList (parameter) {
  return post(api.getPublicApiList, parameter, { errorRedirect: true })
}
// 创建 公共API
export function createPublicApi (parameter) {
  return post(api.createPublicApi, parameter)
}
// 删除 公共API
export function deletePublicApi (parameter) {
  return post(api.deletePublicApi + `/${parameter}`)
}
// 修改 公共API
export function editPublicApi (parameter) {
  return post(api.editPublicApi, parameter)
}

// 获取审计集合
export function getAuditList (parameter) {
  return post(api.getAuditList, parameter, { errorRedirect: true })
}
// 根据日期删除日志
export function deleteAudit (parameter) {
  return post(api.deleteAudit, parameter)
}
