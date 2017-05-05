<template>
  <transition name="pseudoModal">
    <div class="pseudoModal-mask">
      <div class="pseudoModal-wrapper">
        <div class="pseudoModal-container">
          <div class="addPseudoText">
            <div class="addPseudoModal">
              <span id="pseudoTitle">CHOISISSEZ UN PSEUDO</span><br>              
              <br>
              <span id="smallText">Pseudo : <input type="text" style="color: white;" v-model="pseudo"><br></span>
              <br><br>
              <span id="pseudotext" class="redText">Attention, vous ne pourrez plus le modifier</span><br>              
            </div>
          </div>
          <div v-if="pseudo != ''" class="modalClose" @click="showPseudoModal(false),savePseudo({pseudo}),loadPseudo()">
            SAUVEGARDER <img src="../assets/arrow.png" >
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import PseudoService from '../services/PseudoService'

export default {
  data () {
    return {
      end: false,
      pseudo:'',
      pseudoToSave:''   
    }
  },
  methods: {
    ...mapActions(['showPseudoModal','requestAsync','insertPseudo','sendPseudo']),

  /*  loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },*/
    savePseudo: async function(item) {
      this.pseudoToSave= this.pseudo;
     // this.insertPseudo(this.pseudoToSave);
      var result = PseudoService.SavePseudo(this.pseudoToSave);
      this.end = true;
      if(this.end == true){
        var pseudo = await this.requestAsync(() => PseudoService.getPseudo());   
        this.sendPseudo(pseudo.Pseudo);
        console.log(this.end);   
      } else {
        console.log('erreur',this.end);
      }
      
      
    },
    loadPseudo: async function() {
      var pseudo = await this.requestAsync(() => PseudoService.getPseudo());   
      this.sendPseudo(pseudo.Pseudo);
    },
  },
  computed: {
    ...mapGetters(['Pseudo'])
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

.pseudoModal-mask {
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

.pseudoModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.pseudoModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 200px;
  width: 300px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addPseudoModal {
  text-align: center;
}

.addPseudoText {
  height: 100%;
  padding: 30px;
  width: 100%;
}

.modalClose {
  width: 180px;
  padding: 10px;
  background: #de002b;
  color: black;
  font-family: 'Montserrat-Regular';
  cursor: pointer;
  text-transform: uppercase;
  text-align: left;
}

.modalClose img {
  margin-left: 6px;
  width: 25px;
}

#pseudoTitle {
  font-size: 16px;
}

#pseudotext {
  font-size: 15px;
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

.pseudoModal-enter {
  opacity: 0;
}
.pseudoModal-leave-active {
  opacity: 0;
}
.pseudoModal-enter .pseudoModal-container,
.pseudoModal-leave-active .pseudoModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

#closeModal {
  position: absolute;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
  cursor: pointer
}
#closePseudoModal {
  top: 7px;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
  cursor: pointer
}
</style>