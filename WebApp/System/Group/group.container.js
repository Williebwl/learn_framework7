define(['core.container', 'System/Group/group.service.js'],
function (core) {
    'use strict'

    core.controller('GroupContainerCtrl', function ($scope, groupService) {
        core($scope, groupService),
        Create($scope, groupService),
        $scope.$on('$$RefreshContainer', function (e, arg) {
            $scope.Info = arg
        })
    })

    function Create($parentScope, groupService) {
        core.controller('GroupUserContainerCtrl', function ($scope) {

        }),
        core.controller('GroupModuleContainerCtrl', function ($scope) {

        }),
        core.controller('GroupOperatingContainerCtrl', function ($scope) {

        })
    }

})