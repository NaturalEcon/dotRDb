angular.module('xferx')
.service('edgeRequestService'['genericService', function(genericService) {

    var getEdgesForNode = function(nodeId) {
        return genericService.get('Node/EdgesForNode','nodeId', nodeId);
    };

    return {
        getEdgesForNode: getEdgesForNode
    }
}]);