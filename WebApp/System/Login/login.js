
define(['page', 'Login/login.service'],
	function (app) {
	    'use strict';

	    app.controller('LoginCtrl', ['LoginService', function (loginService) {
	        var $scope = this, uid = loginForm.uid, pwd = loginForm.pwd;
	        this.fnLogin = function (isValid) {
	            if (this.error = !isValid) return;

	            loginService.fnLogin({ LoginName: this.UID, Password: this.PWD })
                            .success(function (token) {
                                if (token) {
                                    sessionStorage.Token = token,
                                    location.href = sessionStorage.referrer !== location.href && sessionStorage.referrer || 'index.html'
                                }
                            })
                            .error(function (a, b) {
                                if (b === -1)
                                    console.error('请检查WebApi是否可以访问！');
                                else alert('登录异常');
                            });
	        }

	    }]);
	});