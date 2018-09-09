define(['core.nav', 'System/Menu/menu.service.js'],
function (page) {
    'use strict';

    return page.controller('DefaultNavContainerCtrl',
        function ($scope, moduleService) {

            moduleService.fnGetChildren($scope.$parent.$parent.module && $scope.$parent.$parent.module.ID || 0,
                function (d) {
                    $scope.Modules = d || []
                }, function () {
                    $scope.Modules = []
                })

        })
})