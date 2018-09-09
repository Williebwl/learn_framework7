define(['core.container', 'System/Tag/tag.service.js'],
    function (core) {
        'use strict'

        core.controller('TagGroupContainerCtrl', function ($scope, tagGroupService) {
            core($scope, tagGroupService)
        })
    })