﻿define('lobibox', ['css!Assets/Css/Lobibox.min.css'], function () {
    var Lobibox = Lobibox || {};
    !function () {
        function LobiboxPrompt(type, options) {
            this.$input = null,
            this.$type = "prompt",
            this.$promptType = type,
            options = $.extend({}, Lobibox.prompt.DEFAULT_OPTIONS, options),
            this.$options = this._processInput(options),
            this._init(),
            this.debug(this)
        }
        function LobiboxConfirm(options) {
            this.$type = "confirm",
            this.$options = this._processInput(options),
            this._init(),
            this.debug(this)
        }
        function LobiboxAlert(type, options) {
            this.$type = type,
            this.$options = this._processInput(options),
            this._init(),
            this.debug(this)
        }
        function LobiboxProgress(options) {
            this.$type = "progress",
            this.$progressBarElement = null,
            this.$options = this._processInput(options),
            this.$progress = 0,
            this._init(),
            this.debug(this)
        }
        function LobiboxWindow(type, options) {
            this.$type = type,
            this.$options = this._processInput(options),
            this._init(),
            this.debug(this)
        }
        Lobibox.prompt = function (type, options) {
            return new LobiboxPrompt(type, options)
        }
        ,
        Lobibox.confirm = function (options) {
            return new LobiboxConfirm(options)
        }
        ,
        Lobibox.progress = function (options) {
            return new LobiboxProgress(options)
        }
        ,
        Lobibox.error = {},
        Lobibox.success = {},
        Lobibox.warning = {},
        Lobibox.info = {},
        Lobibox.alert = function (type, options) {
            return ["success", "error", "warning", "info"].indexOf(type) > -1 ? new LobiboxAlert(type, options) : void 0
        }
        ,
        Lobibox.window = function (options) {
            return new LobiboxWindow("window", options)
        }
        ;
        var LobiboxBase = {
            $type: null,
            $el: null,
            $options: null,
            debug: function () {
                this.$options.debug && window.console.debug.apply(window.console, arguments)
            },
            _processInput: function (options) {
                if ($.isArray(options.buttons)) {
                    for (var btns = {}, i = 0; i < options.buttons.length; i++) {
                        var btn = Lobibox.base.OPTIONS.buttons[options.buttons[i]];
                        btns[options.buttons[i]] = btn
                    }
                    options.buttons = btns
                }
                options.customBtnClass = options.customBtnClass ? options.customBtnClass : Lobibox.base.DEFAULTS.customBtnClass;
                for (var i in options.buttons) {
                    var btn = options.buttons[i];
                    options.buttons.hasOwnProperty(i) && (btn = $.extend({}, Lobibox.base.OPTIONS.buttons[i], btn),
                    btn["class"] || (btn["class"] = options.customBtnClass)),
                    options.buttons[i] = btn
                }
                return options = $.extend({}, Lobibox.base.DEFAULTS, options),
                void 0 === options.showClass && (options.showClass = Lobibox.base.OPTIONS.showClass),
                void 0 === options.hideClass && (options.hideClass = Lobibox.base.OPTIONS.hideClass),
                void 0 === options.baseClass && (options.baseClass = Lobibox.base.OPTIONS.baseClass),
                void 0 === options.delayToRemove && (options.delayToRemove = Lobibox.base.OPTIONS.delayToRemove),
                options
            },
            _init: function () {
                var me = this;
                me._createMarkup(),
                me.setTitle(me.$options.title),
                me.$options.draggable && !me._isMobileScreen() && (me.$el.addClass("draggable"),
                me._enableDrag()),
                me.$options.closeButton && me._addCloseButton(),
                me.$options.closeOnEsc && $(document).on("keyup.lobibox", function (ev) {
                    27 === ev.which && me.destroy()
                }),
                me.$options.baseClass && me.$el.addClass(me.$options.baseClass),
                me.$options.showClass && (me.$el.removeClass(me.$options.hideClass),
                me.$el.addClass(me.$options.showClass)),
                me.$el.data("lobibox", me)
            },
            _calculatePosition: function (position) {
                var top, me = this;
                top = "top" === position ? 30 : "bottom" === position ? $(window).outerHeight() - me.$el.outerHeight() - 30 : ($(window).outerHeight() - me.$el.outerHeight()) / 2;
                var left = ($(window).outerWidth() - me.$el.outerWidth()) / 2;
                return {
                    left: left,
                    top: top
                }
            },
            _createButton: function (type, op) {
                var me = this
                  , btn = $("<button></button>").addClass(op["class"]).attr("data-type", type).html(op.text);
                return me.$options.callback && "function" == typeof me.$options.callback && btn.on("click.lobibox", function (ev) {
                    var bt = $(this);
                    me.$options.buttons[type] && me.$options.buttons[type].closeOnClick && me.destroy(),
                    me.$options.callback(me, bt.data("type"), ev)
                }),
                btn.click(function () {
                    me.$options.buttons[type] && me.$options.buttons[type].closeOnClick && me.destroy()
                }),
                btn
            },
            _generateButtons: function () {
                var me = this
                  , btns = [];
                for (var i in me.$options.buttons)
                    if (me.$options.buttons.hasOwnProperty(i)) {
                        var op = me.$options.buttons[i]
                          , btn = me._createButton(i, op);
                        btns.push(btn)
                    }
                return btns
            },
            _createMarkup: function () {
                var me = this
                  , lobibox = $('<div class="lobibox"></div>');
                lobibox.attr("data-is-modal", me.$options.modal);
                var header = $('<div class="lobibox-header"></div>').append('<span class="lobibox-title"></span>')
                  , body = $('<div class="lobibox-body"></div>');
                if (lobibox.append(header),
                lobibox.append(body),
                me.$options.buttons && !$.isEmptyObject(me.$options.buttons)) {
                    var footer = $('<div class="lobibox-footer"></div>');
                    footer.append(me._generateButtons()),
                    lobibox.append(footer),
                    Lobibox.base.OPTIONS.buttonsAlign.indexOf(me.$options.buttonsAlign) > -1 && footer.addClass("text-" + me.$options.buttonsAlign)
                }
                me.$el = lobibox.addClass(Lobibox.base.OPTIONS.modalClasses[me.$type])
            },
            _setSize: function () {
                var me = this;
                me.setWidth(me.$options.width),
                me.setHeight("auto" === me.$options.height ? me.$el.outerHeight() : me.$options.height)
            },
            _calculateBodyHeight: function (height) {
                var me = this
                  , headerHeight = me.$el.find(".lobibox-header").outerHeight()
                  , footerHeight = me.$el.find(".lobibox-footer").outerHeight();
                return height - (headerHeight ? headerHeight : 0) - (footerHeight ? footerHeight : 0)
            },
            _addBackdrop: function () {
                0 === $(".lobibox-backdrop").length && $("body").append('<div class="lobibox-backdrop"></div>')
            },
            _triggerEvent: function (type) {
                var me = this;
                me.$options[type] && "function" == typeof me.$options[type] && me.$options[type](me)
            },
            _calculateWidth: function (width) {
                var me = this;
                return width = Math.min($(window).outerWidth(), width),
                width === $(window).outerWidth() && (width -= 2 * me.$options.horizontalOffset),
                width
            },
            _calculateHeight: function (height) {
                return Math.min($(window).outerHeight(), height)
            },
            _addCloseButton: function () {
                var me = this
                  , closeBtn = $('<span class="btn-close">&times;</span>');
                me.$el.find(".lobibox-header").append(closeBtn),
                closeBtn.on("mousedown", function (ev) {
                    ev.stopPropagation()
                }),
                closeBtn.on("click.lobibox", function () {
                    me.destroy()
                })
            },
            _position: function () {
                var me = this;
                me._setSize();
                var pos = me._calculatePosition();
                me.setPosition(pos.left, pos.top)
            },
            _isMobileScreen: function () {
                return $(window).outerWidth() < 768 ? !0 : !1
            },
            _enableDrag: function () {
                var el = this.$el
                  , heading = el.find(".lobibox-header");
                heading.on("mousedown.lobibox", function (ev) {
                    el.attr("offset-left", ev.offsetX),
                    el.attr("offset-top", ev.offsetY),
                    el.attr("allow-drag", "true")
                }),
                $(document).on("mouseup.lobibox", function () {
                    el.attr("allow-drag", "false")
                }),
                $(document).on("mousemove.lobibox", function (ev) {
                    if ("true" === el.attr("allow-drag")) {
                        var left = ev.clientX - parseInt(el.attr("offset-left"), 10) - parseInt(el.css("border-left-width"), 10)
                          , top = ev.clientY - parseInt(el.attr("offset-top"), 10) - parseInt(el.css("border-top-width"), 10);
                        el.css({
                            left: left,
                            top: top
                        })
                    }
                })
            },
            _setContent: function (msg) {
                var me = this;
                return me.$el.find(".lobibox-body").html(msg),
                me
            },
            hide: function () {
                function callback() {
                    me.$el.addClass("lobibox-hidden"),
                    0 === $(".lobibox[data-is-modal=true]:not(.lobibox-hidden)").length && ($(".lobibox-backdrop").remove(),
                    $("body").removeClass(Lobibox.base.OPTIONS.bodyClass))
                }
                var me = this;
                return me.$options.hideClass ? (me.$el.removeClass(me.$options.showClass),
                me.$el.addClass(me.$options.hideClass),
                setTimeout(function () {
                    callback()
                }, me.$options.delayToRemove)) : callback(),
                this
            },
            destroy: function () {
                function callback() {
                    me.$el.remove(),
                    0 === $(".lobibox[data-is-modal=true]").length && ($(".lobibox-backdrop").remove(),
                    $("body").removeClass(Lobibox.base.OPTIONS.bodyClass)),
                    me._triggerEvent("closed")
                }
                var me = this;
                return me._triggerEvent("beforeClose"),
                me.$options.hideClass ? (me.$el.removeClass(me.$options.showClass),
                me.$el.addClass(me.$options.hideClass),
                setTimeout(function () {
                    callback()
                }, me.$options.delayToRemove)) : callback(),
                this
            },
            setWidth: function (width) {
                return width = this._calculateWidth(width),
                this.$el.css("width", width),
                this
            },
            setHeight: function (height) {
                var me = this;
                height = me._calculateHeight(height),
                me.$el.css("height", height);
                var bHeight = me._calculateBodyHeight(me.$el.innerHeight());
                return me.$el.find(".lobibox-body").css("height", bHeight),
                me
            },
            setSize: function (width, height) {
                var me = this;
                return me.setWidth(width),
                me.setHeight(height),
                me
            },
            setPosition: function (left, top) {
                var position, me = this;
                return "number" == typeof left && "number" == typeof top ? position = {
                    left: left,
                    top: top
                } : "string" == typeof left && (position = me._calculatePosition(left)),
                me.$el.css(position),
                me
            },
            setTitle: function (title) {
                var me = this;
                return me.$el.find(".lobibox-title").html(title),
                me
            },
            getTitle: function () {
                var me = this;
                return me.$el.find(".lobibox-title").html()
            },
            show: function () {
                var me = this;
                return me._triggerEvent("onShow"),
                me.$el.removeClass("lobibox-hidden"),
                $("body").append(me.$el),
                me.$options.modal && ($("body").addClass(Lobibox.base.OPTIONS.bodyClass),
                me._addBackdrop()),
                me._triggerEvent("shown"),
                me
            }
        };
        Lobibox.base = {},
        Lobibox.base.OPTIONS = {
            bodyClass: "lobibox-open",
            modalClasses: {
                error: "lobibox-error",
                success: "lobibox-success",
                info: "lobibox-info",
                warning: "lobibox-warning",
                confirm: "lobibox-confirm",
                progress: "lobibox-progress",
                prompt: "lobibox-prompt",
                "default": "lobibox-default",
                window: "lobibox-window"
            },
            buttonsAlign: ["left", "center", "right"],
            buttons: {
                ok: {
                    "class": "lobibox-btn lobibox-btn-default",
                    text: "确定",
                    closeOnClick: !0
                },
                cancel: {
                    "class": "lobibox-btn lobibox-btn-cancel",
                    text: "取消",
                    closeOnClick: !0
                },
                yes: {
                    "class": "lobibox-btn lobibox-btn-yes",
                    text: "确定",
                    closeOnClick: !0
                },
                no: {
                    "class": "lobibox-btn lobibox-btn-no",
                    text: "取消",
                    closeOnClick: !0
                }
            }
        },
        Lobibox.base.DEFAULTS = {
            horizontalOffset: 5,
            width: 600,
            height: "auto",
            closeButton: !0,
            draggable: !1,
            customBtnClass: "lobibox-btn lobibox-btn-default",
            modal: !0,
            debug: !1,
            buttonsAlign: "center",
            closeOnEsc: !0,
            delayToRemove: 200,
            baseClass: "animated-super-fast",
            showClass: "zoomIn",
            hideClass: "zoomOut",
            onShow: null,
            shown: null,
            beforeClose: null,
            closed: null
        },
        LobiboxPrompt.prototype = $.extend({}, LobiboxBase, {
            constructor: LobiboxPrompt,
            _processInput: function (options) {
                var me = this
                  , mergedOptions = LobiboxBase._processInput.call(me, options);
                return mergedOptions.buttons = {
                    ok: Lobibox.base.OPTIONS.buttons.ok,
                    cancel: Lobibox.base.OPTIONS.buttons.cancel
                },
                options = $.extend({}, mergedOptions, LobiboxPrompt.DEFAULT_OPTIONS, options)
            },
            _init: function () {
                var me = this;
                LobiboxBase._init.call(me),
                me.show(),
                me._setContent(me._createInput()),
                me._position(),
                me.$input.focus()
            },
            _createInput: function () {
                var label, me = this;
                me.$options.multiline ? (me.$input = $("<textarea></textarea>"),
                me.$input.attr("rows", me.$options.lines)) : me.$input = $('<input type="' + me.$promptType + '"/>'),
                me.$input.addClass("lobibox-input"),
                me.$input.attr(me.$options.attrs),
                me.$options.value && me.setValue(me.$options.value),
                me.$options.label && (label = $("<label>" + me.$options.label + "</label>"));
                var innerHTML = $("<div></div>").append(label, me.$input);
                return innerHTML
            },
            setValue: function (val) {
                return this.$input.val(val),
                this
            },
            getValue: function () {
                return this.$input.val()
            }
        }),
        LobiboxPrompt.DEFAULT_OPTIONS = {
            width: 400,
            attrs: {},
            value: "",
            multiline: !1,
            lines: 3,
            type: "text",
            label: ""
        },
        LobiboxConfirm.prototype = $.extend({}, LobiboxBase, {
            constructor: LobiboxConfirm,
            _processInput: function (options) {
                var me = this
                  , mergedOptions = LobiboxBase._processInput.call(me, options);
                return mergedOptions.buttons = {
                    yes: Lobibox.base.OPTIONS.buttons.yes,
                    no: Lobibox.base.OPTIONS.buttons.no
                },
                options = $.extend({}, mergedOptions, Lobibox.confirm.DEFAULTS, options)
            },
            _init: function () {
                var me = this;
                LobiboxBase._init.call(me),
                me.show();
                var d = $("<div></div>");
                me.$options.iconClass && d.append($('<div class="lobibox-icon-wrapper"></div>').append('<i class="lobibox-icon ' + me.$options.iconClass + '"></i>')),
                d.append('<div class="lobibox-body-text-wrapper"><span class="lobibox-body-text">' + me.$options.msg + "</span></div>"),
                me._setContent(d.html()),
                me._position()
            }
        }),
        Lobibox.confirm.DEFAULTS = {
            title: "操作确认",
            width: 500,
            iconClass: "glyphicon glyphicon-question-sign"
        },
        LobiboxAlert.prototype = $.extend({}, LobiboxBase, {
            constructor: LobiboxAlert,
            _processInput: function (options) {
                var me = this
                  , mergedOptions = LobiboxBase._processInput.call(me, options);
                return mergedOptions.buttons = {
                    ok: Lobibox.base.OPTIONS.buttons.ok
                },
                options = $.extend({}, mergedOptions, Lobibox.alert.OPTIONS[me.$type], Lobibox.alert.DEFAULTS, options)
            },
            _init: function () {
                var me = this;
                LobiboxBase._init.call(me),
                me.show();
                var d = $("<div></div>");
                me.$options.iconClass && d.append($('<div class="lobibox-icon-wrapper"></div>').append('<i class="lobibox-icon ' + me.$options.iconClass + '"></i>')),
                d.append('<div class="lobibox-body-text-wrapper"><span class="lobibox-body-text">' + me.$options.msg + "</span></div>"),
                me._setContent(d.html()),
                me._position()
            }
        }),
        Lobibox.alert.OPTIONS = {
            warning: {
                title: "警告",
                iconClass: "glyphicon glyphicon-question-sign"
            },
            info: {
                title: "信息",
                iconClass: "glyphicon glyphicon-info-sign"
            },
            success: {
                title: "成功",
                iconClass: "glyphicon glyphicon-ok-sign"
            },
            error: {
                title: "错误",
                iconClass: "glyphicon glyphicon-remove-sign"
            }
        },
        Lobibox.alert.DEFAULTS = {},
        LobiboxProgress.prototype = $.extend({}, LobiboxBase, {
            constructor: LobiboxProgress,
            _processInput: function (options) {
                var me = this
                  , mergedOptions = LobiboxBase._processInput.call(me, options);
                return options = $.extend({}, mergedOptions, Lobibox.progress.DEFAULTS, options)
            },
            _init: function () {
                var me = this;
                LobiboxBase._init.call(me),
                me.show(),
                me.$progressBarElement = me.$options.progressTpl ? $(me.$options.progressTpl) : me._createProgressbar();
                var label;
                me.$options.label && (label = $("<label>" + me.$options.label + "</label>"));
                var innerHTML = $("<div></div>").append(label, me.$progressBarElement);
                me._setContent(innerHTML),
                me._position()
            },
            _createProgressbar: function () {
                var me = this
                  , outer = $('<div class="lobibox-progress-bar-wrapper lobibox-progress-outer"></div>').append('<div class="lobibox-progress-bar lobibox-progress-element"></div>');
                return me.$options.showProgressLabel && outer.append('<span class="lobibox-progress-text" data-role="progress-text"></span>'),
                outer
            },
            setProgress: function (progress) {
                var me = this;
                if (100 !== me.$progress)
                    return progress = Math.min(100, Math.max(0, progress)),
                    me.$progress = progress,
                    me._triggerEvent("progressUpdated"),
                    100 === me.$progress && me._triggerEvent("progressCompleted"),
                    me.$el.find(".lobibox-progress-element").css("width", progress.toFixed(1) + "%"),
                    me.$el.find('[data-role="progress-text"]').html(progress.toFixed(1) + "%"),
                    me
            },
            getProgress: function () {
                return this.$progress
            }
        }),
        Lobibox.progress.DEFAULTS = {
            width: 500,
            showProgressLabel: !0,
            label: "",
            progressTpl: !1,
            progressUpdated: null,
            progressCompleted: null
        },
        LobiboxWindow.prototype = $.extend({}, LobiboxBase, {
            constructor: LobiboxWindow,
            _processInput: function (options) {
                var me = this
                  , mergedOptions = LobiboxBase._processInput.call(me, options);
                return options.content && "function" == typeof options.content && (options.content = options.content()),
                options.content instanceof jQuery && (options.content = options.content.clone()),
                options = $.extend({}, mergedOptions, Lobibox.window.DEFAULTS, options)
            },
            _init: function () {
                var me = this;
                LobiboxBase._init.call(me),
                me.setContent(me.$options.content),
                me.$options.url && me.$options.autoload ? (me.$options.showAfterLoad || (me.show(),
                me._position()),
                me.load(function () {
                    me.$options.showAfterLoad && (me.show(),
                    me._position())
                })) : (me.show(),
                me._position())
            },
            setParams: function (params) {
                var me = this;
                return me.$options.params = params,
                me
            },
            getParams: function () {
                var me = this;
                return me.$options.params
            },
            setLoadMethod: function (method) {
                var me = this;
                return me.$options.loadMethod = method,
                me
            },
            getLoadMethod: function () {
                var me = this;
                return me.$options.loadMethod
            },
            setContent: function (content) {
                var me = this;
                return me.$options.content = content,
                me.$el.find(".lobibox-body").html("").append(content),
                me
            },
            getContent: function () {
                var me = this;
                return me.$options.content
            },
            setUrl: function (url) {
                return this.$options.url = url,
                this
            },
            getUrl: function () {
                return this.$options.url
            },
            load: function (callback) {
                var me = this;
                return me.$options.url ? ($.ajax(me.$options.url, {
                    method: me.$options.loadMethod,
                    data: me.$options.params
                }).done(function (res) {
                    me.setContent(res),
                    callback && "function" == typeof callback && callback(res)
                }),
                me) : me
            }
        }),
        Lobibox.window.DEFAULTS = {
            width: 480,
            height: 600,
            content: "",
            url: "",
            draggable: !0,
            autoload: !0,
            loadMethod: "GET",
            showAfterLoad: !0,
            params: {}
        }
    }(),
    Math.randomString = function (n) {
        for (var text = "", possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", i = 0; n > i; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        return text
    }
    ;
    var Lobibox = Lobibox || {};
    !function () {
        var LobiboxNotify = function (type, options) {
            this.$type,
            this.$options,
            this.$el,
            this.$sound;
            var me = this
              , _processInput = function (options) {
                  return ("mini" === options.size || "large" === options.size) && (options.width = options.width || Lobibox.notify.OPTIONS[options.size].width),
                  options = $.extend({}, Lobibox.notify.OPTIONS[me.$type], Lobibox.notify.DEFAULTS, options),
                  "mini" !== options.size && options.title === !0 ? options.title = Lobibox.notify.OPTIONS[me.$type].title : "mini" === options.size && options.title === !0 && (options.title = !1),
                  options.icon === !0 && (options.icon = Lobibox.notify.OPTIONS[me.$type].icon),
                  options.sound === !0 && (options.sound = Lobibox.notify.OPTIONS[me.$type].sound),
                  options.sound && (options.sound = options.soundPath + options.sound + options.soundExt),
                  options
              }
              , _init = function () {
                  var notify = _createNotify()
                    , wrapper = _createNotifyWrapper();
                  if (_appendInWrapper(notify, wrapper),
                  me.$el = notify,
                  me.$options.sound) {
                      var snd = new Audio(me.$options.sound);
                      snd.play()
                  }
                  me.$el.data("lobibox", me)
              }
              , _appendInWrapper = function ($el, $wrapper) {
                  if ("normal" === me.$options.size)
                      $wrapper.append($el);
                  else if ("mini" === me.$options.size)
                      $el.addClass("notify-mini"),
                      $wrapper.append($el);
                  else if ("large" === me.$options.size) {
                      var tabPane = _createTabPane();
                      tabPane.append($el);
                      var tabControl = _createTabControl(tabPane.attr("id"));
                      $wrapper.find(".tab-content").append(tabPane),
                      $wrapper.find(".nav-tabs").append(tabControl),
                      tabControl.find(">a").tab("show")
                  }
              }
              , _createTabControl = function (tabPaneId) {
                  var $li = $("<li></li>");
                  return $('<a href="#' + tabPaneId + '"></a>').attr("data-toggle", "tab").attr("role", "tab").append('<i class="tab-control-icon ' + me.$options.icon + '"></i>').appendTo($li),
                  $li.addClass(Lobibox.notify.OPTIONS[me.$type]["class"]),
                  $li
              }
              , _createTabPane = function () {
                  var $pane = $("<div></div>").addClass("tab-pane").attr("id", Math.randomString(10));
                  return $pane
              }
              , _createNotifyWrapper = function () {
                  var selector;
                  selector = "large" === me.$options.size ? ".lobibox-notify-wrapper-large" : ".lobibox-notify-wrapper";
                  var classes = me.$options.position.split(" ");
                  selector += "." + classes.join(".");
                  var wrapper = $(selector);
                  return 0 === wrapper.length && (wrapper = $("<div></div>").addClass(selector.replace(/\./g, " ").trim()).appendTo($("body")),
                  "large" === me.$options.size && wrapper.append($('<ul class="nav nav-tabs"></ul>')).append($('<div class="tab-content"></div>'))),
                  wrapper
              }
              , _createNotify = function () {
                  var notify = $('<div class="lobibox-notify"></div>').addClass(Lobibox.notify.OPTIONS[me.$type]["class"]).addClass(Lobibox.notify.OPTIONS["class"]).addClass(me.$options.showClass)
                    , iconWrapper = $('<div class="lobibox-notify-icon"></div>').appendTo(notify);
                  if (me.$options.img) {
                      var img = iconWrapper.append('<img src="' + me.$options.img + '"/>');
                      iconWrapper.append(img)
                  } else if (me.$options.icon) {
                      var icon = iconWrapper.append('<i class="' + me.$options.icon + '"></i>');
                      iconWrapper.append(icon)
                  } else
                      notify.addClass("without-icon");
                  var $body = $("<div></div>").addClass("lobibox-notify-body").append('<div class="lobibox-notify-msg">' + me.$options.msg + "</div>").appendTo(notify);
                  return me.$options.title && $body.prepend('<div class="lobibox-notify-title">' + me.$options.title + "<div>"),
                  _addCloseButton(notify),
                  ("normal" === me.$options.size || "mini" === me.$options.size) && (_addCloseOnClick(notify),
                  _addDelay(notify)),
                  me.$options.width && notify.css("width", _calculateWidth(me.$options.width)),
                  notify
              }
              , _addCloseButton = function ($el) {
                  if (me.$options.closable) {
                      var close = $('<span class="lobibox-close">&times;</span>');
                      $el.append(close),
                      close.click(function () {
                          me.remove()
                      })
                  }
              }
              , _addCloseOnClick = function ($el) {
                  me.$options.closeOnClick && $el.click(function () {
                      me.remove()
                  })
              }
              , _addDelay = function ($el) {
                  if (me.$options.delay) {
                      if (me.$options.delayIndicator) {
                          var delay = $('<div class="lobibox-delay-indicator"><div></div></div>');
                          $el.append(delay)
                      }
                      var time = 0
                        , interval = 1e3 / 30
                        , timer = setInterval(function () {
                            time += interval;
                            var width = 100 * time / me.$options.delay;
                            width >= 100 && (width = 100,
                            me.remove(),
                            timer = clearInterval(timer)),
                            me.$options.delayIndicator && delay.find("div").css("width", width + "%")
                        }, interval)
                  }
              }
              , _findTabToActivate = function ($li) {
                  var $itemToActivate = $li.prev();
                  return 0 === $itemToActivate.length && ($itemToActivate = $li.next()),
                  0 === $itemToActivate.length ? null : $itemToActivate.find(">a")
              }
              , _calculateWidth = function (width) {
                  return width = Math.min($(window).outerWidth(), width)
              }
            ;
            this.remove = function () {
                me.$el.removeClass(me.$options.showClass).addClass(me.$options.hideClass);
                var parent = me.$el.parent()
                  , wrapper = parent.closest(".lobibox-notify-wrapper-large")
                  , href = "#" + parent.attr("id")
                  , $li = wrapper.find('>.nav-tabs>li:has(a[href="' + href + '"])');
                return $li.addClass(Lobibox.notify.OPTIONS["class"]).addClass(me.$options.hideClass),
                setTimeout(function () {
                    if ("normal" === me.$options.size || "mini" === me.$options.size)
                        me.$el.remove();
                    else if ("large" === me.$options.size) {
                        var $itemToActivate = _findTabToActivate($li);
                        $itemToActivate && $itemToActivate.tab("show"),
                        $li.remove(),
                        parent.remove()
                    }
                }, 500),
                me
            }
            ,
            this.$type = type,
            this.$options = _processInput(options),
            _init()
        }
        ;
        Lobibox.notify = function (type, options) {
            return ["info", "warning", "error", "success"].indexOf(type) > -1 ? new LobiboxNotify(type, options) : void 0
        }
        ,
        Lobibox.notify.DEFAULTS = {
            title: !0,
            size: "normal",
            soundPath: "Assets/sounds/",
            soundExt: ".ogg",
            showClass: "zoomIn",
            hideClass: "zoomOut",
            icon: !0,
            msg: "",
            img: null,
            closable: !0,
            delay: 5e3,
            delayIndicator: !0,
            closeOnClick: !0,
            width: 400,
            sound: !0,
            position: "bottom right"
        },
        Lobibox.notify.OPTIONS = {
            "class": "animated-fast",
            large: {
                width: 500
            },
            mini: {
                "class": "notify-mini"
            },
            success: {
                "class": "lobibox-notify-success",
                title: "成功",
                icon: "glyphicon glyphicon-ok-sign",
                sound: "sound2"
            },
            error: {
                "class": "lobibox-notify-error",
                title: "错误",
                icon: "glyphicon glyphicon-remove-sign",
                sound: "sound4"
            },
            warning: {
                "class": "lobibox-notify-warning",
                title: "警告",
                icon: "glyphicon glyphicon-exclamation-sign",
                sound: "sound5"
            },
            info: {
                "class": "lobibox-notify-info",
                title: "信息",
                icon: "glyphicon glyphicon-info-sign",
                sound: "sound6"
            }
        }
    }();
    return Lobibox;
});
