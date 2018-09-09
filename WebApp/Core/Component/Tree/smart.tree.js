
define('smart.tree', ['page-Route', 'Assets/Js/Plugins/tree.js', 'ext'],
    function (app) {
        'use strict';

        //声明并创建指令逻辑体
        var smartTreeDirectiveLink = function (scope, $element, attr, ctrl) {

            function fnCreate() {
                ctrl.fnCreate();

                $element.empty();

                if (!ctrl.conf || !ctrl.conf.data || !ctrl.conf.data.length) return;

                ctrl.conf.$tree = $element.treeview(ctrl.conf);
            };

            scope.$watchCollection('data', fnCreate);
        };

        //声明并创建指令控制器
        var smartTreeController = function ($scope) {
            var self = this; self.conf = $scope.conf || {};

            self.fnCreate = function () {
                self.data = $scope.data;

                self.conf.data = self.tree = Array.isArray(self.data) && !self.conf.isTree ? self.data.getTree() : self.data;
            }
        };

        //声明并创建指令主体
        var smartTreeDirective = function () {
            return {
                restrict: 'EA',
                priority: 100,
                controller: smartTreeController,
                scope: {
                    conf: '=',
                    data: '='
                },
                link: smartTreeDirectiveLink
            };
        };

        //注册biSmartTree指令
        app.directive('biSmartTree', smartTreeDirective);

        return app;
    });