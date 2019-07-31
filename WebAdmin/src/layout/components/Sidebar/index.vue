<template>
  <div :class="{'has-logo':showLogo}">
    <logo v-if="showLogo" :collapse="isCollapse" />
    <el-scrollbar wrap-class="scrollbar-wrapper">
      <el-menu
        :default-active="activeMenu"
        :collapse="isCollapse"
        :background-color="variables.menuBg"
        :text-color="variables.menuText"
        :unique-opened="false"
        :active-text-color="variables.menuActiveText"
        :collapse-transition="false"
        mode="vertical"
      >
        <sidebar-item v-for="route in routes" :key="route.path" :item="route" :base-path="route.path" />
      </el-menu>
    </el-scrollbar>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import Logo from './Logo'
import SidebarItem from './SidebarItem'
import variables from '@/styles/variables.scss'

export default {
  components: { SidebarItem, Logo },
  computed: {
    ...mapGetters([
      'sidebar'
    ]),
    routes() {
      // 记载菜单所在位置
      // for (let index = 0; index < this.$router.options.routes.length; index++) {
      //  const element = this.$router.options.routes[index]
        // console.log(element)
      // }
      // var s = [{
      //   path: '/user',
      //   meta: { title: '用户管理', icon: 'example', name: 'User' },
      //   children: [{ path: 'list', meta: { title: '用户列表', icon: 'table' }
      //   }, { path: 'add', meta: { title: '用户列表', icon: 'table' }
      //   }]
      // }]
      // this.$store.getters.userInfo.menus
      // console.log('this.$router.options.routes' + JSON.stringify(this.$router.options.routes))
      // console.log(JSON.stringify(this.$store.getters.menus))
      return this.$router.options.routes
    },
    activeMenu() {
      const route = this.$route
      const { meta, path } = route
      // if set path, the sidebar will highlight the path you set
      if (meta.activeMenu) {
        return meta.activeMenu
      }
      return path
    },
    showLogo() {
      return this.$store.state.settings.sidebarLogo
    },
    variables() {
      return variables
    },
    isCollapse() {
      return !this.sidebar.opened
    }
  }
}
</script>
