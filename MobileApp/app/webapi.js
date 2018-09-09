"use strict";
import 'babel-polyfill';
import { createAction, handleActions } from 'redux-actions';
import { NetworkError } from './error.js'

const gateway = "http://localhost:2206/";
class WebApi {
    constructor(name) {
        this._name = name;
        this._actions = {};

        this.createMethod('get', async (id) => await this.call('GET', id));
        this.createMethod('post', async (json) => await this.call('POST', null, null, json));
        this.createMethod('put', async (id, json) => await this.call('PUT', id, null, json));
        this.createMethod('delete', async (id) => await this.call('DELETE', id).then(response => id));
    }
    get name() {
        return this._name;
    }
    get actions() {
        return this._actions;
    }

    createMethod(name, expression) {
        //see also: https://github.com/acdlite/redux-actions/pull/32
        let type = name + ' ' + this.name;
        let action = createAction(type, expression)
        action.toString = () => type;
        return this._actions[name] = action;
    }
    call(method, path = null, query = null, body = null) {
        let url = '';
        for(let key in query) {
            if(query[key])
                url += (url ? '&' : '?') + encodeURIComponent(key) + "=" + encodeURIComponent(query[key]);
        }
        url = `${gateway}${this.name}${path ? '/' + path : ''}${url}`;

        let init = {
            method: method,
            mode: 'cors',
            headers: {}
        };
        if(sessionStorage.Token)
            init.headers.Authorization = sessionStorage.Token;
        if(body) {
            init.body = JSON.stringify(body);
            init.headers['Content-Type'] = 'application/json';
        }
        return fetch(url, init).then(response => {
            if(response.ok)
                return response.json();
            return response.json().then(json => {
                throw new NetworkError(url, json.ExceptionMessage || json.Message);
            }, error => {
                throw new NetworkError(url, `HTTP ${response.status} ${response.statusText}`);
            });
        }, error => {
            throw new NetworkError(url, error.message);
        });
    }
    get(path = null, query = null) {
        return this.call('GET', path, query);
    }
    post(path = null, body = null) {
        return this.call('POST', path, null, body);
    }
}
export function createApi(name) {
    return new WebApi(name);
}