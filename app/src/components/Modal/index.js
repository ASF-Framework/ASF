import M from 'ant-design-vue/es/modal/Modal'
import { Result } from '@/components'
import store from '@/store'

export default {
  name: 'SModal',
  data () {
    return {
      result: {
        isSuccess: false,
        title: '',
        description: ''
      },
      modal: {
        title: this.title,
        footer: this.footer
      },
      complete: false
    }
  },
  components: {
    Result
  },
  props: Object.assign({}, M.props),
  model: {
    prop: ['visible'],
    event: ['cancel', 'ok']
  },
  methods: {
    cancel (e) {
      this.$emit('cancel', e)
      new Promise(resolve => {
        setTimeout(() => { this.complete = false }, 500)
      }).then()
    },
    ok (e) {
      this.$emit('ok', e)
    },
    /**
     * 异常处理
     * @param {*} error Http 请求异常
     */
    exception (error) {
      this.cancel()
      if (error.response) {
        switch (error.response.status) {
          case 500:
            this.$notification.warning({ message: '请求失败', description: '抱歉，服务器出错了' })
            break
          case 404:
            this.$notification.warning({ message: '404', description: '抱歉，你访问的页面不存在或仍在开发中' })
            break
          case 403:
            this.$notification.warning({ message: '拒绝访问', description: '抱歉，你无权访问该页面' })
            break
          case 401:
            this.$notification.error({ message: '登录过期', description: '登录已经过期，请重新登录' })
            store.dispatch('Logout').then(() => {
              setTimeout(() => {
                window.location.reload()
              }, 1500)
            })
            break
          default:
            this.$notification.warning({ message: '处理失败', description: '处理失败，服务器繁忙' })
            break
        }
      }
    },
    /**
     * 执行成功
     * @param {*} title
     * @param {*} description
     */
    success (title, description) {
      this.complete = true
      this.result.isSuccess = true
      this.result.title = title
      this.result.description = description
    },
    /**
     * 执行失败
     * @param {*} title
     * @param {*} description
     */
    error (title, description) {
      this.complete = true
      this.result.isSuccess = false
      this.result.title = title
      this.result.description = description
    },
    // 上一步(重新操作)
    previous () {
      this.complete = false
    },
    // 绘制结果页面
    renderResultContext () {
      const props = {}
      Object.keys(Result.props).forEach(k => {
        if (this.result[k]) {
          props[k] = this.result[k]
        }
        return props[k]
      })
      props['content'] = false

      return (
        <Result v-show={this.complete} { ...{ props }}>
          <template slot="action">
            <a-button style="margin-right:20px;" v-show={!this.result.isSuccess} onClick={this.previous}>上一步</a-button>
            <a-button type="primary" onClick={this.cancel}>知道了</a-button>
          </template>
        </Result>
      )
    },
    // 绘制操作对话框
    renderOperateContext () {
      return (
        Object.keys(this.$slots).map(name => (
          <template slot={name}>
            <div v-show={!this.complete}>
              {this.$slots[name]}
            </div>
          </template>
        ))
      )
    }
  },
  render () {
    const props = {}
    Object.keys(M.props).forEach(k => {
      if (this[k]) {
        props[k] = this[k]
      }
      return props[k]
    })
    this.complete ? props['footer'] = null : props['footer'] = this.modal.footer
    this.complete ? props['title'] = '' : props['title'] = this.modal.title

    return (
      <a-modal {... { props } } onCancel={this.cancel} onOk={this.ok}>
        {this.renderResultContext()}
        {this.renderOperateContext()}
      </a-modal>
    )
  }
}
