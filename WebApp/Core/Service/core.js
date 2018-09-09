define('core', ['page-Route', 'ext'],
    function (app, ext) {
        'use strict'

        var Super,
            FN_ARGS = /^[^\(]*\(\s*([^\)]*)\)/m,
            FN_ARG_SPLIT = /,/,
            FN_ARG = /^\s*(_?)(\S+?)\1\s*$/,
            STRIP_COMMENTS = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg;

        function Core($view, $service, $scope) {
            if (!this || this.constructor === Window) return new Core($view, $service, $scope);
        }

        return $.extend(Core.fn = Core.prototype, {
            extend: Core.inherit = function (target, data) {
                return $.extend(target.prototype || target, data || Core.fn), target;
            },
            super: function ($view, $service, $scope, target) {
                return (Super = (Super || this.constructor).Super) && Super && Super.call(this, $view, $service, $scope || $view.$id && $view || $service.$rootScope), Super === Core && (Super = 0), this
            }
        }) && $.extend(Core, {
            ext: function (target, data) {
                return (target.Super = this), $.extend(Core.inherit(target).fn = target.prototype, data), $.extend(target, Core);
            },
            controller: function (recipeName, factoryFunction) {
                /// <summary>创建控制器</summary>
                /// <param name="recipeName" type="String">控制器名称不需要些Ctrl后缀程序会自动添加</param>
                /// <param name="factoryFunction" type="function、Array">控制器内容</param>

                //创建控制器并返回异步路由容器
                return app.controller(recipeName, factoryFunction);
            },
            forEach: angular.forEach,
            copy: angular.copy,
            equals: angular.equals,
            fromJson: angular.fromJson,
            isArray: angular.isArray,
            isDate: angular.isDate,
            isDefined: angular.isDefined,
            isElement: angular.isElement,
            isFunction: angular.isFunction,
            isNumber: angular.isNumber,
            isObject: angular.isObject,
            isPrototypeOf: angular.isPrototypeOf,
            isString: angular.isString,
            isUndefined: angular.isUndefined,
            noop: angular.noop,
            toJson: angular.toJson,
            extend: angular.extend,
            app: app,
            $injector: app.$injector || $(document.documentElement).data('$injector'),
            annotate: annotate
        });

        function annotate(fn) {
            var $inject,
                fnText,
                argDecl,
                last;

            if (Core.isFunction(fn)) {
                if (!($inject = fn.$inject)) {
                    $inject = [];
                    if (fn.length) {
                        fnText = fn.toString().replace(STRIP_COMMENTS, '');
                        argDecl = fnText.match(FN_ARGS);
                        Core.forEach(argDecl[1].split(FN_ARG_SPLIT), function (arg) {
                            arg.replace(FN_ARG, function (all, underscore, name) {
                                $inject.push(name);
                            });
                        });
                    }
                    fn.$inject = $inject;
                }
            } else if (Core.isArray(fn) && fn.length) {
                last = fn.length - 1;
                $inject = fn.slice(0, last);
            }

            return $inject;
        }
    })