/*
  定义一个require模块，模块名称为core.Service
  该模块依赖jquery、ztree.core、ztree.excheck
  样式文件Skin/DefaultSkin/css/metroStyle.css

  （require资料参考地址：http://requirejs.cn/；
    angular资料参考地址：http://192.168.0.246:88/Angular/docs）

  日期：2015-08-20
*/
define('biTree', ['page', 'jquery', 'ztree.core', 'ztree.excheck', 'css!Skin/DefaultSkin/css/metroStyle.css'],
    function (app, $) {
        'use strict';

        var defConf = { view: { showIcon: false } };

        var ngTreeDirective = function () {
            return {
                scope: {
                    conf: '='
                },
                link: function (scope, $element, $attr) {
                    var conf = scope.conf || {}, seting;

                    if (seting = conf.seting) { for (var i in defConf) if (seting[i] === undefined) seting[i] = defConf[i]; }
                    else seting = defConf;

                    $element.empty();

                    function Init() {
                        conf.tree = $.fn.zTree.init($('<ul class="ztree"></ul>').appendTo($element), seting, conf.data);
                    }
                    scope.$watch(function () {
                        return conf.data && conf.data.length ? conf.data.length : 0;
                    }, Init);
                }
            };
        };

        /*
          
        */
        app.directive('biTree', ngTreeDirective);

        return app;
    });