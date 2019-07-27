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

  NProgress.start()

  document.title = getPageTitle(to.meta.title)
  var urlToken = getUrlParam('token')
  if (urlToken !== null) {
    setToken(urlToken)
    store.dispatch('own/setToken', urlToken)
  }
  var hasToken = getToken()
  var islogout = getUrlParam('logout')
  if (islogout !== null) {
    hasToken = false
  }

  console.log('getToken: ' + hasToken)
  var path = encodeURIComponent(window.location.href)
  if (hasToken) {
    await store.dispatch('own/getInfo').then((res) => {
      if (res.result === 1) {
        next()
      } else {
        window.location.href = `${store.state.settings.loginPath}?redirect=${path}`
      }
    })
    NProgress.done()
  } else {
    window.location.href = `${store.state.settings.loginPath}?redirect=${path}`
    NProgress.done()
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
