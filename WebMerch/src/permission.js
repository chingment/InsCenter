import router from './router'
import store from './store'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import { getUrlParam } from '@/utils/commonUtil'
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

router.beforeEach(async(to, from, next) => {
  NProgress.start()

  document.title = getPageTitle(to.meta.title)
  var token = getUrlParam('token')
  if (token !== null) {
    store.dispatch('own/setToken', token)
  }
  token = getToken()
  console.log('token: ' + token)
  var path = encodeURIComponent(window.location.href)
  if (token) {
    if (store.getters.userInfo == null) {
      await store.dispatch('own/getInfo')
    }
    await store.dispatch('own/checkPermission', '10001').then((res) => {
      next()
    })
    NProgress.done()
  } else {
    window.location.href = `${store.state.settings.loginPath}?logout=2&redirect=${path}`
    NProgress.done()
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
