
define(['page', 'core.http'],
    function (app, $$http) {
        'use strict'

        app.service('LoginService', function ($http) {
            this.fnLogin = function (d) {
                return $$http($http).post('Auth/Login/Login', d)
            }
        });
    });