define('bi.ext', ['page', 'ext', 'uploader', 'css!Assets/Css/attach.css'],
    function (app, ext, uploader) {
        'use strict';

        var biIncludeDirective = ['$templateRequest', '$anchorScroll', '$animate',
                  function ($templateRequest, $anchorScroll, $animate, $rootScope) {
                      return {
                          restrict: 'ECA',
                          priority: 400,
                          terminal: true,
                          transclude: 'element',
                          controller: angular.noop,
                          compile: function (element, attr) {
                              var onloadExp = attr.onload || '',
                                  autoScrollExp = attr.autoscroll,
                                  target = $.trim(attr.target),
                                  event = $.trim(attr.event);

                              return function (scope, $element, $attr, ctrl, $transclude) {
                                  var changeCounter = 0,
                                      currentScope,
                                      previousElement,
                                      currentElement,
                                      srcExp = attr.biInclude || attr.src || attr.templateurl,
                                      ctrlUrl = $attr.controllerurl,
                                      action = $.trim(attr.action),
                                      async = !!(target || action);

                                  if (srcExp) srcExp = scope.$eval(srcExp) || srcExp;

                                  if (srcExp && !ctrlUrl) ctrlUrl = [((ctrlUrl = srcExp.match(/(.+)\.htm+l?$/i)) ? ctrlUrl[1] : srcExp) + '.js'];
                                  else if (ctrlUrl) ctrlUrl = (scope.$eval(srcExp) || ctrlUrl).split(/;*,+;*|,*;+/);

                                  ctrl.Url = ctrlUrl;

                                  var cleanupLastIncludeContent = function () {
                                      if (previousElement) {
                                          previousElement.remove();
                                          previousElement = null;
                                      }
                                      if (currentScope) {
                                          currentScope.$destroy();
                                          currentScope = null;
                                      }
                                      if (currentElement) {
                                          $animate.leave(currentElement).then(function () {
                                              previousElement = null;
                                          });
                                          previousElement = currentElement;
                                          currentElement = null;
                                      }
                                  };

                                  scope.$watch('"' + srcExp + '"', function ngIncludeWatchAction(src) {
                                      var afterAnimation = function () {
                                          if (angular.isDefined(autoScrollExp) && (!autoScrollExp || scope.$eval(autoScrollExp))) {
                                              $anchorScroll();
                                          }
                                      };

                                      var thisChangeId = ++changeCounter, func, load;

                                      if (src) {
                                          func = function (callback) {
                                              if (ctrl.callback) ctrl.callback();
                                              else if (action !== 0) {
                                                  action = 0, typeof callback === 'function' && (ctrl.callback = callback);

                                                  $templateRequest(src, true).then(function (response) {
                                                      if (thisChangeId !== changeCounter) return;
                                                      var newScope = scope.$new();
                                                      ctrl.template = response;

                                                      var clone = $transclude(newScope, function (clone) {
                                                          cleanupLastIncludeContent();
                                                          $animate.enter(clone, null, $element).then(afterAnimation);
                                                      });

                                                      currentScope = newScope;
                                                      currentElement = clone;

                                                      currentScope.$emit('$includeContentLoaded', src);
                                                      scope.$eval(onloadExp);
                                                  }, function () {
                                                      if (thisChangeId === changeCounter) {
                                                          cleanupLastIncludeContent();
                                                          scope.$emit('$includeContentError', src);
                                                      }
                                                  });
                                                  scope.$emit('$includeContentRequested', src);
                                              }
                                          }

                                          if (async) {
                                              if (target) $(document).off(event || 'click', target, load = function () { $(document).off(event || 'click', target, load), func() }).on(event || 'click', target, load)

                                              if (action) (scope.infos || scope)[action] = func;
                                          } else func();
                                      } else {
                                          cleanupLastIncludeContent();
                                          ctrl.template = null;
                                      }
                                  });
                              };
                          }
                      };
                  }];

        var biIncludeFillContentDirective = ['$compile',
          function ($compile) {
              return {
                  restrict: 'ECA',
                  priority: -400,
                  require: 'biInclude',
                  link: function (scope, $element, $attr, ctrl) {
                      if (/SVG/.test($element[0].toString())) {
                          $element.empty();
                          $compile(jqLiteBuildFragment(ctrl.template, document).childNodes)(scope,
                              function namespaceAdaptedClone(clone) {
                                  $element.append(clone);
                              }, { futureParentElement: $element });
                          return;
                      }

                      function fn() {
                          $element.html(ctrl.template);
                          $compile($element.contents())(scope);

                          if (ctrl.callback) ctrl.callback();

                          scope.$digest();
                      }

                      Array.isArray(ctrl.Url) && require(ctrl.Url, fn) || fn();
                  }
              };
          }];

        app.directive('biInclude', biIncludeDirective).directive('biInclude', biIncludeFillContentDirective)


        var biDialogDirective = ['$templateRequest', '$anchorScroll', '$animate',
                  function ($templateRequest, $anchorScroll, $animate) {
                      return {
                          restrict: 'ECA',
                          priority: 400,
                          terminal: true,
                          transclude: 'element',
                          controller: angular.noop,
                          compile: function (element, attr) {
                              var srcExp = attr.ngInclude || attr.src,
                                  ctrlUrl = attr.ctrlurl,
                                  onloadExp = attr.onload || '',
                                  autoScrollExp = attr.autoscroll,
                                  key = attr.id || attr.name || srcExp;

                              if (srcExp && !ctrlUrl) ctrlUrl = [((ctrlUrl = srcExp.match(/(.+)\.htm+l?$/i)) ? ctrlUrl[1] : srcExp) + '.js'];
                              else if (ctrlUrl) ctrlUrl = (scope.$eval(srcExp) || ctrlUrl).split(/;*,+;*|,*;+/);

                              return function (scope, $element, $attr, ctrl, $transclude) {
                                  var changeCounter = 0,
                                      currentScope,
                                      previousElement,
                                      currentElement,
                                      $dialog = scope.$dialog || (scope.$dialog = {}),
                                      currentScopes = scope.hasOwnProperty('$Active') && scope || scope.$parent;

                                  ctrl.Url = ctrlUrl;

                                  if (!currentScopes.ShowDialog) currentScopes.ShowDialog = function (id, d, e) {
                                      var func = $dialog[angular.isString(id) && id || key];
                                      angular.isFunction(func) && func(d || id, e);
                                  };

                                  var cleanupLastIncludeContent = function () {
                                      if (previousElement) {
                                          previousElement.remove();
                                          previousElement = null;
                                      }
                                      if (currentScope) {
                                          currentScope.$destroy();
                                          currentScope = null;
                                      }
                                      if (currentElement) {
                                          $animate.leave(currentElement).then(function () {
                                              previousElement = null;
                                          });
                                          previousElement = currentElement;
                                          currentElement = null;
                                      }
                                  };

                                  scope.$watch('"' + srcExp + '"', function biDialogWatchAction(src) {
                                      var afterAnimation = function () {
                                          if (angular.isDefined(autoScrollExp) && (!autoScrollExp || scope.$eval(autoScrollExp))) {
                                              $anchorScroll();
                                          }
                                      };
                                      var thisChangeId = ++changeCounter;

                                      if (src) {
                                          $templateRequest(src, true).then(function (response) {
                                              if (thisChangeId !== changeCounter) return;

                                              $dialog[key] = function (d, e) {
                                                  var newScope = scope.$new();
                                                  ctrl.ViewData = d,
                                                  ctrl.template = response;

                                                  var clone = $transclude(newScope, function (clone) {
                                                      cleanupLastIncludeContent();
                                                      $animate.enter(clone, null, $element).then(afterAnimation);
                                                  });

                                                  currentScope = newScope;
                                                  currentElement = clone;

                                                  currentScope.$emit('$includeContentLoaded', src);
                                                  scope.$eval(onloadExp);
                                              }
                                          }, function () {
                                              if (thisChangeId === changeCounter) {
                                                  cleanupLastIncludeContent();
                                                  scope.$emit('$includeContentError', src);
                                              }
                                          });
                                          scope.$emit('$includeContentRequested', src);
                                      } else {
                                          cleanupLastIncludeContent();
                                          ctrl.template = null;
                                      }
                                  });
                              };
                          }
                      };
                  }];

        var biDialogFillContentDirective = ['$compile',
          function ($compile) {
              return {
                  restrict: 'ECA',
                  priority: -400,
                  require: 'biDialog',
                  link: function (scope, $element, $attr, ctrl) {
                      if (/SVG/.test($element[0].toString())) {
                          $element.empty();
                          $compile(jqLiteBuildFragment(ctrl.template, document).childNodes)(scope,
                              function (clone) {
                                  $element.append(clone);
                              }, { futureParentElement: $element });
                          return;
                      }

                      function fn() {
                          $element.html(ctrl.template),
                          $compile($element.contents())(scope),
                          angular.isFunction(scope.ShowDialog) && scope.ShowDialog(ctrl.ViewData),
                          scope.$digest()
                      }

                      Array.isArray(ctrl.Url) && require(ctrl.Url, fn) || fn();
                  }
              };
          }];

        app.directive('biDialog', biDialogDirective).directive('biDialog', biDialogFillContentDirective)

        var ngControllerDirective = function () {
            return {
                restrict: 'A',
                priority: 500,
                link: function (scope, $element, attr) { scope.$element = $element, angular.isFunction(scope.fnInit) && scope.fnInit($element, attr) }
            }
        }

        app.directive('ngController', ngControllerDirective)

        biNavFactory.$inject = ['$route', '$anchorScroll', '$animate'];
        function biNavFactory($route, $anchorScroll, $animate) {
            return {
                restrict: 'ECA',
                terminal: true,
                priority: 400,
                transclude: 'element',
                link: function (scope, $element, attr, ctrl, $transclude) {
                    var currentScope,
                        currentElement,
                        previousLeaveAnimation,
                        autoScrollExp = attr.autoscroll,
                        onloadExp = attr.onload || '';

                    scope.$on('$routeChangeSuccess', update);
                    update();

                    function cleanupLastView() {
                        if (previousLeaveAnimation) {
                            $animate.cancel(previousLeaveAnimation);
                            previousLeaveAnimation = null;
                        }

                        if (currentScope) {
                            currentScope.$destroy();
                            currentScope = null;
                        }
                        if (currentElement) {
                            previousLeaveAnimation = $animate.leave(currentElement);
                            previousLeaveAnimation.then(function () {
                                previousLeaveAnimation = null;
                            });
                            currentElement = null;
                        }
                    }

                    function update() {
                        var locals = $route.current && $route.current.locals,
                            template = locals && locals.$navTemplate;

                        if (angular.isDefined(template) || (locals && $route.current.$$route.IsApp)) {
                            var newScope = scope.$new();
                            var current = $route.current;

                            var clone = $transclude(newScope, function (clone) {
                                $animate.enter(clone, null, currentElement || $element).then(function onNgViewEnter() {
                                    if (angular.isDefined(autoScrollExp)
                                      && (!autoScrollExp || scope.$eval(autoScrollExp))) {
                                        $anchorScroll();
                                    }
                                });
                                cleanupLastView();
                            });

                            currentElement = clone;
                            currentScope = current.scope = newScope;
                            currentScope.$emit('$NavContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$emit('$ContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$eval(onloadExp);
                        } else if (!$route.hasOwnProperty('current') || $route.current.hasOwnProperty('loadedNavTemplateUrl')) {
                            cleanupLastView();
                        }
                    }
                }
            };
        }

        biNavFillContentFactory.$inject = ['$compile', '$controller', '$route'];
        function biNavFillContentFactory($compile, $controller, $route) {
            return {
                restrict: 'ECA',
                priority: -400,
                link: function (scope, $element) {
                    var current = $route.current,
                        locals = current.locals;

                    $element.html(locals.$navTemplate)

                    var link = $compile($element.contents());

                    if (current.navController) {
                        locals.$scope = scope;
                        var controller = $controller(current.navController, locals);
                        if (current.controllerAs) {
                            scope[current.controllerAs] = controller;
                        }
                        $element.data('$ngControllerController', controller);
                        $element.children().data('$ngControllerController', controller);
                    }

                    link(scope);
                }
            };
        }

        app.directive('biNav', biNavFactory).directive('biNav', biNavFillContentFactory);

        biToolBarFactory.$inject = ['$route', '$anchorScroll', '$animate'];
        function biToolBarFactory($route, $anchorScroll, $animate) {
            return {
                restrict: 'ECA',
                terminal: true,
                priority: 400,
                transclude: 'element',
                link: function (scope, $element, attr, ctrl, $transclude) {
                    var currentScope,
                        currentElement,
                        previousLeaveAnimation,
                        autoScrollExp = attr.autoscroll,
                        onloadExp = attr.onload || '';

                    scope.$on('$routeChangeSuccess', update);
                    update();

                    function cleanupLastView() {
                        if (previousLeaveAnimation) {
                            $animate.cancel(previousLeaveAnimation);
                            previousLeaveAnimation = null;
                        }

                        if (currentScope) {
                            currentScope.$destroy();
                            currentScope = null;
                        }
                        if (currentElement) {
                            previousLeaveAnimation = $animate.leave(currentElement);
                            previousLeaveAnimation.then(function () {
                                previousLeaveAnimation = null;
                            });
                            currentElement = null;
                        }
                    }

                    function update() {
                        var locals = $route.current && $route.current.locals,
                            template = locals && locals.$toolBarTemplate;

                        if (angular.isDefined(template)) {
                            var newScope = scope.$new();
                            var current = $route.current;

                            var clone = $transclude(newScope, function (clone) {
                                $animate.enter(clone, null, currentElement || $element).then(function onNgViewEnter() {
                                    if (angular.isDefined(autoScrollExp)
                                      && (!autoScrollExp || scope.$eval(autoScrollExp))) {
                                        $anchorScroll();
                                    }
                                });
                                cleanupLastView();
                            });

                            currentElement = clone;
                            currentScope = current.scope = newScope;
                            currentScope.$emit('$ToolBarContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$emit('$ContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$eval(onloadExp);
                        } else if (!$route.hasOwnProperty('current') || $route.current.hasOwnProperty('loadedToolBarTemplateUrl')) {
                            cleanupLastView();
                        }
                    }
                }
            };
        }

        biToolBarFillContentFactory.$inject = ['$compile', '$controller', '$route'];
        function biToolBarFillContentFactory($compile, $controller, $route) {
            return {
                restrict: 'ECA',
                priority: -400,
                link: function (scope, $element) {
                    var current = $route.current,
                        locals = current.locals;

                    $element.html(locals.$toolBarTemplate);

                    var link = $compile($element.contents());

                    if (current.toolBarController) {
                        locals.$scope = scope;
                        var controller = $controller(current.toolBarController, locals);
                        if (current.controllerAs) {
                            scope[current.controllerAs] = controller;
                        }
                        $element.data('$ngControllerController', controller);
                        $element.children().data('$ngControllerController', controller);
                    }

                    locals.$navTemplate && $element.closest('#content').removeAttr('style')

                    link(scope);
                }
            };
        }

        app.directive('biToolbar', biToolBarFactory).directive('biToolbar', biToolBarFillContentFactory);

        biContainerFactory.$inject = ['$route', '$anchorScroll', '$animate'];
        function biContainerFactory($route, $anchorScroll, $animate) {
            return {
                restrict: 'ECA',
                terminal: true,
                priority: 400,
                transclude: 'element',
                link: function (scope, $element, attr, ctrl, $transclude) {
                    var currentScope,
                        currentElement,
                        previousLeaveAnimation,
                        autoScrollExp = attr.autoscroll,
                        onloadExp = attr.onload || '';

                    scope.$on('$routeChangeSuccess', update);
                    update();

                    function cleanupLastView() {
                        if (previousLeaveAnimation) {
                            $animate.cancel(previousLeaveAnimation);
                            previousLeaveAnimation = null;
                        }

                        if (currentScope) {
                            currentScope.$destroy();
                            currentScope = null;
                        }
                        if (currentElement) {
                            previousLeaveAnimation = $animate.leave(currentElement);
                            previousLeaveAnimation.then(function () {
                                previousLeaveAnimation = null;
                            });
                            currentElement = null;
                        }
                    }

                    function update() {
                        var locals = $route.current && $route.current.locals,
                            template = locals && locals.$containerTemplate;

                        if (angular.isDefined(template)) {
                            var newScope = scope.$new();
                            var current = $route.current;

                            var clone = $transclude(newScope, function (clone) {
                                $animate.enter(clone, null, currentElement || $element).then(function onNgViewEnter() {
                                    if (angular.isDefined(autoScrollExp)
                                      && (!autoScrollExp || scope.$eval(autoScrollExp))) {
                                        $anchorScroll();
                                    }
                                });
                                cleanupLastView();
                            });

                            currentElement = clone;
                            currentScope = current.scope = newScope;
                            currentScope.$emit('$ContainerContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$emit('$ContentLoaded', { $element: currentElement, scope: currentScope });
                            currentScope.$eval(onloadExp);
                        } else if (!$route.hasOwnProperty('current') || $route.current.hasOwnProperty('loadedContainerTemplateUrl')) {
                            cleanupLastView();
                        }
                    }
                }
            };
        }

        biContainerFillContentFactory.$inject = ['$compile', '$controller', '$route'];
        function biContainerFillContentFactory($compile, $controller, $route) {
            return {
                restrict: 'ECA',
                priority: -400,
                link: function (scope, $element) {
                    var current = $route.current,
                        locals = current.locals;

                    $element.html(locals.$containerTemplate);

                    var link = $compile($element.contents());

                    if (current.containerController) {
                        locals.$scope = scope;
                        var controller = $controller(current.containerController, locals);
                        if (current.controllerAs) {
                            scope[current.controllerAs] = controller;
                        }
                        $element.data('$ngControllerController', controller);
                        $element.children().data('$ngControllerController', controller);
                    }

                    locals.$navTemplate && $element.closest('#content').removeAttr('style')

                    link(scope);
                }
            };
        }

        app.directive('biContainer', biContainerFactory).directive('biContainer', biContainerFillContentFactory);

        var biCheckboxDirective = function () {

            return {
                restrict: 'AC',
                controller: function ($scope) {
                    var ids = ($scope.infos || $scope).CheckedInfos = [], self = this, $cbxs = [], $cbxAll = self.$cbxAll = [];

                    function fnSetAll(ck) {
                        $cbxAll.forEach(function ($cbx) { $cbx.prop('checked', ck); });
                    }

                    this.bind = function ($val) {
                        var ck = $val.ck || $val.key.ck || $val.key.checked || 0;

                        if ($val.$index === 0) { if (ck) fnSetAll(ck); ids.length = $cbxs.length = 0; }
                        else if (!ck) fnSetAll(ck);

                        $cbxs.push(this.prop('checked', ck));

                        if (ck) ids.push($val);

                        return $val;
                    };

                    this.fnCheckedAll = function () {
                        var ck = $(this).prop('checked');

                        fnSetAll(ck), ids.length = 0;

                        $cbxs.forEach(function ($cbx) {
                            $cbx.prop('checked', ck);
                            ck && ids.push($cbx.$val);
                        })

                    }

                    this.fnChecked = function (e) {
                        var ck = $(this).prop('checked'), $val = e.data.$val;
                        ck ? ids.push($val) : ids.remove($val);
                        fnSetAll($cbxs.length === ids.length);
                    }
                },
                compile: function (tpl, tplAttr) {
                    var expression = $('[ng-repeat]:first', tpl).attr('ng-repeat'),
                    match = expression.match(/^\s*([\s\S]+?)\s+in\s+([\s\S]+?)(?:\s+as\s+([\s\S]+?))?(?:\s+track\s+by\s+([\s\S]+?))?\s*$/);
                    match = match ? match[1].match(/^(?:(\s*[\$\w]+)|\(\s*([\$\w]+)\s*,\s*([\$\w]+)\s*\))$/) : [];
                    var key = match[3] || match[1];

                    return function (scope, $element, attr, ctrl) {
                        ctrl.key = key;
                    }
                }
            };
        };

        app.directive('biCheckbox', biCheckboxDirective);

        function checkboxORradio(type, css) {
            css = type + ' ' + type + '-primary ' + css;

            if (!this.parent().is('label')) this.wrap('<label></label>');

            var $p = this.parent().append('<i class="fa fa-' + (type === 'radio' ? 'circle' : 'square') + '-o"></i>'), $pt = $p.parent();

            $pt.is('td') || $pt.is('th') ? $p.wrap('<div class="' + css + '"></div>') : $pt.addClass(css);
        }

        var inputDirective = function () {
            return {
                restrict: 'E',
                require: '?^biCheckbox',
                compile: function (element, attr) {
                    if (attr.type === 'checkbox' || attr.type === 'radio') {
                        return function (scope, $element, attr, ctrl) {
                            var $p, isall, css = ($element.prop('disabled') || (attr.ngDisabled && scope.$eval(attr.ngDisabled))) ? 'disabled' : '';
                            if (attr.type === 'checkbox') { $p = $element.parent(), isall = !angular.isUndefined(attr.isall) || $p.is('th') || ($p = $p.parent()).is('th') || $p.parent().is('th'); }

                            checkboxORradio.call($element, attr.type, css);

                            if (attr.type !== 'checkbox' || css) return;

                            if (ctrl && ctrl.fnCheckedAll && ctrl.fnChecked) {
                                isall ? $element.on('click', ctrl.$cbxAll.push($element) && ctrl.fnCheckedAll) :
                                $element.on('click', { $val: ctrl.bind.call($element, $element.$val = { $index: scope.$index, key: scope.$eval(attr.ngValue || ctrl.key) }) }, ctrl.fnChecked);
                            }
                        }
                    } else element.addClass('form-control');
                }
            };
        }

        app.directive({
            input: inputDirective,
            textarea: inputDirective
        });

        var biAttachTemplate = '<div class="attach">\
                                    <div id="file"></div>\
                                    <ul class="ViewFile">\
                                        <li ng-repeat="file in files" id="{{file.id}}">\
                                             <a href="javascript:;" ng-click="fnDownload(file)">\
                                                <i class="ioc" ng-class="file.Icon"></i>\
                                                <span ng-bind="file.name"></span>\
                                             </a>\
                                             <span class="fileSize" ng-bind="file.formatSize"></span>\
                                             <em ng-click="fnDel(file)" title="删除"></em>\
                                         </li>\
                                    </ul>\
                                    <div class="Download"></div>\
                                </div>';

        var biAttachDirective = function ($rootScope, attachService) {

            var uploaderOpt = {
                auto: true,
                swf: 'Assets/Swf/Uploader.swf',
                server: null,
                threads: 5
            }

            return {
                restrict: 'ECA',
                template: biAttachTemplate,
                replace: true,
                scope: { Config: '=' },
                controller: function ($scope) {
                    var files = $scope.files = [], $self = this, DelIDs = $self.DelIDs = [];

                    $scope.fnDownload = function (file) {
                        file.ID && $('<iframe src="' + attachService.fnGetDownSrc(file.ID) + '"></iframe>').appendTo($self.$Down);
                    }

                    $scope.fnDel = function (file) {
                        files.remove(file) && file.ID && DelIDs.push(file.ID),
                        $self.uploader.cancelFile(file),
                        $self.uploader.removeFile(file)
                    }

                    this.fnGetAttach = function (d) {
                        this.sucNumber += 1,
                        $self.uploader.options.server = attachService.fnGetUploadSrc($self.Key = d.Key), files = $scope.files = d.Files || [],
                        $self.uploader.refresh(),
                        $self.uploader.reset(),
                        (this.sucNumber + this.errNumber) === this.sumNumber && (this.sumNumber === this.sucNumber && isFunction(this.success) && this.success() || isFunction(this.error) && this.error())
                    }

                    this.fnGetAttachError = function (d) {
                        this.errNumber += 1,
                        $self.uploader.refresh(),
                        $self.uploader.reset(),
                        (this.sucNumber + this.errNumber) === this.sumNumber && isFunction(this.error) && this.error()
                    }

                    this.bindUploader = function (upload) {
                        ($self.Config.Uploader = $self.uploader = upload)
                            .on('fileQueued', function (file) {
                                file.formatSize = uploader.formatSize(file.size),
                                file.Icon = file.ext + $self.IconSize,
                                isFunction($self.Config.fileQueued) && $self.Config.fileQueued(files, DelIDs),
                                files.push(file),
                                $scope.$digest(),
                                file.Percent = (file.Progress = $('<div class="progress"><div class="progress-bar"></div></div>')).appendTo(file.info = $self.$View.find('#' + file.id)).find('.progress-bar')
                            }).on('uploadProgress', function (file, percentage) {
                                file.Percent.css('width', percentage * 100 + '%')
                            }).on('uploadSuccess', function (file, d) {
                                d.Files && (file.ID = d.Files[0].ID)
                            }).on('uploadError', function (file) {
                                file.info.addClass('error')
                            }).on('uploadComplete', function (file) {
                                file.Progress.remove(),
                                delete file.Progress,
                                delete file.Percent
                            }),
                            isFunction($self.Config.bindUploader) && $self.Config.bindUploader(upload)
                    }

                    $rootScope.$on('$$SaveAttach', function (e, d) {
                        d.sumNumber += 1,
                        attachService.fnSaveAttach([{ TableID: d.ID || 0, TableName: $self.TableName, CustomType: $self.CustomType, AttachKey: $self.Key, FileIDs: DelIDs }],
                            function (isOk) {
                                isOk ? d.sucNumber += 1 : d.errNumber += 1,
                                (d.sucNumber + d.errNumber) === d.sumNumber && (d.sumNumber === d.sucNumber && isFunction(d.success) && d.success() || isFunction(d.error) && d.error())
                            },
                            function (d) {
                                d.errNumber += 1,
                                (d.sucNumber + d.errNumber) === d.sumNumber && isFunction(d.error) && d.error()
                            })
                    })
                },
                compile: function compile(element, attr) {
                    return function (scope, $element, attr, ctrl) {
                        ctrl.TableName = $.trim(attr.biAttach) || $.trim(attr.table),
                        ctrl.CustomType = parseInt(attr.type) || 0,
                        ctrl.IconSize = attr.iconsize || '16',
                        ctrl.$Down = $element.find('.Download:first'),
                        ctrl.$View = $element.find('.ViewFile:first'),
                        ctrl.Config = ext.extend(scope.Config || {}, uploaderOpt);
                        ctrl.Config.pick = (ctrl.multiple = ($.trim(attr.multiple).toLowerCase() !== 'false')) ? $element.find('#file') : { id: $element.find('#file'), multiple: false },
                        scope.$eval(attr.isimg) && (ctrl.Config.accept = { title: 'Images', extensions: 'gif,jpg,jpeg,bmp,png', mimeTypes: 'image/*' }),
                        ctrl.bindUploader(uploader.create(ctrl.Config)),
                        $rootScope.$on('$$LoadAttach', function (e, d) {
                            d.sumNumber += 1,
                            attachService.fnGetAttach(ctrl.TableName, d.ID || 0, ctrl.CustomType, ctrl.fnGetAttach.bind(d), ctrl.fnGetAttachError.bind(d))
                        })
                    }
                }
            };

            function isFunction(o) {
                return typeof o === 'function'
            }
        }

        app.directive('biAttach', biAttachDirective);
    });