"use strict";
import { combineReducers } from 'redux'
import * as auth from './auth/index.js'
import * as system from './system/index.js'

export default combineReducers([
    auth, system
].reduce(Object.assign, {}))