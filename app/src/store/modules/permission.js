import { asyncRouterMap, constantRouterMap } from '@/config/router.config'

/**
 * 过滤账户是否拥有某一个权限，并将菜单从加载列表移除
 *
 * @param permission
 * @param route
 * @returns {boolean,object}
 */
function hasPermission (permission, route) {
  const res = {
    success: false,
    permission: null
  }
  if (route.meta && route.meta.permission) {
    let flag = false
    for (let i = 0, len = permission.length; i < len; i++) {
      const permissionId = route.meta.permission instanceof String && [route.meta.permission] || route.meta.permission
      flag = permissionId.includes(permission[i].id)
      if (flag) {
        res.permission = permission[i]
        res.success = true
      }
    }
  } else {
    res.success = true
  }
  return res
}

/**
 * 单账户多角色时，使用该方法可过滤角色不存在的菜单
 *
 * @param roles
 * @param route
 * @returns {*}
 */
// eslint-disable-next-line
function hasRole(roles, route) {
  if (route.meta && route.meta.roles) {
    return route.meta.roles.includes(roles.id)
  } else {
    return true
  }
}

function filterAsyncRouter (routerMap, roles) {
  const accessedRouters = routerMap.filter(route => {
    const res = hasPermission(roles.permissionList, route)
    if (res.success) {
      route.sort = res.permission ? res.permission.sort : 0
      if (route.children && route.children.length) {
        route.children = filterAsyncRouter(route.children, roles)
      }
      return true
    }
    return false
  })
  accessedRouters.sort(by('sort'))
  return accessedRouters
}

// by函数接受一个成员名字符串和一个可选的次要比较函数做为参数
// 并返回一个可以用来包含该成员的对象数组进行排序的比较函数
// 当o[age] 和 p[age] 相等时，次要比较函数被用来决出高下
var by = function (name, minor) {
  return function (o, p) {
    var a, b
    if (o && p && typeof o === 'object' && typeof p === 'object') {
      a = o[name]
      b = p[name]
      if (a === b) {
        return typeof minor === 'function' ? minor(o, p) : 0
      }
      if (typeof a === typeof b) {
        return a < b ? -1 : 1
      }
      return typeof a < typeof b ? -1 : 1
    } else {
      // eslint-disable-next-line no-undef
      thro('error')
    }
  }
}

const permission = {
  state: {
    routers: constantRouterMap,
    addRouters: []
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.addRouters = routers
      state.routers = constantRouterMap.concat(routers)
    }
  },
  actions: {
    GenerateRoutes ({ commit }, data) {
      return new Promise(resolve => {
        const { roles } = data
        const accessedRouters = filterAsyncRouter(asyncRouterMap, roles)
        commit('SET_ROUTERS', accessedRouters)
        resolve()
      })
    }
  }
}

export default permission
