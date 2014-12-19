angular.module('xferx')
.controller('nodeItemCtrl', ['$scope', '$routeParams', 'graphService', 'dataService',
    function ($scope, $routeParams, graphService, dataService) {
        graphService.getNode($routeParams.nodeId)
            .then(function (data) {
                $scope.node = data.data;
                graphService.getNodeType($routeParams.nodeId)
                    .then(function(data) {
                        graphService.getEdgeTypes(data.data, undefined, undefined)
                            .then(function(data) {
                                $scope.edgeTypes = data.data;
                            });
                    });
        });
        
        
        $scope.edges = {};
        $scope.edgeSetLoaded = {};
        $scope.data = undefined;
        $scope.getData = function (nodeId) {
            return dataService.getNodeDatum(nodeId)
                .then(function(data) {
                $scope.data = data.data;
            });
        }
        if ($routeParams.nodeId) {
            $scope.getData($routeParams.nodeId);
        }
        $scope.getEdgeSet = function (edgeTypeId) {
            if (!$scope.edgeSetLoaded[edgeTypeId]) {
                $scope.edgeSetLoaded[edgeTypeId] = true;
                return graphService.getEdgeSet($routeParams.nodeId, edgeTypeId)
                    .then(function (data) {
                        $scope.edges[edgeTypeId] = data.data;
                    });
            }
        }
}]);