/*
  定义一个require模块,模块名称为 page-Route
  该模块依赖page、angularAMD、angular-route、paging、bigTree。

  （require资料参考地址：http://requirejs.cn/；
    angular资料参考地址：http://192.168.0.246:88/Angular/docs）

  日期：2015-08-20
*/
define('page-Route', ['page', 'angularAMD', 'ext', 'core.http', 'System/Index/common.js', 'paging', 'angular-route', 'bi.ext', 'System/Index/index.js'],
    function (app, ngRoutes, ext, core, com) {
        /// <summary>为程序添加路由，并将其加载到页面。</summary>
        /// <param name="app" type="angular.module">page模块返回对象</param>
        /// <param name="ngRoutes" type="angularAMD">angularAMD模块返回对象</param>
        'use strict';

        var routeApp;//angular.module require异步对象

        //为程序添加依赖模块（ngRoute）。
        app.requires.push('ngRoute');

        //为程序添加路由（路由规则定义详见angular官方文档）
        app.config(['$routeProvider', function ($routeProvider) {
            ext.ajax.Init({
                url: core.Api + 'Tenant/Menu/GetRoute',
                success: function (d) {
                    if (d.RedirectTo)
                        $routeProvider.otherwise({
                            redirectTo: '/' + d.RedirectTo //如果当前路由无法识别，将其重定向到指定路由（当前指定到（/module路由））
                        });

                    if (Array.isArray(d.Routes))
                        d.Routes.forEach(function (route) {
                            route.navTemplateUrl && (route.navTemplateUrl = require.toUrl(route.navTemplateUrl)),
                            route.toolBarTemplateUrl && (route.toolBarTemplateUrl = require.toUrl(route.toolBarTemplateUrl)),
                            route.containerTemplateUrl && (route.containerTemplateUrl = require.toUrl(route.containerTemplateUrl)),
                            route.resolve = { rest: function ($injector) { routeApp.$injector || (routeApp.$injector = $injector); } },
                            $routeProvider.when('/' + route.Route, ngRoutes.route(route))
                        });

                    var hash = location.hash, i = hash.indexOf('?'), a = (a = hash.indexOf('r=', i) - 1) > 0 ? i === a ? a + 1 : a : hash.length;

                    location.hash = (hash ? hash.substr(0, a) : d.RedirectTo) + (i < 0 ? '?' : '&') + 'r=' + Math.random();
                }, error: function () { console.log('模块信息加载失败！') }
            }).send();
        }]);

        app.run(function ($rootScope) {
            $rootScope.$on('$ToolBarContentLoaded', com.ToolBarLoaded),
            $rootScope.$on('$NavContentLoaded', com.NavLoaded),
            $rootScope.$on('$ContainerContentLoaded', com.ContainerLoaded),
            $rootScope.$on('$ContentLoaded', com.ContentLoaded),
            $rootScope.$on('$routeChange', RouteChange),
            $rootScope.$on('$routeChangeSuccess', function (e, arg) {
                !document.oldTitle && (document.oldTitle = document.title),
                document.title = document.oldTitle + (arg && arg.$$route && arg.$$route.Title ? ' - ' + arg.$$route.Title : '')
            });
        });

        //注册初始化程序，并返回angular.module require异步对象
        return routeApp = ngRoutes.bootstrap(app);

        function RouteChange(s, locals, $templateRequest, $sce, nextRoute, lastRoute) {
            var template, templateUrl;

            if (angular.isDefined(template = nextRoute.navTemplate)) {
                if (angular.isFunction(template)) {
                    template = template(nextRoute.params);
                }
            } else if (angular.isDefined(templateUrl = nextRoute.navTemplateUrl)) {
                if (angular.isFunction(templateUrl)) {
                    templateUrl = templateUrl(nextRoute.params);
                }
                if (angular.isDefined(templateUrl)) {
                    nextRoute.loadedNavTemplateUrl = $sce.valueOf(templateUrl);
                    template = $templateRequest(templateUrl);
                }
            }
            if (angular.isDefined(template)) {
                locals['$navTemplate'] = template;
            }

            if (angular.isDefined(template = nextRoute.toolBarTemplate)) {
                if (angular.isFunction(template)) {
                    template = template(nextRoute.params);
                }
            } else if (angular.isDefined(templateUrl = nextRoute.toolBarTemplateUrl)) {
                if (angular.isFunction(templateUrl)) {
                    templateUrl = templateUrl(nextRoute.params);
                }
                if (angular.isDefined(templateUrl)) {
                    nextRoute.loadedToolBarTemplateUrl = $sce.valueOf(templateUrl);
                    template = $templateRequest(templateUrl);
                }
            }
            if (angular.isDefined(template)) {
                locals['$toolBarTemplate'] = template;
            }

            if (angular.isDefined(template = nextRoute.containerTemplate)) {
                if (angular.isFunction(template)) {
                    template = template(nextRoute.params);
                }
            } else if (angular.isDefined(templateUrl = nextRoute.containerTemplateUrl)) {
                if (angular.isFunction(templateUrl)) {
                    templateUrl = templateUrl(nextRoute.params);
                }
                if (angular.isDefined(templateUrl)) {
                    nextRoute.loadedContainerTemplateUrl = $sce.valueOf(templateUrl);
                    template = $templateRequest(templateUrl);
                }
            }
            if (angular.isDefined(template)) {
                locals['$containerTemplate'] = template;
            }
        }
    })