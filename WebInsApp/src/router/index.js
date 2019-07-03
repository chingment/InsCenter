import Vue from 'vue'
import Router from 'vue-router'

import HomeIndex from '@/pages/Home/Index'
import InsCarIndex from '@/pages/InsCar/Index'
import InsCarAsCarInfo from '@/pages/InsCar/As/CarInfo'
import InsCarAsCarModelSearch from '@/pages/InsCar/As/CarModelSearch'
import InsCarAsChooseKind from '@/pages/InsCar/As/ChooseKind'
import InsCarAsInsOffer from '@/pages/InsCar/As/InsOffer'

import InsCarMsCarInfo from '@/pages/InsCar/Ms/CarInfo'
import InsMarketIndex from '@/pages/InsMarket/Index'
import InsClaimIndex from '@/pages/InsClaim/Index'
import MyIndex from '@/pages/My/Index'
import Hello from '@/pages/hello'

Vue.use(Router)

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/Home/Index', name: 'HomeIndex', component: HomeIndex, children: [
        { path: '/InsCar', name: 'InsCar', component: InsCarIndex,meta: {requireAuth: true}},
        { path: '/InsMarket', name: 'InsMarket', component: InsMarketIndex,meta: {requireAuth: true} },
        { path: '/InsClaim', name: 'InsClaim', component: InsClaimIndex,meta: {requireAuth: true} },
        { path: '/My', name: 'My', component: MyIndex,meta: {requireAuth: false} }
      ]
    },
    {
      path: '/Hello', name: 'Hello', component: Hello
    },
    { path: '/InsCar/As/CarInfo', name: 'InsCarAsCarInfo', component: InsCarAsCarInfo,meta: {requireAuth: true}  },
    { path: '/InsCar/As/CarModelSearch', name: 'InsCarAsCarModelSearch', component: InsCarAsCarModelSearch,meta: {requireAuth: true}},
    { path: '/InsCar/As/ChooseKind', name: 'InsCarAsChooseKind', component: InsCarAsChooseKind,meta: {requireAuth: true}},
    { path: '/InsCar/As/InsOffer', name: 'InsCarAsInsOffer', component: InsCarAsInsOffer,meta: {requireAuth: true}},
    { path: '/InsCar/Ms/CarInfo', name: 'InsCarMsCarInfo', component: InsCarMsCarInfo,meta: {requireAuth: true}}
  ]
})
