angular.module('xferx')
.controller('resourcePageCtrl', ['$scope', 'resourceService', function ($scope, resourceService) {
    var bindList = function () {
        resourceService.getResourcePage($scope.pageNumber)
        .then(function (data) {
            $scope.Page = data.data;
            $scope.viewMetadata = resourceService.getMetadata;
        });
    }
    bindList();
    // List resources
    if (!$scope.pageNumber) {
        $scope.pageNumber = 1;
    }
    $scope.confirmationMessage = '';
    // Add single resource
    $scope.resource = {
        subclasses: [],
        superclasses: [],
        forwardDependencies: [],
        backwardDependencies: []
    };
    $scope.doAddResource = false;
    $scope.addResource = function () {
        resourceService.isNew($scope.resource)
            .then(function (data) {
                if (data.data.Item1 === "true") { //is new
                    var newData = resourceService.addResource($scope.resource);
                    newData.then(function (newdata) {
                        $scope.resource = newdata.data;
                        $scope.confirmationMessage = 'Resource ' + newdata.data.ResourceId + ' added.';
                        $scope.doAddResource = false;
                        bindList();
                    });
                } else {
                    $scope.validationMessage = 'A resource with that name already exists.  Possible candidates:';
                    var listPromise = resourceService.resourceList(data.data.Item2);
                    listPromise.then(function (rdata) {
                        $scope.resourceCandidates = rdata.data;
                    });
                }
            });
    }

    $scope.deleteResource = function(resourceId) {
        resourceService.deleteResource({ resourceId: resourceId })
            .then(function(data) {
                var conf = data.data > 0 ? 'was' : 'was not';
                $scope.confirmationMessage = 'Resource ' + resourceId + ' ' + conf + ' deleted.';
                bindList();
        });
    }

    $scope.addMetadata = function () {
        $scope.doAddMetadata = true;
        var b = resourceService.addMetadata;
    }

}])

.controller('resourceItemCtrl', ['$scope', '$routeParams', 'resourceService', 'dataService',
    function ($scope, $routeParams, resourceService, dataService) {
        resourceService.resourceList([$routeParams.resourceId])
                       .then(function (data) {
        $scope.resource = data.data[0];
    });
    var dataPromise = dataService.getResourceData($routeParams.resourceId);
    dataPromise.then(function(data) {
        $scope.data = data.data;
    });
}])

.controller('resourceAddCtrl', ['$scope', 'resourceService', function($scope, resourceService) {
    

}]);