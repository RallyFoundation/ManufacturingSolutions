(function (app) {
    var DataManagementService = function ($http, movieApiUrl) {
        var dblist = function () {
            $http.get('/api/DataManagement/Database/',
              {
                  headers: { 'Authorization': "oauth_token=xxxx" }
              }
               ).success(function (data) {
                   $scope.loading = false;

               }).error(function (data) {
                   $scope.error = "Error reading data. Details: " + data;
                   $scope.loading = false;
               });
        };

        return
        {
            dblist: dblist
        };
    };
    app.factory("DataManagementService", DataManagementService);
})(angular.module("DataManagement"));