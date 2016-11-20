import * as types from './mutation-types'

export const increment = ({commit}) => {
    commit(types.INCREMENT)
}

export const decrement = ({commit}) => {
    commit(types.DECREMENT)
}

export const update = ({commit}) => {
    commit(types.UPDATE)
}

export const add = ({commit}, payload) => {
    commit(types.ADD, payload)
}

export const addText = ({commit}, payload) => {
    commit(types.ADDTEXT, payload)
}

export const makeActive = ({commit}, payload) => {
  
    commit(types.MAKEACTIVE, payload)
}

export const showModal = ({commit}, payload) => {
  
    commit(types.SHOWMODAL, payload)
}

export const sendMoods = ({commit}, payload) => {
  
    commit(types.SENDMOODS, payload)
}

export const sendCriterias = ({commit}, payload) => {
  
    commit(types.SENDCRITERIAS, payload)
}

export const enableCriterias = ({commit}, payload) => {
  
    commit(types.ENABLECRITERIAS, payload)
}

export const addMood = ({commit}, payload) => {
  
    commit(types.ADDMOOD, payload)
}