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

    alert("Hello, world!");

    angular
        .module("nestedDataTable",[])
        .constant("outerDefaults", {/*Default configuration for outer table */
            "aoColumnDefs": [
                { "bSortable": false, "aTargets": [0] } // disable sorting on first column
            ],
            "aaSorting": []//initial sorting is disabled in outer dataTable
        })
        .constant("innerDefaults", {/*Default configuration for inner nested table */
            "aoColumnDefs": [
                { "bSortable": false, "aTargets": [0] } // disable sorting on first column
            ],
            "aaSorting": [],
            "bRetrieve": true,
            "bDestroy": true,
            "bPaginate": false,// disable pagination in inner nested dataTable
            "bFilter": false,
            "bInfo": false
        })
        .directive("ngNestedDatatable",["$rootScope","$timeout","$compile","outerDefaults","innerDefaults", nestedDirective]);

    var uuid = 0; //unique identifier ID to differentiate each nested table

    /*Inner Nested Table Template, we can reuse same custom element here
     but this time we give innerTable configuration to outerTableConfig parameter*/
    var innerTemplate = function () {
        return '<div class="innerDiv"><ng-nested-datatable data="innerData" outer-table-config="innerDefaults"></ng-nested-datatable></div>';
    };

    /*AngularJS custom directive code*/
    function nestedDirective($rootScope,$timeout, $compile, outerDefaults, innerDefaults){
        return {
            restrict: "EA",
            scope: {
                data: "=",
                outerTableConfig: "=",
                innerTableConfig: "="
            },
            compile: function(){
                var link = function($scope){
                    $scope.uuid = 'dataTable'+(++uuid);
                    var config = {};
                    if(angular.isObject($scope.innerTableConfig)){
                        config = angular.extend({},outerDefaults,$scope.outerTableConfig);
                        $rootScope.innerTableConfig = $scope.innerTableConfig;
                    }
                    else {
                        config = $scope.outerTableConfig;
                    }
                    try{
                        //using $timeout, so that that dataTable will initialize only after all rows are constructed by then
                        $timeout(function(){
                            angular.element('#'+$scope.uuid).dataTable(config);
                        });
                    }
                    catch (err) {/*Catching dataTable configuration errors*/
                        console.log("Error in dataTable configuration: "+err);
                    }
                };
                return link;
            },
            controller: function($scope,$rootScope){
                /*Toggling between open and close nested table Event Handler*/
                $scope.toggleRow = function ($event, tr) {
                    alert("toggleRow");

                    alert($event.target);

                    var targetElem = $($event.target);
                    var table = targetElem.parents(".dataTable");
                    var nTr = targetElem.parents('tr:eq(0)')[0];
                    $scope.innerData = tr;
                    $scope.innerDefaults = angular.extend({},innerDefaults,$rootScope.innerTableConfig);
                    if (table.dataTable().fnIsOpen(nTr)) {
                        targetElem.html("+");
                        table.dataTable().fnClose(nTr);
                    }
                    else {
                        targetElem.html("-");//$($event.target).parents('tr:eq(0)').next(".innerTable")
                        table.dataTable().fnOpen(nTr,innerTemplate() ,"innerTable");
                        $compile(targetElem.parent().parent().next())($scope);
                    }
                }
            },
            templateUrl: "/Scripts/Test/nestedtable.tmpl.html"
        }
    }
})();
