import { getInfo, checkPermission } from '@/api/own'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { constantRoutes } from '@/router'
import router from '@/router'
import Layout from '@/layout'

function generaMenu(routers, data) {
  data.forEach((item) => {
    const menu = {
      path: item.path,
      component: item.component == null ? Layout : () => import(`@/views${item.component}`),
      children: undefined,
      hidden: item.hidden,
      name: item.name,
      meta: item.meta
    }
    if (item.children) {
      if (menu.children === undefined) {
        menu.children = []
      }
      generaMenu(menu.children, item.children)
    }
    routers.push(menu)
  })
}

function generateRoutes(menus) {
  generaMenu(constantRoutes, menus)
  constantRoutes.push({ path: '*', redirect: '/404', hidden: true })
  router.addRoutes(constantRoutes)
}

const state = {
  token: getToken(),
  userInfo: null
}

const mutations = {
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_USERINFO: (state, userInfo) => {
    state.userInfo = userInfo
  }
}

const actions = {
  setToken({ commit }, token) {
    commit('SET_TOKEN', token)
    setToken(token)
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo(state.token, 'merch').then(response => {
        const { data } = response
        if (!data) {
          reject('Verification failed, please Login again.')
        }

        generateRoutes(data.menus)

        commit('SET_USERINFO', data)
        resolve(data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      commit('SET_TOKEN', '')
      removeToken()
      router.resetRouter()
      resolve()
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit('SET_TOKEN', '')
      removeToken()
      resolve()
    })
  },
  // checkperminssion
  checkPermission({ commit }, code) {
    return new Promise((resolve, reject) => {
      checkPermission(code).then(response => {
        resolve(response)
      }).catch(error => {
        reject(error)
      })
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

