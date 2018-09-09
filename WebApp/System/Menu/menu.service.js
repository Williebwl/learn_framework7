
define(['core.Service'],
    function (core) {
        'use strict'

        function menuService() {
            this.fnGetRoot = function (success, error) {
                return this.get('GetRoot', success, error)
            },
            this.fnGetChildren = function (id, success, error) {
                return this.get('GetChildren/' + (id || 0), success, error)
            },
            this.fnGetChildrenAndSelf = function (id, success, error) {
                return this.get('GetChildrenAndSelf/' + id, success, error)
            },
            this.fnGetChildrenTree = function (id, success, error) {
                return this.get('GetChildrenTree/' + (id || 0), success, error)
            },
            this.fnGetChildrenAndSelfTree = function (id, success, error) {
                return this.get('GetChildrenAndSelfTree/' + (id || 0), success, error)
            },
            this.fnGetInfoByAppId = function (id) {
                return this.get('GetInfoByAppId/' + (id || 0))
            }
        }

        core.service('menu', menuService, 'tenant.Menu')
    });