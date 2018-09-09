define(['core.container', 'System/App/app.service.js', 'System/Menu/menu.service.js'],
function (core) {
    'use strict'

    core.controller('AppContainerCtrl', function ($scope, appService) {
        Create($scope, appService),
        core($scope, appService),
        $scope.$on('$$RefreshContainer', function (e, arg) {
            $scope.fnRefreshParams(arg)
        })

    })

    function Create($parentScope, appService) {
        core.controller('AppContainerParamsCtrl', function ($scope, menuService) {
            var appInfo;

            core($scope, menuService),
            $parentScope.fnRefreshParams = function (info) {
                if (!info) return;

                appInfo = info,
                appService.fnGet(info.ID)
                          .success(function (d) { $scope.AppInfo = $parentScope.Info = d })
                          .error(function () { $scope.AppInfo = $parentScope.Info = {} }),
                menuService.fnGetInfoByAppId(info.ID)
                           .success(function (d) {
                               $scope.MeunInfo = d[0] || {},
                               $scope.View.Items = $scope.MeunInfos = d,
                               $parentScope.icon = $scope.MeunInfo.Icon,
                               $parentScope.background = $scope.MeunInfo.IconBackGround
                           }).error(function () {
                               $scope.MeunInfo = {}, $scope.View.Items = $scope.MeunInfos = []
                           })
            },
            $scope.fnEdit = function () {
                if (!appInfo) return;

                $scope.ShowDialog(appInfo)
            },
            $scope.$on('$DataPutSuccess', function (s, e) {
                core.extend(e.Source, e.View.App)
            })
        }),
        core.controller('AppContainerRoleCtrl', function ($scope) {
            core($scope, appService),
            $parentScope.fnRefreshRole = function () {
                var info = this.Info;
            }
        }),
        core.controller('AppContainerOperateCtrl', function ($scope) {
            core($scope, appService),
            $parentScope.fnRefreshOperate = function () {
                var info = this.Info;
            }
        }),
        core.controller('AppContainerDeployCtrl', function ($scope) {
            core($scope, appService),
            $parentScope.fnRefreshDeploy = function () {
                var info = this.Info;
            }
        })
    }
})