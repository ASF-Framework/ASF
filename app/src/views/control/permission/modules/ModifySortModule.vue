<template>
  <div class="editable-cell">
    <div class="editable-cell-text-wrapper">
      <a-input-number
        size="small"
        :min="1"
        :max="1000"
        v-model="value"
        @blur="onChange"
        class="input-width"/>
    </div>
  </div>
</template>

<script>
import { modifySort } from '@/api/manage'
export default {
  props: {
    text: { type: Number, default: 0 },
    id: { type: String, default: '' }
  },
  data () {
    return {
      value: this.text,
      // eslint-disable-next-line vue/no-dupe-keys
      pid: this.id
    }
  },
  methods: {
    onChange (e) {
      if (e.target.value === '') {
        e.target.value = 0
        return
      }
      this.value = e.target.value
      const parameter = {
        id: this.pid,
        sort: this.value
      }
      modifySort(parameter).then(res => {
        if (res.status === 200) {
          this.modifyComplete()
        }
      })
    },
    modifyComplete () {
      this.$emit('modifyComplete', this.pid, this.value)
    }
  }
}
</script>
<style lang="less" scoped>
  .input-width {
    width: 60px;
    text-align: center;
  }
</style>
