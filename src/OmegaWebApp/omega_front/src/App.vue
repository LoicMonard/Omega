<template>
  <div>
    <div v-if="identity != false" class="col-12 navbarContainer">
      <div class="col-12 appNavbar">
        <div class="col-4">
          <router-link to="/playlist"><img src="./assets/triangleGrey.png" id="appLogo"></router-link><span class="appTitle"><span id="appRedText">o</span>mega</span>
        </div>
        <div class="col-4 appProviders">
          <span @click="relogin('Facebook')">FACEBOOK</span>
          <span @click="relogin('Deezer')" id="appRedText">DEEZER</span>
          <span @click="relogin('Spotify')">SPOTIFY</span>
        </div>
        <div class="col-4 appProfile">
          <img src="./assets/profile.png" id="appProfile"><span class="appProfileText">{{pseudo}}</span></span>              
          <pseudoModal v-if="pseudoModalActive == true && loadPseudoEnd == true && pseudo == null"><pseudoModal>  
        </div>
      </div>
      <div class="col-12 appControlPanel">
        <div><router-link to="/playlist"><span @click="showPlaylistHelperModal(true)"><img src="./assets/playlistsIcon.png"><br>playlists</span></router-link></div>
        <div><router-link to ="/events"><span><img src="./assets/eventIcon.png"><br>évènements</router-link></span></div>
        <div><router-link to ="/groups"><span><img src="./assets/groupIcon.png"><br>groupes</router-link></span></div>
        <div><router-link to="/moods"><span><img src="./assets/moodIcon.png" v-if="currentMood !== ''"><img src="./assets/moodOff.png" v-if="currentMood == ''"><br>ambiances</router-link></span></div>
        <div><router-link to="/mix"><span @click="startMix()"><img src="./assets/MixLogo.png" id="mixImage" v-if="currentMood == ''"><img src="./assets/triangleGrey.png" v-if="currentMood !== ''"><br>mix</router-link></span></div>
      </div>
    </div>
    <router-view></router-view>
  </div>
</template>

<script>
  import Login from './components/Login.vue'
  import playlist from './components/playlists2.vue'
  import moods from './components/moods2.vue'
  import events from './components/events2.vue'
  import groups from './components/groups.vue'
  import pseudoModal from './components/pseudoModal.vue'
  import Modal from './components/modal.vue'
  import MixService from './services/MixService'
  import { mapGetters, mapActions } from 'vuex'
  import AuthService from './services/AuthService'
  import PseudoService from './services/PseudoService'  
  import Vue from 'vue'
  import $ from 'jquery'

  export default {
  data () {
    return {
      endpoint: null,
      active: 'playlistsTab',
      isActive: true,
      true: true,
      loadPseudoEnd: false,
      Connected: false,
      check: false,
      label: '',
      image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png',
      localCriterias: [
        { label: 'energy', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'popularity', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'instrumentalness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'speechiness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'acousticness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'danceability', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'tempo', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'valence', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'duration_ms', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" }
      ]
    }
  },
  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(this.onAuthenticated);
  },
  methods: {
    ...mapActions(['showPlaylistHelperModal','sendPseudo','showPseudoModal', 'showEventModal', 'showMoodsModal', 'getIdentity', 'requestAsync', 'sendMoods', 'mix', 'sendMix', 'setLoading']),
    login(provider) {
    AuthService.login(provider);
    },
    async relogin(provider) {
      await AuthService.relogin(provider);
    },
    onAuthenticated() {
    this.$router.replace('/playlist');
    },
    authVerify : function ()  {
      if (AuthService.identity != null) {
        this.Connected = true;
        this.getIdentity(this.Connected);
      }
    },
    loadPseudo: async function() {
      var pseudo = await this.requestAsync(() => PseudoService.getPseudo());   
      this.sendPseudo(pseudo.Pseudo);
      this.loadPseudoEnd = true;
    },
    startMix: async function() {
      if(this.currentMood != "") {
        this.mix();
        this.setLoading(true);
        var mix = await MixService.mix(this.mixToMix);
        this.sendMix(mix);
      }
    }
  },
  computed: {
    ...mapGetters(['facebookConnected', 'deezerConnected', 'spotifyConnected', 'active','pseudo','playlists','pseudoModalActive','currentMood','checkedPlaylists','mixModalActive', 'playlistHelperModalActive', 'moods', 'test', 'enabledCriterias', 'criterias', 'authenticated', 'identity', 'mixToMix','loading'])
  },
  name: 'app',
  components: {
    Login,
    Modal,
    playlist,
    moods,
    events,
    groups,
    pseudoModal
  },
  mounted () {
  },
  created () {
    AuthService.registerAuthenticatedCallback(this.onAuthenticated);
    this.authVerify();
    this.loadPseudo();
  }
}
</script>

<style src="./styles/app.css">
</style>