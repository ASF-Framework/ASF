<template>
  <div>
    <span @click="open"> 出来吧</span>
    <s-modal
      ref="model"
      title="删除审计"
      :width="550"
      v-model="visible"
      @ok="submit"
      :destroyOnClose="true"
      @cancel="cancel">
      <div>
        测试测试fgfgf
        <input type="text"/>
      </div>
    </s-modal>
  </div>
</template>

<script>
import { SModal } from '@/components'

export default {
  data () {
    return {
      visible: false
    }
  },
  components: {
    SModal
  },
  methods: {
    open () {
      this.visible = true
    },
    submit (e) {
      // this.$refs.model.success('修改用户成功')
      // this.$refs.model.error('修改用户失败')
      this.$refs.model.exception({
        response: {
          status: 500
        }
      })
    },
    cancel (e) {
      this.visible = false
    }
  }
}
</script>
