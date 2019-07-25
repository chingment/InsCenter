import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken, setToken } from '@/utils/auth' // get token from cookie
import { getUrlParam } from '@/utils/commonUtil'
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)

  var urlToken = getUrlParam('token')
  // console.log('urlToken：' + urlToken)
  if (urlToken !== null) {
    setToken(urlToken)
    store.dispatch('own/setToken', urlToken)
  }
  // determine whether the user has logged in
  const hasToken = getToken()
  // console.log('getToken: ' + hasToken)
  var path = encodeURIComponent(window.location.href)
  if (hasToken) {
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
        window.location.href = `${store.state.settings.loginPath}?redirect=${path}`
        NProgress.done()
      }
    }
  } else {
    /* has no token*/
    console.log(store.state.settings.loginPath)
    // other pages that do not have permission to access are redirected to the login page.
    // next(`${store.state.settings.loginPath}?redirect=${to.path}`)
    console.log(path)
    window.location.href = `${store.state.settings.loginPath}?redirect=${path}`
    NProgress.done()
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
