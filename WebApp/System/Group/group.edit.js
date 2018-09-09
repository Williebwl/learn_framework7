define(['core.edit', 'System/Group/group.service.js'],
function (core) {
    'use strict'

    core.controller('GroupEditCtrl', function ($scope, groupService) {
        core($scope, groupService)
    });
})