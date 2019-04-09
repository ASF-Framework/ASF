import Vue from 'vue'
import axios from 'axios'
import store from '@/store'
import router from '@/router'
import { VueAxios } from './axios'
import notification from 'ant-design-vue/es/notification'
import { ACCESS_TOKEN } from '@/store/mutation-types'

// 创建 axios 实例
const service = axios.create({
  baseURL: '/api', // api base_url
  timeout: 6000 // 请求超时时间
})

const err = (error) => {
  if (error.response) {
    if (error.response.config && error.response.config.errorIntercept) {
      switch (error.response.status) {
        case 500:
          notification.warning({ message: '请求失败', description: '抱歉，服务器出错了' })
          router.push({ path: '/500' })
          break
        case 404:
          notification.warning({ message: '404', description: '抱歉，你访问的页面不存在或仍在开发中' })
          router.push({ path: '/404' })
          break
        case 403:
          notification.warning({ message: '拒绝访问', description: '抱歉，你无权访问该页面' })
          router.push({ path: '/403' })
          break
        case 401:
          notification.error({ message: '登录过期', description: '登录已经过期，请重新登录' })
          store.dispatch('Logout').then(() => {
            setTimeout(() => {
              window.location.reload()
            }, 1500)
          })
          break
        default:
          break
      }
    }
  }
  return Promise.reject(error)
}

service.defaults.headers.post['Content-Type'] = 'application/json;charset=UTF-8'
// request interceptor
service.interceptors.request.use(config => {
  const token = Vue.ls.get(ACCESS_TOKEN)
  if (token) {
    config.headers[ 'Authorization' ] = 'Bearer ' + token // 让每个请求携带自定义 token 请根据实际情况自行修改
  }
  return config
}, err)

// response interceptor
service.interceptors.response.use((response) => {
  return response.data
}, err)

const installer = {
  vm: {},
  install (Vue, router = {}) {
    Vue.use(VueAxios, router, service)
  }
}

export {
  installer as VueAxios,
  service as axios
}
