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
      hidden: true,
      children: [],
      name: item.name,
      meta: { title: item.title, icon: item.icon }
    }
    if (item.children) {
      generaMenu(menu.children, item.children)
    }
    routers.push(menu)
  })
  // const menu = {
  //   path: '/',
  //   component: () => import(`@/views/home/index`),
  //   hidden: true,
  //   children: [],
  //   name: 'Home',
  //   meta: { title: '用户礼拜二', icon: '' }
  // }
  // if (item.children) {
  //   generaMenu(menu.children, item.children)
  // }
  // routers.push(menu)
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

        // console.log(JSON.stringify(constantRoutes))
        generaMenu(constantRoutes, data.menus)
        router.addRoutes(constantRoutes)
        // console.log(JSON.stringify(constantRoutes))
        commit('SET_USERINFO', data)
        resolve(response)
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

