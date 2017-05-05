<template>
  <div class="col-12 moodsGlobal">
    <div class="col-12 moodContainer">
      <div class="currentMood" v-for="mood in moods">
        <div class="topCurrentMood">
          <img v-bind:src="mood.cover" v-if="mood.cover !== ''" id="testt">
          <img src="../assets/moodNoCover.png" v-else>
          <span id="currentMoodName">{{mood.rowKey}}</span>
          <span v-if="mood.rowKey == 'Dance' || mood.rowKey == 'Energy' || mood.rowKey == 'Lounge' || mood.rowKey == 'Mad'"></span>
          <span id="deleteMood" @click="deleteMood(mood.rowKey)" v-else></span>
        </div>
        <div class="middleCurrentMood">
          <span v-for="data in mood.metadonnees">
            <img v-if="data < 1 && data > 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: 150*data +'px'}">
            <img v-if="data <= 100 && data > 1" src="../assets/bar.png" id="dataBar" v-bind:style="{height: data*1.5 +'px'}">
            <img v-if="data == 1 && data !== ''" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*75)+75 +'px'}">
            <img v-if="data == 0 && data !== ''" src="../assets/bar.png" id="dataBar" v-bind:style="{height: 1 +'px'}">
            <img v-if="data < 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*150)/(-60) +'px'}">
            <img v-if="data == '' || data == null" src="../assets/bar.png" id="dataBar" v-bind:style="{height: '150px', filter: 'grayscale(100%)', opacity: '0.99'}">
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
        <div class="bottomCurrentMood" v-if="currentMood.rowKey !== mood.rowKey" @click="setCurrentMood(mood)">
          <div class="selectMood">
            SÉLECTIONNER
          </div>
        </div>
        <div class="bottomCurrentMood green" v-if="currentMood.rowKey == mood.rowKey" @click="setCurrentMood(mood)">
          <div class="selectMood">
            SÉLECTIONNER
          </div>
        </div>
      </div>
      <!--<div class="currentMood addMood" @click="showMoodsModal(true)">
        <div class="topCurrentMood">
        </div>
        <div class="middleCurrentMood addMood" @click="showMoodsModal(true)">
          <img src="../assets/plus.png" id="plusMood">
        </div>
        <div class="bottomCurrentMood addMood" @click="showMoodsModal(true)">
          &nbsp
        </div>
      </div>-->
    </div>
    <div class="addMood">
      <img src="../assets/plus.png" id="plusMood" @click="showMoodsModal(true)">
    </div>
    <moodModal v-if="moodsModalActive == true"></moodmodal>
  </div>
</template>


<style>

body {
  overflow: hidden;
}

.moodsGlobal {
  height: 76vh;
  background: #0e1014;
  color: white;
}

.moodContainer {
  padding-left: 40px;
  height: 400px;
  white-space: nowrap;
  display: inline-block;
  text-align: center;
  overflow-x: auto;
}

.currentMood {
  display: inline-block;
  height: 380px;
  width: 220px;
  margin-right: 40px;
}

.topCurrentMood {
  overflow: hidden;
  position: relative;
  height: 120px;
}

.topCurrentMood img {
  width: 100%;
  filter: brightness(80%);
}

#currentMoodName {
  margin-top: 95px;
  padding-left: 10px;
  left: 0;
  top: 0;
  position: absolute;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
}

#deleteMood {
  padding-left: 10px;
  top: 5px;
  padding: 5px;
  cursor: pointer;
  right: 5px;
  position: absolute;
  color: white;
  width: 16px;
  height: 20px;
  background-image: url("../assets/closedTrash.png");
  background-size: 16px 20px;
  transition: all .3s ease;
}

#deleteMood:hover {
  background-image: url("../assets/oppenedTrash.png");
}

#deleteMood img {
  width: 16px;
  height: 20px;
}

.middleCurrentMood {
  text-align: center;
  height: 220px;
  background: white;
}

.bottomCurrentMood {
  height: 40px;
  background: #D9534F;
  color: #fff;
  font-family: 'Montserrat-Regular';
  letter-spacing: 1px;
  font-size: 16px;
  padding: 10px;
  cursor: pointer;
  transition: all .5s ease;
}

.bottomCurrentMood:hover {
  background: #5CB85C;
}

#dataBar {
  margin: 6px;
  width: 6px;
  margin-top: 30px;
}

.addMood {
  text-align: center;
}

#plusMood {
  margin-top: 20px;
  height: 100px;
  cursor: pointer;
}

#dataLetter {
  color: #0e1014;
  font-family: 'Montserrat-Ultra-Light';
  margin: 0 auto;
  display: table;
}

#dataLetter div {
  margin-left: 6px;
  margin-right: 6px;
  float: left;
  cursor: pointer;
}

#dataTitle {
  color: #de002b;
  font-family: 'Montserrat-Regular';
  font-size: 14px;
}

#dOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#eOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#sOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#aOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#iOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#lOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}

#pOverlay {
  font-family: 'Montserrat-Ultra-Light';
  visibility: hidden;
  position: absolute;
  background: #191B27;
  z-index: 2;
  width: 200px;
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  text-align: left;
  color: white;
  font-size: 12px;
  padding: 7px;
}


#danceability:hover > #dOverlay {
  visibility: visible;
}

#energy:hover > #eOverlay {
  visibility: visible;
}

#speechiness:hover > #sOverlay {
  visibility: visible;
}

#accousticness:hover > #aOverlay {
  visibility: visible;
}

#instrumentalness:hover > #iOverlay {
  visibility: visible;
}

#liveness:hover > #lOverlay {
  visibility: visible;
}

#popularity:hover > #pOverlay {
  visibility: visible;
}


.green {
  transition: all .8s ease;
  background: #5CB85C;
}

</style>

<script>

import { mapGetters, mapActions } from 'vuex'
import MoodService from '../services/MoodService'
import moodModal from '../components/addMoodModal.vue'


export default {
  data () {
    return {
      moodCover: '',
      moodName: '',
      moodCriterias: [
        { label: 'Accousticness', value: null},
        { label: 'Danceability', value: null},
        { label: 'Energy', value: null},
        { label: 'Instrumentalness', value: null},
        { label: 'Speechiness', value: null},
        { label: 'Liveness', value: null},
        { label: 'Loudness', value: null},
        { label: 'Popularity', value: null}
      ],
      metadonnees: {'Accousticness': null, 'Danceability': null, 'Energy': null, 'Instrumentalness': null, 'Speechiness': null, 'Liveness': null, 'Popularity': null},
      data: '',
      moodToCreate: {
        'cover': null,
        'name': null, 
        'metadonnees': null
      }
    }
  },
  methods: {
    ...mapActions(['showMoodsModal', 'sendMoods','setCurrentMood', 'requestAsync', 'insertMood']),

    loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },
    deleteMood: async function(mood) {
      var data = await MoodService.deleteMood(mood);
      this.loadMoods();
    },
    createLocalMood: async function(item) {
      this.moodToCreate.cover = this.moodCover;
      this.moodToCreate.name = this.moodName;
      if(this.moodCriterias[0].value == null) {
        this.metadonnees.Accousticness = this.moodCriterias[0].value;
      } else {
        this.metadonnees.Accousticness = this.moodCriterias[0].value/100;
      }

      if(this.moodCriterias[1].value == null) {
        this.metadonnees.Danceability = this.moodCriterias[1].value;
      } else {
        this.metadonnees.Danceability = this.moodCriterias[1].value/100;
      }

      if(this.moodCriterias[2].value == null) {
        this.metadonnees.Energy = this.moodCriterias[2].value;
      } else {
        this.metadonnees.Energy = this.moodCriterias[2].value/100;
      }

      if(this.moodCriterias[3].value == null) {
        this.metadonnees.Instrumentalness = this.moodCriterias[3].value;
      } else {
        this.metadonnees.Instrumentalness = this.moodCriterias[3].value/100;
      }

      if(this.moodCriterias[4].value == null) {
        this.metadonnees.Speechiness = this.moodCriterias[4].value;
      } else {
        this.metadonnees.Speechiness = this.moodCriterias[4].value/100;
      }

      if(this.moodCriterias[5].value == null) {
        this.metadonnees.Liveness = this.moodCriterias[5].value
      } else {
        this.metadonnees.Liveness = this.moodCriterias[5].value/100;
      }

      if(this.moodCriterias[6].value == null) {
        this.metadonnees.Loudness = this.moodCriterias[6].value
      } else {
        this.metadonnees.Loudness = (this.moodCriterias[6].value)/(-100)*60;
      }

      if(this.moodCriterias[7].value == null) {
        this.metadonnees.Popularity = this.moodCriterias[7].value;
      } else {
        this.metadonnees.Popularity = this.moodCriterias[7].value/100;
      }
      
      this.moodToCreate.metadonnees = this.metadonnees;
      this.insertMood(this.moodToCreate);
      var result = MoodService.createMood(this.moodToCreate);
    }
  },
  computed: {
    ...mapGetters(['moods', 'currentMood', 'moodsModalActive'])
  },
  created () {
    this.loadMoods()
  },
  components: {
    moodModal
  }
}
</script>
