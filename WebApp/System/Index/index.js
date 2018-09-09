
define(['page', 'preloader', 'lobibox', 'System/Index/index.service.js'],
    function (app, preloader, lobibox) {
        'use strict';

        function indexCtrl($scope, indexService, $timeout) {
            $scope.Menus = [];
            var pop = preloader.show();
            indexService.fnGetCurrentUserMenus().success(function (d) {
                $scope.Menus = d,
                pop.hide()
            });

            $scope.$on('$routeChangeSuccess', function (e, arg) {
                $scope.PageRoute = arg.$$route.Route
            });
        }

        app.controller('indexCtrl', indexCtrl)






        var mode = ["info", "warning", "error", "success"];

        app.controller('NoticeCtrl',
            function ($scope, $rootScope, $timeout) {
                $scope.Notices = [];

                function $editNotice(e, arg) {
                    arg.class = mode[arg.mode] || 'info',

                    $scope.Notices.push(arg),

                    $timeout(function () { $scope.Notices.remove(arg); }, 2000),

                    angular.isFunction(arg.callback) && arg.callback()
                }

                $rootScope.$on('$editNotice', $editNotice);
            });

        //app.run(function ($rootScope, $animate) {

        //    function $message(e, d) {
        //        lobibox.notify(mode[d.mode] || 'info', d);
        //    }

        //    $rootScope.$on('$$msg', $message);
        //});

        return app;
    });