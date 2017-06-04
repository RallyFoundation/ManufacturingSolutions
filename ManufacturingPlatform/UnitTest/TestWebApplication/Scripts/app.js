/*
 * AngularJS Nested DataTable custom directive
 * Version: 0.1
 *
 * Copyright 2015 Shailendra Kumar.
 * All Rights Reserved.
 * Use, reproduction, distribution, and modification of this code is subject to the terms and
 * conditions of the MIT license, available at http://www.opensource.org/licenses/mit-license.php
 *
 * Author: Shailendra Kumar
 */
(function () {
    "use strict";

    angular
        .module("myApp", ["nestedDataTable"])
        .controller("MyController",["$scope","DataService", "$timeout",appController])
        .service("DataService",["$http",dataService]);
        /*page app controller*/
        function appController($scope, DataService, $timeout){
            //tableData supplied to custom element.
            $scope.tableData = {};
            $scope.showTable = false; //initial value false
            //outer datatable configuration
            $scope.outerDefaults = {
                sPaginationType: "full_numbers",
                bRetrieve: true,
                bDestroy: true,
                aaSorting: [[1, "asc"]]
            };
            //inner datatable configuration
            $scope.innerDefaults = {
                aaSorting: [[2, "asc"]]
            };
            /*get table data from data.json file*/
            DataService.get()
                .success(function(data){
                    $scope.showTable = true;
                    $scope.tableData = data;
                })
                .error(function(data, status) {
                    console.log("Error:"+status);
                });

            //When ever you change data, just re-initiate the directive using $scope.showTable
            $timeout(function(){
                $scope.showTable = false;
                DataService.newData()
                  .success(function(data){
                      $scope.showTable = true;
                      $scope.tableData = data;
                  })
                  .error(function(data, status) {
                      console.log("Error:"+status);
                  });
            },3000);// timeout of 3 seconds
        }
        /*service to get table data*/
        function dataService($http) {
            this.get = function () {
                //alert("data.json");
                return $http.get("/Scripts/Test/data.json");
            };

            this.newData = function () {
                //alert("data1.json");
                return $http.get("/Scripts/Test/data1.json");
            }
        }
})();
