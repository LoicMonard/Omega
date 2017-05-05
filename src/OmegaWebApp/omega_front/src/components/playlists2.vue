<template>
  <div class="col-12 playlistGlobal">
    <div class="col-12 playlistContainer">  
      <img src="../assets/rightButton.png" class="rightScroll" @click="scrollRight">
      <img src="../assets/leftButton.png" class="leftScroll" @click="scrollLeft">
      <div class="playlistWrapper" id="playlistWrapper">
      <scale-loader class="playlistLoading" v-if="loading == true" :loading="loading"></scale-loader>                
        <transition-group name="fade" appear>
          <div v-for="p in playlists" id="spanPlaylist" key="p" v-if="visible">
            <img v-if="p.check == false" v-bind:src="p.Cover" class="playlistImage">
            <img v-else="p.check == true" v-bind:src="p.Cover" class="checkedImage">
            <img v-if="p.check == true" src="../assets/checkIcon.png" id="checkIcon">
            <span class="imageOverlay">
              <div class="selectOverlay" @click="selectPlaylist(p)">
                <div class="selectOverlayImg" v-if="p.check == false">&nbsp</div>
                <div class="selectedOverlayImg" v-else>&nbsp</div>
              </div>
              <div class="playOverlay" @click="setCurrentPlaylist(p), setSPlayer(), setDPlayer()">
                <div class="playOverlayImg">&nbsp</div>
              </div>
            </span>
            <span class="playlistProvider">
              <span v-if="p.provider == 's'" class="sPlaylistBanner">
                <img src="../assets/spotifyWhiteLogo.png">
              </span>
              <span v-if="p.provider == 'd'" class="sPlaylistBanner">
                <img src="../assets/deezerWhiteLogo.png">
              </span>
            </span>
            <p>{{p.Name}}</p>
            <p id="albumName">{{p.Pseudo}}</p>
          </div>
        </transition>
      </div>
    </div>

    <!--Contains the bottom part of the playlist vue-->

    <div class="col-12 playlistNavbar">

      <!--Left-->

      <div class="col-7 leftBottom">
        <div class="playlistAndTracks">
          <img v-bind:src="currentPlaylist.Cover" id="currentPlaylistCover">
          <iframe v-if="currentPlaylist.provider == 's'" v-bind:src="sPlayer" width="400px" height="99%" frameborder="0" allowtransparency="true"></iframe>
          <iframe v-if="currentPlaylist.provider == 'd'" v-bind:src="DzPlayer" scrolling="no" frameborder="0" allowTransparency="true"  width="400px" height="99%"></iframe>
        </div>
      </div>

      <!--Right-->
      
      <div class="col-5 rightBottom">
        <div class="pCurrentMood" v-if="currentMood !== ''">
          <div class="pTopCurrentMood">
            <img v-bind:src="currentMood.cover">
            <span id="pCurrentMoodName">{{currentMood.rowKey}}</span>
            <span id="deleteMood">X</span>
          </div>
          <div class="pMiddleCurrentMood">
            
            <span v-for="data in currentMood.metadonnees">
              <img v-if="data < 1 && data > 0" src="../assets/bar.png" id="pDataBar" v-bind:style="{height: 150*data +'px'}">
              <img v-if="data <= 100 && data > 1" src="../assets/bar.png" id="pDataBar" v-bind:style="{height: data*1.5 +'px'}">
              <img v-if="data == 1 || data == 0 && data != ''" src="../assets/bar.png" id="pDataBar" v-bind:style="{height: (data*75)+75 +'px'}">
              <img v-if="data < 0" src="../assets/bar.png" id="pDataBar" v-bind:style="{height: (data*150)/(-60) +'px'}">
              <img v-if="data == '' || data == null" src="../assets/bar.png" id="pDataBar" v-bind:style="{height: '150px', filter: 'grayscale(100%)', opacity: '0.99'}">
            </span><br>
            <span id="dataLetter">
              <div id="danceability">D<span id="dOverlay"><span id="dataTitle">Danceability</span><br> définit le caractère dansant des musiques</span></div>
              <div id="energy">E<span id="eOverlay"><span id="dataTitle">Energy</span><br> définit le caractère energique des musiques</span></div>
              <div id="speechiness">S<span id="sOverlay"><span id="dataTitle">Speechiness</span><br> définit le taux de paroles des musiques</span></div>
              <div id="accousticness">A<span id="aOverlay"><span id="dataTitle">Accousticness</span><br> définit le caractère acoustic des musiques</span></div>
              <div id="instrumentalness">I<span id="iOverlay"><span id="dataTitle">Instrumentalness</span><br> définit le taux d'instrumentale des musiques</span></div>
              <div id="liveness">L<span id="lOverlay"><span id="dataTitle">Liveness</span><br> définit la présence de live dans les musiques</span></div>
              <div id="popularity">P<span id="pOverlay"><span id="dataTitle">Popularity</span><br> définit la popularité des musiques</span></div>
            </span>
          </div>
        </div>
        <div class="pCurrentEvent" v-if="currentEvent !== ''">
          <div id="pEventCover">
            <img v-bind:src="currentEvent.Cover">
          </div>
          <div class="pEventInfo">
            <span id="pEventName">{{currentEvent.Name}}</span>
            <span id="pEventLocation">{{currentEvent.Location}}</span>
            <div class="pEventDateTime">
              <span id="pEventDay">{{currentEvent.Day}}</span><br>
              <span id="pEventMonth">{{currentEvent.Month}}</span>
            </div>
            <div class="pRemainingTime">
              <span id="pTempsRestant">temps restant</span><br>
              <span id="pDaysLeft">{{currentEvent.timeRemaining}}</span>
              jours
            </div>
          </div>
        </div> 
      </div>
        </div>
    <PlaylistHelperModal v-if="playlistHelperModalActive == true && playlists.length == 0 && pseudoModalActive == false"></PlaylistHelperModal>
    <EventModal v-if="eventModalActive == true"></EventModal>
    <MoodsModal v-if="moodsModalActive == true"><MoodsModal>
  </div>
  </div>
</template> 

<script>
import { mapGetters, mapActions } from 'vuex'
import PlaylistHelperModal from '../components/playlistHelperModal.vue'
import EventModal from '../components/events.vue'
import MoodsModal from '../components/moods.vue'
import PlaylistApiService from '../services/PlaylistApiService'
import SpotifyApiService from '../services/SpotifyApiService'
import DeezerApiService from '../services/DeezerApiService'
import MoodService from '../services/MoodService'
import PseudoService from '../services/PseudoService'
import MixService from '../services/MixService'
import AuthService from '../services/AuthService'
import ScaleLoader from 'vue-spinner/src/ScaleLoader.vue'


export default {
  data () {
    return {
      localfinalMix: '',
      sPlayer: '',
      visible: true,
      DzPlayer: '',
      sPlaylists: [],
      Connected: false,
      dPlaylists: [],
      SDplaylists: [],
      currentTrack: '',
      pseudo:'',
      number: 1,
      height: '',
      localPlaylists: [{"id":0,"name":"Apero Party","image":"http://d817ypd61vbww.cloudfront.net/sites/default/files/styles/media_responsive_widest/public/tile/image/A-116-01comp.jpg?itok=LjWmwzjU"},{"id":1,"name":"Mexico","image":"https://caliloved.files.wordpress.com/2013/07/deer-album-cover-new.jpg"},{"id":2,"name":"Chanson francaise","image":"http://www.designformusic.com/wp-content/uploads/2016/01/perfectly-chilled-album-cover-artwork-design-500x500.jpg"},{"id":3,"name":"Cam box","image":"https://www.smashingmagazine.com/images/music-cd-covers/64.jpg"},{"id":4,"name":"Jaccuzi money billey","image":"https://img.buzzfeed.com/buzzfeed-static/static/2016-01/27/11/enhanced/webdr14/enhanced-6784-1453912540-22.jpg"},{"id":5,"name":"Beer-Pong","image":"http://androidjones.com/wp-content/uploads/2012/05/HOPE-1024x1024.jpg"},{"id":6,"name":"Runing Time","image":"http://illusion.scene360.com/wp-content/uploads/2014/10/computergraphics-album-covers-2014-05.jpg"},{"id":7,"name":"Soiree OKLM","image":"http://www.fuse.tv/image/5682ea90ac0e76bd68000055/768/512/brown-eyed-girls-basic-album-cover-full-size.jpg"},{"id":8,"name":"Apero Party","image":"http://takuya.fr/wp-content/uploads/2016/06/takuya-fr-thedoubt.jpg"}],
      mood: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
    }
  },
  methods: {
    ...mapActions(['sendMix','pseudoModalActive','setLoading', 'checkPlaylist','sendPseudo', 'setCurrentPlaylist','getPseudo', 'selectPlaylist', 'sendPlaylists', 'requestAsync', 'inserteMood', 'mix', 'getIdentity', 'showPlaylistHelperModal']),
    setSPlayer: function() {
      var player = 'https://embed.spotify.com/?uri=spotify:user:'+ this.currentPlaylist.OwnerId +':playlist:'+ this.currentPlaylist.PlaylistId;
      this.sPlayer = player;
    },
    toggle: function() {
      this.visible = !this.visible;
    },
    setDPlayer: function() {
     var player = 'https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=true&width=350&height=350&color=007FEB&layout=dark&size=medium&type=playlist&id='+ this.currentPlaylist.PlaylistId +'&app_id=176241';
      this.DzPlayer = player;
    },
    scrollRight: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('playlistWrapper').scrollLeft += scroll + 300;
    },
    scrollLeft: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('playlistWrapper').scrollLeft -= scroll + 300;
    },
    insertMood: async function() {
      this.$http.post('http://localhost:5000/api/Ambiance/InsertAmbiance', this.mood, function () {
       })
    },
    loadPlaylists: async function() {
      this.setLoading(true);      
      this.SDplaylists = await this.requestAsync(() => PlaylistApiService.getPlaylists());
      this.SDplaylists.map(m => { this.$set(m, 'check', true); return m});
      this.sendPlaylists(this.SDplaylists);
      if(this.playlists[0].provider == 's') 
        this.setSPlayer()
      else 
        this.setDPlayer()
    },
    loadSpotifyPlaylist: async function() {
      var spplaylist = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
      this.sPlaylists.push(spplaylist);
    },
    loadDeezerPlaylist: async function() {
      var dzplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
      this.dPlaylists.push(dzplaylist);
    },
    loadPseudo: async function() {
      var pseudo = await this.requestAsync(() => PseudoService.getPseudo());   
      this.sendPseudo(pseudo.Pseudo);
      this.loadPseudoEnd = true;
    },
    startMix: async function() {
      this.mix();
      this.localfinalMix = await MixService.mix(this.mixToMix);
      this.sendMix(this.localfinalMix);
    }
  },
  computed: {
    ...mapGetters(['playlistHelperModalActive','pseudo','loading', 'eventModalActive', 'moodsModalActive', 'playlists', 'currentPlaylist', 'currentMood', 'currentEvent', 'currentGroup', 'checkedPlaylists', 'moodToInsert', 'mixToMix', 'identity','finalMix'])
  },
  created () {
    if(this.playlists.length === 0) {
      this.loadPlaylists();
      this.loadSpotifyPlaylist();
      this.loadDeezerPlaylist();
      this.loadPseudo();
    } else {
    }
    this.getIdentity(true);
    this.loadPseudo();
  },
  mounted () {
  },
  components: {
    PlaylistHelperModal,
    EventModal,
    MoodsModal,
    ScaleLoader
  }
}
</script>

<style>
/*Set the element size and position*/
.playlistGlobal {
  height: 76vh;
}

.playlistContainer {
  height: 30vh;
  min-height: 169px;
  background: #0e1014;
  padding-left: 40px;
  padding-right: 40px;
}

.playlistNavbar {
  height: 46vh;
  min-height: 299px;
  background: #171c27;
}

/*Playlists display*/
.playlistWrapper {
  height: 100%;
  width: 100%;
  white-space: nowrap;
  overflow-x: auto;
  position: relative;
}

.playlistLoading {
  position: relative;
}

#spanPlaylist {
  position: relative;
  transition: all .5s ease;
  color: white;  
  display: inline-block;
  width: 200px;
  height: 220px;
  margin-bottom: 10px;
  margin: 10px;
}

#spanPlaylist p {
  margin: 0;
  font-family: 'Montserrat-Ultra-Light';
  width: 100%;
  font-size: 12px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.playlistImage {
  width: 200px;
  filter: grayscale(100%);
  box-shadow: 0px 0px 24px 1px rgba(0,0,0,1);
  transition: all .5s ease;
}

#spanPlaylist:hover > .imageOverlay {
  opacity: 1;
}

.checkedImage {
  width: 200px;
  box-shadow: 0px 0px 24px 1px rgba(0,0,0,1);
  transition: all .5s ease;
}

.imageOverlay {
  position: absolute;
  width: 200px;
  height: 200px;
  left: 0;
  opacity: 0;
  transition: all .5s ease;
  font-family: 'Montserrat-ultra-light';
  font-size: 14px;
  text-align: center;
  text-transform: uppercase;
  text-overflow: ellipsis;
  z-index: 4;
}

.selectOverlay {
  background: black;
  opacity: 0.8;
  width: 100%;
  height: 50%;
  display: table;
  margin: 0 auto;
  position: relative;
}

.selectOverlayImg {
  height: 50px;
  width: 50px;
  background-image: url('../assets/selectIcon.png');
  position: absolute;
  top: 25px;
  left: 75px;
  margin: 0 auto;
  background-size: 50px 50px;
  transition: all .5s ease;
  cursor: pointer;
}

.selectOverlayImg:hover {
  background-image: url('../assets/selectedIcon.png');
}

.selectedOverlayImg {
  height: 50px;
  width: 50px;
  background-image: url('../assets/selectedIcon.png');
  position: absolute;
  top: 25px;
  left: 75px;
  margin: 0 auto;
  background-size: 50px 50px;
  transition: all .5s ease;
  cursor: pointer;
}

.selectedOverlayImg:hover {
  background-image: url('../assets/selectIcon.png');
}

.playOverlay {
  background: #0e1014;
  opacity: 0.7;
  width: 100%;
  height: 50%;
  display: table;
  margin: 0 auto;
  position: relative;
}

.playOverlayImg {
  height: 50px;
  width: 50px;
  background-image: url('../assets/playIcon.png');
  position: absolute;
  top: 25px;
  left: 75px;
  margin: 0 auto;
  background-size: 50px 50px;
  transition: all .5s ease;
  cursor: pointer;
}

.playOverlayImg:hover {
  background-image: url('../assets/playedIcon.png');
}

.playlistProvider {
  height: 100%;
  width: 100%;
  position: absolute;
  top: 0;
  left: 0;
}

#checkIcon {
  position: absolute;
  top: 0;
  right: 0;
  width: 20px;
  height: 20px;
  padding: 3px;
  z-index: 5;
}

#settingsImage {
  width: 30px;
  filter: invert(1);
  vertical-align: middle;
}

.sPlaylistBanner {
  position: absolute;
  top: 0;
  left: 0;
  height: 20px;
  padding: 4px;
}

.sPlaylistBanner img {
  height: 20px;
  width: 20px;
  opacity: 0.8;
}

.dPlaylistBanner {
  position: absolute;
  bottom: 0;
  left: 0;
  height: 25px;
  background: green;
  border-radius: 0px 0px 2px 2px;
  padding: 2px;
  font-size: 13px;
  font-family: 'Montserrat-ultra-light';
}

.playlistSettings:hover {
  background: blue;
  height: 100px;
}

.rightScroll {
  position: absolute;
  width: 18px;
  opacity: 0.5;
  z-index: 5;
  right: 0;  
  margin-right: 10px;
  top: 35%;
  transition: all .3s ease;
}

.leftScroll {
  position: absolute;
  width: 18px;
  opacity: 0.5;
  z-index: 5;
  left: 0;
  margin-left: 10px;
  top: 35%;
  transition: all .3s ease;
}

.leftScroll:hover, .rightScroll:hover {
  width: 22px;
  opacity: 0.7;
}

/*Left bottom panel display*/

.leftBottom {
  padding: 10px;
  height: 100%;
}

.playlistAndTracks {
  height: 100%;
  float: right;
  color: white;
  display: inline-block;
  white-space: nowrap;
  overflow: hidden;
}

#currentPlaylistCover {
  height: 100%;
}

.tracks {
  min-width: 200px;
  height: 100%;
  float: left;
  white-space: nowrap;
  display: inline-block;
  overflow-x: hidden;
}

.track {
  font-size: 12px;
  font-family: 'Montserrat-ultra-light';
  padding-left: 10px;
  padding-top: 10px;
  white-space: nowrap;
  height: 35px;
}

#playlistTitle {
  font-family: 'Montserrat-ultra-light';
  font-size: 24px;
  text-transform: uppercase;
  margin-left: 10px;
}

#playlistOwner {
  font-family: 'Montserrat-regular';
  font-size: 14px;
  line-height: 30px;
  margin-left: 10px;
}

#tracksLength {
  font-family: 'Montserrat-ultra-light';
  line-height: 40px;
  font-size: 14px;
  margin-left: 10px;
}

.playlistCover-enter-active, .playlistCover-leave-active {
  transition: opacity .5s
}

.playlistCover-enter, .playlistCover-leave-active {
  opacity: 0
}

/*Right bottom panel display*/

.rightBottom {
  height: 100%;
  padding: 10px;
  background: #12161e;
  font-family: 'Montserrat-Ultra-Light';
  color: white;
  display: inline-block;
  white-space: nowrap;
  position: relative;
}

.pCurrentMood {
  height: 100%;
  width: 220px;
  overflow: hidden;
  white-space: nowrap;
  display: inline-block;
}

.pTopCurrentMood {
  height: 33%;
  overflow: hidden;
  position: relative;
}

.pTopCurrentMood img {
  width: 100%;
  filter: brightness(50%);
}

#pCurrentMoodName {
  position: absolute;
  bottom: 5px;
  left: 5px;
  font-family: 'Montserrat-Ultra-Light';
  color: white;
}

#pDeleteMood {
  position: absolute;
  top: 5px;
  right: 5px;
}

.pMiddleCurrentMood {
  height: 67%;
  text-align: center;
  background: white;
}

#pDataBar {
  margin: 6px;
  width: 6px;
  margin-top: 30px;
}

.pCurrentEvent {
  width: 220px;
  background: #fff;
  white-space: nowrap;
  display: inline-block;
  height: 100%;
  text-overflow: ellipsis;
  text-align: left;
  float: left;
  margin-right: 10px;
}

#pEventCover {
  width: 100%;  
  height: 33%;
  overflow: hidden;
}

.pEventInfo {
  float: left;
  padding-top: 30px;
  padding-left: 20px;
  color: black;
  width: 220px;
  position: relative;
  height: 67%;
}

#pEventName {
  font-family: 'Montserrat-Regular';
  text-transform: uppercase;
  font-size: 24px; 
  text-overflow: ellipsis; 
  display: block; 
  overflow: hidden; 
  overflow-wrap: break-word;
  white-space: normal;
  max-height: 60px;
  width: 160px;
}

#pEventLocation {
  font-family: 'Montserrat-Regular';
  text-overflow: ellipsis; 
  display: block; 
  overflow: hidden; 
  overflow-wrap: break-word;
  white-space: normal;
  font-size: 12px;
  color: #FCB42A;
  width: 150px;
}

.pEventDateTime {
  position: absolute;
  top: 27px;
  right: 20px;
  text-align: center;
}

#pEventDay {
  font-family: 'Montserrat-Regular';
  font-size: 30px;
  font-weight: bold;
  color: #FCB42A;
}

#pEventMonth {
  font-size: 14px;
  font-family: 'Montserrat-Ultra-Light';
  text-transform: uppercase;
}

.pRemainingTime {
  margin-top: 60px;
  color: silver;
  position: absolute;
  bottom: 100px;
}

#pTempsRestant {
  font-family: 'Montserrat-Ultra-Light';
  font-size: 12px;
}

#pDaysLeft {
  font-family: 'Montserrat-Ultra-Light';
  font-size: 26px;
  margin-left: 20px;
  color: black;
}


@media screen and (max-height: 900px) {
  .playlistImage, .checkedImage, .imageOverlay, #spanPlaylist {
    width: 180px;
    height: 180px;
  }
  .playOverlayImg, .selectedOverlayImg {
    height: 40px;
    width: 40px;
    background-size: 40px 40px;
    left: 70px;
    top: 20px;
  }
}

@media screen and (max-height: 800px) {
  .playlistImage, .checkedImage, .imageOverlay, #spanPlaylist {
    width: 160px;
    height: 160px;
  }
  .playOverlayImg, .selectedOverlayImg {
    height: 35px;
    width: 35px;
    background-size: 35px 35px;
    left: 63px;
    top: 22px;
  }
}

@media screen and (max-height: 700px) {
  .playlistImage, .checkedImage, .imageOverlay, #spanPlaylist {
    width: 140px;
    height: 140px;
  }
  .playOverlayImg, .selectedOverlayImg {
    height: 30px;
    width: 30px;
    background-size: 30px 30px;
    left: 55px;
    top: 20px;
  }
}

</style>