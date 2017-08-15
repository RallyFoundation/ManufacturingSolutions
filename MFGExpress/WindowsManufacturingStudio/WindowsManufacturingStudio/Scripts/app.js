(function () {
    var app = angular.module("WindowsManufacturingStudio", ['ui.bootstrap', 'ngCookies', 'ngSanitize', 'ngDialog'])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        }])
        .run(function ($http, $cookies) {
            $http.defaults.headers.common.Authorization = $cookies.get('WDSAPIToken');
            $http.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        });
}());