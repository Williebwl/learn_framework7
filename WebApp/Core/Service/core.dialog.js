define('core.dialog', ['page-Route', 'ext'],
    function (app, ext) {
        'use strict';

        //声明并创建指令逻辑体
        var coreDialogDirectiveLink = function (scope, $element, attrs, ctrl) {
            var defConf = { showcheck: true }, conf = scope.conf ? ext.extend(scope.conf, defConf) : (scope.conf = defConf),
                data = { Text: { value: null, val: [], toString: toString, }, IDs: { value: null, val: [], toString: toString }, infos: [] },
                dialog = scope.dialog = { data: data, conf: conf, $scope: scope, CheckInfo: ctrl.CheckInfo, isCheckInfo: $.isFunction(ctrl.CheckInfo) };

            scope.fnOK = function () {
                var isFunc = $.isFunction, action = isFunc(conf.fnAction) && conf.fnAction || isFunc(scope.action) && scope.action;

                if (isFunc(action) && action.call(data) === false) return;

                conf.fnHide();
            }

            conf.fnShow = function (OldValues) {
                (dialog.OldValues = OldValues) && initCheck.call(dialog);

                $element.modal('show');
            }

            scope.fnHide = conf.fnHide = function () {
                $element.modal('hide');
            }

            scope.fnClear = function (c) {
                if (!c) {
                    if (scope.PageInfo) scope.PageInfo.ck = false;

                    data.infos.forEach(function (o) { o = (o.info || o); !o.disabled && (o.ck = false) });

                    data.Text.value = data.IDs.value = null;
                }

                data.Text.val.length = data.IDs.val.length = data.infos.length = 0;
            }

            //scope.$on('PagingChange', function () { scope.fnClear(); });

            scope.$on('PagingChanged', function () { initCheck.call(dialog); });

            scope.fnCheckedAll = function () {
                if (!scope.PageInfo) return;

                scope.fnClear(1);

                scope.PageInfo.Items.forEach(function (o) {
                    if (!o.disabled && (o.ck = scope.PageInfo.ck)) {
                        Checked.call(dialog, setCheckInfo.apply(dialog, arguments));
                    }
                });

                data.Text.toString();
                dialog.OldValues = data.IDs.toString();
            }

            scope.fnChecked = function (o) {
                if (!scope.PageInfo) return;

                setCheckInfo.apply(dialog, arguments);

                if (!conf.singlecheck) {
                    var pageInfo = scope.PageInfo, ck;

                    if (ck = o.ck) {
                        Checked.call(dialog, o.CheckInfo);

                        for (var i = 0, items = pageInfo.Items, l = items.length; i < l; i++) if (!items[i].disabled && !items[i].ck) { pck = false; break; };
                    }
                    else {
                        data.Text.val.remove(o.CheckInfo.Text || '');
                        data.IDs.val.remove(o.CheckInfo.ID || '');

                        data.infos.removeGrep(function () { return this.ID; }, o.CheckInfo.ID);
                    }

                    pageInfo.ck = ck;
                }
                else {
                    var last = data.infos[0];

                    if (last) (last.info || last).ck = false;

                    Checked.call(dialog, o.CheckInfo);
                }

                data.Text.toString();
                dialog.OldValues = data.IDs.toString();
            }
        };

        //树指令默认参数
        var defSetings = {
            restrict: 'EA',
            priority: 100,
            replace: true,
            scope: { conf: '=', action: '=' },
            link: coreDialogDirectiveLink,
            controller: function () { }
        };

        return {
            coreDialogCtrl: function (ctrl, $scope, coreService, $routeParams) {
                var search = $scope.search = { i: 0 };

                //初始化分页指令
                var pageConfig = $scope.PageConfig = {
                    pageSelect: 0,
                    itemsPerPage: 8,
                    pagesLength: 3,
                    onChange: function (i) {
                        /// <summary>调用查询</summary>
                        /// <param name="i" type="Number">查询标示</param>

                        var val = (val = search.inputVal) && val != '输入关键字' ? val : '';

                        //调用Service.fnGetAll方法查询数据
                        coreService.fnGetAll({ Key: val, PageIndex: pageConfig.currentPage, PageSize: pageConfig.itemsPerPage, TagGroupID: $routeParams.groupID },
                            function (d) {
                                if (angular.isNumber(i) && search.i !== i) return;

                                $scope.PageInfo = d;
                            });


                    }
                };

                $scope.fnGetAll = $.proxy(pageConfig, 'onChange');

                ctrl.CheckInfo = function (info) {
                    this.ID = info.ID;
                    this.Text = info.Name;
                    this.info = info;
                }
            },
            dialog: function (dialogTitle, recipeName, directiveSetings) {
                /// <summary>创建窗口指令</summary>
                /// <param name="title" type="String">窗口标题</param>
                /// <param name="recipeName" type="String">指令名称</param>
                /// <param name="directiveSetings" type="json、function">指令配置信息或指令控制器函数</param>

                directiveSetings.template = fnGetTemplate(dialogTitle, directiveSetings.template || '');

                //指令配置信息处理
                directiveSetings = ext.extend($.isPlainObject(directiveSetings) ? directiveSetings : { controller: directiveSetings || $.noop, template: directiveSetings.template }, defSetings);

                return app.directive(recipeName, function () {
                    return directiveSetings;
                });
            }
        };

        function fnGetTemplate(dialogTitle, modalBody) {
            return '<div class="modal fade bs-modal-sm bs-modal-static" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">\
                        <div class="modal-dialog" style="width:430px;" data-draggable="true">\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <button aria-hidden="true" ng-click="fnHide()" class="close" type="button">&times;</button>\
                                    <h4 class="modal-title">' + dialogTitle + '</h4>\
                                </div>\
                                <div class="modal-body">' + modalBody + '</div>\
                                <div class="modal-footer">\
                                    <button type="button" class="btn btn-primary" ng-click="fnOK()">确定</button>\
                                    <button type="button" class="btn btn-default" ng-click="fnClear()">清空</button>\
                                    <button type="button" class="btn btn-default" ng-click="fnHide()">取消</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>';
        }

        function toString() { if (!Array.isArray(this.val)) return this.value = null; return this.value = this.val.join(','); }

        function initCheck() {
            if (!this.$scope.PageInfo || !this.$scope.PageInfo.Items || !this.$scope.PageInfo.Items.length) return;

            this.OldValues != this.data.IDs.value && this.$scope.fnClear();

            if (this.OldValues) (this.conf.singlecheck ? singleCheck : multiCheck).call(this, this.OldValues);

            this.data.Text.toString();
            this.data.IDs.toString();
        }

        function singleCheck(OldValues) {
            var infos = this.$scope.PageInfo.Items, info;

            for (var i = 0, l = infos.length; i < l; i++) {
                setCheckInfo.call(this, info = infos[i]);
                if (OldValues == info.CheckInfo.ID) {
                    info.ck = true; Checked.call(this, info.CheckInfo); break;
                }
            }
        }

        function multiCheck(OldValues) {
            OldValues = ',' + OldValues + ',';
            var infos = this.$scope.PageInfo.Items, info, s = 0, l = infos.length;

            for (var i = 0 ; i < l; i++) {
                setCheckInfo.call(this, info = infos[i]);
                if (OldValues.indexOf(',' + info.CheckInfo.ID + ',') >= 0) {
                    info.ck = true;
                    Checked.call(this, info.CheckInfo);
                    s++;
                }
            }

            if (s === l - 1) this.$scope.PageInfo.ck = true;
        }

        function Checked(o) {
            var txt = o.Text || '', id = o.ID || '';
            if (this.conf.singlecheck) {
                this.data.infos[0] = o;

                this.data.Text.val[0] = o.info.ck ? txt : '';
                this.data.IDs.val[0] = o.info.ck ? id : '';
            }
            else {
                this.data.infos.push(o);

                this.data.Text.val.push(txt);
                this.data.IDs.val.push(id);
            }
        }

        function setCheckInfo(o) { return o.CheckInfo || (o.CheckInfo = this.isCheckInfo ? new this.CheckInfo(o) : o); }
    });