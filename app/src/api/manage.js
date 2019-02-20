import { axios } from '@/utils/request'

const api = {
  admin: '/asf/account/list',
  userdetail: '/user/detail',
  role: '/asf/role/list',
  service: '/service',
  permission: '/permission',
  permissionNoPager: '/permission/no-pager',
  orgTree: '/org/tree'
}

export default api

export function getUserDetail (parameter) {
  return axios({
    url: api.userdetail,
    method: 'get',
    params: parameter
  })
}

export function getAdminList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.admin,parameter ).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

export function getRoleList (parameter) {
  return new Promise((resolve, reject) => {
    axios.post(api.role, parameter).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err)
    })
  })
}

export function getServiceList (parameter) {
  return axios({
    url: api.service,
    method: 'get',
    params: parameter
  })
}

export function getPermissions (parameter) {
  return axios({
    url: api.permissionNoPager,
    method: 'get',
    params: parameter
  })
}

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
