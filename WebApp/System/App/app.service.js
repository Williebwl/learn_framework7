define(['core.Service'],
    function (core) {
        'use strict'

        function appService() {
            this.fnGet = function (id) {
                return this.get('Get/' + id)
            },
           this.fnGetAll = function () {
               return this.get('GetAll')
           },
           this.fnSetStatus = function (id, status) {
               return this.put('SetStatus/' + id + '/' + status)
           },
           this.fnGetIcons = function () {
               return [{ name: '任务', icon: 'fa fa-paper-plane', background: 'background-a' },
                       { name: '审批', icon: 'fa fa-users', background: 'background-b' },
                       { name: '通讯录', icon: 'fa fa-users', background: 'background-c' },
                       { name: '文件', icon: 'fa fa-users', background: 'background-h' },
                       { name: '日程', icon: 'fa fa-users', background: 'background-l' },
                       { name: '项目', icon: 'fa fa-users', background: 'background-p' },
                       { name: '日报', icon: 'fa fa-users', background: 'background-x' },
                       { name: '报表', icon: 'fa fa-users', background: 'background-d' }]
           },
           this.fnGetDisplayModes = function () {
               return [{ v: 0, n: '默认模板' }, { v: 1, n: '自定义模板' }, { v: 2, n: '外部主页' }];
           },
           this.fnGetEditInfo = function (id) {
               return this.get('GetEditInfo/' + id)
           }
        }

        core.service('app', appService, 'tenant.App')
    })