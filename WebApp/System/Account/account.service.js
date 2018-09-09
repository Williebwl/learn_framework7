define(['core.Service'],
    function (core) {
        'use strict'

        function authAccountService() {
            this.fnGetAllStatus = function () {
                return this.get('GetAllStatus')
            }
        }

        core.service('auth/Account', authAccountService)
    })