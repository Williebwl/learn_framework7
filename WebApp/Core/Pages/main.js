/*
  设置登录页面require配置信息（相关require配置详细说明请查看相关文档）
  （require参考地址：http://requirejs.cn/；
    angular参考地址：http://192.168.0.246:88/Angular/docs）

    日期：2015-08-20
*/
(function (window, require) {
    'use strict'

    //设置require配置信息
    require.config({
        //所有模块的查找根路径。
        baseUrl: 'Buisness',
        //path映射那些不直接放置于baseUrl下的模块名。
        paths: {
            'angular': 'Assets/Js/Ng/angular.min.js',//模块名称：模块js相对baseUrl的路径
            'angular-route': 'Assets/Js/Ng/angular-route.min.js',
            'angularAMD': 'Assets/Js/Ng/angularAMD.js',
            'jquery': 'Assets/Js/Lib/jquery.min.js',
            'jquery-ui': 'Assets/Js/Lib/jquery-ui.min.js',
            //'jquery_migrate': 'Assets/Js/Lib/jquery.migrate.js',
            'bootstrap': 'Assets/Js/Lib/bootstrap.min.js',
            'scrollbar': 'Assets/Js/Plugins/scrollbar.js',
            'paging': 'Core/Component/Paging/pagination.js',
            'preloader': 'Core/Component/Preloader/preloader.js',
            'ext': 'Core/Utils/ext.js',
            //'biTree': 'Core/Component/Tree/ng.tree.js',
            //'ztree.core': 'Assets/Js/Plugins/ztree.core.js',
            //'ztree.excheck': 'Assets/Js/Plugins/ztree.excheck.js',
            //'module.tree': 'System/Module/module.tree.js',
            //'smart.tree': 'Core/Component/Tree/smart.tree.js',
            'core.tree': 'Core/Service/core.tree.js',
            'core.dialog': 'Core/Service/core.dialog.js',
            //'jqValidate': 'Assets/Js/Lib/jquery.validate.min.js',
            'formValidate': 'Core/Component/Validate/ng.formValidate.js',
            'bi.ext': 'Core/Component/ExtDirective/ng.extension.js',
            'css': 'Assets/Js/Lib/require.css.js',
            'lobibox': 'Assets/Js/Plugins/lobibox.js',
            'uploader': 'Assets/Js/Plugins/webuploader.min.js',
            'page': 'Core/Container/page.js',
            'page-Route': 'Core/Container/page.route.js',
            'core': 'Core/Service/core.js',
            'core.http': 'Core/Service/core.http.js',
            'core.Service': 'Core/Service/core.service.js',
            'core.api': 'Core/Service/core.api.js',
            'core.page': 'Core/Service/core.page.js',
            'core.container': 'Core/Service/core.page.js',
            'core.nav': 'Core/Service/core.page.js',
            'core.state': 'Core/Service/core.state.js'
        },
        //指定要加载的一个依赖数组。
        deps: ['css', 'jquery', 'angular', 'angular-route', 'angularAMD', 'bootstrap', 'scrollbar'],
        //RequireJS获取资源时附加在URL后面的额外的query参数。
        urlArgs: 'v=20160226a',
        //为没有使用define()来声明依赖关系、设置模块的"浏览器全局变量注入"型脚本做依赖和导出配置。
        shim: {
            'jquery': { exports: 'jQuery' },//设置jQuery以让require能够检测jquery模块是否正常加载
            'angular': { deps: ['jquery'], exports: 'angular' },//设置angular模块依赖jquery模块并设置检测关键字
            "angular-route": ['angular'],//设置angular-route模块依赖angular模块
            'angularAMD': ['angular', 'angular-route'],
            'scrollbar': ['jquery'],
            'bootstrap': { deps: ['jquery'], exports: 'bootstrap' },
            'lobibox': ['jquery'],
            'uploader': { deps: ['jquery'], exports: 'WebUploader' },
            'jquery-ui': ['jquery']
        }
    })

    /*
     加载page-Route、System/Index/common.js模块
    */
    require(['page-Route'])
}(window, window.require));




