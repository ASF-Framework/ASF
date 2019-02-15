<template>
  <div class='editable-cell'>
    <div v-if="editable" class='editable-cell-input-wrapper'>
      <a-input size="small" :value="value" @change="handleChange" @pressEnter="check" />
    </div>
    <div v-else class='editable-cell-text-wrapper'>
      <a-input :value="value || ' '" @click="edit" size="small" />
    </div>
  </div>
</template>
<script>
export default {
  props: {
    text: Number
  },
  data() {
    return {
      value: this.text,
      editable: false
    }
  },
  methods: {
    handleChange(e) {
      const value = e.target.value
      this.value = value
    },
    check() {
      this.editable = false
      this.$emit('change', this.value)
    },
    edit() {
      console.log(1111111)
      this.editable = true
    }
  }
}
</script>