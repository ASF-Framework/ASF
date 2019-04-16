<template>
  <a-drawer :visible="visible" @close="close" width="500" >
    <a-spin :spinning="confirmLoading">
      <p :style="[layout.title]"> 管理员详情</p>
      <p><a-avatar :size="64" :src="detailData.avatar" /></p>
      <description-list title="基本信息" size="small" :col="2">
        <description-item term="标识">
          {{ detailData.id }}
        </description-item>
        <description-item term="用户名">{{ detailData.username }}</description-item>
        <description-item term="昵称">{{ detailData.name }}</description-item>
        <description-item term="状态">
          <a-badge v-if="detailData.status===1" status="success" text="正常"/>
          <a-badge v-else status="error" text="禁止登陆"/>
        </description-item>
        <description-item term="手机号码">{{ detailData.telephone }}</description-item>
        <description-item term="邮箱地址">{{ detailData.email }}</description-item>
        <description-item term="是否删除">
          <a-badge v-if="detailData.isDeleted" status="error" text="已删除"/>
          <a-badge v-else status="success" text="正常"/>
        </description-item>
      </description-list>
      <a-divider/>
      <description-list title="状态信息" size="small" :col="2">
        <description-item term="创建管理员">{{ detailData.createUser }}</description-item>
        <description-item term="创建时间">{{ detailData.createTime | dayFormat('YYYY-MM-DD HH:mm') }}</description-item>
        <description-item term="上次登录IP">{{ detailData.loginIp }}</description-item>
        <description-item term="登录时间">{{ detailData.loginTime | dayFormat('YYYY-MM-DD HH:mm') }}</description-item>
        <description-item term="描述">{{ detailData.description }}</description-item>
      </description-list>
      <a-divider/>
      <description-list title="拥有角色" size="small" :col="1">
        <description-item>
          <a-tag v-if="detailData.isSystem" color="cyan">超级管理员</a-tag>
          <a-tag v-else v-for="role in detailData.rules" :key="role" color="cyan">{{ role }}</a-tag>
        </description-item>
      </description-list>
    </a-spin>
  </a-drawer>
</template>
<script>
import { getAccountDetail } from '@/api/control'
import { DescriptionList } from '@/components'
const DescriptionItem = DescriptionList.Item
export default {
  data () {
    return {
      visible: false,
      confirmLoading: false,
      // 详情数据
      detailData: {},
      layout: {
        title: {
          fontSize: '16px',
          color: 'rgba(0,0,0,0.85)',
          lineHeight: '24px',
          display: 'block',
          marginBottom: '24px'
        }
      }
    }
  },
  components: {
    DescriptionList, DescriptionItem
  },
  methods: {
    /**
     * 显示对话框
     * @param {Number} id 管理员账户ID
     */
    show (id, roles) {
      this.visible = true
      this.loadAccountDetail(id, roles)
    },
    /**
     * 关闭对话框
     */
    close () {
      this.visible = false
    },
    /**
     * 加载管理账户信息
     * @param {Number} id 管理账户ID
     * @param {Array} roles 角色集合
     */
    loadAccountDetail (id, roles) {
      this.confirmLoading = true
      getAccountDetail(id).then(res => {
        this.confirmLoading = false
        if (res.status === 200) {
          this.detailData = res.result
          this.detailData.rules = roles
        } else {
          this.$notification.error({ message: '获取管理账户详情失败', description: res.message })
        }
      }).catch(() => { this.confirmLoading = false })
    }
  }
}
</script>
