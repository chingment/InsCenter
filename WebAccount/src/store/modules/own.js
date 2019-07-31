import { loginByAccount, logout, getInfo, checkPermission } from '@/api/own'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { constantRoutes, resetRouter } from '@/router'
import router from '@/router'
import Layout from '@/layout'

function _generateRoutes(routers, data) {
  data.forEach((item) => {
    const menu = {
      path: item.path,
      component: item.component == null ? Layout : () => import(`@/views${item.component}`),
      children: undefined,
      hidden: item.hidden,
      name: item.name,
      meta: item.meta,
      redirect: item.redirect
    }
    if (item.children) {
      if (menu.children === undefined) {
        menu.children = []
      }
      _generateRoutes(menu.children, item.children)
    }
    routers.push(menu)
  })
}

function generateRoutes(data) {
  _generateRoutes(constantRoutes, data)
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
  },
  // user login
  loginByAccount({ commit }, userInfo) {
    const { username, password, loginWay } = userInfo
    return new Promise((resolve, reject) => {
      loginByAccount({ username: username.trim(), password: password, loginWay: loginWay }).then(response => {
        const { data } = response
        commit('SET_TOKEN', data.token)
        setToken(data.token)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo(state.token, 'account').then(response => {
        const { data } = response

        if (!data) {
          reject('Verification failed, please Login again.')
        }
        generateRoutes(data.menus)
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
      logout(state.token).then(() => {
        commit('SET_TOKEN', '')
        removeToken()
        resetRouter()
        resolve()
      }).catch(error => {
        reject(error)
      })
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

