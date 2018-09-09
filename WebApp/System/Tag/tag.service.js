
define(['core.Service'],
    function (core) {
        'use strict'

        function tagService() {
            this.fnGetTagByTagClass = function (tagClass) {
                return this.get('GetTagByTagClass', { params: { tagClass: tagClass } })
            }
        }

        function tagClassService() {

        }

        return core.service('tag', tagService, 'Tag/Tag').service('tagGroup', 'Tag/tagGroup').service('tagClass', tagClassService, 'Tag/TagClass')
    });