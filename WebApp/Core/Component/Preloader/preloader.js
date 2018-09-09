define('preloader', ['jquery'],
    function ($) {
        'use strict';

        var isShow = 0, Preloader = {
            $preloader: $('#preloader'),
            i: 0,
            show: function (speed, fn) {
                if (!isShow) { isShow = 1; this.$preloader.show(speed, fn); }
                return {
                    i: ++Preloader.i, hide: function (speed, fn) {
                        if (this.i !== Preloader.i) return;
                        Preloader.hide(speed, fn);
                        isShow = 0;
                    }
                }
            },
            hide: function (speed, fn) {
                this.$preloader.delay(200).fadeOut(speed || "slow", fn);
            }
        };
        return Preloader;
    });