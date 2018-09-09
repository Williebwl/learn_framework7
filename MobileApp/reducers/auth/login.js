"use strict";
import { createAction, handleActions } from 'redux-actions';
import { browserHistory } from 'react-router';
import { createApi, NetworkError } from '../../app/index.js'

const api = createApi('auth/login');
api.createMethod('login', async (loginName, password) => await api.post('Login', { LoginName: loginName, Password: password }));

export const loginAction = api.actions;
export const loginReducer = handleActions({
    [loginAction.login]: {
        next: (state, action) => { 
            sessionStorage.Token = action.payload;
            browserHistory.goBack(); 
            return true; 
        },
        throw: (state, action) => { alert(action.payload.message); return state; }
    }
}, false);