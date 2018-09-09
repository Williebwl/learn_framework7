/*
  设置登录页面require配置信息（相关require配置详细说明请查看相关文档）
  （require参考地址：http://requirejs.cn/；
    angular参考地址：http://192.168.0.246:88/Angular/docs）

    日期：2015-08-20
*/
(function (window, require) {
    'use strict';

    //设置require配置信息
    require.config({
        //所有模块的查找根路径。
        baseUrl: 'System',
        //path映射那些不直接放置于baseUrl下的模块名。
        paths: {
            'angular': 'Assets/Js/Ng/angular.min.js',//模块名称：模块js相对baseUrl的路径
            'jquery': 'Assets/Js/Lib/jquery.min.js',
            'bootstrap': 'Assets/Js/Lib/bootstrap.min.js',
            'ext': 'Core/Utils/ext.js',
            'canvasbg': 'Assets/Js/Plugins/canvasbg.js',
            'page': 'Core/Container/page.js',
            'core.http': 'Core/Service/core.http.js'
        },
        //指定要加载的一个依赖数组。
        deps: ['jquery', 'canvasbg', 'Core/Component/CanvasBg/canvasbg.js', 'angular', 'bootstrap'],
        //RequireJS获取资源时附加在URL后面的额外的query参数。
        urlArgs: 'v=2018031004.js',
        //为那些没有使用define()来声明依赖关系、设置模块的"浏览器全局变量注入"型脚本做依赖和导出配置。
        shim: {
            'jquery': { exports: 'jQuery' },// 设置jQuery以让require能够检测jquery模块是否正常加载(输出的变量名，表示这个模块外部调用的名称)
            'angular': { deps: ['jquery'], exports: 'angular' },
            'bootstrap': { deps: ['jquery'], exports: 'bootstrap' },
            'canvasbg': { deps: ['jquery'], exports: 'CanvasBG' }
        }
    });

}(window, require));

/*
  加载Login/login、angular模块并初始化页面
*/
require(['page', 'Login/login'],
    function (page) {
        'use strict'

        /// <summary></summary>
        /// <param name="app" type="angular.module">Login/login模块返回对象</param>
        /// <param name="angular" type="angular">angular模块返回对象</param>
        // 手动注册并初始化页面
        angular.bootstrap(document.getElementById('login'), [page.name]);
    });