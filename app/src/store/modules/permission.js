import { constantRouterMap, asyncRouterMap } from '@/config/router.config'

/**
 * 权限框架存储
 */
const permission = {
  state: {
    routers: constantRouterMap,
    dynamicRouters: [],
    menus: []
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.dynamicRouters = routers
      state.routers = constantRouterMap.concat(routers)
    },
    SET_MENUS: (state, menus) => {
      state.menus = menus
    }
  },
  actions: {
    GenerateRoutes ({ commit }, menuMap) {
      return new Promise(resolve => {
        menuMap = menuMap || asyncRouterMap.find(r => r.path === '/').children
        const menus = BuildMenu(menuMap, asyncRouterMap)
        commit('SET_MENUS', menus)
        const dynamicRouters = FilterAsyncRouter(asyncRouterMap)
        commit('SET_ROUTERS', dynamicRouters)
        console.log(dynamicRouters)
        resolve()
      })
    }
  }
}
export default permission

const menuKeyList = [] // 菜单Key集合,用于筛选动态路由
const whiteList = ['/', '*'] // no redirect whitelist
/**
 * 组装菜单
 * @param {Array} menuMap 菜单集合
 * @param {Array} routerMap 配置路由集合
 * @param {Object} showMenu 展示的菜单，由于有些菜单隐藏，触发菜单时候显示上级的菜单
 */
function BuildMenu (menuMap, routerMap) {
  return menuMap.map(menu => {
    menuKeyList.push(menu.name)
    menu.meta = menu.meta || {}
    menu.meta.title = menu.title || menu.meta.title || '~~' // 菜单标题
    menu.meta.icon = menu.icon || menu.meta.icon || undefined // 菜单图标
    // 寻找配置的路由
    const router = FindRouter(routerMap, menu.name)
    if (router) {
      menu.path = router.path
      // 在路由中注入菜单关联
      router.meta = []
      router.meta.title = menu.meta.title
      router.meta.actions = menu.actions || menu.permission
    } else {
      // 没有找到前端路由，使用菜单的重定向
      if (menu.redirect) {
        menu.meta.target = '_blank'
        menu.path = menu.redirect
      } else {
        menu.hidden = true
      }
    }

    // 组装子菜单
    if (menu.children && menu.children.length) {
      // 检查子菜单是否全部隐藏
      if (menu.children.filter(child => !child.hidden).length === 0) {
        menu.hideChildrenInMenu = true
      }
      menu.children = BuildMenu(menu.children, routerMap)
    } else {
      menu.children = null
    }

    return menu
  })
}

/**
 * 根据 name 在 routerMap 寻找对应路由
 * @param {Array} routerMap 路由配置集合
 * @param {String} name 路由名称
 */
function FindRouter (routerMap, name) {
  if (!Array.isArray(routerMap)) {
    return null
  }
  for (const route of routerMap) {
    if (route.name === name) {
      return route
    }
    if (route.children && route.children.length) {
      const r = FindRouter(route.children, name)
      if (r) { return r }
    }
  }
  return null
}

/**
 * 筛选动态路由
 * @param { Array } routerMap 路由配置
 * RouteConfig 定义：https://router.vuejs.org/zh/api/#routes
 */
function FilterAsyncRouter (routerMap) {
  if (!Array.isArray(routerMap)) {
    return null
  }
  return routerMap.filter(route => {
    if (menuKeyList.includes(route.name) || whiteList.includes(route.path)) {
      if (route.children && route.children.length) {
        route.children = FilterAsyncRouter(route.children)
      }
      return true
    }
    return false
  })
}
