define(['core.container', 'System/Account/account.service.js'],
function (core) {
    'use strict'

    core.controller('AccountContainerCtrl',
        function ($scope, authAccountService) {
            core($scope, authAccountService);

            $scope.$on('$$RefreshSearch', function (s, e) {
                authAccountService.fnGet('')
                                  .success(function (d) { $scope.PageInfo = d })
                                  .error(function () { $scope.PageInfo = {} });

            })
        })
})