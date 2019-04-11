import M from 'ant-design-vue/es/modal/Modal'
import { Result } from '@/components'

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
    *执行成功
    * @param {String} title
    * @param {String} description
    * @param {Boolean} autoDown 是否自动关闭对话框  true：自动关闭  false:手动关闭
    */
    success (title, description, autoDown = true) {
      this.complete = true
      this.result.isSuccess = true
      this.result.title = title
      this.result.description = description

      // 自动关闭对话框
      if (autoDown) {
        new Promise(resolve => {
          setTimeout(() => { this.cancel() }, 1800)
        }).then()
      }
    },
    /**
     * 执行失败
     * @param {String} title
     * @param {String} description
     */
    error (title, description) {
      this.complete = true
      this.result.isSuccess = false
      this.result.title = title
      this.result.description = description
    },
    /**
     *  上一步(重新操作)
     */
    previous () {
      this.complete = false
    },
    /**
     * 绘制结果页面
     */
    renderResultContext () {
      const props = {}
      Object.keys(Result.props).forEach(k => {
        if (this.result[k]) {
          props[k] = this.result[k]
        }
        return props[k]
      })

      this.result.isSuccess ? props['type'] = 'success' : props['type'] = 'error'

      return (
        <Result v-show={this.complete} { ...{ props }}>
          <template slot="action">
            <a-button style="margin-right:20px;" v-show={!this.result.isSuccess} onClick={this.previous}>上一步</a-button>
            <a-button type="primary" onClick={this.cancel}>知道了</a-button>
          </template>
        </Result>
      )
    },
    /**
     * 绘制结果页面
     */
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
      props[k] = this[k]
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
