<template>
  <el-breadcrumb class="app-breadcrumb" separator="/">
    <transition-group name="breadcrumb">
      <el-breadcrumb-item v-for="(item,index) in levelList" :key="item.path">
        <span v-if="item.redirect==='noRedirect'||index==levelList.length-1" class="no-redirect">{{ item.meta.title }}</span>
        <a v-else @click.prevent="handleLink(item)">{{ item.meta.title }}</a>
      </el-breadcrumb-item>
    </transition-group>
  </el-breadcrumb>
</template>

<script>
import pathToRegexp from 'path-to-regexp'
import { Tree } from 'element-ui'
import { close } from 'fs'
import { getBreadcrumb } from '@/utils/ownResource'
export default {
  data() {
    return {
      levelList: null
    }
  },
  watch: {
    $route() {
      this._getBreadcrumb()
    }
  },
  created() {
    this._getBreadcrumb()
  },
  methods: {
    _getBreadcrumb() {

      var  matched=getBreadcrumb(this.$route)
      const first = matched[0]

      if (!this.isDashboard(first)) {
        matched = [{ path: '/home', meta: { title: '主页' }}].concat(matched)
      }

      this.levelList =matched
    },
    isDashboard(route) {
      if(route=== undefined)
        return false
      
      var path=route.path
      if(path=== undefined)
      return false

      path=path.trim().toLocaleLowerCase()

      if(path==='/'|| path==='/home' )
      return true
  
      return false
    },
    pathCompile(path) {
      // To solve this problem https://github.com/PanJiaChen/vue-element-admin/issues/561
      const { params } = this.$route
      var toPath = pathToRegexp.compile(path)
      return toPath(params)
    },
    handleLink(item) {
      const { redirect, path } = item
      if (redirect) {
        this.$router.push(redirect)
        return
      }
      this.$router.push(this.pathCompile(path))
    }
  }
}
</script>

<style lang="scss" scoped>
.app-breadcrumb.el-breadcrumb {
  display: inline-block;
  font-size: 14px;
  line-height: 50px;
  margin-left: 8px;

  .no-redirect {
    color: #97a8be;
    cursor: text;
  }
}
</style>
