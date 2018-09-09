/*
  定义一个require模块,模块名称为 paging
  该模块依赖page、angular。

  （require资料参考地址：http://requirejs.cn/；
    angular资料参考地址：http://192.168.0.246:88/Angular/docs）

  日期：2015-08-20
*/
define('paging', ['page', 'ext'],
    function (app, ext) {
        'use strict';

        /// <field type="json">分页默认配置信息</field>
        var defConf = { pageSelect: 1, pageTurning: 1, currentPage: 1, totalItems: 0, itemsPerPage: 15, inputValue: 1, umpPageNum: 1, sFirst: "首页", sLast: "尾页", sNext: "下一页", sPrevious: "上一页" };

        var biPaginationTemplate = '<div class="table-paging clearfix">\
                                        <div class="page-select pull-left" ng-if="conf.pageSelect">\
                                            <span>\
                                                每页显示\
                                                <select class="number-select" ng-model="conf.itemsPerPage" \
                                                        ng-options="option for option in conf.perPageOptions " ng-change="PerPageChange($event)"></select> 条\
                                            </span>\
                                            <span>共<span class="total" ng-bind="conf.totalItems"></span>条</span>\
                                            <span>当前第<input type="text" class="number-input" ng-model="conf.jumpPageNum" ng-change="jumpToPage($event)"> 页</span>\
                                        </div>\
                                        <ul class="pagination pagination-sm pull-right" ng-if="conf.pageTurning">\
                                            <li ng-class="{disabled: conf.currentPage <= 1}" ng-click="changeCurrentPage(1)"><a href="javascript:;" ng-bind="conf.sFirst">首页</a></li>\
                                            <li ng-class="{disabled: conf.currentPage <= 1}" ng-click="prevPage()"><a href="javascript:;" ng-bind="conf.sPrevious">上一页</a></li>\
                                            <li ng-repeat="item in pageList track by $index" ng-class="{active: item == conf.currentPage, disabled:conf.currentPage == conf.numberOfPages&&conf.currentPage==1,separate: item == \'...\'}"\
                                                ng-click="changeCurrentPage(item)">\
                                                <a href="javascript:;" ng-bind="item"></a>\
                                            </li>\
                                            <li ng-class="{disabled: conf.currentPage == conf.numberOfPages}" ng-click="nextPage()"><a href="javascript:;" ng-bind="conf.sNext">下一页</a></li>\
                                            <li ng-class="{disabled: conf.currentPage == conf.numberOfPages}" ng-click="changeCurrentPage(conf.numberOfPages)"><a href="javascript:;" ng-bind="conf.sLast">尾页</a></li>\
                                        </ul>\
                                    </div>';

        //控制器
        var biPaginationCtrl = function ($scope) {
            var conf;

            if (conf = $scope.conf) ext.extend(conf, defConf);
            else conf = $scope.conf = defConf;

            conf.Ctrl = this, conf.perPageOptions = [];

            for (var i = conf.itemsPerPage = conf.itemsPerPage || 15, s = i, l = i * 6; i <= l; i += s) conf.perPageOptions.push(i);

            // 定义分页的长度必须为奇数 (default:9)
            conf.pagesLength = parseInt(conf.pagesLength, 10) || 9;

            if (conf.pagesLength % 2 === 0) {
                // 如果不是奇数的时候处理一下
                conf.pagesLength = conf.pagesLength - 1;
            }

            // pageList数组
            this.getPagination = function getPagination() {
                // conf.currentPage
                conf.currentPage = parseInt(conf.currentPage, 10) || 1;

                // conf.totalItems
                conf.totalItems = parseInt(conf.totalItems, 10) || 0;

                // numberOfPages
                conf.numberOfPages = Math.ceil(conf.totalItems / conf.itemsPerPage) || 1;

                // jumpPageNum
                conf.jumpPageNum = conf.currentPage = conf.currentPage < 1 ? 1 : conf.currentPage > conf.numberOfPages ? conf.numberOfPages : conf.currentPage;

                if ((conf.currentPage == 1 || this.change) && conf.numberOfPages <= conf.pagesLength) {
                    $scope.pageList = [];
                    for (i = 1, l = conf.numberOfPages ; i <= l; i++) $scope.pageList.push(i);
                }
                else if (conf.numberOfPages > conf.pagesLength) {
                    var c = conf.pagesLength, offset = (c - 1) / 2, a = conf.currentPage, b = conf.numberOfPages, isLast = a == b;
                    $scope.pageList = [];

                    if (a > offset + 1) $scope.pageList.push('...');

                    for (var i = (i = (i = a - offset) < 1 ? 1 : i), l = (l = i + c - 1) > b && (i = (i = b - c + 1) < 1 ? 1 : i) ? b : l; i <= l; i++) $scope.pageList.push(i);

                    if (l < b) $scope.pageList.push('...');
                }

                this.change = false;
            }

            this.pagingChanged = function pagingChanged(pageInfo) {
                if (pageInfo) {
                    pageInfo.ck = false;
                    conf.totalItems = pageInfo.TotalItems;
                    conf.Items = pageInfo.Items;
                }
                $scope.$parent.$broadcast(($scope.conf.id || '') + 'PagingChanged', conf);
            }

            // prevPage
            this.prevPage = $scope.prevPage = function () {
                if (conf.currentPage > 1) {
                    conf.currentPage -= 1;
                }
            }

            this.nextPage = $scope.nextPage = function () {
                if (conf.currentPage < conf.numberOfPages) {
                    conf.currentPage += 1;
                }
            }

            // 变更当前页
            this.changeCurrentPage = $scope.changeCurrentPage = function (item) {
                conf.currentPage = parseInt(item) || conf.currentPage;
            }

            // 跳转页
            this.jumpToPage = $scope.jumpToPage = function () {
                conf.currentPage = conf.jumpPageNum = parseInt(conf.jumpPageNum) || conf.currentPage;
            }
        }

        //Link函数
        var biPaginationLink = function (scope, element, attrs, ctrl) {
            var conf = scope.conf;

            scope.PerPageChange = function () {
                ctrl.change = true;
            }

            scope.$watch(function () {
                return Math.ceil(conf.totalItems / conf.itemsPerPage) || 1;
            }, function () { ctrl.change = true; })

            scope.$watch(function () {
                return conf.currentPage + ' ' + conf.totalItems + ' ' + conf.itemsPerPage;
            }, $.proxy(ctrl.getPagination, ctrl));

            scope.$parent.$watch('PageInfo', ctrl.pagingChanged);
            scope.$parent.$watch('infos.PageInfo', ctrl.pagingChanged);

            scope.$watch(function () {
                return conf.currentPage + ' ' + conf.itemsPerPage;
            }, function () {
                scope.$parent.$broadcast((conf.id || '') + 'PagingChange', conf);

                if (typeof conf.onChange === 'function') conf.onChange();
            });
        }

        var biPaginationDirective = function () {
            /// <summary>创建分页指令</summary>
            /// <returns type="json">分页指令配置信息（分页显示以及分页处理逻辑）</returns>

            return {
                restrict: 'EA',
                template: biPaginationTemplate,
                replace: true,
                scope: {
                    conf: '='
                },
                controller: biPaginationCtrl,
                link: biPaginationLink
            };
        };

        /*
          创建指令并将其命名为 biPagination
        */
        app.directive('biPagination', biPaginationDirective);
    });