angular.module('xferx')
.controller('nodeAddDataCtrl', ['$scope', '$routeParams', 'graphService', 'dataService',
    function ($scope, $routeParams, graphService, dataService) {
        var nodeId = $routeParams.nodeId;
        $scope.doAddDatum = false;
        $scope.nodes = [];
        $scope.messsage = '';
        $scope.getNodes = function(s) {
            return;
        };
        $scope.node = undefined;
        $scope.formDatum = {
            Name: undefined,
            DatumType: undefined,
            NodeId: undefined,
            Value: undefined,
            Unit: undefined
        };
        $scope.nodePicked = false;

        if (nodeId) {
            graphService.getNode(nodeId)
                .then(function(data) {
                    $scope.node = data.data;
                    $scope.formDatum.NodeId = data.data.NodeId;
                    $scope.formDatum.DatumType = data.data.NodeTypeId;
                    $scope.nodePicked = true;
            });
        } else {
            $scope.getNodes = function(s) {
                return graphService.searchNodes(s, $scope.selectedType)
                    .then(function(data) {
                        return data.data;
                    });
            }
        }
        $scope.addDatum = function() {
            var requiredFields = [
                'DatumType',
                'NodeId',
                'Value',
                'Unit'
            ];
            var hasUndefined = graphService.hasUndefined($scope.formDatum, requiredFields);
            if (hasUndefined) {
                $scope.message = 'One or more values were not entered.';
                $scope.messageType = 'error';
            } else {
                dataService.addDatum($scope.formDatum).then(function(data) {
                    $scope.message = data.data + ' data saved.';
                    $scope.messageType = 'success';
                });
                
            }
        }

        $scope.selectNode = function (node) {
            graphService.getNode(node.NodeId)
            .then(function (data) {
                $scope.formDatum.node = data.data;
                $scope.nodePicked = true;
            });
        }
        $scope.setType = function(typeId) {
            $scope.type = typeId;
        }
    }]);