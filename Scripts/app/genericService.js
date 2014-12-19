angular.module('xferx')
.service('genericService', ['$http', function ($http) {
    var urlBase = 'http://localhost:51831/';

    //Shorthand method to perform a get request.
    //e.g. genericService.get('api/Resources/All').then(...)
    //     genericService.get('api/Resources/Since','date',0).then(...)
    //     genericService.get('api/Resources/Search', ['s','publishedSince'], [s, dateParam]).then(...)
    var get = function (controller, paramNames, paramList) {
        var queryString = '?' + paramNames + '=' + paramList;
        if (paramList === undefined || (Array.isArray(paramList) && allUndefined(paramList)) ) {
            queryString = '';
        }
        if(queryString !== '' && Array.isArray(paramList) && Array.isArray(paramNames)) {
            var queryList = [];
            for (var i = 0; i < paramList.length; i++) {
                if (paramList[i] !== undefined) {
                    queryList.push(paramNames[i] + '=' + paramList[i]);
                }
            }
            queryString = '?' + queryList.join('&');
        }
        return $http.get(urlBase + controller + queryString)
                    .success(function (data) {
                        return data;
                    })
                    .error(function (data, textStatus) {
                        console.log('Load failed, try again.\n Status: ' + textStatus + '\n Data: ' + (data && data[0]));
                    });
        
    }

    var post = function(controller, param) {
        return $http({
            method: 'POST',
            url: urlBase + controller,
            data: param,
            success: function (data) {
                return data;
            },
            error: function (data, textStatus) {
                console.log('Send failed, try again.\n Status: ' + textStatus + '\n Data: ' + (data && data[0]));
            }
        });
    }

    var hasUndefined = function(list, requiredFields) {
        var checkList = list;
        if (!_.isUndefined(requiredFields)) {
            checkList = [];
            for(var field in requiredFields) {
                checkList.push(list[requiredFields[field]]);
            }
        }
        return _.some(checkList, function (x) { return _.isUndefined(x); });
    }

    var allUndefined = function(list) {
        return _.every(list, function(item) { return item === undefined; });
    }

    var allUndefinedNoDep = function(list) {
        for (var item in list) {
            if (list[item] === undefined) {
                return false;
            }
        }
        return true;
    }

    return {
        urlBase: urlBase,
        get: get,
        post: post,
        hasUndefined: hasUndefined,
        allUndefined: allUndefined
    };
}]);