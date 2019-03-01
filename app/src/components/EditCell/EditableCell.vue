<template>
  <div class="editable-cell">
    <div v-if="editable" class="editable-cell-input-wrapper">
      <a-input
        size="small"
        :value="value"
        @change="handleChange"
        @focus="editFocus"
        @pressEnter="check"
        @blur="editBlurInput" />
    </div>
    <div v-else class="editable-cell-text-wrapper">
      <a-input :value="value || ' '" @click="edit" size="small" />
    </div>
  </div>
</template>
<script>
export default {
  props: {
    text: Number
  },
  data () {
    return {
      value: this.text,
      editable: false
    }
  },
  methods: {
    // 失去焦点事件，触发父级函数
    editBlurInput () {
      this.$emit('editBlurInput', this.value)
      // this.$notification.open({
      //   message: '温馨提醒！',
      //   description: '当失去input焦点或者修改完成按Enter键，使修改生效！',
      //   icon: <a-icon type="smile" style="color: #108ee9" />
      // })
    },
    editFocus () {
    },
    handleChange (e) {
      this.value = e.target.value
      this.$emit('handleChange', this.value)
    },
    check () {
      this.editable = false
      this.$emit('change', this.value)
    },
    edit () {
      this.editable = true
    }
  }
}
</script>
