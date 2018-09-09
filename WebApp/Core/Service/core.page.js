(function ($, ng, define, require, requirejs) {
    'use strict'


    /**********************************************
    ********     页面内容区域核心组件      ********
    ***********************************************/
    define('core.container', ['core.page', 'ext'],
        function (page) {
            //function fnEdit($view, $service, $scope) {
            //    var page = this, pageConfig = $view.PageConfig;
            //    $view.fnPost = function () {
            //        /// <summary>发起添加操作</summary>

            //        $service.CallBack({ e: 0, info: {}, pageConfig: pageConfig });
            //    }

            //    $view.fnPut = function (info) {
            //        /// <summary>发起编辑操作</summary>
            //        /// <param name="info" type="json">需要编辑维护的数据对象</param>

            //        $service.CallBack({ e: 1, info: info });
            //    }

            //    $view.fnDelete = function (i, id) {
            //        /// <summary>发起列表数据删除操作</summary>
            //        /// <param name="i" type="Number">数据在列表中的索引</param>
            //        /// <param name="id" type="Number">数据唯一标示</param>

            //        var pageInfo = $view.PageInfo;
            //        if (arguments.length) {

            //            /*
            //               单条数据删除
            //            */

            //            page.confirm('确定删除？', function (p) {
            //                if (!p.s) return;

            //                //调用Web API 删除数据
            //                $service.fnDelete(id).success(function (d) {
            //                    if (pageInfo.Items.length === 1) {
            //                        pageConfig.currentPage = pageConfig.currentPage > 1 ? pageConfig.currentPage - 1 : pageConfig.currentPage;
            //                        pageConfig.onChange();
            //                    }
            //                    else pageInfo.Items.splice(i, 1);
            //                }).error(function () { page.errorNotice('数据删除失败！'); });
            //            })

            //        } else if ($view.CheckedInfos) {

            //            /*
            //              多条数据删除
            //            */

            //            var ids = ($view.CheckedInfos ? $view.CheckedInfos.select(function () { return this.key.ID || this.key }) : []).join(',');

            //            if (!ids.length) { page.alert('请选择需要删除的项！'); return false; }

            //            page.confirm('确定要删除选择的项？',
            //                function (p) {
            //                    if (!p.s) return;

            //                    //调用Web API 删除数据
            //                    $service.fnDelete(ids)
            //                            .success(function (d) {
            //                                ids = $view.CheckedInfos;
            //                                if ((pageConfig.numberOfPages > pageConfig.currentPage) || (pageConfig.numberOfPages === pageConfig.currentPage && pageInfo.Items.length === ids.length)) {
            //                                    pageConfig.currentPage = pageConfig.numberOfPages > pageConfig.currentPage ? pageConfig.currentPage : pageConfig.currentPage - 1;
            //                                    pageConfig.onChange();
            //                                }
            //                                else {
            //                                    ids.sort(function (a, b) { return a.$index - b.$index; });
            //                                    for (var i = ids.length - 1; i >= 0; i--) pageInfo.Items.splice(ids[i].$index, 1);
            //                                }
            //                                ids.length = 0;
            //                            }).error(function () { page.errorNotice('数据删除失败！'); });
            //                });
            //        }
            //    }

            //    $view.fnSequence = function () {
            //        if (!$view.View.CanSort) {
            //            page.alert('没有需要排序的项！');
            //            return;
            //        }

            //        var infos = ($view.PageConfig.Items || $view.View.Items).grepAll(function () { return this.SChanged; }, 1),
            //            data = infos.select(function () { return { ID: this.ID, Sequence: this.Sequence } });

            //        $service.fnSequence(data).success(function () {
            //            $view.View.CanSort = !1;
            //            infos.forEach(function (o) { o.SChanged = !1; });
            //        });
            //    }
            //}

            function ListPage($view, $service, $scope) {
                if (!this || this.constructor === Window) return new ListPage($view, $service, GetScope($view, $scope));

                this.super($view, $service, $scope), this.Type = 'core.container', fnSearch.apply(this, arguments)
            }

            return page.ext(ListPage)

            function fnSearch($view, $service, $scope) {
                var params = $view.Params = { i: 0 },
                    pageConfig = $view.PageConfig = {
                        pageSelect: 0,
                        onChange: function (i) {
                            /// <summary>调用查询</summary>
                            /// <param name="i" type="Number">查询标示</param>

                            //调用Service.fnGetAll方法查询数据
                            $service.fnGetPaged($view.GetSearchParams(pageConfig, params))
                                    .success(function (d) {
                                        if (angular.isNumber(i) && params.i !== i) return;

                                        $view.PageInfo = d;
                                    });
                        }
                    }

                $view.GetSearchParams = function (pageConfig, params) {
                    var val = (val = params.inputVal) && val != '输入关键字' ? val : '';

                    return { Key: val, PageIndex: pageConfig.currentPage, PageSize: pageConfig.itemsPerPage };
                }

                $view.fnSearch = function () {
                    /// <summary>搜索框自动调用查询</summary>

                    if (params.lastVal === params.inputVal) return;

                    if (params.lastSetTimeout) clearTimeout(params.lastSetTimeout);

                    params.lastSetTimeout = setTimeout(function (o) { pageConfig.onChange(++o.i) }, 600, params);

                    params.lastVal = params.inputVal;
                }

                $view.fnGetAll = this.fnRefreshSearch = pageConfig.onChange.bind(pageConfig)

                $scope.$on('$$RefreshSearch', this.fnRefreshSearch);
            }

        })

    /******************************************
    ********     编辑页面核心组件      ********
    *******************************************/
    define('core.edit', ['core.page', 'formValidate', 'ext'],
        function (page) {

            function EditPage($view, $service, $scope) {
                if (!this || this.constructor === Window) return new EditPage($view, $service, GetScope($view, $scope));

                var $self = this.super($view, $service, $scope);

                this.Type = 'core.edit',

                //保存前调用
                $self.fnSaveing = $self.fnViewRest = page.noop,
                //显示页面
                $self.fnShowView = fnShowView,
                //关闭页面
                $self.fnCloseView = fnCloseView,
                fnbindPage.call($scope.$element),
                //在上级页面中注册ShowDialog方法
                $scope.$parent.ShowDialog = function (editInfo) { $self.fnLoadPage(editInfo) },
                $self.fnLoadPage = function (editInfo) { $self.fnBindPage(editInfo) },//页面加载
                //绑定页面数据
                $self.fnBindPage = function (editInfo) {
                    $self.$valid = !1,//是否验证通过
                    $self.fnSetEditInfo(editInfo),//设置编辑信息
                    $self.fnViewRest(),//重置视图
                    $self.fnFormRest(),//表单重置
                    $self.fnLoadAttach($view.editInfo.ID),//加载附件资源
                    $self.fnShowView.call($scope, $view.editInfo)//显示页面
                },
                //设置页面数据
                $self.fnSetEditInfo = function (editInfo) {
                    $self.$editInfo = editInfo = editInfo || {},
                    $view.editInfo = $self.editInfo = ($view.IsEdit = $self.IsEdit = !!editInfo.ID) ? page.extend({}, editInfo) : editInfo
                },
                //保存
                $view.fnSave = function () {
                    $self.fnValidate() || $self.fnSaveing() !== false && ($self.IsEdit ? $service.fnPut : $service.fnPost).call($service, $self.PostInfo || $view.editInfo).success($self.fnSaveSuccess.bind($self)).error($self.fnSaveError.bind($self))
                },
                //保存成功时调用
                $self.fnSaveSuccess = function (d) {
                    d ? ($self.$editInfo.ID = $view.editInfo.ID = d) && $self.fnSaveOther.apply(this, arguments) : this.fnSaveError.apply(this, arguments)
                },
                //保存出错时调用
                $self.fnSaveError = function (d) {
                    this.errorNotice(d && d.Message || '保存失败！')
                },
                //保存关联信息（附件等）
                $self.fnSaveOther = function (d) {
                    this.fnSaveAttach(d, ($self.IsEdit ? $self.SaveUpdateRefresh : $self.SaveAddRefresh).bind($self), $self.fnSaveError.bind($self))
                },
                //添加成功
                $self.SaveAddRefresh = function (d) {
                    if (d) {
                        var back = { View: $self.editInfo, PostInfo: $self.PostInfo };
                        this.successNotice('保存成功！'),
                        $scope.$emit('$DataPostSuccess', back),
                        this.fnCloseView.call($scope, back)
                    } else this.fnSaveError.apply(this, arguments)
                },
                //修改成功
                $self.SaveUpdateRefresh = function (d) {
                    if (d) {
                        var back = { Source: $self.$editInfo, View: $self.editInfo, PostInfo: $self.PostInfo };
                        this.successNotice('保存成功！'),
                        $scope.$emit('$DataPutSuccess', back),
                        this.fnCloseView.call($scope, back)
                    } else this.fnSaveError.apply(this, arguments)
                }
            }

            return page.ext(EditPage)
        })



    //向页面添加公共方法
    function fnbindPage() {
        var $element = ($element = this.is('.modal') ? this : this.closest('.modal')).is('.modal') ? $element : this.is('.dropdown-box') ? this : this.closest('.dropdown-box');
        $('.close', this).on('click', function () { $element.is('.modal') && $element.modal('hide') || $element.is('.dropdown-box') && $element.hide() })
    }

    //显示页面
    function fnShowView(editInfo) {
        var $element = ($element = this.$element.is('.modal') ? this.$element : this.$element.closest('.modal')).is('.modal') ? $element : this.$element.is('.dropdown-box') ? this.$element : this.$element.closest('.dropdown-box');
        this.$emit('$ShowViewStart', editInfo),
        this.$broadcast('$ShowViewStart', editInfo),
        $element.is('.modal') && $element.modal('show') || $element.is('.dropdown-box') && $element.show(),
        this.$emit('$ShowViewSuccess', editInfo),
        this.$broadcast('$ShowViewSuccess', editInfo),
        $('.close', $element).on('click', function () { $element.is('.modal') && $element.modal('hide') || $element.is('.dropdown-box') && $element.hide() })
    }

    //关闭页面
    function fnCloseView(editInfo) {
        var $element = ($element = this.$element.is('.modal') ? this.$element : this.$element.closest('.modal')).is('.modal') ? $element : this.$element.is('.dropdown-box') ? this.$element : this.$element.closest('.dropdown-box');
        this.$emit('$DataSaveSuccess', editInfo),
        this.$emit('$CloseViewStart', editInfo),
        this.$broadcast('$CloseViewStart', editInfo),
        $element.is('.modal') && $element.modal('hide') || $element.is('.dropdown-box') && $element.hide(),
        this.$emit('$CloseViewSuccess', editInfo),
        this.$broadcast('$CloseViewSuccess', editInfo)
    }


    /******************************************
    ********     查看页面核心组件      ********
    *******************************************/
    define('core.view', ['core.page', 'ext'],
        function (page) {
            function ViewPage($view, $service, $scope, dialog) {
                if (!this || this.constructor === Window) return new ViewPage($view, $service, GetScope($view, $scope), dialog);

                this.super($view, $service, $scope);

                this.Type = 'core.view',

                $service.CallBack = function (editInfo) {
                    /// <summary>传入数据查看对象</summary>
                    /// <param name="editInfo" type="json">数据查看对象</param>

                    $view.editInfo = editInfo
                }
            }

            return page.ext(ViewPage)
        })

    /******************************************
    ********     导航区域核心组件      ********
    *******************************************/
    define('core.nav', ['core.page'],
    function (page) {
        function ViewPage($view, $service, $scope) {
            if (!this || this.constructor === Window) return new ViewPage($view, $service, GetScope($view, $scope));

            var self = this.super($view, $service, $scope);

            this.Type = 'core.nav',

            //搜索对象
            $view.Search = {
                //当前操作对象
                Active: null,
                LastActive: null
            },
            $view.fnSelected = $view.fnSearchChange = function (active) {
                this.LastActive = this.Active, this.Active = active,
                self.$rootScope.$broadcast('$$RefreshSearch', { target: self.Type, Search: this })
            }.bind($view.Search)
        }

        return page.ext(ViewPage)
    })

    /******************************************
    ********     工具栏核心组件      **********
    *******************************************/
    define('core.toolbar', ['core.page'],
    function (page) {
        function ViewPage($view, $service, $scope) {
            if (!this || this.constructor === Window) return new ViewPage($view, $service, GetScope($view, $scope));

            this.super($view, $service, $scope),

            this.Type = 'core.toolbar',

            $scope.fnAdd = function () {
                $scope.$emit('$$ToolBarAdd')
            },
            $scope.fnDelete = function () {
                $scope.$emit('$$ToolBarDelete')
            }
        }

        return page.ext(ViewPage)
    })

    /**************************************
    ********     页面核心组件      ********
    ***************************************/
    define('core.page', ['core', 'lobibox', 'core.state'],
        function (core, lobibox, coreState) {
            function Page($view, $service, $scope) {
                if (!this || this.constructor === Window) return new Page($view, $service, GetScope($view, $scope));

                //拓展继承
                this.super($view, $service, $scope),

                //设置类型
                this.Type = 'core.page',

                //页面状态
                this.PageState = coreState,

                this.$rootScope = core.$injector.get('$rootScope'),

                //页面表单状态
                coreState.FormState = !0,

                //视图信息
                $view.View = {},

                //当前活动信息
                $view.$Active = {},

                //添加
                $view.fnAdd = function (data) {
                    $scope.$emit('$$Add', { target: self.Type, data: data })
                },

                //修改
                $view.fnDelete = function (data) {
                    $scope.$emit('$$Delete', { target: self.Type, data: data })
                },

                //活动对象变化
                $view.fnActiveChange = function (data) {
                    $scope.$emit('$$ActiveChange', { target: self.Type, data: data })
                },

                //保存附件
                this.fnSaveAttach = function (id, success, error) {
                    var p = { ID: id, success: success, error: error, sumNumber: 0, sucNumber: 0, errNumber: 0 };
                    $scope.$emit('$$SaveAttach', p), !p.sumNumber && core.isFunction(p.success) && p.success(id)
                }

                //加载附件
                this.fnLoadAttach = function (id, success, error) {
                    var p = { ID: id, success: success, error: error, sumNumber: 0, sucNumber: 0, errNumber: 0 };
                    $scope.$emit('$$LoadAttach', p), !p.sumNumber && core.isFunction(p.success) && p.success(id)
                },

                //验证表单
                this.fnValidate = function (mark) {
                    var s = { IsValid: 1, errorNotice: this.errorNotice.bind(this), mark: mark };
                    return $scope.$emit('$VMValidate', s), !(coreState.FormState = s.IsValid)
                }

                //表单重置
                this.fnFormRest = function () { $scope.$emit('$FormRest', $view.editInfo) }

                //消息提醒
                var msgs = this.msg = {
                    base: lobibox,
                    //通知
                    notice: function (msg, mode, callback, type) {
                        $scope.$emit(type || '$$notice', Page.isObject(msg) ? msg : { alert: msg, mode: mode || 0, callback: callback });
                    },
                    //提醒
                    alert: function (msg, callback, type) {
                        this.base.alert(type || 'info', Page.isObject(msg) ? msg : {
                            msg: msg,
                            callback: function ($this, type, ev) {
                                if (Page.isFunction(callback)) callback.call($this, { s: type === 'yes', type: type, event: ev });
                            }
                        })
                    },
                    //确认
                    confirm: function (msg, callback, title) {
                        this.base.confirm(Page.isObject(msg) ? msg : {
                            title: title || '确认信息',
                            msg: msg,
                            callback: function ($this, type, ev) {
                                if (Page.isFunction(callback)) callback.call($this, { s: type === 'yes', type: type, event: ev });
                            }
                        });
                    },
                    //消息
                    message: function (msg, mode, title, type) {
                        $scope.$emit(type || '$$msg', Page.isObject(msg) ? msg : { msg: msg, mode: mode || 0, title: title || !0 });
                    }
                };

                //通知
                this.notice = msgs.notice.bind(msgs),

                //提醒
                this.alert = msgs.alert.bind(msgs),

                //确认
                this.confirm = msgs.confirm.bind(msgs),

                //消息
                this.message = msgs.message.bind(msgs),

                //错误通知
                this.errorNotice = function (msg, callback) {
                    this.msg.notice(msg, 2, callback, '$editNotice');
                },

                //成功通知
                this.successNotice = function (msg, callback) {
                    this.msg.notice(msg, 3, callback, '$editNotice');
                }
            }

            return core.ext(Page)
        })

    function GetScope($view, $scope) {
        return $scope || $view.$id ? $view : null;
    }

}(window.jQuery, window.angular, window.define, window.require, window.requirejs))