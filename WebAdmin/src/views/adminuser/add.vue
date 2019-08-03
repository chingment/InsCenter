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
      <el-form-item label="所属机构" prop="orgId">
        <el-cascader
          v-model="form.orgId"
          :options="cascader_org_options"
          :props="{ multiple: false, checkStrictly: true }"
          placeholder="请选择"
          clearable
          @change="cascader_org_change"
        />
      </el-form-item>
      <el-form-item label="职位" prop="positionId">
        <el-cascader
          v-model="form.orgId"
          :options="cascader_org_options"
          :props="{ multiple: false, checkStrictly: true }"
          placeholder="请选择"
          clearable
          @change="cascader_org_change"
        />
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
// https://element.eleme.cn/#/zh-CN/component/cascader
import { MessageBox } from 'element-ui'
import { addUser } from '@/api/adminuser'
import fromReg from '@/utils/formReg'
import { goBack } from '@/utils/commonUtil'
export default {
  data() {
    return {
      form: {
        userName: '',
        password: '',
        fullName: '',
        phoneNumber: '',
        email: '',
        orgId: ''
      },
      rules: {
        userName: [{ required: true, message: '必填,且由3到20个数字、英文字母或下划线组成', trigger: 'change', pattern: fromReg.userName }],
        password: [{ required: true, message: '必填,且由6到20个数字、英文字母或下划线组成', trigger: 'change', pattern: fromReg.password }],
        fullName: [{ required: true, message: '必填', trigger: 'change' }],
        orgId: [{ required: true, message: '必填' }],
        phoneNumber: [{ required: false, message: '格式错误,eg:13800138000', trigger: 'change', pattern: fromReg.phoneNumber }],
        email: [{ required: false, message: '格式错误,eg:xxxx@xxx.xxx', trigger: 'change', pattern: fromReg.email }]
      },
      cascader_org_props: { multiple: true, checkStrictly: true },
      cascader_org_options: [{
        value: 1,
        label: '东南',
        children: [{
          value: 2,
          label: '上海',
          children: [
            { value: 3, label: '普陀' },
            { value: 4, label: '黄埔' },
            { value: 5, label: '徐汇' }
          ]
        }, {
          value: 7,
          label: '江苏',
          children: [
            { value: 8, label: '南京' },
            { value: 9, label: '苏州' },
            { value: 10, label: '无锡' }
          ]
        }, {
          value: 12,
          label: '浙江',
          children: [
            { value: 13, label: '杭州' },
            { value: 14, label: '宁波' },
            { value: 15, label: '嘉兴' }
          ]
        }]
      }]
    }
  },
  methods: {
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
      this.$refs['form'].validate((valid) => {
        if (valid) {
          MessageBox.confirm('确定要保存', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            addUser(this.form).then(res => {
              this.$message(res.message)
              if (res.result === 1) {
                goBack(this)
              }
            })
          })
        }
      })
    },
    cascader_org_change() {
      console.log('dasd')
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

