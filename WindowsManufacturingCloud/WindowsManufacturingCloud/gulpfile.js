/// <binding BeforeBuild='min' AfterBuild='min' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

gulp.task('min:js', function () {
    // place code for your default task here
    return gulp.src('Scripts/*.js')
        .pipe(gulp.dest('wwwroot/js'));
});

//gulp.task('min:cfg', function () {
//    // place code for your default task here
//    return gulp.src('Configs/config.json')
//        .pipe(gulp.dest('wwwroot/js'));
//});


gulp.task("min", ["min:js"]);