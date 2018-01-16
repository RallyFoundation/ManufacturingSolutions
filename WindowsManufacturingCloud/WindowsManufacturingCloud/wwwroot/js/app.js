(function () {
    var app = angular.module("WinMfgCloud", ['ui.bootstrap', 'ngCookies', 'ngSanitize', 'ngDialog'])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        }])
        .run(function ($http, $cookies) {
            $http.defaults.headers.common.Authorization = $cookies.get('WinMfgCloudToken');
            $http.defaults.headers.patch['Content-Type'] = 'application/json;charset=utf-8';
        });
}());