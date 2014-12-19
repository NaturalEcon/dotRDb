angular.module('xferx')
.controller('edgeAddCtrl', ['$scope', '$routeParams', 'graphService',
    function ($scope, $routeParams, graphService) {
        var fromType = null;
        var toType = null;
        $scope.selectedToNode = undefined;
        $scope.toNode = '';
        $scope.selectedEdgeType = {};
        $scope.selectedFromNode = undefined;
        $scope.fromNode = '';
        $scope.edgeSetLoaded = {};

        $scope.routeParam = false;

        $scope.message = '';
        $scope.messageType = '';
        $scope.navigation = history.back;
        $scope.edgesAdded = 0;

        if ($routeParams.nodeId) {
            $scope.routeParam = true;
            graphService.getNode($routeParams.nodeId)
                .then(function(data) {
                    $scope.fromNode = data.data;
                    if ($scope.fromNode) {
                        fromType = $scope.fromNode.NodeTypeId;
                    }

                    if ($scope.toNode) {
                        toType = $scope.toNode.NodeTypeId;
                    }

                    graphService.getEdgeTypes(fromType, toType)
                        .then(function (data) {
                            $scope.edgeTypes = data.data;
                        });
                });
        }
        $scope.selectEdgeType = function() {
            $scope.edgeType = $scope.selectedEdgeType;
        }
        $scope.selectToNode = function(node) {
            graphService.getNode(node.NodeId)
            .then(function(data) {
                $scope.toNode = data.data;
            });
        }
        $scope.selectFromNode = function (node) {
            graphService.getNode(node.NodeId)
            .then(function (data) {
                $scope.fromNode = data.data;
            });
        }


        $scope.nodeSearch = function(s, field) {
            return graphService.getNodesForNewEdge(s, field, $scope.fromNode.NodeId, $scope.fromNode.NodeTypeId, $scope.toNode.NodeId, $scope.toNode.NodeTypeId, $scope.selectedEdgeType.EdgeTypeId)
                    .then(function(data) {
                        return data.data;
                    });
            }
        $scope.addEdge = function () {
            //Sanity check
            if (!$scope.edgeType || !$scope.edgeType.EdgeTypeId || !$scope.fromNode.NodeId || !$scope.toNode.NodeId) {
                $scope.message = 'You must make a selection for each field to create an edge.';
                $scope.messageType = 'error';
                $scope.navigation = null;
                $scope.navigationMessage = '';
                return;
            }
            //Build edge
            var edge = {
                EdgeTypeId: $scope.edgeType.EdgeTypeId,
                FromNodeId: $scope.fromNode.NodeId,
                ToNodeId: $scope.toNode.NodeId
            };
            //Check if one exists.
            graphService.getEdgeIsNew(edge)
                .then(function(data) {
                    if (data.data !== 'False') {
                        graphService.addEdge(edge)
                            .then(function(data) {
                                $scope.edgesAdded += data.data;
                                var pluraltext = $scope.edgesAdded > 1 ? 'edges' : 'edge';
                                $scope.message = $scope.edgesAdded + ' ' + pluraltext + ' added.';
                                $scope.messageType = 'confirmation';
                                $scope.navigation = history.back;
                                $scope.navigationMessage = 'Back';
                            });
                    //The edge already exists!  Fool!
                    } else {
                        $scope.message = 'That edge already exists.  Edges must be unique, but you can add as much data to an edge as you like.';
                        $scope.messageType = 'error';
                        $scope.navigation = function () { }; //todo: go to edge to add data
                        $scope.navigationMessage = 'Go to edge';
                    }
                });

        }
        $scope.edgeTypes = [];
    }]);

//toResult.Name + ' (' + toResult.NodeType.TypeName + ')' for
//toResult.NodeId as 