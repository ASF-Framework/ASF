import { post, get } from '@/utils/request'

const api = {
  // 管理员
  createAccount: '/asf/account/create',
  modifyAccount: '/asf/account/midify',
  getAccountDetail: '/asf/account/GetDetails',

  // 角色
  getRoleSimpleList: '/asf/role/GetListAll',

  // 公共API
  getPublicApiList: '/asf/permission/GetOpenApiList',
  createPublicApi: '/asf/permission/CreateOpenApi',
  modifyPublicApi: '/asf/permission/ModifyOpenApi',
  deletePublicApi: '/asf/permission/Delete',

  // 审计
  getAuditList: '/asf/Logger/GetList',
  deleteAudit: 'asf/Logger/Delete'

}
export default api

// 创建 管理员
export function createAccount (parameter) {
  return post(api.createAccount, parameter)
}
// 修改 管理员
export function modifyAccount (parameter) {
  return post(api.modifyAccount, parameter)
}
// 获取 管理员详情
export function getAccountDetail (parameter) {
  return get(api.getAccountDetail + '/' + parameter)
}

// 获取简单角色计划
export function getRoleSimpleList () {
  return get(api.getRoleSimpleList)
}

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
export function modifyPublicApi (parameter) {
  return post(api.modifyPublicApi, parameter)
}

// 获取审计集合
export function getAuditList (parameter) {
  return post(api.getAuditList, parameter, { errorRedirect: true })
}
// 根据日期删除日志
export function deleteAudit (parameter) {
  return post(api.deleteAudit, parameter)
}
