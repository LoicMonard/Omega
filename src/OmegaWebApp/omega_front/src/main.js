import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import App from './App.vue'
import controlPanel from './components/controlPanel.vue'
import Login from './components/Login.vue'
import Home from './components/Home.vue'
import home2 from './components/home2.vue'
import playlist from './components/playlists2.vue'
import board from './components/board.vue'
import events from './components/events2.vue'
import moods from './components/moods2.vue'
import groups from './components/groups.vue'
import relogin from './components/relogin.vue'
import AuthService from './services/AuthService'
import mix from './components/mix.vue'
Vue.use(Router)
Vue.use(require('vue-resource'))

function requireAuth (to, from, next)  {
  if (!AuthService.isConnected) {
    next({
      path: '/',
      query: { redirect: to.fullPath }
    });

    return;
  }

  var requiredProviders = to.meta.requiredProviders;

  if(requiredProviders && !AuthService.isBoundToProvider(requiredProviders)) {
    next( false )
  };

  next();
}

const router = new Router({
  mode: 'history',
  routes: [
    { path: '', component: home2 },
    { path: '/board', component: board },
    { path: '/playlist', component: playlist },
    { path: '/groups', component: groups },
    { path: '/events', component: events },
    { path: '/moods', component: moods },
    { path: '/relogin', component: relogin },
    { path: '/mix' , component : mix }
  ]
})

AuthService.allowedOrigins = ['http://localhost:5000'];

//AuthService.logoutEndpoint = '/Account/LogOff';

AuthService.providers = {
  'Base': {
    endpoint: '/'
  },
  'Facebook': {
    endpoint: '/Account/ExternalLogin?provider=Facebook'
  },
  'Deezer': {
    endpoint: '/Account/ExternalLogin?provider=Deezer'
  },
  'Spotify': {
    endpoint: '/Account/ExternalLogin?provider=Spotify'
  },
};

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
