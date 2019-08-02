<template>
<div class="app-container">
  <el-table
    :data="listData"
    style="width: 100%;margin-bottom: 20px;"
    row-key="id"
    border
    default-expand-all
    v-loading="listLoading"
    :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
    >
    <el-table-column
      prop="label"
      label="机构名称"
      min-width="50">
    </el-table-column>
    <el-table-column
      prop="description"
      label="描述"
      min-width="50">
    </el-table-column>
    <el-table-column label="操作" align="center" width="280">
      <template slot-scope="scope">
        <button type="button" class="el-button el-button--default el-button--small">
            编辑
        </button>
        <el-button
        v-if="scope.row.extAttr.canDelete"
          size="small"
          type="danger"
        >
        删除
        </el-button>
        <button type="button" class="el-button el-button--success el-button--small">
            添加子机构
        </button>
      </template>
    </el-table-column>
  </el-table>
</div>
</template>
<script>
  import { fetchList } from '@/api/adminorg'
  export default {
    data() {
      return {
        listLoading: true,
        listData: []
      }
    },
    created(){
      this.getList()
 
    },
    methods: {
          getList() {

      fetchList().then(res => {
        this.listLoading = false
        this.listData = res.data
             this. expandAll()
      })
    },
    expandAll () {
         this.$nextTick(() => {
      var els = document.getElementsByClassName('el-table__expand-icon')  //获取点击的箭头元素
      for (let i = 0; i < els.length; i++) {
        els[i].click()
      }
         })
    }
    }
  }
</script>