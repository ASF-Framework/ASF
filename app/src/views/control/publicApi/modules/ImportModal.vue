<template>
  <a-modal
    :title="title"
    :width="600"
    :visible="visible"
    @ok="handleOk"
    okText="开始上传"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="菜单JSON">
          <a-upload
            name="file"
            :fileList="fileList"
            :beforeUpload="beforeUpload"
            :remove="handleRemove"
          >
              <a-button>
                <a-icon type="upload"/>选择文件
              </a-button>
          </a-upload>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { importPublicApi } from '@/api/control'
export default {
  name: 'ImportModal',
  data () {
    return {
      labelCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 5
        }
      },
      wrapperCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 16
        }
      },
      title: '导入',
      isChecked: false,
      visible: false, // 弹框是否显示
      confirmLoading: false, // 弹框中的提交按钮是否加载中
      queryParam: {
        projectFile: ''
      }, // 导入参数
      form: this.$form.createForm(this), // 表单对象
      importCapital: '',
      fileList: [],
      uploading: false,

      textData:{
        list:[]
      },
    }
  },
  beforeCreate(){
    // 读取文件
    FileReader.prototype.reading=function ({encode}=pms) {
      let bytes=new Uint8Array(this.result);//无符号整型数组
      let text=new TextDecoder(encode || 'UTF-8').decode(bytes);
      return text;
    };
    /* 重写readAsBinaryString函数 */
			FileReader.prototype.readAsBinaryString = function (f) {
				if (!this.onload)       //如果this未重写onload函数，则创建一个公共处理方式
					this.onload = e => {  //在this.onload函数中，完成公共处理
						let rs = this.reading();
					};
				this.readAsArrayBuffer(f);  //内部会回调this.onload方法
			};
  },
  created () {},
  methods: {
    // 导入
    import () {
      this.visible = true
    },
    // 关闭方法
    close () {
      this.form = this.$form.createForm(this)
      this.queryParam = {}
      this.fileList = []
      this.$emit('close')
      this.visible = false
    },
    //读取json文件
    read(f){
      let rd = new FileReader();
			rd.onload = e => {  
            //this.readAsArrayBuffer函数内，会回调this.onload函数。在这里处理结果
        let cont = rd.reading({encode: 'UTF-8'});
        this.textData.list=JSON.parse(cont)
			};
			rd.readAsBinaryString(f);
    },
    // 手动上传-- 弹框提交方法
    handleOk () {
      const _this = this
      _this.confirmLoading = true
      
      this.uploading = true
      //模拟后端请求 2000 毫秒延迟
      new Promise(resolve => {
        setTimeout(() => resolve(), 2000)
      })
        .then(() => {
          importPublicApi(_this.textData).then(res => {
             if (res.status === 200) {
              _this.fileList = []
              _this.confirmLoading = false
              _this.uploading = false
               this.$emit('ok')
              _this.$notification['success']({
                message: '提示',
                description: '导入成功',
                duration: 4
              })
            } else {
              _this.uploading = false
              _this.confirmLoading = false
              _this.$notification['error']({
                message: '错误提示',
                description: res.message,
                duration: 4
              })
            }
          })
        })
        .catch(() => {
          // Do something
        })
        .finally(() => {
          _this.confirmLoading = false
          _this.close()
        })
    },
    // 弹框关闭
    handleCancel () {
      this.close()
    },
    // 移除上传的文件
    handleRemove (file) {
      const index = this.fileList.indexOf(file)
      const newFileList = this.fileList.slice()
      newFileList.splice(index, 1)
      this.fileList = newFileList
    },
    // 钩子返回false为手动上传 （file文件数据里包含名称，地址，等信息）
    beforeUpload (file) {
      // this.fileList = [...this.fileList, file]//这个就是多个文件上传
      var fileArr = []
      // 获取新的上传列表
      fileArr.push(file)
      // 进行赋值保存
      this.fileList = fileArr
      this.read(file)
      return false
    },
    // 自动上传
    handleChange (info) {
      const _this = this
      if (info.file.status !== 'uploading') {
      }
      if (info.file.status === 'done') {
        if (info.file.response.success) {
          // 模拟后端请求 2000 毫秒延迟
          new Promise(resolve => {
            setTimeout(() => resolve(), 2000)
          })
            .then(() => {
              _this.$notification['success']({
                message: '提示',
                description: `${info.file.name} 上传成功`,
                duration: 4
              })
            })
            .catch(() => {
              // Do something
            })
            .finally(() => {
              _this.confirmLoading = false
              _this.close()
            })
        } else {
          _this.$notification['error']({
            message: '错误提示',
            description: `${info.file.response.msg}`,
            duration: 4
          })
        }
      } else if (info.file.status === 'error') {
        _this.$notification['error']({
          message: '错误提示',
          description: `${info.file.name} 上传失败，请稍后再试`,
          duration: 4
        })
      }
    }
  }
}
</script>

<style scoped>
.row {
  padding: 10px;
}
.col-text-right {
  text-align: right;
}
/* .col-backgroud-color1{
       background: rgba(0, 160, 233, 0.7);
    }
    .col-backgroud-color2{
       background: #00a0e9;
    } */
</style>
