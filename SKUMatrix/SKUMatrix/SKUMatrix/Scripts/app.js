(function () {
    var app = angular.module("SKUMatrixTool", ['ui.bootstrap', 'ngCookies', 'ngSanitize', 'ngDialog'])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        }])
        .run(function ($http, $cookies) {
            $http.defaults.headers.common.Authorization = $cookies.get('SKUMatrixoken');
            $http.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        });
}());