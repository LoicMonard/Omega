<template>
  <transition name="moodModal">
    <div class="moodModal-mask">
      <div class="moodModal-wrapper">
        <div class="moodModal-container">
          <span id="closeModal" @click="showMoodsModal(false)">X</span>
          <div class="addMoodText">
            <div class="addMoodModal">
              <span id="moodTitle">NOUVELLE AMBIANCE</span><br>
              <br>
              <span id="smallText">Nom : <input type="text" v-model="moodName"><br><span>
              <span id="smallText">Image : <input type="text" v-model="moodCover"></span>
              <!--<input type="file" name="image">-->
              <br><br>
              <span id="aDataLetter">
                <div id="danceability">D<span id="dOverlay"><span id="dataTitle">Danceability</span><br> définit le caractère dansant des musiques</span></div>
                <div id="energy">E<span id="eOverlay"><span id="dataTitle">Energy</span><br> définit le caractère energique des musiques</span></div>
                <div id="speechiness">S<span id="sOverlay"><span id="dataTitle">Speechiness</span><br> définit le taux de paroles des musiques</span></div>
                <div id="accousticness">A<span id="aOverlay"><span id="dataTitle">Accousticness</span><br> définit le caractère acoustic des musiques</span></div>
                <div id="instrumentalness">I<span id="iOverlay"><span id="dataTitle">Instrumentalness</span><br> définit le taux d'instrumentale des musiques</span></div>
                <div id="liveness">L<span id="lOverlay"><span id="dataTitle">Liveness</span><br> définit la présence de live dans les musiques</span></div>
                <div id="popularity">P<span id="pOverlay"><span id="dataTitle">Popularity</span><br> définit la popularité des musiques</span></div>
              </span>
              <span class="allCriterias" v-for="data in moodCriterias">
                <input v-if="data.value !== null" type="range" v-model="data.value" id="singleCriteria" v-bind:class="{active: data.value == null}"><span v-if="data.value !== null" id="criteriaValue">{{data.value}}</span>
                <input v-if="data.value == null" type="range" v-model="data.value" id="singleCriteria"><span v-if="data.value == null" id="criteriaValue">{{data.value}}</span>
              </span><br>
            </div>
          </div>
          <div v-if="moodName != ''" class="moodModalClose" @click="createLocalMood({moodCriterias, moodName, moodCover}), showMoodsModal(false)">
            créer <img src="../assets/arrow.png">
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import MoodService from '../services/MoodService'

export default {
  data () {
    return {
      input: '',
      moodCover: '',
      moodName: '',
      moodCriterias: [
        { label: 'Accousticness', value: null},
        { label: 'Danceability', value: null},
        { label: 'Energy', value: null},
        { label: 'Instrumentalness', value: null},
        { label: 'Speechiness', value: null},
        { label: 'Liveness', value: null},
        { label: 'Popularity', value: null}
      ],
      metadonnees: {'Danceability': null, 'Energy': null, 'Speechiness': null, 'Accousticness': null, 'Instrumentalness': null, 'Liveness': null, 'Popularity': null},
      data: '',
      moodToCreate: {
        'cover': null,
        'name': null, 
        'metadonnees': null
      },
      mood: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
      
    }
  },
  methods: {
    ...mapActions(['showMoodsModal', 'sendMoods','setCurrentMood', 'requestAsync', 'insertMood']),

    loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },
    createLocalMood: async function(item) {
      this.moodToCreate.cover = this.moodCover;
      this.moodToCreate.name = this.moodName;
      if(this.moodCriterias[0].value == null) {
        this.metadonnees.Danceability = this.moodCriterias[0].value;
      } else {
        this.metadonnees.Danceability = this.moodCriterias[0].value/100;
      }

      if(this.moodCriterias[1].value == null) {
        this.metadonnees.Energy = this.moodCriterias[1].value;
      } else {
        this.metadonnees.Energy = this.moodCriterias[1].value/100;
      }

      if(this.moodCriterias[2].value == null) {
        this.metadonnees.Speechiness = this.moodCriterias[2].value;
      } else {
        this.metadonnees.Speechiness = this.moodCriterias[2].value/100;
      }

      if(this.moodCriterias[3].value == null) {
        this.metadonnees.Accousticness = this.moodCriterias[3].value;
      } else {
        this.metadonnees.Accousticness = this.moodCriterias[3].value/100;
      }

      if(this.moodCriterias[4].value == null) {
        this.metadonnees.Instrumentalness = this.moodCriterias[4].value;
      } else {
        this.metadonnees.Instrumentalness = this.moodCriterias[4].value/100;
      }

      if(this.moodCriterias[5].value == null) {
        this.metadonnees.Liveness = this.moodCriterias[5].value
      } else {
        this.metadonnees.Liveness = this.moodCriterias[5].value/100;
      }

      if(this.moodCriterias[6].value == null) {
        this.metadonnees.Popularity = this.moodCriterias[6].value;
      } else {
        this.metadonnees.Popularity = this.moodCriterias[6].value/100;
      }
      
      this.moodToCreate.metadonnees = this.metadonnees;
      this.insertMood(this.moodToCreate);
      var result = await MoodService.createMood(this.moodToCreate);
      this.loadMoods();
    }
  },
  computed: {
    ...mapGetters(['moods', 'currentMood'])
  },
  created () {
    this.loadMoods()
  },
}

</script>

<style>
@font-face {
    font-family: 'montserrat-ultra-light';
    src:url('../assets/montserrat-ultra-light.otf');
    font-family: 'Montserrat-Regular';
    src:url('../assets/Montserrat-Regular.otf');
}

.moodModal-mask {
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  display: table;
  transition: opacity .3s ease;
}

.moodModal-wrapper {
  position: absolute;
  top: 30%;
  left: 35%;
  display: table-cell;
}

.moodModal-container {
  height: 300px;
  width: 300px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addMoodModal {
  text-align: center;
}

.allCriterias {
  float: left;
}

#criteriaValue {
  position: absolute;
  margin-left: 10px;
}

.addMoodText {
  height: 100%;
  padding: 20px;
  width: 100%;
}

.moodModalClose {
  width: 120px;
  padding: 10px;
  background: #de002b;
  color: black;
  font-family: 'Montserrat-Regular';
  cursor: pointer;
  text-transform: uppercase;
}

.moodModalClose img {
  margin-left: 6px;
  width: 25px;
}

#moodTitle {
  font-size: 22px;
}

#smallText {
  font-size: 14px;
}

#singleCriteria {
  background: silver;
}

#aDataLetter {
  width: 20px;
  color: #fff;
  font-family: 'Montserrat-Ultra-Light';
  display: table;
  float: left;
  margin-right: 10px;
}

#aDataLetter :not(#dataTitle) {
  margin-left: 20px;
  z-index: 9999;
}

#closeModal {
  position: absolute;
  right: 7px;
  top: 7px;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
  cursor: pointer
}

input[type="text"] {
  border: 0;
  border-bottom: 1px solid silver;
  width: auto;
  background: #191B27;
  color: white;
}

.moodModal-enter {
  opacity: 0;
}
.moodModal-leave-active {
  opacity: 0;
}
.moodModal-enter .moodModal-container,
.moodModal-leave-active .moodModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>