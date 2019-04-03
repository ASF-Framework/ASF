// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import { RoleList, PermissionList, PermissionDetails, AuditList, AdminList } from '@/views/control'

export const asyncRouterMap = [{
  path: '/',
  name: 'index',
  component: BasicLayout,
  meta: { title: '首页' },
  redirect: '/account/center',
  children: [
    // dashboard
    {
      path: '/dashboard',
      name: 'dashboard',
      redirect: '/dashboard/analysis',
      component: RouteView,
      meta: { title: '仪表盘', keepAlive: true, icon: 'dashboard', permission: ['asf'] },
      children: [{
        path: '/dashboard/analysis',
        name: 'Analysis',
        component: () => import('@/views/dashboard/Analysis'),
        meta: { title: '分析页', keepAlive: true, permission: ['asf'] }
      },
      {
        path: '/dashboard/workplace',
        name: 'Workplace',
        component: () => import('@/views/dashboard/Workplace'),
        meta: { title: '工作台', keepAlive: true, permission: ['asf'] }
      }]
    },
    // control
    {
      path: '/control',
      name: 'control',
      redirect: '/control/administrator',
      component: PageView,
      meta: { title: '控制面板', icon: 'table', permission: ['asf'] },
      children: [
        // 管理员列表
        {
          path: '/control/administrator',
          name: 'administrator',
          component: AdminList,
          meta: { title: '管理员', keepAlive: true, permission: ['asf_account'] }
        },
        // 角色列表
        {
          path: '/control/role',
          name: 'role',
          component: RoleList,
          meta: { title: '角色管理', keepAlive: true, permission: ['asf_role'] }
        },
        // 权限列表
        {
          path: '/control/permission',
          name: 'permission',
          redirect: '/control/permission/list',
          component: RouteView,
          hideChildrenInMenu: true,
          meta: { title: '权限管理', permission: 'asf_permission', keepAlive: true },
          children: [
            {
              path: '/control/permission/details',
              name: 'permissionDetail',
              hidden: true,
              component: PermissionDetails,
              meta: { title: '权限详情', hidden: true, permission: 'asf_permission', keepAlive: false }
            },
            {
              path: '/control/permission/list',
              name: 'PermissionList',
              hidden: true,
              component: PermissionList,
              meta: { title: '权限列表', keepAlive: true, hidden: true, permission: 'asf_permission' }
            }
          ]
        },
        // 审计管理
        {
          path: '/control/audit',
          name: 'Audit',
          component: AuditList,
          meta: { title: '审计管理', keepAlive: true, permission: ['asf_logging'] }
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
      children: [{
        path: '/account/center',
        name: 'center',
        component: () => import('@/views/account/center/Index'),
        meta: { title: '个人中心', keepAlive: true }
      },
      {
        path: '/account/settings',
        name: 'settings',
        component: () => import('@/views/account/settings/Index'),
        meta: { title: '个人设置', hideHeader: true, keepAlive: true },
        redirect: '/account/settings/Security',
        alwaysShow: true,
        children: [{
          path: '/account/settings/Security',
          name: 'SecuritySettings',
          component: () => import('@/views/account/settings/Security'),
          meta: { title: '账户设置', hidden: true, keepAlive: true }
        },
        {
          path: '/account/settings/custom',
          name: 'CustomSettings',
          component: () => import('@/views/account/settings/Custom'),
          meta: { title: '个性化设置', hidden: true, keepAlive: true }
        },
        {
          path: '/account/settings/notification',
          name: 'NotificationSettings',
          component: () => import('@/views/account/settings/Notification'),
          meta: { title: '新消息通知', hidden: true, keepAlive: true }
        }]
      }]
    }]
},
{
  path: '*',
  redirect: '/404',
  hidden: true
}]

/**
 * 基础路由
 * @type { *[] }
 */
export const constantRouterMap = [{
  path: '/user',
  component: UserLayout,
  redirect: '/user/login',
  hidden: true,
  children: [
    {
      path: 'login',
      name: 'login',
      component: resolve => require(['@/views/user/Login'], resolve)
    }]
},
{
  path: '/test',
  component: BlankLayout,
  redirect: '/test/home',
  children: [
    {
      path: 'home',
      name: 'TestHome',
      component: () => import('@/views/Home')
    }
  ]
},
{
  path: '/404',
  component: () => import('@/views/404')
}]
