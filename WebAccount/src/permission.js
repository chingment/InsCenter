import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import { getUrlParam } from '@/utils/commonUtil'
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login'] // no redirect whitelist

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)

  var islogout = getUrlParam('logout')
  // console.log('islogout：' + islogout)
  if (islogout !== null) {
    await store.dispatch('own/logout')
  }

  // determine whether the user has logged in
  const hasToken = getToken()
  // console.log('getToken: ' + hasToken)
  if (hasToken) {
    var hasRedirect = false
    var redirectPath = getUrlParam('redirect')
    if (redirectPath != null) {
      if (redirectPath.toLowerCase().indexOf('http') > -1) {
        hasRedirect = true
      }
    }

    if (hasRedirect) {
      window.location.href = decodeURIComponent(redirectPath)
    } else {
      if (to.path === '/login') {
      // if is logged in, redirect to the home page
        next({ path: '/' })
        NProgress.done()
      } else {
        const hasGetUserInfo = store.getters.name
        if (hasGetUserInfo) {
          next()
        } else {
          try {
          // get user info
            await store.dispatch('own/getInfo')
            next()
          } catch (error) {
          // remove token and go to login page to re-login
            await store.dispatch('own/resetToken')
            Message.error(error || 'Has Error')
            next(`/login?redirect=${to.path}`)
            NProgress.done()
          }
        }
      }
    }
  } else {
    /* has no token*/

    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})