/*
  定义一个require模块,模块名称为 page
  该模块依赖angular。

 （require资料参考地址：http://requirejs.cn/；
   angular资料参考地址：http://192.168.0.246:88/Angular/docs）

  日期：2015-08-20
*/
define('page', ['angular'],
    function (angular) {
        'use strict'
        /*
          创建并返回angular名称为page的module，该module不依赖其它module
        */
        return angular.module('page', []);
    });