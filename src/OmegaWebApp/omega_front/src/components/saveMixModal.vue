<template>
  <transition name="mixModal">
    <div class="mixModal-mask">
      <div class="mixModal-wrapper">
        <div class="mixModal-container">
          <div class="addMixText">
            <div class="addMixModal">
              <span id="mixTitle">NOUVEAU MIX</span><br>
              <br>
              <span id="smallText">Nom : <input type="text" style="color: white;" v-model="name"><br><span>
            </div>
          </div>
          <div class="modalClose" @click="saveMix({name, playlist}), showMixModal(false)">
            <span>SAUVEGARDER</span><img src="../assets/arrow.png">
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import MixService from '../services/MixService'

export default {
  data () {
    return {
      name:'',
      playlist: this.finalMix,
      mixToSave: {
        'name': null,
        'playlist': this.finalMix
      },
    }
  },
  methods: {
    ...mapActions(['showMixModal','requestAsync','insertMix', 'retrieveMix']),

  /*  loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },*/
    saveMix: async function(item) {
      this.mixToSave.name = this.name;
      this.mixToSave.playlist = this.finalMix;
      this.insertMix(this.mixToSave);
      var result = await MixService.SaveMix(this.mixToSave);
      this.loadMix();
    },
    loadMix: async function() {
      var data = await this.requestAsync(() => MixService.getMix());
      this.retrieveMix(data);
    }
  },
  computed: {
    ...mapGetters(['finalMix'])
  },
  created () {
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

.mixModal-mask {
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

.mixModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.mixModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 100px;
  width: 300px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addMixModal {
  text-align: center;
}

.addMixText {
  height: 100%;
  padding: 20px;
  width: 100%;
}

.modalClose {
  font-size: 16px;
}

.modalClose img {
  margin-left: 6px;
  width: 25px;
}

#mixTitle {
  font-size: 22px;
}

#smallText {
  font-size: 14px;
}

input[type="text"] {
  border: 0;
  border-bottom: 1px solid silver;
  width: auto;
  background: #191B27;
}

.mixModal-enter {
  opacity: 0;
}
.mixModal-leave-active {
  opacity: 0;
}
.mixModal-enter .mixModal-container,
.mixModal-leave-active .mixModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>