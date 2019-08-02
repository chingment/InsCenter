<template>
<div class="app-container">
  <el-table
    :data="tableData"
    style="width: 100%;margin-bottom: 20px;"
    row-key="id"
    border
    default-expand-all
    :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
    >
    <el-table-column
      prop="date"
      label="机构名称"
      min-width="50">
    </el-table-column>
    <el-table-column
      prop="name"
      label="负责人"
      min-width="50">
    </el-table-column>
    <el-table-column label="操作" align="center" width="280">
      <template>
        <button type="button" class="el-button el-button--default el-button--small">
            编辑
        </button>
        <el-button
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
  export default {
    data() {
      return {
        tableData: [{
          id: 1,
          date: '2016-05-02',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1518 弄'
        }, {
          id: 2,
          date: '2016-05-04',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1517 弄'
        }, {
          id: 3,
          date: '2016-05-01',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1519 弄',
          children: [{
              id: 31,
              date: '2016-05-01',
              name: '王小虎',
              address: '上海市普陀区金沙江路 1519 弄'
            }, {
              id: 32,
              date: '2016-05-01',
              name: '王小虎',
              address: '上海市普陀区金沙江路 1519 弄'
          }]
        }, {
          id: 4,
          date: '2016-05-03',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1516 弄'
        }],
        tableData1: [{
          id: 1,
          date: '2016-05-02',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1518 弄'
        }, {
          id: 2,
          date: '2016-05-04',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1517 弄'
        }, {
          id: 3,
          date: '2016-05-01',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1519 弄',
          hasChildren: true
        }, {
          id: 4,
          date: '2016-05-03',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1516 弄'
        }]
      }
    },
    created(){
      this. expandAll()
    },
    methods: {
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