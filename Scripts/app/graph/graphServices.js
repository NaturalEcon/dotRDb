angular.module('xferx')
.service('graphService', ['genericService', function (genericService) {

    var getEdgeIsNew = function(edge) {
        return genericService.post('api/Graph/EdgeIsNew', edge);
    }

    var getNodeTypes = function() {
        return genericService.get('api/Graph/GetNodeTypes', '');
    }

    var getNode = function (nodeId) {
        return genericService.get('api/Graph/GetNode','nodeId', nodeId);
    }

    var getNodeType = function(nodeId) {
        return genericService.get('api/Graph/GetNodeType','nodeId', nodeId);
    }

    var getNodeAndData = function (nodeId) {
        return genericService.get('api/Graph/GetNodeAndData','nodeId', nodeId);
    }

    var getEdge = function(edgeId) {
        return genericService.get('api/Graph/GetEdge','edgeId' + edgeId);
    }

    var getEdgeTypes = function (anyType, fromType, toType) {
        return genericService.get('api/Graph/GetEdgeTypes', ['anyType', 'fromType', 'toType'], [anyType, fromType, toType]);
    }

    var getEdgeSet = function(nodeId, edgeTypeId) {
        return genericService.get('api/Graph/GetEdgeSet', ['nodeId','edgeTypeId'], [nodeId, edgeTypeId]);
    }

    var getEdgeAndData = function (edgeId) {
        return genericService.get('api/Graph/GetEdgeAndData','edgeId', edgeId);
    }

    var getNodesForNewEdge = function(s, field, from, to, edgeTypeId, maxResults) {
        return genericService.get('api/Graph/GetNodesForNewEdge',
                                 ['s', 'field', 'from', 'to', 'edgeTypeId', 'maxResults'],
                                 [s, field, from, to, edgeTypeId, maxResults]);
    }

    var searchNodes = function(s,type) {
        return genericService.get('api/Graph/SearchNodes',['s','t'], [s, type]);
    }

    var addEdge = function(edge) {
        return genericService.post('api/Graph/AddEdge', edge);
    }

    return {
        addEdge: addEdge,
        hasUndefined: genericService.hasUndefined,
        getNodeTypes: getNodeTypes,
        getNodeType: getNodeType,
        getNode: getNode,
        getNodeAndData: getNodeAndData,
        getEdgeTypes: getEdgeTypes,
        getEdge: getEdge,
        getEdgeIsNew: getEdgeIsNew,
        getEdgeSet: getEdgeSet,
        getEdgeAndData: getEdgeAndData,
        getNodesForNewEdge: getNodesForNewEdge,
        searchNodes: searchNodes
    }
}]);