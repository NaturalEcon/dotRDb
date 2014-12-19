angular.module('xferx', ['ngRoute', 'ui.bootstrap'])
.config(function ($routeProvider) {
    $routeProvider
       /*
            Resource
                        */
        .when('/resource/list', {
            templateUrl: '/ngTemplates/resource/list.html',
            controller: 'resourcePageCtrl'
        })
        .when('/resource/add', {
            templateUrl: '/ngTemplates/resource/add.html',
            controller: 'resourceAddCtrl'
        })
        .when('/resource/detail/:resourceId', {
            templateUrl: '/ngTemplates/resource/detail.html',
            controller: 'resourceItemCtrl'
        })
        .when('/resource/edit/:ResourceId', {
            templateUrl: '/ngTemplates/resource/edit.html',
            controller: 'resourceEditCtrl'
        })
        /*
             Graph
                      */
        .when('/graph/node/:nodeId', {
            templateUrl: 'ngTemplates/graph/node/node.html',
            controller: 'nodeItemCtrl'
        })
        .when('/graph/edge/addfor/:nodeId?', {
            templateUrl: 'ngTemplates/graph/edge/add.html',
            controller: 'edgeAddCtrl'
        })
        /*
             Datum
                      */
        .when('/data/addfor/:nodeId', {
            templateUrl: 'ngTemplates/data/add.html',
            controller: 'nodeAddDataCtrl'
        })
        .when('/data/:datumId', {
            templateUrl: 'ngTemplates/data/item.html',
            controller: 'dataItemCtrl'
        });
});