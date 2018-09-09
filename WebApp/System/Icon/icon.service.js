define(['core.Service'],
    function (core) {
        'use strict'

        function iconService() {
            this.fnGetAll = function () {
                return [{ name: '' }]
            }
        }

        core.service('icon', iconService)
    })