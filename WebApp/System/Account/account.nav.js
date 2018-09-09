define(['core.nav', 'System/Account/account.service.js'],
function (core) {
    'use strict'

    core.controller('AccountNavCtrl', function ($scope, authAccountService) {
        var page = core($scope, authAccountService);
        authAccountService.fnGetAllStatus()
                          .success(function (d) { $scope.AllStatus = d })
                          .error(function () { $scope.AllStatus = [] })
    })
})