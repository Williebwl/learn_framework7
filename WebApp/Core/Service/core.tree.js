
define('core.tree', ['smart.tree','ext'],
    function (app, ext) {
        'use strict';

        //声明并创建指令逻辑体
        var coreTreeDirectiveLink = function (scope, $element) {
            var defConf = { showcheck: true }, conf = scope.conf ? ext.extend(scope.conf, defConf) : (scope.conf = defConf);

            scope.fnOK = function () {
                var isFunc = $.isFunction, action = isFunc(conf.action) && conf.action || isFunc(scope.action) && scope.action;

                if (isFunc(action) && action.call(conf.$tree, conf) === false) return;

                conf.fnHide();
            }

            conf.fnShow = function (OldValues) {
                conf.$tree.setSelected(OldValues);

                $element.modal('show');
            }

            scope.fnHide = conf.fnHide = function () {
                $element.modal('hide');
            }

            scope.fnClearAll = function () {
                conf.$tree.clearSelectNodes();
            }
        };

        //树指令默认参数
        var defSetings = {
            restrict: 'EA',
            priority: 100,
            replace: true,
            scope: { conf: '=', action: '=' },
            link: coreTreeDirectiveLink
        };

        //返回树指令创建函数
        return function (title, recipeName, directiveSetings) {
            /// <summary>创建树指令</summary>
            /// <param name="title" type="String">标题</param>
            /// <param name="recipeName" type="String">指令名称</param>
            /// <param name="directiveSetings" type="json、Array、function">指令配置信息获取指令控制器数组或函数</param>

            //设置指令模版
            if (!$.trim(directiveSetings.template)) directiveSetings.template = fnGetTemplate(title);

            //指令配置信息处理
            directiveSetings = ext.extend($.isPlainObject(directiveSetings) ? directiveSetings : { controller: directiveSetings || $.noop, template: directiveSetings.template }, defSetings);

            //创建
            return app.directive(recipeName, function () {
                return directiveSetings;
            });
        };

        function fnGetTemplate(title) {
            return '<div class="modal fade bs-modal-sm bs-modal-static" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">\
                        <div class="modal-dialog modal-sm" data-draggable="true">\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <button aria-hidden="true" ng-click="fnHide()" class="close" type="button">&times;</button>\
                                    <h4 class="modal-title">' + (title || '') + '</h4>\
                                </div>\
                                <div class="modal-body" style="min-height:300px;max-height:500px;">\
                                    <div bi-smart-tree conf="conf" data="data"></div>\
                                </div>\
                                <div class="modal-footer">\
                                    <button type="button" class="btn btn-primary" ng-click="fnOK()">确定</button>\
                                    <button type="button" class="btn btn-default" ng-hide="conf.singlecheck||conf.endsinglecheck" ng-click="fnClearAll()">清空</button>\
                                    <button type="button" class="btn btn-default" ng-click="fnHide()">取消</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>';
        }
    });