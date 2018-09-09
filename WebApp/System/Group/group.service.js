
define(['core.Service'],
    function (core) {
        'use strict'

        function groupService() {
            this.fnGetNav = function (key) {
                return this.get('Get', { params: { Key: key } })
            },
            this.fnGetAppGroups = function (appid, success, error) {
                return this.get('GetAppGroups/' + (appid || 0), success, error)
            }
        }

        core.service('group', groupService, 'Institution.Group')
    });