(function (app) {
    var KnowledgeManageService = function ($http, movieApiUrl) {
        var getfuwdxlist = function () {
            $http.get('/api/FUWDXApi/Search?sortOrder=&searchString=' + $scope.searchtext + '&page=' + $scope.currentpage,
              {
                  headers: { 'Authorization': "oauth_token=xxxx" }
              }
               ).success(function (data) {
                   $scope.loading = false;

               }).error(function (data) {
                   $scope.error = "读取数据错误，原因是： " + data;
                   $scope.loading = false;
               });
        };

  

        return {
            getfuwdxlist: getfuwdxlist
        };
    };
    app.factory("KnowledgeManageService", KnowledgeManageService);
})(angular.module("KnowledgeManage"));