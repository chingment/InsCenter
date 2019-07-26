<template>
  <div id="useradd_container" class="app-container">
    <el-form ref="form" :model="form" :rules="rules" label-width="120px">
      <el-form-item label="用户名" prop="userName">
        {{ form.userName }}
      </el-form-item>
      <el-form-item label="密码" prop="password">
        
      </el-form-item>
      <el-form-item label="姓名" prop="fullName">
        <el-input v-model="form.fullName" />
      </el-form-item>
      <el-form-item label="手机号码" prop="phoneNumber">
        <el-input v-model="form.phoneNumber" />
      </el-form-item>
      <el-form-item label="邮箱" prop="email">
        <el-input v-model="form.email" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { MessageBox } from 'element-ui'
import { editUser, initEdit } from '@/api/user'
import  fromReg  from '@/utils/formReg'
import { getUrlParam } from '@/utils/commonUtil'
export default {
  data() {
    return {
      form: {
        userId:'',
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: ''
      },
      rules: {
        fullName: [{ required: true, message: '必填', trigger: 'change' }],
        phoneNumber: [{ required: false, message: '格式错误,eg:13800138000', trigger: 'change', pattern: fromReg.phoneNumber }],
        email: [{ required: false, message: '格式错误,eg:xxxx@xxx.xxx', trigger: 'change', pattern: fromReg.email }]
      }
    }
  },
  methods: {
    init() {
      var userId = getUrlParam('userId')
      initEdit({userId:userId}).then(res => {
         if(res.result === 1){
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
        editUser(this.form).then(res => {
          this.$message(res.message)
          if (res.result === 1) {
          }
        })
      })
        }
      })
    }
  },
  created() {
    this.init()
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

