import axios from 'axios'
import store from '@/store'
import notification from 'ant-design-vue/es/notification'
import { ACCESS_TOKEN } from '@/store/mutation-types'
const http = {}
axios.defaults.headers.post['Content-Type'] = 'application/json;charset=UTF-8';
var instance = axios.create({
  timeout: 6000,
  baseURL:'/api'
})
instance.defaults.headers.post['Content-Type'] = 'application/json;charset=UTF-8';

const err = (error) => {
    if (error.response) {
      const data = error.response.data
      const token = Vue.ls.get(ACCESS_TOKEN)
      if (error.response.status === 403) {
        notification.error({ message: 'Forbidden', description: data.message })
      }
      if (error.response.status === 401) {
        notification.error({ message: 'Unauthorized', description: 'Authorization verification failed' })
        if (token) {
          store.dispatch('Logout').then(() => {
            setTimeout(() => {
              window.location.reload()
            }, 1500)
          })
        }
      }
    }
    return Promise.reject(error)
  }
// 添加请求拦截器
instance.interceptors.request.use(
    config => {
        const token = Vue.ls.get(ACCESS_TOKEN)
        if (token) {
          config.headers[ 'Authorization' ] = 'Bearer ' + token // 让每个请求携带自定义 token 请根据实际情况自行修改
        }
        return config
      }, err
)

// 响应拦截器即异常处理
instance.interceptors.response.use(
  response => {
      console.log('faefaefsaefefae')
    return response.data
  },
  err => {
    if (err && err.response) {
      switch (err.response.status) {
        case 400:
          err.message = '请求出错'
          window.history.go(-1);
          break
        case 401:
          err.message = '授权失败，请重新登录'
          store.commit('LOGIN_OUT')
          setTimeout(() => {
            window.location.reload()
          }, 1000)

          return
        case 403:
          err.message = '拒绝访问'
          break
        case 404:
          err.message = '请求错误,未找到该资源'
          setTimeout(() => {
            window.location.href="/home";
          }, 2000)          
          break
        case 500:
          err.message = '服务器端出错'
          window.history.go(-1);
          break
      }
    } else {
      err.message = '连接服务器失败'
    }
    notification.error({ message: '错误提示', description: err.message })
    return Promise.reject(err.response)
  }
)

/* axios请求二次封装 */
http.get = function (url, options) {
  return new Promise((resolve, reject) => {
    instance
      .get(url, options)
      .then(response => {

        if (response.status === "success") {
          resolve(response)
        } else {
          notification.error({ message: 'Forbidden', description: response.message })
          reject(response.message)
        }
      })
      .catch(e => {
        console.log(e)
      })
  })
}

/* axios请求二次封装 */
http.post = function (url, data, options) {
  return new Promise((resolve, reject) => {
    instance
      .post(url, data, options)
      .then(response => {
          console.log(999999999)
        if (response.status === "success") {
          resolve(response)
        } else {
          notification.error({ message: 'Forbidden', description: response.message })
          reject(response.message)
        }
      })
      .catch(e => {
        console.log(e)
      })
  })
}
export default http
