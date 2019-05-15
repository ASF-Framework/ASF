import { post, get } from '@/utils/request'

const api = {
  // 管理员
  createAccount: '/asf/account/create',
  modifyAccount: '/asf/account/midify',
  modifyStatusAccount: '/asf/account/midifystatus',
  getAccountDetail: '/asf/account/GetDetails',
  getAccountList: '/asf/account/getlist',
  resetPassword: '/asf/account/ResetPassword',
  deleteAccount: '/asf/account/delete',

  // 权限
  getPermissionMenuSimpleAll: '/asf/permission/GetMenuAllList',

  // 角色
  getRoleSimpleList: '/asf/role/GetListAll',
  getRoleDetail: '/asf/role/GetDetails',
  createRole: '/asf/role/Create',
  modifyRole: '/asf/role/Modify',
  deleteRole: '/asf/role/delete',
  modifyRoleStatus: '/asf/role/ModifyStatus',
  getRoleList: '/asf/role/getlist',

  // 公共API
  getPublicApiList: '/asf/permission/openapi/GetList',
  createPublicApi: '/asf/permission/openapi/Create',
  modifyPublicApi: '/asf/permission/openapi/Modify',
  deletePublicApi: '/asf/permission/openapi/Delete',

  // 审计
  getAuditList: '/asf/Logger/GetList',
  deleteAudit: 'asf/Logger/Delete'

}
export default api

// 获取简单角色计划
export function getRoleSimpleList () {
  return get(api.getRoleSimpleList)
}
// 获取角色详情
export function getRoleDetail (parameter) {
  return get(api.getRoleDetail + '/' + parameter)
}
// 创建 角色
export function createRole (parameter) {
  return post(api.createRole, parameter)
}
// 修改 角色
export function modifyRole (parameter) {
  return post(api.modifyRole, parameter)
}
// 获取 角色集合
export function getRoleList (parameter) {
  return post(api.getRoleList, parameter, { errorRedirect: true })
}
// 修改 角色状态
export function modifyRoleStatus (parameter) {
  return post(api.modifyRoleStatus, parameter)
}
// 删除 角色
export function deleteRole (parameter) {
  return post(api.deleteRole + '/' + parameter)
}

// 创建 管理员
export function createAccount (parameter) {
  return post(api.createAccount, parameter)
}
// 修改 管理员
export function modifyAccount (parameter) {
  return post(api.modifyAccount, parameter)
}
// 重置 管理员密码
export function resetPassword (parameter) {
  return post(api.resetPassword, parameter)
}
// 获取 管理员详情
export function getAccountDetail (parameter) {
  return get(api.getAccountDetail + '/' + parameter)
}
// 获取 管理员集合
export function getAccountList (parameter) {
  return post(api.getAccountList, parameter, { errorRedirect: true })
}
// 修改 管理员状态
export function modifyStatusAccount (parameter) {
  return post(api.modifyStatusAccount, parameter)
}
// 删除 管理员
export function deleteAccount (parameter) {
  return post(api.deleteAccount + '/' + parameter)
}

// 获取 导航权限简单信息
export function getPermissionMenuSimpleAll () {
  return get(api.getPermissionMenuSimpleAll)
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
