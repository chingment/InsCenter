<template>
  <div id="useradd_container" class="app-container">
    <el-form ref="form" :model="form" label-width="120px">
      <el-form-item label="用户名">
        <el-input v-model="form.userName" />
      </el-form-item>
        <el-form-item label="密码">
        <el-input v-model="form.password" />
      </el-form-item>
      <el-form-item label="姓名">
        <el-input v-model="form.fullName" />
      </el-form-item>
      <el-form-item label="手机号码">
        <el-input v-model="form.phoneNumber" />
      </el-form-item>
      <el-form-item label="邮箱">
        <el-input v-model="form.email" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
        <el-button @click="onCancel">返回</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { MessageBox, Message } from 'element-ui'
import { addUser } from '@/api/user'
export default {
  data() {
    return {
      form: {
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: ''
      }
    }
  },
  methods: {
    onSubmit() {

        MessageBox.confirm('确定要保存', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          addUser(this.form).then((res) => {
             this.$message(res.message)
             if (res.result ==1){

        MessageBox.confirm('是否进入用户列表？', '新建成功', {
          confirmButtonText: '确定',
          cancelButtonText: '继续新建',
          type: 'warning'
        }).then(() => {
        
     
        })

             }
          })
     
        })
      //this.$message('submit!')
    },
    onCancel() {
      this.$message({
        message: 'cancel!',
        type: 'warning'
      })
    }
  }
}
</script>

<style scoped>
.line{
  text-align: center;
}
#useradd_container {
  max-width: 600px;
}

</style>

