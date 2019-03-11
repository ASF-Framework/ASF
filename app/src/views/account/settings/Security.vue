<template>
  <div class="account-settings-info-view">
    <a-row :gutter="16">
      <a-col :md="24" :lg="16">
        <a-list itemLayout="horizontal" ref="list">
          <a-list-item>
            <a-list-item-meta>
              <a slot="title">用户昵称</a>
              <span slot="description">
                <span class="security-list-description">您的昵称</span>
                <span v-if="userinfo.name"> : </span>
                <span class="security-list-value">{{ userinfo.name }}</span>
              </span>
            </a-list-item-meta>
            <a slot="actions" @click="$refs.modal.edit(userinfo,'name','昵称','请输入昵称',false)">修改</a>
          </a-list-item>
          <a-list-item>
            <a-list-item-meta>
              <a slot="title">登录密码</a>
              <span slot="description">
                <span class="security-list-description">此密码只用来登录：</span>
                <span class="security-list-value">******</span>
              </span>
            </a-list-item-meta>
            <a slot="actions" @click="$refs.modal.edit(userinfo,'oldPassword','旧登陆密码','请输入旧登录密码',true)" >修改</a>
          </a-list-item>
          <a-list-item>
            <a-list-item-meta>
              <a slot="title">绑定手机</a>
              <span slot="description">
                <span v-if="userinfo.telephone" class="security-list-description">已绑定手机</span>
                <span v-else class="security-list-description">请绑定手机</span>
                <span v-if="userinfo.telephone"> : </span>
                <span class="security-list-value">{{ userinfo.telephone }}</span>
              </span>
            </a-list-item-meta>
            <a v-if="userinfo.telephone" slot="actions" @click="$refs.modal.edit(userinfo,'number','手机号码','请输入手机号码',false)" >修改</a>
            <a v-else slot="actions" @click="$refs.modal.edit(userinfo,'number','手机号码','请输入手机号码',false)" >绑定</a>
          </a-list-item>
          <a-list-item>
            <a-list-item-meta>
              <a slot="title">绑定邮箱</a>
              <span slot="description">
                <span v-if="userinfo.email" class="security-list-description">已绑定邮箱</span>
                <span v-else class="security-list-description">请绑定邮箱</span>
                <span v-if="userinfo.email"> : </span>
                <span class="security-list-value">{{ userinfo.email }}</span>
              </span>
            </a-list-item-meta>
            <a v-if="userinfo.email" slot="actions" @click="$refs.modal.edit(userinfo,'email','邮箱','请输入邮箱',false)">修改</a>
            <a v-else slot="actions" @click="$refs.modal.edit(userinfo,'email','邮箱','请输入邮箱',false)">绑定</a>
          </a-list-item>
        </a-list>
      </a-col>
      <a-col :md="24" :lg="8" :style="{ minHeight: '180px' }">
        <div class="ant-upload-preview" @click="handleEdit()">
          <a-icon type="cloud-upload-o" class="upload-icon" />
          <div class="mask">
            <a-icon type="plus" />
          </div>
          <img :src="option.img" />
        </div>
      </a-col>
    </a-row>
    <test-modal ref="testModal">
    </test-modal>
    <security-modal ref="modal" @ok="handleOk"></security-modal>
  </div>
</template>

<script>
/* eslint-disable */
  import {
    getUserDetail
  } from '@/api/manage'
  //  import AvatarModal from './AvatarModal'
  import TestModal from './TestModal'
  import SecurityModal from './SecurityModal'
  export default {
    components: {
      TestModal,
      SecurityModal
    },
    data() {
      return {
        userinfo: {},
        option: {
          img: '',
          info: true,
          size: 1,
          outputType: 'jpeg',
          canScale: false,
          autoCrop: true,
          // 只有自动截图开启 宽度高度才生效
          autoCropWidth: 180,
          autoCropHeight: 180,
          fixedBox: true,
          // 开启宽度和高度比例
          fixed: true,
          fixedNumber: [1, 1]
        }
      }
    },
    created() {
      this.loadUserDetail()
    },
    methods: {
      handleEdit() {
        const that = this
        that.$message.success('功能正在升级当中')
      },
      //重新获取管理员信息
      handleOk() {
        this.loadUserDetail()
      },
      //加载管理员信息
      loadUserDetail(){
        getUserDetail().then(res => {
        this.userinfo = res.result;
        this.option.img=this.userinfo.avatar
      })
      }
    }
  }
</script>

<style lang="scss" scoped>
  .avatar-upload-wrapper {
    height: 200px;
    width: 100%;
  }
  .ant-upload-preview {
    position: relative;
    margin: 0 auto;
    width: 100%;
    max-width: 180px;
    border-radius: 50%;
    box-shadow: 0 0 4px #ccc;
    .upload-icon {
      position: absolute;
      top: 0;
      right: 10px;
      font-size: 1.4rem;
      padding: 0.5rem;
      background: rgba(222, 221, 221, 0.7);
      border-radius: 50%;
      border: 1px solid rgba(0, 0, 0, 0.2);
    }
    .mask {
      opacity: 0;
      position: absolute;
      background: rgba(0, 0, 0, 0.4);
      cursor: pointer;
      transition: opacity 0.4s;
      &:hover {
        opacity: 1;
      }
      i {
        font-size: 2rem;
        position: absolute;
        top: 50%;
        left: 50%;
        margin-left: -1rem;
        margin-top: -1rem;
        color: #d6d6d6;
      }
    }
    img,
    .mask {
      width: 100%;
      max-width: 180px;
      height: 100%;
      border-radius: 50%;
      overflow: hidden;
    }
  }
</style>
