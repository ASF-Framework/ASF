<template>
  <s-modal
    ref="modal"
    title="删除审计"
    :width="500"
    :confirmLoading="this.confirmLoading"
    @ok="submit"
    @cancel="close"
    :visible="visible">
    <a-form layout="inline" :form="form" hideRequiredMark>
      <a-row :gutter="48">
        <a-col>
          <a-form-item label="删除范围">
            <a-range-picker
              name="buildTime"
              @change="changeDeleteTimeRange"
              :disabledDate="initDisabledDate"
              v-decorator="['buildTime', {rules: [{ required: true, message: '请选择删除日期' }] ,initialValue:[beginTime,endTime]}]" >
              <template slot="dateRender" slot-scope="current">
                <div class="ant-calendar-date" >
                  {{ current.date() }}
                </div>
              </template>
            </a-range-picker>
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
    <div style="margin-top:30px;">
      <a-alert message="三个月之内的审计日志不能删除。" type="warning" showIcon />
    </div>
  </s-modal>
</template>

<script>
import moment from 'moment'
import { SModal } from '@/components'
import { auditDelete } from '@/api/manage'
export default {
  name: 'AuditDelete',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),

      beginTime: moment().subtract(4, 'month'),
      endTime: moment().subtract(3, 'month')
    }
  },
  components: {
    SModal
  },
  methods: {
    moment,
    show () {
      this.visible = true
    },
    close () {
      this.visible = false
    },
    // 筛选出3个月之后的日子的不能选择
    initDisabledDate (current) {
      return current && current > moment().subtract(3, 'month')
    },

    changeDeleteTimeRange (date, dateString) {
      this.beginTime = date[0]
      this.endTime = date[1]
    },
    // 删除日志
    submit () {
      this.form.validateFields((err, values) => {
        if (err) { return }
        const reqData = { beginTime: this.beginTime.format(), endTime: this.endTime.format() }
        this.confirmLoading = true
        auditDelete(reqData).then(res => {
          this.confirmLoading = false
          if (res.status === 200) {
            this.$refs.modal.success('删除审计日志成功')
            this.$emit('complete')
          } else {
            this.$refs.modal.error('删除失败', res.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
