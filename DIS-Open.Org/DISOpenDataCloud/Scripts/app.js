(function () {
    // 创建了一个新模块atTheMovies
    // 第二个参数为空[]，代表这个模块依赖于核心Angular模块ng
    // var app = angular.module("atTheMovies", []);

    var app = angular.module("KnowledgeManage", ['ui.bootstrap', 'ngCookies', 'ngSanitize'])
        .run(function ($http, $cookies) {
            $http.defaults.headers.common.Authorization = $cookies.get('CMSBizAPIToken');
        });

    angular.module('KnowledgeManage').filter('cut', function () {
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
    // 注册常量值
    app.constant('enums', {
        status: {
            0: '已提交',
            1: '审批通过',
            2: '审批失败',
            3: '草稿',
            4: '删除'
        },
        topicStatus: {
            0: '打开',
            1: '关闭',
        }
    });
    app.constant('Webapiurl', "http://localhost:39174");

   

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
                                //把图片绘制到canvas
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
                // 创建编辑器
                scope.editor = new wangEditor('editor-trigger');
                scope.editor.config.pasteFilter = false;
                scope.editor.onchange = function () {
                    // 从 onchange 函数中更新数据
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
                            //添加文件到文件列表
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
                        alert("上传文件不能大于50兆");
                        return;
                    }
                    
                    reader.readAsDataURL(file);
                });
            }
        }
    }]);
    
}());