<template>
  <el-form>
    <el-form-item label="账号">
      {{ user.userName }}
    </el-form-item>
    <el-form-item label="姓名">
      <el-input v-model.trim="user.fullName" />
    </el-form-item>
    <el-form-item label="邮件">
      <el-input v-model.trim="user.email" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submit">保存</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  props: {
    user: {
      type: Object,
      default: () => {
        return {
          name: '',
          email: ''
        }
      }
    }
  },
  methods: {
    submit() {
      this.$message({
        message: 'User information has been updated successfully',
        type: 'success',
        duration: 5 * 1000
      })
    }
  }
}
</script>
