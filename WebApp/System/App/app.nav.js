define(['core.nav', 'System/App/app.service.js'],
function (core) {
    'use strict'

    core.controller('AppNavCtrl', function ($scope, $rootScope, appService) {
        var page = core($scope, appService);
        appService.fnGetAll().success(function (d) { $scope.apps = d, $scope.fnActive(d[0]) }),
        $scope.fnActive = function (app) {
            if (this.View == app) return;

            $rootScope.$broadcast('$$RefreshContainer', this.View = app)
        }.bind($scope),
        $scope.fnAdd = function () {
            $scope.ShowDialog()
        },
        $scope.fnEdit = function (app) {
            $scope.ShowDialog('modal', app)
        },
        $scope.fnDel = function (e, app) {
            e.stopPropagation();

            page.confirm('确定要禁用该应用？', function (e) {
                if (!e.s) return;

                var s = app.IsValid ? 0 : 1;
                appService.fnSetStatus(app.ID, s, function (d) { d ? app.IsValid = s : error; }, error)
            })
        },
        $scope.$on('$DataPostSuccess', function (s, e) {
            $scope.apps.push({
                ID: e.View.ID,
                AppName: e.View.App.AppName,
                AppCode: e.View.App.AppCode,
                AppType: e.PostInfo.App.AppType,
                AppTypeID: e.PostInfo.App.AppTypeID,
                IsValid: 1
            })
        }),
        $scope.$on('$DataPutSuccess', function (s, e) {
            core.extend(e.Source, e.View.App)
        })

        function error() { page.errorNotice('应用禁用失败！'); }
    })
})