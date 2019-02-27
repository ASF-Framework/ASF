<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline" :model="queryParam">
        <a-row type="flex" justify="space-around">
          <a-col :md="8" :sm="24">
            <a-tooltip>
              <template slot="title">新增角色</template>
              <a-button type="primary" icon="plus" @click="$refs.modal.add()" style="margin-right:10px"></a-button>
            </a-tooltip>
             <a-radio-group defaultValue="-1" v-model="queryParam.Enable"  buttonStyle="solid" @change="handleSearch()">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">启用</a-radio-button>
              <a-radio-button value="0">停用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col
            :span="8"
            :md="{span:12,offset:4}"
            :sm="{span:24,offset:0}"
            :xs="{span:24,offset:0}"
            :offset="8"
          >
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input placeholder="角色标识、名称" style="width:300px;margin-right:10px" v-model="queryParam.vague"/>
              <a-button-group>
                <a-tooltip>
                  <template slot="title">查询</template>
                  <a-button type="primary" icon="search" @click="handleSearch()"></a-button>
                </a-tooltip>
                <a-tooltip>
                  <template slot="title">清除查询条件</template>
                  <a-button icon="undo" @click="() => queryParam = {vague:'', enable:-1,pagedCount:10,skipPage:1}"></a-button>
                </a-tooltip>
              </a-button-group>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table ref="table" size="default" :pagination="pagination" :columns="columns" :dataSource="loadData" @change="handleChange" :rowKey="record => record.id">     
      <span slot="enable" slot-scope="text">{{ text | statusFilter }}</span>
      <span slot="createTime" slot-scope="text">{{ text*1000 | moment }}</span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.modal.edit(record)">编辑</a>
        <a-divider type="vertical"/>
        <a-dropdown>
          <a class="ant-dropdown-link">更多
            <a-icon type="down"/>
          </a>
          <a-menu slot="overlay">
            <a-menu-item>
              <a href="javascript:;" @click="handleStatus(record)">{{ShowStatus(record.enable)}}</a>
            </a-menu-item>
            <a-menu-item>
              <a href="javascript:;" @click="handleDelete(record.id)">删除</a>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </span>
    </a-table>
    <role-modal ref="modal" @ok="handleOk"></role-modal>
  </a-card>
</template>

<script>
import STable from '@/components/table/'
import RoleModal from './modules/RoleModal'
import {    getRoleList,modifyStatusRole,deleteRole  } from '@/api/manage'
export default {
  name: 'TableList',
  components: {
    STable,
    RoleModal
  },
  data() {
    return {
      description:
        '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
        filters:{},
      // 查询参数
      queryParam: {
        vague:"",
        enable:-1,
        pagedCount:10,
        skipPage:1,
      },
      // 表头
      columns: [
        {
          title: '唯一识别码',
          dataIndex: 'id',
          sorter: true
        },
        {
          title: '角色名称',
          dataIndex: 'name'
        },
        {
          title: '状态',
          dataIndex: 'enable',
           scopedSlots: {
            customRender: 'enable'
          }
        },
        {
          title: '描述',
          dataIndex: 'description',
        },
        {
          title: '创建时间',
          dataIndex: 'createTime',
          sorter: false,
          scopedSlots: {
            customRender: 'createTime'
          }
        },
        {
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        }
      ],
      //分页对象
      pagination:{
        pageSizeOptions:['10','20','40','50'],
        showSizeChanger:true,
        total:0
      },
      loadData:[],
      //状态数据
      status: [{
            value: -1,
            label: "全部"
          },
          {
            value: 1,
            label: "启用"
          },
          {
            value: 0,
            label: "禁用"
          }
      ]
    }
  },
  created(){
    this.loadDataList()
  },
  filters: {
    statusFilter(status) {
      const statusMap = {        
        1: '启用',
        0: '禁用'
      }
      return statusMap[status?1:0]
    }
  },
  methods: {
    //弹框点击确认后的方法
    handleOk() {
      // 新增/修改 成功时，重载列表
      this.$refs.table.refresh(true)
    },
    //显示状态
    ShowStatus(value) {
      let retValue = "";
      this.status.forEach(function(opt) {
        if (opt.value != value) {
          retValue = opt.label;
        }
      });
      return retValue;
    },
    //返回item状态的其它状态
    ReturnStatus(value){
      let retValue = "";
      this.status.forEach(function(opt) {
        if (opt.value != value) {
          retValue = opt.value;
        }
      });
      return retValue;
    },
    //禁用/启用方法
    handleStatus(item){      
      const that = this
      let par={};
      par.RoleId=item.id
      par.Enable=this.ReturnStatus(item.enable)
      let text=this.ShowStatus(item.enable)
      console.log(par)
      that.$confirm({
        title: '提示',
        content: '确定要修改角色状态为'+text+'吗 ?',
        onOk() {
          modifyStatusRole(par).then(res=>{
            if(res.status==200){
                that.$refs.table.refresh(true)
                that.$message.success('修改成功')
            }else{
              that.$message.error(res.message)
            }
          })
        },
        onCancel() {}
      })
    },
    //删除
    handleDelete(id) {
      const that = this
      this.$confirm({
        title: '提示',
        content: '确定要删除吗 ?',
        onOk() {
          deleteRole(id).then(res=>{
            if(res.status==200){
              that.loadDataList()
              that.$message.success('删除成功')
            }else{
              that.$message.error(res.message)
            }
          })
          
        },
        onCancel() {}
      })
    },   
    //查询数据
    handleSearch(){
      //当有条件查询时查询页码必须等于1，后端才会从第一页数据开始查询并返回数据
        if(this.queryParam.Vague!='' || this.queryParam.enable!=-1){
          this.queryParam.skipPage=1
        }
        this.loadDataList()
    },
    //加载数据
    loadDataList(){
      const _this=this
      getRoleList(this.queryParam).then(res=>{       
        if(res.status==200){
          _this.loadData=res.result
          const pager={... _this.pagination}
          pager.total=res.totalCount
          _this.pagination=pager
        }else{
          _this.$message.success(res.message)
        }
      })
    },
    //点击页码分页显示
    handleChange(pagination, filters, sorter){
      const pager={... this.pagination}
      pager.current=pagination.current
      pager.pageSize=pagination.pageSize
      this.queryParam.skipPage=pager.current
      this.queryParam.pagedCount=pager.pageSize
      this.loadDataList()
    }
  },
  watch: {
    
  }
}
</script>