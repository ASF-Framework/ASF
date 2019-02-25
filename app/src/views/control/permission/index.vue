<template>
  <div class="page-header-index-wide">
      <div>
        <router-view></router-view>
      </div>
  </div>
</template>

<script>
export default {
}
</script>
<style>
  /* .rownew:before,
              .rownew:after {
                content: '';
                display: none;
              }
              .rownew {
                display: flex;
                justify-content: space-between;
              } */
  .editable-cell {
    position: relative;
  }

  .editable-add-btn {
    margin-bottom: 8px;
  }

  .editable-cell .ant-input {
    width: 50px;
  }
</style>
