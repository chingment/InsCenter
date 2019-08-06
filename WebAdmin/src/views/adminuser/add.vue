<template>
  <div id="useradd_container" class="app-container">
    <el-form ref="form" :model="form" :rules="rules" label-width="80px">
      <el-form-item label="用户名" prop="userName">
        <el-input v-model="form.userName" />
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input v-model="form.password" type="password" />
      </el-form-item>
      <el-form-item label="姓名" prop="fullName">
        <el-input v-model="form.fullName" />
      </el-form-item>
      <el-form-item label="所属机构" prop="orgIds">
        <el-cascader
          v-model="form.orgIds"
          :options="cascader_org_options"
          :props="cascader_org_props"
          placeholder="请选择"
          clearable
          style="width:100%"
        />
      </el-form-item>
      <el-form-item label="手机号码" prop="phoneNumber">
        <el-input v-model="form.phoneNumber" />
      </el-form-item>
      <el-form-item label="邮箱" prop="email">
        <el-input v-model="form.email" />
      </el-form-item>
        <el-form-item label="角色">
        <el-tree
          ref="treerole"
          :data="tree_role_options"
          :props="tree_role_props"
          node-key="id"
          class="filter-tree"
          show-checkbox
          default-expand-all
        />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
// https://element.eleme.cn/#/zh-CN/component/cascader
import { MessageBox } from 'element-ui'
import { addUser, initAddUser } from '@/api/adminuser'
import fromReg from '@/utils/formReg'
import { getCheckedKeys, goBack } from '@/utils/commonUtil'
export default {
  data() {
    return {
      form: {
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: '',
        orgIds: [],
        roleIds:['地推活动','单纯品牌曝光']
      },
      rules: {
        userName: [{ required: true, message: '必填,且由3到20个数字、英文字母或下划线组成', trigger: 'change', pattern: fromReg.userName }],
        password: [{ required: true, message: '必填,且由6到20个数字、英文字母或下划线组成', trigger: 'change', pattern: fromReg.password }],
        fullName: [{ required: true, message: '必填', trigger: 'change' }],
        orgIds: [{ required: true, message: '必选' }],
        phoneNumber: [{ required: false, message: '格式错误,eg:13800138000', trigger: 'change', pattern: fromReg.phoneNumber }],
        email: [{ required: false, message: '格式错误,eg:xxxx@xxx.xxx', trigger: 'change', pattern: fromReg.email }]
      },
      cascader_org_props: { multiple: true, checkStrictly: true,emitPath:false },
      cascader_org_options: [],
      tree_role_options: [],
      tree_role_props: {
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
      initAddUser().then(res => {
        if (res.result === 1) {
          var d = res.data
          this.cascader_org_options= d.orgs
          this.tree_role_options = d.roles
        }
      })
    },
    resetForm() {
      this.form = {
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: ''
      }
    },
    onSubmit() {
      console.log(JSON.stringify(this.form))
      this.$refs['form'].validate((valid) => {
        if (valid) {
          MessageBox.confirm('确定要保存', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
             this.form.roleIds = getCheckedKeys(this.$refs.treerole)
            addUser(this.form).then(res => {
              this.$message(res.message)
              if (res.result === 1) {
                goBack(this)
              }
            })
          })
        }
      })
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

