<template>
  <div id="useradd_container" class="app-container">
    <el-form ref="form" :model="form" :rules="rules" label-width="80px">
      <el-form-item label="角色名称" prop="userName">
        {{ form.name }}
      </el-form-item>
        <el-form-item label="描述">
        <el-input v-model="form.description" type="textarea" />
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
        userId: '',
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: ''
      },
      rules: {
        password: [{ required: false, message: '必填,且由6到20个数字、英文字母或下划线组成', trigger: 'change', pattern: fromReg.password }],
        fullName: [{ required: true, message: '必填', trigger: 'change' }],
        phoneNumber: [{ required: false, message: '格式错误,eg:13800138000', trigger: 'change', pattern: fromReg.phoneNumber }],
        email: [{ required: false, message: '格式错误,eg:xxxx@xxx.xxx', trigger: 'change', pattern: fromReg.email }]
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
          this.form = res.data
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

