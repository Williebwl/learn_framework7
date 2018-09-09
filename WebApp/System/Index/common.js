define(['jquery', 'bootstrap', 'core.state', 'scrollbar', 'jquery-ui'],
    function ($, bootstrap, coreState) {
        'use strict'

        /** 表单向导 start**/
        $(document).on("click", ".wizard .wizard-header li", function () {
            var $self = $(this);
            showWizard($self.closest(".wizard"), $self.index())
        }).on("click", ".wizard-footer .next", function (e) {
            if (!coreState.FormState) return;
            var $self = $(this).closest(".wizard");
            showWizard($self, $self.find(".wizard-pane.active").index() + 1)
        }).on("click", ".wizard-footer .previous:not(.disabled)", function (e) {
            var $self = $(this).closest(".wizard");
            showWizard($self, $self.find(".wizard-pane.active").index() - 1)
        })

        function showWizard(obj, i) {
            i > 0 ? obj.find(".previous").removeClass("disabled") :
                obj.find(".previous").addClass("disabled")
            if (i == obj.find(".wizard-header li").length - 1) {
                obj.find(".next").hide().siblings().removeClass("hide");
            } else {
                obj.find(".next").show().siblings().addClass("hide");
            }
            obj.find(".wizard-header li:eq(" + i + ")").addClass("active").siblings("li").removeClass("active");
            obj.find(".wizard-pane:eq(" + i + ")").addClass("active").siblings(".wizard-pane").removeClass('active');
        }
        /** 表单向导 end**/

        /** 选项卡 start**/
        $(document).on("click", ".tabs > .nav > li", function () {
            var $nav = $(this).addClass("active"),
                $tabs = $nav.closest('.tabs'),
                $pane = $tabs.children('.tab-content').children('.tab-pane:eq(' + $nav.index() + ')').addClass("active"),
                last = $tabs.data('last');

            last && (last.$nav.removeClass("active"), last.$pane.removeClass("active")) || ($nav.siblings("li.active:first").removeClass("active"), $pane.siblings(".tab-pane.active:first").removeClass("active")),

            $tabs.data('last', { $nav: $nav, $pane: $pane })
        })
        /** 选项卡 end**/
        /** 隐藏浮动层 start**/
        .on('click', function (e) {
            if (!$(e.target).closest(".dropdown-box").is('.dropdown-box') && e.target.parentNode) $('.dropdown-box').hide()
        })
        /** 隐藏浮动层 end**/

        /** 导航 start**/
        $(document).on('click', '.header-bar > .toggle-menu', function () {
            var w = $(window).width();
            if (w <= 800 && w > 480) medium()
            else if (w <= 480) small()
            else large.bind(this)()
        });

        function large() {
            var $nav = $('body > #nav'), $c = $nav.next(), w = $c.outerWidth(true);
            $('i', this).toggleClass('fa-caret-right', 'fa-caret-left')
            if (parseInt($nav.css('marginLeft')) < 0) {
                $nav.stop().animate({ 'marginLeft': 0 }, 200),
                $c.animate({ 'width': (w - 200) + 'px' }, 200, function () { $c.css('width', 'calc(100% - 270px)') })
            } else {
                $nav.stop().animate({ 'marginLeft': -200 }, 200)
                $c.animate({ 'width': w + 200 + 'px' }, 200, function () { $c.css('width', 'calc(100% - 70px)') })
            }
        }

        function medium() { }

        function small() { }
        /** 导航 end**/

        /** 滑动层 **/
        $(document).on('click', '[data-prop-class]', function () {
            var target = (target = $(this).attr('data-prop-class')) && $(target);
            target && target.each(function () {
                var $target = $(this), css = {}, align = $target.attr('data-prop-direction') || 'left';
                $target.stop().animate((css[align] = 0) || css, 200).data('prop-Val', $target.css(align))
            })
        }).on('click', '[data-close-slider]', function (e) {
            var target = $((target = $(this).attr('data-close-slider')) == 'self' ? this : target).closest('.prop-slider'), align = target.attr('data-prop-direction') || 'left', css = {};
            css[align] = target.data('prop-Val'),
            target.stop().animate(css, 200)
        });
        /** 滑动层 **/


        /** 左边栏滚动条 **/
        $(function () { CustomScrollbar.call($('.aside > .nav')) })
        /** 左边栏滚动条 **/

        return {
            ToolBarLoaded: function (s, e) {

            },
            NavLoaded: function (s, e) {
                /** 导航区滚动条 **/
                CustomScrollbar.call($('.subcontent', e.$element))
                /** 导航区滚动条 **/
            },
            ContainerLoaded: function (s, e) {

            },
            ContentLoaded: function (s, e) {
                $('[data-toggle="tooltip"]', e.$element).tooltip()
                //$('[data-toggle="popover"]', e.$element).popover()
            }
        };

        //滚动条设置
        function CustomScrollbar() {
            this.mCustomScrollbar({ theme: "minimal" });
        }
    });