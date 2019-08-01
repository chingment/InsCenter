<template>
  <div id="useradd_container" class="app-container">
    <el-form ref="form" :model="form" :rules="rules" label-width="80px">
      <el-form-item label="角色名称" prop="userName">
        {{ form.name }}
      </el-form-item>
        <el-form-item label="描述">
        <el-input v-model="form.description" type="textarea" />
      </el-form-item>
      <el-form-item label="菜单">
            <el-tree
      ref="treemenus"
      :data="treeData"
      :props="defaultProps"
      node-key="id"
      class="filter-tree"
      show-checkbox
      default-expand-all
      :default-checked-keys="treeDataDefaultChecked"
    />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { MessageBox } from 'element-ui'
import { editRole, initEditRole } from '@/api/adminrole'
import fromReg from '@/utils/formReg'
import { getUrlParam } from '@/utils/commonUtil'
export default {
  data() {
    return {
      isOpenEditPassword: false,
      form: {
        roleId:'',
        name: '',
        description: ''
      },
      rules: {
      },
      treeData: [],
      treeDataDefaultChecked: [],
      defaultProps: {
        children: 'children',
        label: 'label'
      }
    }
  },
  created() {
    this.init()
  },
  methods: {
    init() {
      var roleId = getUrlParam('roleId')
      initEditRole({ roleId: roleId }).then(res => {
        if (res.result === 1) {
          var d = res.data
          this.form.roleId = d.roleId
          this.form.name = d.name
          this.form.description = d.description
          this.treeData = d.menus
          this.treeDataDefaultChecked = d.checkedMenuIds
        }
      })
    },
    onSubmit() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          MessageBox.confirm('确定要保存', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            editRole(this.form).then(res => {
              this.$message(res.message)
            })
          })
        }
      })
    },
    openEditPassword() {
      if (this.isOpenEditPassword) {
        this.isOpenEditPassword = false
        this.form.password = ''
        this.rules.password[0].required = false
      } else {
        this.isOpenEditPassword = true
        this.rules.password[0].required = true
      }
    }
  }
}
</script>

<style scoped>
.line {
  text-align: center;
}
#useradd_container {
  max-width: 600px;
}
</style>

