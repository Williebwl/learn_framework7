
define(['page', 'core.http'],
    function (app, $$http) {
        'use strict'

        app.service('indexService', function ($http) {
            $$http = this.$$http = $$http($http);

            this.fnGetCurrentUserMenus = function () {
                return $$http.get('Tenant/Menu/GetRoot')
            }
        });
    });