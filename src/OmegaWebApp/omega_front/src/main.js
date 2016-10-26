import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import Login from './components/Login.vue'

Vue.use(Router)
Vue.use(require('vue-resource'))

new Vue({
  el: '#app',
  store,
  render: h => h(Login)
})
