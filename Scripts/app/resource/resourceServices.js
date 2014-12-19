angular.module('xferx')
.service('resourceService', ['genericService', function (genericService) {

    //Getters
    var allResources = function() {
        return genericService.get('api/Resource/All', '');
    }

    var deleteResource = function(resourceId) {
        return genericService.post('api/Resource/DeleteResource', resourceId);
    }

    var searchResources = function(name) {
        return genericService.get('api/Resource/SearchResources', 's', name);
    }

    var getResource = function(resourceId) {
        return genericService.get('api/Resource/GetResource','resourceId', resourceId);
    }

    var resourceList = function (resourceIds) {
        return genericService.get('api/Resource/ResourceList','resourceIds', resourceIds);
    }

    var getMetadata = function(resourceId) {
        return genericService.get('api/Resource/ResourceMetadata','resourceId', resourceId);
    }

    var getResourcePage = function(pageNumber) {
        return genericService.get('api/Resource/ResourcePage','p', undefined);
    }

    var isNew = function(resource) {
        return genericService.get('api/Resource/IsNew','resourceName', resource.Name);
    }

    //Setters

    var addResource = function (resource) {
        return genericService.post('api/Resource/AddResource', resource);
    }

    return {
        allResources: allResources,
        deleteResource: deleteResource,
        getMetadata: getMetadata,
        getResourcePage: getResourcePage,
        resourceList: resourceList,
        isNew: isNew,
        addResource: addResource,
        getResource: getResource,
        searchResources: searchResources
    };
}]);