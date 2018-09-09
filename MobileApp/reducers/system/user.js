"use strict";
import { createAction, handleActions } from 'redux-actions';
import { createApi, NetworkError } from '../../app/index.js'

const api = createApi('portal/article');

export const userAction = api.actions;
export const userReducer = handleActions({
    [userAction.get]: {
        next: (state, action) => action.payload.Items,
        throw: (state, action) => { alert(action.payload.message); return state; }
    },
    [userAction.post]: (state, action) => [action.payload].concat(state),
    [userAction.put]: (state, action) => state.map(item => item.ID == action.payload.ID ? action.payload : item),
    [userAction.delete]: (state, action) => state.filter(item => item.ID != action.payload)
}, []);