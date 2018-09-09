define('core.http',
        function () {
            'use strict';

            var api = coreHttp.Api = 'http://webapi.demo.com/';

            function coreHttp($http) {
                if (sessionStorage.Token) $http.defaults.headers.common.Authorization = sessionStorage.Token;
                return {
                    fnUnLogin: fnUnLogin,
                    get: function (url, config) {
                        /// <summary>get请求</summary>
                        /// <param name="url" type="String">请求地址</param>
                        /// <param name="config" type="json">angular $http 配置信息</param>

                        return $http.get(api + url, config).error(fnUnLogin);
                    },
                    post: function (url, data, config) {
                        /// <summary>post请求</summary>
                        /// <param name="url" type="String">请求地址</param>
                        /// <param name="data" type="json">需要提交的数据</param>
                        /// <param name="config" type="json">angular $http 配置信息</param>

                        return $http.post(api + url, data, config).error(fnUnLogin);
                    },
                    put: function (url, data, config) {
                        /// <summary>put请求</summary>
                        /// <param name="url" type="String">请求地址</param>
                        /// <param name="data" type="json">需要提交的数据</param>
                        /// <param name="config" type="json">angular $http 配置信息</param>

                        return $http.put(api + url, data, config).error(fnUnLogin);
                    },
                    delete: function (url, config) {
                        /// <summary>delete请求</summary>
                        /// <param name="url" type="String">请求地址</param>
                        /// <param name="config" type="json">angular $http 配置信息</param>

                        return $http.delete(api + url, config).error(fnUnLogin);
                    }
                }
            }

            return coreHttp;

            function fnUnLogin(d, error) {
                /// <summary>将未登录的用户转向到登录页面</summary>
                /// <param name="arg" type="Array">请求响应参数集合</param>
                /// <param name="error" type="Function">请求失败执行函数</param>

                if (location.pathname.toLowerCase() !== '/login.html' &&
                    (error === 401 || error === -1)) {
                    sessionStorage.referrer = location.href,
                    location.href = "login.html"
                    return;
                }
            }
        });