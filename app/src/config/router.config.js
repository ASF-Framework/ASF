/* eslint-disable */
import {
    UserLayout,
    BasicLayout,
    RouteView,
    BlankLayout,
    PageView
} from '@/components/layouts'
import Forbidden from '@/views/exception/403'
// import { reject, resolve } from 'node_modules/@types/q/index';

export const asyncRouterMap = [{
        path: '/',
        name: 'index',
        component: BasicLayout,
        meta: {
            title: '首页'
        },
        redirect: '/account/center',
        children: [
            // dashboard
            {
                path: '/dashboard',
                name: 'dashboard',
                redirect: '/control/user-list',
                component: RouteView,
                meta: {
                    title: '仪表盘',
                    keepAlive: true,
                    icon: 'dashboard',
                    permission: ['asf']
                },
                children: [{
                        path: '/dashboard/analysis',
                        name: 'Analysis',
                        component: () =>
                            import ('@/views/dashboard/Analysis'),
                        meta: {
                            title: '分析页',
                            keepAlive: true,
                            permission: ['asf']
                        }
                    },
                    {
                        path: '/dashboard/workplace',
                        name: 'Workplace',
                        component: () =>
                            import ('@/views/dashboard/Workplace'),
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
                    //管理员列表
                    {
                        path: '/control/user-list',
                        name: 'UserList',
                        component: () =>
                            import ('@/views/control/UserList'),
                        meta: {
                            title: '管理员列表',
                            keepAlive: true,
                            permission: ['asf_account']
                        }
                    },
                    //角色列表
                    {
                        path: '/control/role-list',
                        name: 'RoleList',
                        component: () =>
                            import ('@/views/control/RoleList'),
                        meta: {
                            title: '角色列表',
                            permission: ['asf_role']
                        }
                    },
                    //权限列表
                    {
                        path: '/control/permission-list',
                        name: 'PermissionList',
                        component: () =>
                            import ('@/views/control/PermissionList'),
                        meta: {
                            title: '权限列表',
                            permission: 'asf_permission',
                        }
                    },

                    {
                        path: '/control/permission/details',
                        name: 'PermissionDetail',

                        hidden: true,
                        component: () =>
                            import ('@/views/control/permission/details'),
                        meta: {

                            title: '权限详情',
                            keepAlive: true,
                            hidden: true,
                            hiddenHeaderContent: true
                        }
                    },

                    // 日志管理
                    {
                        path: '/control/Logger',
                        name: 'Logtable',
                        component: () =>
                            import ('@/views/control/Logger'),
                        meta: {
                            title: '日志管理',
                            permission: ['asf']
                        }
                    }

                ]
            },
            // Exception
            {
                path: '/exception',
                name: 'exception',
                hidden: true,
                component: RouteView,
                redirect: '/exception/403',
                meta: {
                    title: '异常页',
                    icon: 'warning'
                },
                children: [{
                        path: '/exception/403',
                        name: 'Exception403',
                        component: () =>
                            import ( /* webpackChunkName: "fail" */ '@/views/exception/403'),
                        meta: {
                            title: '403'
                        }
                    },
                    {
                        path: '/exception/404',
                        name: 'Exception404',
                        component: () =>
                            import ( /* webpackChunkName: "fail" */ '@/views/exception/404'),
                        meta: {
                            title: '404'
                        }
                    },
                    {
                        path: '/exception/500',
                        name: 'Exception500',
                        component: () =>
                            import ( /* webpackChunkName: "fail" */ '@/views/exception/500'),
                        meta: {
                            title: '500'
                        }
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
                        component: () =>
                            import ('@/views/account/center/Index'),
                        meta: {
                            title: '个人中心',
                            keepAlive: true
                        }
                    },
                    {
                        path: '/account/settings',
                        name: 'settings',
                        component: () =>
                            import ('@/views/account/settings/Index'),
                        meta: {
                            title: '个人设置',
                            hideHeader: true,
                            keepAlive: true
                        },
                        redirect: '/account/settings/Security',
                        alwaysShow: true,
                        children: [{
                                path: '/account/settings/Security',
                                name: 'SecuritySettings',
                                component: () =>
                                    import ('@/views/account/settings/Security'),
                                meta: {
                                    title: '账户设置',
                                    hidden: true,
                                    keepAlive: true
                                }
                            },
                            {
                                path: '/account/settings/custom',
                                name: 'CustomSettings',
                                component: () =>
                                    import ('@/views/account/settings/Custom'),
                                meta: {
                                    title: '个性化设置',
                                    hidden: true,
                                    keepAlive: true
                                }
                            },

                            {
                                path: '/account/settings/notification',
                                name: 'NotificationSettings',
                                component: () =>
                                    import ('@/views/account/settings/Notification'),
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
export const constantRouterMap = [{
        path: '/user',
        component: UserLayout,
        redirect: '/user/login',
        hidden: true,
        children: [{
                path: 'login',
                name: 'login',
                component: resolve => require(['@/views/user/Login'], resolve)
                    // component: () => import(/* webpackChunkName: "user" */ '@/views/user/Login')
            },
            {
                path: 'register',
                name: 'register',
                component: resolve => require(['@/views/user/Register'], resolve)
                    // component: () => import(/* webpackChunkName: "user" */ '@/views/user/Register')
            },
            {
                path: 'register-result',
                name: 'registerResult',
                component: resolve => require(['@/views/user/RegisterResult'], resolve)
                    // component: () => import(/* webpackChunkName: "user" */ '@/views/user/RegisterResult')
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
            component: () =>
                import ('@/views/Home')
        }]
    },

    {
        path: '/404',
        component: () =>
            import ( /* webpackChunkName: "fail" */ '@/views/exception/404')
    }
]