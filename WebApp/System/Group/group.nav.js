define(['core.nav', 'System/Group/group.service.js'],
function (core) {
    'use strict'

    core.controller('GroupNavCtrl', function ($scope, $rootScope, groupService) {
        core($scope, groupService),
        groupService.fnGetAll().success(function (d) { $scope.Groups = d, $scope.fnActive(d[0]) }),
        $scope.fnActive = function (group) {
            if (this.Active == group) return;

            $rootScope.$broadcast('$$RefreshContainer', this.Active = group)
        }.bind($scope)

        $scope.fnAdd = function () {
            $scope.ShowDialog()
        }
    })
})