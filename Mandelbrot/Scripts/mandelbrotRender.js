"use strict";

//Set namespace
var mandelbrot = mandelbrot || {};

//Store fractals
mandelbrot.fractals = [];

$(document).ready(
    function () {
        //Load up vars from DOM
        mandelbrot.min_c_re = $("#min_c_re");
        mandelbrot.min_c_im = $("#min_c_im");
        mandelbrot.max_c_re = $("#max_c_re");
        mandelbrot.max_c_im = $("#max_c_im");
        mandelbrot.x = $("#x");
        mandelbrot.y = $("#y");
        mandelbrot.inf_n = $("#inf_n");
        mandelbrot.p_threads = $("#p_threads");

        //Set default values
        mandelbrot.min_c_re.val(-2.5);
        mandelbrot.min_c_im.val(-1.0);
        mandelbrot.max_c_re.val(1);
        mandelbrot.max_c_im.val(1);
        mandelbrot.x.val(256);
        mandelbrot.y.val(256);
        mandelbrot.inf_n.val(255);
        mandelbrot.p_threads.val(4);


        //Set event handlers
        $("#renderMandelbrot").click(function () {
            mandelbrot.fractals.push(new Fractal(mandelbrot.min_c_re.val(), mandelbrot.min_c_im.val(),
            mandelbrot.max_c_re.val(), mandelbrot.max_c_im.val(), mandelbrot.x.val(), mandelbrot.y.val(),
             mandelbrot.inf_n.val(), mandelbrot.p_threads.val()));
            return false;
        });

    });

function Fractal(min_c_re, min_c_im, max_c_re, max_c_im, x, y, inf_n, p_threads) {

    //Help functions
    var checkZero = function (number) {
        if (number.length == 1) {
            return "0" + number;
        }
        return number;
    }

    var img = $("<img />").attr('src', 'http://somedomain.com/image.jpg')
    .load(function () {
        if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
            alert('broken image!');
        } else {
            $("#something").append(img);
        }
    });

    //Apply modulo 256
    x = x - x % 256;
    y = y - y % 256;

    //Calculate image partitions in whole pixels
    var partitionWidth = Math.floor(x / p_threads);

    //Prepare render data for display
    var header = "<h3>Input Data</h3>";
    var min_c_reLabel = "<p><span class='fractalInputLabel'>Min C real:</span><span>" + min_c_re + "</span></p>";
    var min_c_imLabel = "<p><span class='fractalInputLabel'>Min C imaginary:</span><span>" + min_c_im + "</span></p>";
    var max_c_reLabel = "<p><span class='fractalInputLabel'>Max C real:</span><span>" + max_c_re + "</span></p>";
    var max_c_imLabel = "<p><span class='fractalInputLabel'>Max C imaginary:</span><span>" + max_c_im + "</span></p>";
    var xLabel = "<p><span class='fractalInputLabel'>Width:</span><span>" + x + "</span></p>";
    var yLabel = "<p><span class='fractalInputLabel'>Height:</span><span>" + y + "</span></p>";
    var inf_nLabel = "<p><span class='fractalInputLabel'>Max iterations:</span><span>" + inf_n + "</span></p>";
    var p_threadsLabel = "<p><span class='fractalInputLabel'>Parallell threads:</span><span>" + p_threads + "</span></p>";
    var startDate = new Date();
    var startTime = "<p><span class='fractalInputLabel'>Started:</span><span>" +
        checkZero(startDate.getHours().toString()) + ":" + checkZero(startDate.getMinutes().toString()) + ":" +
        checkZero(startDate.getSeconds().toString()) + "</span></p>";
    var imageElement = "<div class='partialImageWrapper'>";
    for (var i = 0; i < p_threads - 1; i++) {
        //Renders all parts except last one
        imageElement += "<img class='partialImage' style='left: " + (i * partitionWidth) + "px' src='/mandelbrot?min_c_re=" +
            min_c_re + "&min_c_im=" + min_c_im + "&max_c_re=" + max_c_re + "&max_c_im=" + max_c_im + "&x=" + x + "&y=" + y +
        "&inf_n=" + inf_n + "&start_x=" + (i * partitionWidth) + "&end_x=" + ((i + 1) * partitionWidth) + "' />";
    }
    //Last image partition compensates for rounding.
    imageElement += "<img class='partialImage' style='left: " + (partitionWidth * (p_threads - 1)) + "px' src='/mandelbrot?min_c_re=" +
        min_c_re + "&min_c_im=" + min_c_im + "&max_c_re=" + max_c_re + "&max_c_im=" + max_c_im + "&x=" + x + "&y=" + y +
        "&inf_n=" + inf_n + "&start_x=" + (partitionWidth * (p_threads - 1)) + "&end_x=" + x + "' />";
    imageElement += "</div>";
    var wrapperHeight = y + 20 > 340 ? y + 20 : 340;
    $("#mandelWrapper").prepend("<div class='fractalSet' style='height: " + wrapperHeight + "px'>" + header +
        min_c_reLabel + min_c_imLabel + max_c_reLabel + max_c_imLabel +
        xLabel + yLabel + inf_nLabel + p_threadsLabel + startTime + imageElement + "</div>");
}