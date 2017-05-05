<template>
  <div class="col-12 mixGlobal" id="mixGlobal"> 
    <div class="trackContainer">
      <scale-loader class="mixLoading" v-if="loading == true" :loading="loading"></scale-loader>
      <transition-group name="mFade" tag="div">
        <div v-for="track in finalMix" @click="selectTrack(track), addNextTrack(track)" class="singleTrack" v-bind:key="track.trackId">
          <img v-if="track.deezerId !== null" v-bind:src="track.cover" id="imageTrack">
          <scale-loader v-if="track.deezerId == playingTrack" id="imageTrackOverlay" :color="color"></scale-loader>
          <p>{{track.title}}<br><span id="albumName">{{track.albumName}}</span></p>
        </div>
      </transition-group>
      <div class="oldMixesContainer">
        <div class="showMixesButton" @click="show = !show">
          <img src="../assets/arrowLeft.png" v-if="show == false">
          <img src="../assets/arrowRight.png" v-if="show == true">
        </div>
        <div class="oldMixes" v-bind:class="{oppened: show}">
          <div v-for="mix in allMix" id="singleMix" @click="playOldMix(mix)">
            <img v-bind:src="mix.parsedPlaylist[0].cover">
            <div class="mixOverlay">
              <span>{{mix.rowKey}}</span>
            </div>
            <div class="deleteMix" @click="deleteMix(mix.rowKey)">
              &nbsp
            </div>
          </div>
        </div>
      </div>
      <mixModal v-if="mixModalActive == true"></mixModal>
    </div>
    <div class="saveMix" @click="showMixModal(true)" v-if="finalMix.length != 0">
      <span>SAUVEGARDER LE MIX</span>
    </div>
    <div id="dz-root">
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import mixModal from '../components/saveMixModal.vue'
import MixService from '../services/MixService'
import AuthService from '../services/AuthService'
import ScaleLoader from 'vue-spinner/src/GridLoader.vue'



import $ from 'jquery'	
 
export default {
  data () {
    return {
      DzPlayer: ['136336110','65938270','3129782'],
      mix: [],
      data: '',
      offsetWidth: '',
      show: true,
      playingTrack: ''
    }
  },
  mounted () {
    var me = this;
    this.getOffsetWidth()
    DZ.init({
      appId  : '176241',
      channelUrl : 'http://localhost:5000/mix',
      player: {
        container: 'dz-root',
        height:90,
        width: this.offsetWidth,
        playlist: false,
        onload : function(){
          me.playingTrack = DZ.player.getCurrentTrack().id;
        }
      }
    }); 
    DZ.Event.subscribe('current_track', function(track, evt_name){
      me.playingTrack = DZ.player.getCurrentTrack().id;
      me.setPlayingTrack(me.playingTrack);
    });
  },
  methods: {
    ...mapActions(['setCurrentTrack','deleteMix','playOldMix', 'selectTrack','retrieveMix','mixToMix','sendMix','mix','addNextTrack','showMixModal','requestAsync', 'setPlayingTrack']),
    setDeezerPlayer: function() {
      DZ.player.playTracks(this.finalPlaylist);
    },
    loadMix: async function() {
      var data = await this.requestAsync(() => MixService.getMix());
      this.retrieveMix(data);
    },
    deleteMix: async function(mix) {
      var data = await MixService.deleteMix(mix);
      this.loadMix();
    },
    getOffsetWidth: function() {
      this.offsetWidth = document.getElementById('mixGlobal').offsetWidth - 7;
    },
    getPlayingTrack: function(deezerId) {
      this.playingTrack = DZ.Event.subscribe('current_track', function(track, evt_name) {
        this.playingTrack = DZ.player.getCurrentTrack().id;
        console.log('Current track : '+this.playingTrack+', deezer Id : '+deezerId);
        return this.playingTrack;
      })
      // this.playingTrack = DZ.player.getCurrentTrack().id;
      // console.log(this.playingTrack);
    }
  },
  created () {  
    this.loadMix()
  },
  computed: {
    ...mapGetters(['mixModalActive','finalMix', 'identity', 'currentTrack','finalPlaylist','loading','allMix', 'toPlayer', 'trackInPlayer'])
  },
  components: {
     mixModal,
     ScaleLoader
  }
}
</script>

<style>
.mixGlobal {
  height: 76vh;
  background: #0e1014;
  color: white;
  position: relative;
  font-family: 'Montserrat-Ultra-Light';
  font-size: 10px;
}

.mixLoading {
  position: relative;
}

.trackContainer {
  position: relative;
  height: 60vh;
  overflow-y: scroll;
  padding: 100px;
  padding-top: 0px;
  margin: 0 auto;
}


.deleteMix {
  width: 14px;
  height: 17px;
  top: 3px;
  right: 3px;
  position: absolute;
  background-image: url('../assets/closedTrash.png');
  background-size: 14px 17px;
  z-index: 2;
  cursor: pointer;
}

.singleTrack {
  height: 140px;
  width: 120px;
  float: left;
  margin: 14px;
  margin-bottom: 25px;
  position: relative;
}

.singleTrack p {
  margin-top: 5px;
  width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

#imageTrack {
  height: 120px;
  width: 120px;
  float: left;
  margin-bottom: 15px;
  position: relative
}

#imageTrackOverlay {
  position: absolute;
  height: 100%;
  width: 100%;
  overflow: hidden;
}

#albumName {
  color: silver;
}

#dz-root {
  width: 100%;
  position: absolute;
  bottom: 110px;
  width: 100%;
  height: 10vh;
}

#nextTrack {
  color:white;
  background-color: black;
  height: 20px;
  width: 20px;
  position: absolute;
  z-index: 1;
}

.oldMixesContainer {
  position: absolute; 
  right: 0;
  font-size: 14px;
  top: 10vh;
}

.showMixesButton {
  float: left;
  margin-top: 130px;
}

.showMixesButton img {
  height: 40px;
  width: 40px;
}

.oldMixes {
  background: #171c27;
  width: 0px;
  height: 280px;
  float: left;
  transition: all .3 ease;
  padding: 5px;
  visibility: hidden;
  overflow-x: hidden;
  position: relative;
  overflow-y: auto;
}

#singleMix {
  position: relative;
  width: 90px;
  height: 90px;
}

.mixOverlay {
  position: absolute;
  background: black;
  height: 100%;
  width: 100%;
  opacity: 0;
  top: 0;
  left: 0;
  transition: all .3s ease;
  font-size: 12px;
  z-index: 2;
  padding: 4px;
}

.saveMix {
  font-size: 20px;
  text-align: center;
}

.saveMix span {
  cursor: pointer;
}

.mixOverlay span {
}

#singleMix:hover .mixOverlay {
  opacity: 0.7;
}

.oldMixes img {
  width: 90px;
  height: 90px;
}

.oppened {
  transition: all .3 ease;
  width: 100px;
  visibility: visible;
}

.mFade-move {
  transition: transform 1s;
}

.mFade-enter-active, .mFade-leave-active {
  transition: opacity .5s, transform .5s;
}

.mFade-enter, .mFade-leave-active {
  opacity: 0;
  transform: translateY(-200px);
}


</style>