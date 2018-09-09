"use strict";
import React from 'react';
import { Router, Route, IndexRoute, hashHistory } from 'react-router'
import { Provider } from 'react-redux'
import { applyMiddleware, createStore, combineReducers, compose } from 'redux'
import promise from 'redux-promise';
import { routers } from '../routers/index.js';
import reducers from '../reducers/index.js';

const finalCreateStore = compose(
    applyMiddleware(promise),
    window.devToolsExtension ? window.devToolsExtension() : f => f
)(createStore);
const store = finalCreateStore(reducers);

export default class Container extends React.Component {
    render() {
        return (
            <Provider store={store}>
                <Router history={hashHistory} routes={routers}>
                </Router>
            </Provider>
        );
    }
}