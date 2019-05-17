// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import { RoleList, PermissionList, PermissionDetails, AuditList, AdminList, PublicApiList } from '@/views/control'

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
      children: [{
        path: '/dashboard/analysis',
        name: 'Analysis',
        component: () => import('@/views/dashboard/Analysis')
      },
      {
        path: '/dashboard/workplace',
        name: 'Workplace',
        component: () => import('@/views/dashboard/Workplace')
      }]
    },
    // control
    {
      path: '/control',
      name: 'asf',
      redirect: '/control/administrator',
      component: PageView,
      children: [
        // 管理员列表
        {
          path: '/control/administrator',
          name: 'asf_account',
          component: AdminList
        },
        // 角色列表
        {
          path: '/control/role',
          name: 'asf_role',
          component: RoleList
        },
        // 权限列表
        {
          path: '/control/permission',
          name: 'asf_permission',
          redirect: '/control/permission/list',
          component: RouteView,
          hideChildrenInMenu: true,
          children: [
            {
              path: '/control/permission/details',
              name: 'asf_permission_details',
              hidden: true,
              component: PermissionDetails
            },
            {
              path: '/control/permission/list',
              name: 'asf_permission_list',
              hidden: true,
              component: PermissionList
            }
          ]
        },
        // 审计管理
        {
          path: '/control/audit',
          name: 'asf_audit',
          component: AuditList
        },
        // 公共API管理
        {
          path: '/control/publicapi',
          name: 'asf_publicapi',
          component: PublicApiList
        }
      ]
    }
  ]
}]

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
        component: resolve => require(['@/views/user/Login'], resolve)
      }]
  },
  {
    path: '*',
    redirect: '/404'
  },
  // 异常页面
  {
    path: '/exception',
    component: BasicLayout,
    children: [{
      path: '/404',
      name: '404',
      component: () => import('@/views/exception/404'),
      meta: { title: '404' }
    }, {
      path: '/403',
      name: '403',
      component: () => import('@/views/exception/403'),
      meta: { title: '403' }
    },
    {
      path: '/500',
      name: '500',
      component: () => import('@/views/exception/500'),
      meta: { title: '500' }
    }]
  },
  // 个人页面
  {
    path: '/profiles',
    component: BasicLayout,
    hidden: true,
    name: 'profiles',
    meta: { title: '个人页', icon: 'user' },
    children: [{
      path: '/profiles/profiles',
      name: 'center',
      component: () => import('@/views/profiles/center/Index'),
      meta: { title: '个人中心' }
    },
    {
      path: '/profiles/settings',
      name: 'settings',
      component: () => import('@/views/profiles/settings/Index'),
      redirect: '/profiles/settings/Security',
      children: [{
        path: '/profiles/settings/Security',
        name: 'SecuritySettings',
        component: () => import('@/views/profiles/settings/Security'),
        meta: { title: '账户设置' }
      },
      {
        path: '/profiles/settings/custom',
        name: 'CustomSettings',
        component: () => import('@/views/profiles/settings/Custom'),
        meta: { title: '个性化设置' }
      },
      {
        path: '/profiles/settings/notification',
        name: 'NotificationSettings',
        component: () => import('@/views/profiles/settings/Notification'),
        meta: { title: '新消息通知' }
      }]
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
  }]
