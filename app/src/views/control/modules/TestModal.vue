<template>
  <a-modal
    title="操作"
    :width="800"
    :visible="visible"
    @cancel="handleCancel"
  >
    <a-spin :spinning="false">
      <div>
    <a-row >
      <a-col class="a-row" :span="12">col-12</a-col>
      <a-col class="a-col-z" :span="12">col-12</a-col>
    </a-row>
    <a-row >
      <a-col class="a-row" :span="8">col-8</a-col>
      <a-col class="a-col-z" :span="8">col-8</a-col>
      <a-col class="a-row" :span="8">col-8</a-col>
    </a-row>
    <a-row>
      <a-col class="a-row" :span="6">col-6</a-col>
      <a-col class="a-col-z" :span="6">col-6</a-col>
      <a-col class="a-row" :span="6">col-6</a-col>
      <a-col class="a-col-z" :span="6">col-6</a-col>
    </a-row>
  </div>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
  name: 'testmodal',
  data() {
    return {
      visible: false
    }
  },
  created() {},
  methods: {
     //弹框关闭
    handleCancel () {
      this.visible = false
    },
    test() {
      this.visible = true
    }
  }
}
</script>

<style scoped>
.a-row {
      background: rgba(0, 160, 233, 0.7);
}
.a-col-z{
  background: #00a0e9;
  text-align:center
}
</style>
