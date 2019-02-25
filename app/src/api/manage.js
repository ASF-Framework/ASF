import { axios } from '@/utils/request'

const api = {
  admin: '/asf/account/getlist',
  userdetail: '/user/detail',
  role: '/asf/role/getlist',
  addRole:'/asf/role/Create',
  editRole:'/asf/role/Modify',
  roleAll:'/asf/role/getlistAll',
  modifyStatusRole:'/asf/role/ModifyStatus',
  deleteRole:'/asf/role/delete',
  getRoleDetail:'/asf/role/GetDetails',
  createAccount:'/asf/account/create',
  modifyStatusAccount:'/asf/account/midifystatus',
  deleteAccount:'/asf/account/delete',
  modifyAccount:'/asf/account/midify',
  getAccountDetail:'/asf/account/GetDetails',
  service: '/service',
  permission: '/asf/permission/getmenulist',
  permissionAll:'/asf/permission/GetMenuAllList',
  permissionNoPager: '/permission/no-pager',
  orgTree: '/org/tree',
  getActionDetails: '/asf/Permission/GetActionDetails',
  modifyAction: '/asf/Permission/ModifyAction',
  modifySort: '/asf/Permission/ModifySort',
  getMenuDetails: '/asf/Permission/GetMenuDetails',
  getDetails: '/asf/Permission/GetActionDetails',
  getActionList: '/asf/Permission/GetActionList',
  deleteAction: '/asf/Permission/Delete',
  CreateAction: '/asf/Permission/CreateAction',
  CreateMenu: '/asf/Permission/CreateMenu'
}

export default api

// 管理员列表
export function getAdminList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.admin, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 角色列表
export function getRoleList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.role, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 角色全部列表
export function getRoleListAll () {
  return new Promise((resolve, reject) => {
    axios.get(api.roleAll).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 权限列表
export function getPermissions (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.permission, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//获取全部权限
export function getPermissionAll (parameter) {
  return new Promise((resolve, reject) => {
    axios.get(api.permissionAll).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 根据ID获取操作权限详情
export function getActionDetails (id) {
  return new Promise((resolve, reject) => {
    axios.get(api.getActionDetails + `/${id}`).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
export function CreateAction (id) {
  return new Promise((resolve, reject) => {
    axios.post(api.CreateAction, id).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 创建导航权限
export function CreateMenu (id) {
  return new Promise((resolve, reject) => {
    axios.post(api.CreateMenu, id).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 根据ID获取导航权限详情
export function getMenuDetails (id) {
  return new Promise((resolve, reject) => {
    axios.get(api.getMenuDetails + `/${id}`).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 根据ID获取操作权限详情
export function getDetails (id) {
  return new Promise((resolve, reject) => {
    axios.get(api.getDetails + `/${id}`).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
export function getActionList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.getActionList, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

export function deleteAction (id) {
  return new Promise((resolve, reject) => {
    axios.post(api.deleteAction + `/${id}`).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 修改操作权限
export function modifyAction (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifyAction, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 修改排序
export function modifySort (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifySort, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 新增管理员
export function createAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.createAccount, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 修改管理员状态
export function modifyStatusAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifyStatusAccount, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 删除管理员
export function deleteAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.deleteAccount + '/' + parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
// 修改管理员
export function modifyAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifyAccount, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

// 获取管理员详情
export function getAccountDetail (parameter) {
  return new Promise((resolve, reject) => {
    axios.get(api.getAccountDetail + '/' + parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//----------角色管理-----------
//添加角色
export function addRole (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.addRole,parameter,).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//修改角色状态
export function modifyStatusRole (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifyStatusRole, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//删除角色
export function deleteRole (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.deleteRole+'/'+parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//编辑角色
export function editRole (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.editRole,parameter,).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
//获取角色详情
export function getRoleDetail (parameter) {
  return new Promise((resolve, reject) => {
    axios.get(api.getRoleDetail+'/'+parameter,).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//--------------下面是旧的 -------------------//
export function getUserDetail (parameter) {
  return axios({
    url: api.userdetail,
    method: 'get',
    params: parameter
  })
}

export function getServiceList (parameter) {
  return axios({
    url: api.service,
    method: 'get',
    params: parameter
  })
}

// export function getPermissions (parameter) {
//   return axios({
//     url: api.permissionNoPager,
//     method: 'get',
//     params: parameter
//   })
// }

export function getOrgTree (parameter) {
  return axios({
    url: api.orgTree,
    method: 'get',
    params: parameter
  })
}

// id == 0 add     post
// id != 0 update  put
export function saveService (parameter) {
  return axios({
    url: api.service,
    method: parameter.id === 0 ? 'post' : 'put',
    data: parameter
  })
}
