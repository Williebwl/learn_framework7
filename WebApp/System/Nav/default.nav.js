define(['core.nav', 'System/Menu/menu.service.js'],
function (page) {
    'use strict'

    page.controller('DefaultNavCtrl',
       function ($scope, menuService, $routeParams) {
           page($scope, menuService)

           //moduleService.fnGetChildrenAndSelfTree($routeParams.id,
           //    function (d) {
           //        $scope.moduleInfo = d && d[0] || {}, Array.isArray($scope.moduleInfo.Children) && $scope.moduleInfo.Children.forEach(function (o) {
           //            if (!o.NavUrl) return;

           //            var a = o.NavUrl.toLowerCase(), b = a.length, c = a.substr(b - 3), d = a.substr(b - 4), e = a.substr(b - 5);

           //            if (c === '.js') o.NavUrl = o.NavUrl.substr(0, b - 3);
           //            else if (d === '.htm' || e === '.html') return;

           //            o.NavUrl = o.NavUrl + '.html'
           //        })
           //    }, function () { $scope.moduleInfo = {} })
       })
})