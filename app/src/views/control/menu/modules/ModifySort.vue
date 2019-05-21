<template>
  <div class="editable-cell">
    <div class="editable-cell-text-wrapper">
      <a-input-number
        size="small"
        :min="0"
        :max="1000"
        v-model="value"
        @blur="onChange"
        class="input-width"/>
    </div>
  </div>
</template>

<script>
import { modifyActionSort, modifyMenuSort } from '@/api/control'
export default {
  props: {
    text: { type: Number, default: 0 },
    id: { type: String, default: '' },
    type: { type: String, required: true }
  },
  data () {
    return {
      value: this.text,
      pid: this.id
    }
  },
  methods: {
    onChange (e) {
      if (this.text === this.value) {
        return
      }
      if (e.target.value === '') {
        this.value = this.text
        return
      }
      this.value = this.value
      const parameter = {
        id: this.pid,
        sort: this.value
      }
      if (this.type === 'menu') {
        modifyMenuSort(parameter).then(res => {
          if (res.status === 200) {
            this.$emit('complete', this.pid, this.value)
          }
        })
      } else {
        modifyActionSort(parameter).then(res => {
          if (res.status === 200) {
            this.$emit('complete', this.pid, this.value)
          }
        })
      }
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
