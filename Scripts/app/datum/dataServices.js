angular.module('xferx')
.service('dataService', ['genericService', function(genericService) {

    var getResourceData = function(resourceId) {
        return genericService.get('Data/GetResourceData','resourceId', resourceId);
    };

    var getUnitTypes = function() {
        return genericService.get('Data/GetUnitTypes', '');
    }

    var addDatum = function (datum) {
        return genericService.post('Data/AddDatum', datum);
    };

    var getNodeDatum = function(nodeId) {
        return genericService.get('api/Graph/GetNodeData','nodeId', nodeId);
    }

    return {
        getResourceData: getResourceData,
        getUnitTypes: getUnitTypes,
        addDatum: addDatum,
        getNodeDatum: getNodeDatum
    };
}]);