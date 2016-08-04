(function () {
    var app = angular.module("DISOpenDataCloud", ['ui.bootstrap', 'ngCookies', 'ngSanitize'])
        .run(function ($http, $cookies) {
            $http.defaults.headers.common.Authorization = $cookies.get('DISAPIToken');
        });

    angular.module('DISOpenDataCloud').filter('cut', function () {
        return function (value, wordwise, max, tail) {
            if (!value) return '';

            max = parseInt(max, 10);
            if (!max) return value;
            if (value.length <= max) return value;

            value = value.substr(0, max);
            if (wordwise) {
                var lastspace = value.lastIndexOf(' ');
                if (lastspace != -1) {
                    value = value.substr(0, lastspace);
                }
            }

            return value + (tail || ' …');
        };
    });
    // Register domain enums
    app.constant('enums', {
        dpkStatus: {
            0: 'Invalid',
            1: 'Fulfilled',
            2: 'Consumed',
            3: 'Bound',
            4: 'NotifiedBound',
            5: 'Returned',
            7: 'ReportedBound',
            8: 'ReportedReturn',
            9: 'ActivationEnabled',
            10: 'ActivationDenied',
            11: 'Assigned',
            12: 'Retrieved',
            13: 'ActivationEnabledPendingUpdate'
        },
        serviceStatus: {
            0: 'Created',
            1: 'Published',
            2: 'Subscribed',
            3: 'Closed',
        }
    });
    app.constant('Webapiurl', "http://localhost:34308");

   

    app.directive("fileread", [function () {
        return {
            scope: {
                fileread: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    var reader = new FileReader();
                    reader.readAsDataURL(changeEvent.target.files[0]);
                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            var image = new Image();
                            image.src = loadEvent.target.result;
                            var canvas = document.getElementById("thumbCanvas");
                            var ctx = canvas.getContext("2d");
                            var MAX_HEIGHT = 235;
                            image.onload = function () {
                                if (image.height > MAX_HEIGHT) {
                                    image.width *= MAX_HEIGHT / image.height;
                                    image.height = MAX_HEIGHT;
                                }
                                canvas.width = image.width;
                                canvas.height = image.height;
                                //Draw the image to canvas
                                ctx.drawImage(image, 0, 0, image.width, image.height);
                                scope.fileread = canvas.toDataURL("image/jpeg");
                            };
                        });
                    }
                });
            }
        }
    }]);

    app.directive('contenteditable', function() {
        return {
            restrict: 'A' ,
            require: 'ngModel',
            link: function (scope, element, attrs, ctrl) {
                // Create editor
                scope.editor = new wangEditor('editor-trigger');
                scope.editor.config.pasteFilter = false;
                scope.editor.onchange = function () {
                    // Update data from the "onchange" event
                    scope.$apply(function () {
                        var html = scope.editor.$txt.html();
                        ctrl.$setViewValue(html);
                    });
                };
                scope.editor.create();
            }
        };
    });

    app.directive("mutifileread", [function () {
        return {
            scope: {
                mutifileread: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    var reader = new FileReader();
                    var file = changeEvent.target.files[0];
                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            var item = {
                                name : file.name,
                                extension : file.type,
                                size: file.size,
                                body: reader.result.match(/^data.+base64,/).toString(),
                                bytes : reader.result.replace(/^data.+base64,/, ""),
                                creationTime : new Date(),
                                modificationTime: file.lastModifiedDate,
                                isUpload: false,
                            };
                            //Add the file to file list
                            scope.mutifileread.push(item);
                            
                        });
                    }
                    for (i = 0; i < scope.mutifileread.length; ++i) {
                        if (scope.mutifileread[i].name == file.name) {
                            return;
                        }
                    }
                    if (file.size > 50 * 1024 * 1024)
                    {
                        alert("File size should NOT exceed 50MB!");
                        return;
                    }
                    
                    reader.readAsDataURL(file);
                });
            }
        }
    }]);
    
}());