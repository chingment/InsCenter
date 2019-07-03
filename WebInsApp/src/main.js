// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './store'
import http from "./utils/http";

import Calendar from 'vue2-datepick'; //日期控件

Vue.use(Calendar);//日期控件

//import lumosui from 'lumos-ui'

//import './assets/css/base.css'

//Vue.use(lumosui)

//单个组件导入
//import tabbar from "@/components/tabbar/src/tabbar";
//Vue.component("lumos-tabbar", tabbar);


//整套组件导入 通过components下的index.js文件导入组件,遍历 index.js 对象
// import components from './lib/index'
// Object.keys(components).forEach((key) => {
// 	Vue.component(components[key].name, components[key])
// });

import lumoslib from './lib/index'

var lumoslib_components=lumoslib.components;

Object.keys(lumoslib_components).forEach((key) => {
	Vue.component(lumoslib_components[key].name, lumoslib_components[key])
});

var lumoslib_uses=lumoslib.uses;

Object.keys(lumoslib_uses).forEach((key) => {
	Vue.use(lumoslib_uses[key]);
});



//然后通过 USE方法全局注册
// import Loading from './lib/loading'
// Vue.use(Loading);


Vue.prototype.$http = http;


// //方法挂靠全局
// Object.keys(global).forEach((key) => {
// 	Vue.prototype[key] = global[key];
// });


router.beforeEach((to, from, next) => {
  
  // if (to.matched.some(record => record.meta.requireAuth)){  // 判断该路由是否需要登录权限
  // next({
  //   path: '/My',
  //   query: { redirect: to.fullPath }
  // })
  // }
  // else {
  //   next();
  // }

   next();
})



Vue.prototype.getNowFormatDate = function() {
  var date = new Date();
  var seperator1 = "-";
  var year = date.getFullYear();
  var month = date.getMonth() + 1;
  var strDate = date.getDate();
  if (month >= 1 && month <= 9) {
    month = "0" + month;
  }
  if (strDate >= 0 && strDate <= 9) {
    strDate = "0" + strDate;
  }
  var currentdate = year + seperator1 + month + seperator1 + strDate;
  return currentdate;
};


/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>'
})



