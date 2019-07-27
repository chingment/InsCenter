import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import { getUrlParam, changeURLArg } from '@/utils/commonUtil'
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login'] // no redirect whitelist

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)

  var islogout = getUrlParam('logout')
  var hasToken = getToken()
  // console.log('islogout：' + islogout)
  if (islogout !== null) {
    hasToken = false
    await store.dispatch('own/logout')
  }

  // determine whether the user has logged in
  console.log('getToken: ' + hasToken)
  if (hasToken) {
    
    var hasRedirect = false
    var redirectPath = getUrlParam('redirect')
    if (redirectPath != null) {
      if (redirectPath.toLowerCase().indexOf('http') > -1) {
        hasRedirect = true
      }
    }

    await store.dispatch('own/getInfo').then((res) => {
      if (res.result === 1) {
        if (hasRedirect) {
          var url = changeURLArg(decodeURIComponent(redirectPath), 'token', getToken())
          window.location.href = url
        } else {
          if (to.path === '/login') {
            // if is logged in, redirect to the home page
            next({ path: '/' })
            NProgress.done()
          } else {
            next()
          }
        }
      } else {
        next(`/login?redirect=${to.path}`)
      }
    })
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
