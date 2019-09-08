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

  // 菜单
  getMenuSimpleAll: '/asf/permission/menu/GetAllList',
  getMenuDetails: '/asf/Permission/menu/GetDetails',
  getMenuList: '/asf/permission/menu/GetList',
  createMenu: '/asf/permission/menu/Create',
  modifyMenuSort: '/asf/permission/menu/ModifySort',
  modifyMenu: '/asf/permission/menu/Modify',
  deleteMenu: '/asf/permission/menu/Delete',
  exportMenu: '/asf/permission/menu/Export',
  importMenu:'/asf/permission/menu/Import',

  // 功能
  getActionList: '/asf/Permission/action/GetList',
  getActionDetails: '/asf/Permission/action/GetDetails',
  createAction: '/asf/Permission/action/Create',
  modifyAction: '/asf/Permission/action/Modify',
  modifyActionSort: '/asf/permission/action/ModifySort',
  deleteAction: '/asf/permission/action/Delete',

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
  importPublicApi:'/asf/permission/openapi/Import',

  // 审计
  getAuditList: '/asf/Logger/GetList',
  deleteAudit: 'asf/Logger/Delete'

}
export default api

// 操作功能
export function getActionList (parameter) {
  return post(api.getActionList, parameter, { errorRedirect: true })
}
export function getActionDetails (parameter) {
  return get(api.getActionDetails + '/' + parameter)
}
export function createAction (parameter) {
  return post(api.createAction, parameter)
}
export function modifyAction (parameter) {
  return post(api.modifyAction, parameter)
}
export function modifyActionSort (parameter) {
  return post(api.modifyActionSort, parameter)
}
export function deleteMenu (parameter) {
  return post(api.deleteMenu + '/' + parameter)
}

// 菜单
export function getMenuList (parameter) {
  return post(api.getMenuList, parameter, { errorRedirect: true })
}
export function getMenuDetails (parameter) {
  return get(api.getMenuDetails + '/' + parameter)
}
export function getMenuSimpleAll () {
  return get(api.getMenuSimpleAll)
}
export function modifyMenuSort (parameter) {
  return post(api.modifyMenuSort, parameter)
}
export function createMenu (parameter) {
  return post(api.createMenu, parameter)
}
export function modifyMenu (parameter) {
  return post(api.modifyMenu, parameter)
}
export function deleteAction (parameter) {
  return post(api.deleteAction + '/' + parameter)
}

export function exportMenu (parameter) {
  return post(api.exportMenu ,  parameter)
}

export function importMenu (parameter) {
  return post(api.importMenu ,  parameter)
}

// 角色
export function getRoleSimpleList () {
  return get(api.getRoleSimpleList)
}
export function getRoleDetail (parameter) {
  return get(api.getRoleDetail + '/' + parameter)
}
export function createRole (parameter) {
  return post(api.createRole, parameter)
}
export function modifyRole (parameter) {
  return post(api.modifyRole, parameter)
}
export function getRoleList (parameter) {
  return post(api.getRoleList, parameter, { errorRedirect: true })
}
export function modifyRoleStatus (parameter) {
  return post(api.modifyRoleStatus, parameter)
}
export function deleteRole (parameter) {
  return post(api.deleteRole + '/' + parameter)
}

// 管理员
export function createAccount (parameter) {
  return post(api.createAccount, parameter)
}
export function modifyAccount (parameter) {
  return post(api.modifyAccount, parameter)
}
export function resetPassword (parameter) {
  return post(api.resetPassword, parameter)
}
export function getAccountDetail (parameter) {
  return get(api.getAccountDetail + '/' + parameter)
}
export function getAccountList (parameter) {
  return post(api.getAccountList, parameter, { errorRedirect: true })
}
export function modifyStatusAccount (parameter) {
  return post(api.modifyStatusAccount, parameter)
}
export function deleteAccount (parameter) {
  return post(api.deleteAccount + '/' + parameter)
}

// 公共API
export function getPublicApiList (parameter) {
  return post(api.getPublicApiList, parameter, { errorRedirect: true })
}
export function createPublicApi (parameter) {
  return post(api.createPublicApi, parameter)
}
export function deletePublicApi (parameter) {
  return post(api.deletePublicApi + `/${parameter}`)
}
export function modifyPublicApi (parameter) {
  return post(api.modifyPublicApi, parameter)
}

export function importPublicApi (parameter) {
  return post(api.importPublicApi ,  parameter)
}

// 审计
export function getAuditList (parameter) {
  return post(api.getAuditList, parameter, { errorRedirect: true })
}
export function deleteAudit (parameter) {
  return post(api.deleteAudit, parameter)
}
