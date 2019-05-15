<template>
  <a-drawer :visible="visible" @close="onClose" width="350" >
    <p :style="[layout.title]"> 公共 API</p>
    <description-list title="" size="small" :col="1">
      <description-item term="标识">{{ detailData.id }}</description-item>
      <description-item term="名称">{{ detailData.name }}</description-item>
      <description-item term="状态">
        <a-badge v-if="detailData.enable" status="success" :text="detailData.enable | statusFilter"/>
        <a-badge v-else status="error" :text="detailData.enable | statusFilter"/>
      </description-item>
      <description-item term="API 地址">{{ detailData.apiTemplate }}</description-item>
      <description-item term="创建时间">{{ detailData.createTime | dayFormat('YYYY-MM-DD HH:mm') }}</description-item>
      <description-item term="描述">{{ detailData.description }}</description-item>
    </description-list>
  </a-drawer>
</template>

<script>
import { DescriptionList } from '@/components'
const DescriptionItem = DescriptionList.Item

export default {
  data () {
    return {
      visible: false,
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
    show (data) {
      this.detailData = data
      this.visible = true
    },
    onClose () {
      this.visible = false
    }
  }
}
</script>
