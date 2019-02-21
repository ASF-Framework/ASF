import { axios } from '@/utils/request'

const api = {
  admin: '/asf/account/getlist',
  userdetail: '/user/detail',
  role: '/asf/role/getlist',
  roleAll:'/asf/role/getlistAll',
  createAccount:'/asf/account/create',
  modifyStatusAccount:'/asf/account/midifystatus',
  deleteAccount:'/asf/account/delete',
  service: '/service',
  permission: '/asf/permission/getmenulist',
  permissionNoPager: '/permission/no-pager',
  orgTree: '/org/tree'
}

export default api


//管理员列表
export function getAdminList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.admin,parameter ).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//角色列表
export function getRoleList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.role, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}
//角色全部列表
export function getRoleListAll(){
  return new Promise((resolve, reject) => {
    axios.get(api.roleAll).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//权限列表
export function getPermissions (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.permission, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//新增管理员
export function createAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.createAccount, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

//修改管理员状态
export function modifyStatusAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.modifyStatusAccount, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}


//删除管理员
export function deleteAccount (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.deleteAccount+'/'+parameter).then(res => {
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
