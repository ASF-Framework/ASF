// eslint-disable-next-line
import {
  UserLayout,
  BasicLayout,
  RouteView,
  BlankLayout,
  PageView
} from '@/components/layouts'

export const asyncRouterMap = [

  {
    path: '/',
    name: 'index',
    component: BasicLayout,
    meta: {
      title: '首页'
    },
    redirect: '/dashboard/workplace',
    children: [
      // dashboard
      {
        path: '/dashboard',
        name: 'dashboard',
        redirect: '/dashboard/workplace',
        component: RouteView,
        meta: {
          title: '仪表盘',
          keepAlive: true,
          icon: 'dashboard',
          permission: ['asf']
        },
        children: [
          {
            path: '/dashboard/analysis',
            name: 'Analysis',
            component: () => import('@/views/dashboard/Analysis'),
            meta: {
              title: '分析页',
              keepAlive: true,
              permission: ['asf']
            }
          },
          {
            path: '/dashboard/workplace',
            name: 'Workplace',
            component: () => import('@/views/dashboard/Workplace'),
            meta: {
              title: '工作台',
              keepAlive: true,
              permission: ['asf']
            }
          }
        ]
      },
      // control
      {
        path: '/control',
        name: 'control',
        component: PageView,
        redirect: '/control/user-list',
        meta: {
          title: '控制面板',
          icon: 'table',
          permission: ['asf']
        },
        children: [
          {
            path: '/control/user-list',
            name: 'UserList',
            component: () => import('@/views/control/UserList'),
            meta: {
              title: '管理员列表',
              keepAlive: true,
              permission: ['asf']
            }
          },
          {
            path: '/control/role-list',
            name: 'RoleList',
            component: () => import('@/views/control/RoleList'),
            meta: {
              title: '角色列表',
              permission: ['asf']
            }
          },
          {
            path: '/control/permission-list',
            name: 'PermissionList',
            component: () => import('@/views/control/PermissionList'),
            meta: {
              title: '权限列表',
              permission: ['asf']
            }
          }
        ]
      },
      // profile
      {
        path: '/profile',
        name: 'profile',
        component: RouteView,
        redirect: '/profile/basic',
        meta: { title: '详情页', icon: 'profile', permission: [ 'profile' ] },
        children: [
          {
            path: '/profile/basic',
            name: 'ProfileBasic',
            component: () => import('@/views/profile/basic/Index'),
            meta: { title: '基础详情页', permission: [ 'profile' ] }
          },
          {
            path: '/profile/advanced',
            name: 'ProfileAdvanced',
            component: () => import('@/views/profile/advanced/Advanced'),
            meta: { title: '高级详情页', permission: [ 'profile' ] }
          }
        ]
      },
      // account
      {
        path: '/account',
        component: RouteView,
        hidden: true,
        name: 'account',
        meta: {
          title: '个人页',
          icon: 'user',
          keepAlive: true
        },
        children: [
          {
            path: '/account/center',
            name: 'center',
            component: () => import('@/views/account/center/Index'),
            meta: {
              title: '个人中心',
              keepAlive: true
            }
          },
          {
            path: '/account/settings',
            name: 'settings',
            component: () => import('@/views/account/settings/Index'),
            meta: {
              title: '个人设置',
              hideHeader: true,
              keepAlive: true
            },
            redirect: '/account/settings/security',
            alwaysShow: true,
            children: [
              {
                path: '/account/settings/base',
                name: 'BaseSettings',
                component: () => import('@/views/account/settings/BaseSetting'),
                meta: {
                  title: '基本设置',
                  hidden: true,
                  keepAlive: true
                }
              },
              {
                path: '/account/settings/security',
                name: 'SecuritySettings',
                component: () => import('@/views/account/settings/Security'),
                meta: {
                  title: '账户设置',
                  hidden: true,
                  keepAlive: true
                }
              },
              {
                path: '/account/settings/custom',
                name: 'CustomSettings',
                component: () => import('@/views/account/settings/Custom'),
                meta: {
                  title: '个性化设置',
                  hidden: true,
                  keepAlive: true
                }
              },
              {
                path: '/account/settings/binding',
                name: 'BindingSettings',
                component: () => import('@/views/account/settings/Binding'),
                meta: {
                  title: '账户绑定',
                  hidden: true,
                  keepAlive: true
                }
              },
              {
                path: '/account/settings/notification',
                name: 'NotificationSettings',
                component: () => import('@/views/account/settings/Notification'),
                meta: {
                  title: '新消息通知',
                  hidden: true,
                  keepAlive: true
                }
              }
            ]
          }
        ]
      }
    ]
  },
  {
    path: '*',
    redirect: '/404',
    hidden: true
  }
]

/**
 * 基础路由
 * @type { *[] }
 */
export const constantRouterMap = [
  {
    path: '/user',
    component: UserLayout,
    redirect: '/user/login',
    hidden: true,
    children: [
      {
        path: 'login',
        name: 'login',
        component: () => import(/* webpackChunkName: "user" */ '@/views/user/Login')
      },
      {
        path: 'register',
        name: 'register',
        component: () => import(/* webpackChunkName: "user" */ '@/views/user/Register')
      },
      {
        path: 'register-result',
        name: 'registerResult',
        component: () => import(/* webpackChunkName: "user" */ '@/views/user/RegisterResult')
      }
    ]
  },

  {
    path: '/test',
    component: BlankLayout,
    redirect: '/test/home',
    children: [{
      path: 'home',
      name: 'TestHome',
      component: () => import('@/views/Home')
    }]
  },

  {
    path: '/404',
    component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404')
  }
]
