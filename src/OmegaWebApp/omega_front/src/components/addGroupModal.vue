<!--Nom, image, lieu, startime-->
<template>
  <transition name="groupModal">
    <div class="groupModal-mask">
      <div class="groupModal-wrapper">
        <div class="groupModal-container">
          <span id="closeModal" @click="showGroupModal(false)">X</span>
          <div class="addGroupText">
            <div class="addGroupModal">
              <span id="groupTitle">CRÉER UN GROUPE OMEGA</span><br>
              <br>
              <div class="newGroupInfos">
                <span id="smallText">Nom : <input type="text" v-model="groupName"><br></span>
                <span id="smallText">Image : <input v-bind:ref="avatar" type="file" name="avatar" id="inputFile" @change="upload"></span>
              </div>
              <!--<div class="newGroup">
                <div class="newGroupCover">
                  <img :src="groupCover">
                </div>
                <div class="newGroupInfo">
                  <span id="newGroupName">{{groupName}}</span>
                </div>
              </div>-->
              <!--<button @click="createGroup()">Créer</button>-->
            </div>
          </div>
          <div class="groupModalClose" @click="showGroupModal(false), createGroup()">
            CRÉER <img src="../assets/arrow.png">
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import FacebookApiService from '../services/FacebookApiService'
import $ from 'jquery'

export default {
  data () {
    return {
      formData: new FormData,
      avatar: '',
      groupName: 'Nom',
      groupCover: 'Image',
      groupCriterias: [
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
      groupToCreate: {
        'name': null,
      },
      group: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
      
    }
  },
  methods: {
    ...mapActions(['showGroupModal','sendIdToAddMember', 'sendGroups','setCurrentGroup', 'requestAsync', 'insertGroup']),
    upload: function(e) {
      var fichierSelectionne = document.getElementById('inputFile').files[0];
      console.log(fichierSelectionne);
      this.formData.append('files', fichierSelectionne);
    },
    loadGroups: async function() {
      var data = await this.requestAsync(() => GroupService.getGroups());
      this.sendGroups(data);
    },
    createGroup: async function(group) {
      this.groupToCreate.name = this.groupName;
      this.insertGroup(this.formData);
      var result = await FacebookApiService.createGroup(this.groupToCreate.name);
      var result2 = await FacebookApiService.uploadGroupCover(this.formData, result.EventGroupGuid, result.EventGroupName);
      var data = await this.requestAsync(() => FacebookApiService.getFacebookGroups());
      this.sendGroups(data);
    },
    createLocalGroup: async function(item) {
      this.groupToCreate.cover = this.groupCover;
      this.groupToCreate.name = this.groupName;
      if(this.groupCriterias[0].value == null) {
        this.metadonnees.Accousticness = this.groupCriterias[0].value;
      } else {
        this.metadonnees.Accousticness = this.groupCriterias[0].value/100;
      }

      if(this.groupCriterias[1].value == null) {
        this.metadonnees.Danceability = this.groupCriterias[1].value;
      } else {
        this.metadonnees.Danceability = this.groupCriterias[1].value/100;
      }

      if(this.groupCriterias[2].value == null) {
        this.metadonnees.Energy = this.groupCriterias[2].value;
      } else {
        this.metadonnees.Energy = this.groupCriterias[2].value/100;
      }

      if(this.groupCriterias[3].value == null) {
        this.metadonnees.Instrumentalness = this.groupCriterias[3].value;
      } else {
        this.metadonnees.Instrumentalness = this.groupCriterias[3].value/100;
      }

      if(this.groupCriterias[4].value == null) {
        this.metadonnees.Speechiness = this.groupCriterias[4].value;
      } else {
        this.metadonnees.Speechiness = this.groupCriterias[4].value/100;
      }

      if(this.groupCriterias[5].value == null) {
        this.metadonnees.Liveness = this.groupCriterias[5].value
      } else {
        this.metadonnees.Liveness = this.groupCriterias[5].value/100;
      }

      if(this.groupCriterias[6].value == null) {
        this.metadonnees.Popularity = this.groupCriterias[6].value;
      } else {
        this.metadonnees.Popularity = this.groupCriterias[6].value/100;
      }
      
      this.groupToCreate.metadonnees = this.metadonnees;
      this.insertGroup(this.groupToCreate);
      var result = GroupService.createGroup(this.groupToCreate);
    }
  },
  computed: {
    ...mapGetters(['groups', 'currentGroup','idToAddMember'])
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

.groupModal-mask {
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

.groupModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.groupModal-container {
  height: 150px;
  width: 400px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addGroupModal {
}

.allCriterias {
}

#criteriaValue {
  position: absolute;
  margin-left: 10px;
}

.addGroupText {
  height: 100%;
  padding: 20px;
  width: 100%;
}

.newGroup {
  width: 350px;
  height: 200px;
  background: red;
}

.newGroupCover {
  float: left; 
  width: 100px;
  height: 100%;
}

.newGroupCover img {
  height: 100%;
}

.newGroupInfo {
  width: 250px;
  height: 100%;
  float: left;
  text-align: left;
}

.groupModalClose {
  width: 120px;
  padding: 10px;
  background: #de002b;
  color: black;
  font-family: 'Montserrat-Regular';
  cursor: pointer;
  text-transform: uppercase;
}

.groupModalClose img {
  margin-left: 6px;
  width: 25px;
}

#groupTitle {
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

.groupModal-enter {
  opacity: 0;
}
.groupModal-leave-active {
  opacity: 0;
}
.groupModal-enter .groupModal-container,
.groupModal-leave-active .groupModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>