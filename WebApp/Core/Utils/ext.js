/*
  定义一个require模块，模块名称为ext
  该模块依赖jquery

  （require资料参考地址：http://requirejs.cn/）

  日期：2015-08-20
*/
define('ext', ['jquery'],
    function ($) {
        'use strict';

        //从数组中删除指定值
        //v:需要删除的对象
        Array.prototype.remove = function (v) {
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (o != v) continue;
                this.splice(i, 1);
                return o;
            }
        };

        //从数组中删除所有指定值
        //v:需要删除的对象
        Array.prototype.removeAll = function (v) {
            var os = [];
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (o != v) continue;
                this.splice(i, 1);
                os.push(o);
            }
            return os;
        };

        //从数组中删除值
        //callback:回调函数
        //inv:对比对象或对比模式
        Array.prototype.removeGrep = function (callback, inv) {
            if (!$.isFunction(callback)) return;
            if (inv == undefined) inv == true;
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (callback.call(o) != inv) continue;
                this.splice(i, 1);
                return o;
            }
        };

        //从数组中删除值
        //callback:回调函数
        //inv:对比对象或对比模式
        Array.prototype.removeGrepAll = function (callback, inv) {
            if (!$.isFunction(callback)) return;
            if (inv == undefined) inv == true;
            var os = [];
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (callback.call(o) != inv) continue;
                this.splice(i, 1);
                os.push(o);
            }
            return os;
        };

        //确定元素是否在数组中
        //callback:回调函数
        //inv:对比对象或对比模式
        Array.prototype.inArray = function (callback, inv) {
            if (!$.isFunction(callback)) return;
            if (inv == undefined) inv == true;
            for (var i = this.length - 1; i >= 0; i--) {
                if (callback.call(this[i]) != inv) continue;
                return i;
            }
            return -1;
        };

        //根据数组创建对象集合
        //callback:回调函数
        Array.prototype.select = function (callback) {
            var t = [];
            for (var i = this.length - 1; i >= 0; i--) t.push(callback.call(this[i]));
            return t;
        };

        //返回对应的元素
        //callback:回调函数
        //inv:对比对象或对比模式
        Array.prototype.grep = function (callback, inv) {
            if (!$.isFunction(callback)) return;
            if (inv == undefined) inv == true;
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (callback.call(o) != inv) continue;
                return o;
            }
        };

        //返回对应的元素集合
        //callback:回调函数
        //inv:对比对象或对比模式
        Array.prototype.grepAll = function (callback, inv) {
            if (!$.isFunction(callback)) return;
            if (inv == undefined) inv == true;
            var os = [];
            for (var i = this.length - 1; i >= 0; i--) {
                var o = this[i];
                if (callback.call(o) != inv) continue;
                os.push(o);
            }
            return os;
        };

        Array.prototype.getTree = function (tree, showcklayer) {
            var tree = Array.isArray(tree) ? tree : [], data = this, node, l = $.isNumeric(showcklayer) ? showcklayer : 0;

            data.forEach(function (o) {
                if (o.showcheck == undefined) o.showcheck = ($.isNumeric(o.layer) ? o.layer : 0) >= l;

                if (Array.isArray(o.children) && o.children.length) o.children.length = 0;

                o.toString = function () { return this.path || spit(this.pid) + ',' + spit(this.id); };
            });

            data.sort();

            data.forEach(function (o) {
                if (!o.pid || !node) tree.push(node = o);
                else {
                    if (node.ID == o.pid || (node = data.grep(function () { return this.id; }, o.pid))) {
                        if (!Array.isArray(node.children)) node.children = [];
                        o.parent = node; node.children.push(o);
                    }
                    else tree.push(node = o);
                }
            });

            return tree;
        }

        function spit(id) { var z = '0000000000' + id; return z.substr(z.length - 10); };

        if (!Array.prototype.find) Array.prototype.find = function (predicate, thisArg) {
            var o;
            for (var i = 0, l = this.length; i < l; i++) { if (predicate.call(thisArg, o = this[i])) return o }
        }

        //获取两个日期的差值
        //interval：操作模式.有:y,m,d,w,h,n,s,l
        //s:开始日期
        //e:结束日期
        window.DateDiff = function (interval, s, e) {
            if (!(s instanceof Date) || !(e instanceof Date)) return;
            var long = e.getTime() - s.getTime(); //相差毫秒 
            switch (interval.toLowerCase()) {
                case "y": return parseInt(e.getFullYear() - s.getFullYear());
                case "m": return parseInt((e.getFullYear() - s.getFullYear()) * 12 + (e.getMonth() - s.getMonth()));
                case "d": return parseInt(long/1000/60 / 60 / 24);
                case "w": return parseInt(long/1000/60 / 60 / 24 / 7);
                case "h": return parseInt(long/1000/60 / 60);
                case "n": return parseInt(long/1000/60);
                case "s": return parseInt(long/1000);
                case "l": return parseInt(long);
            }
        };

        //对日期进行加值
        //interval:操作模式.有:s,n,h,d,w,q,m,y
        //Number:增量
        Date.prototype.DateAdd = function (interval, Number) {
            if (!$.isNumeric(Number)) return this;
            switch (interval.toLowerCase()) {
                case 's': return new Date(Date.parse(this) + (1000 * Number));
                case 'n': return new Date(Date.parse(this) + (60000 * Number));
                case 'h': return new Date(Date.parse(this) + (3600000 * Number));
                case 'd': return new Date(Date.parse(this) + (86400000 * Number));
                case 'w': return new Date(Date.parse(this) + ((86400000 * 7) * Number));
                case 'q': return new Date(this.getFullYear(), (this.getMonth()) + Number * 3, this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds());
                case 'm': return new Date(this.getFullYear(), (this.getMonth()) + Number, this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds());
                case 'y': return new Date((this.getFullYear() + Number), this.getMonth(), this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds());
            }
            return this;
        };

        //获取两个日期的差值
        //interval：操作模式.有:y,m,d,w,h,n,s,l
        //d:对比日期
        Date.prototype.DateDiff = function (interval, d) {
            return window.DateDiff(interval, d, this);
        };

        var ajaxDefSet = { url: null, type: 'GET', contentType: 'application/json', data: null, dataType: 'json', async: true, success: $.noop, error: $.noop };

        return {
            ajax: {
                Init: function (set) {
                    if (set.Send) delete set.Send;
                    return $.extend({
                        send: function (data) {
                            if (!$.trim(this.url)) { if ($.isFunction(this.error)) this.error('地址不能为空！'); return; }
                            if (data) this.data = data;
                            if (sessionStorage.Token) (this.headers || (this.headers = {})).Authorization = sessionStorage.Token;
                            $.support.cors = true, $.ajax(this)
                        }
                    }, ajaxDefSet, set);;
                }
            },
            extend: function (o, d) {
                if ($.isPlainObject(o) && $.isPlainObject(d)) {
                    for (var i in d) if (o[i] === undefined) o[i] = d[i];
                }
                return o;
            }
        };
    });