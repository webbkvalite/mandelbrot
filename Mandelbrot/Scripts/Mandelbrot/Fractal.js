﻿"use strict";

function Fractal(id, min_c_re, min_c_im, max_c_re, max_c_im, x, y, inf_n, p_threads, fast_bitmap) {
    //Class members
    var partitionCounter = 1;
    var startDate = new Date();

    //Array of servers to distribute the calculation load upon
    var serverURLs = ["http://mandelbrot.azurewebsites.net", "http://mandelbrot2.azurewebsites.net"];


    //**************** Help functions *****************
    //Fills in zeros in time stamps
    var checkZero = function (number) {
        if (number.length == 1) {
            return "0" + number;
        }
        return number;
    }

    //Renders time part from JS date
    var renderTime = function (date) {
        return checkZero(date.getHours().toString()) + ":" + checkZero(date.getMinutes().toString()) + ":" +
            checkZero(date.getSeconds().toString());
    }

    //Creates the GET request for Fractal BMP
    //Handles timer for Fractal partitions
    var loadNewPartition = function (start_x, end_x, serverURL) {
        var img = $("<img />")
            .attr("style", "left: " + start_x + "px")
            .attr("src", serverURL + "/mandelbrot?min_c_re=" + min_c_re + "&min_c_im=" + min_c_im +
            "&max_c_re=" + max_c_re + "&max_c_im=" + max_c_im + "&x=" + x + "&y=" + y +
            "&inf_n=" + inf_n + "&start_x=" + start_x + "&end_x=" + end_x + "&fast_bitmap=" + fast_bitmap + 
            "&nocache=" + Date.now())
        .load(function () {
            if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
                alert('could not render fractal partition: ' + partitionCounter);
                partitionCounter += 1;
            } else {
                $("#mandelWrapper #fractalId_" + id + " .partialImageWrapper").append(img);
                var endDate = new Date();
                var endTimeLabel = "<p><span class='fractalInputLabel'>Partition " + partitionCounter +
                    " finished at:</span><span>" + renderTime(endDate) + "</span></p>";
                $("#mandelWrapper #fractalId_" + id).append(endTimeLabel);
                var endTimeDifference = "<p class='elapsedTime'><span class='fractalInputLabel'>Elapsed time:</span><span>" +
                    ((endDate - startDate)/1000).toString() + " sec</span></p>";
                $("#mandelWrapper #fractalId_" + id).append(endTimeDifference);
                partitionCounter += 1;
            }
        });
    };

    //**************** End of Help functions *****************

    //Apply modulo 256 to input
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
    var fast_bitmapLabel = "<p><span class='fractalInputLabel'>Fast Bitmap:</span><input type='checkbox' disabled='disabled' ";
    if (fast_bitmap) {
        fast_bitmapLabel += "checked='checked'";
    }
    fast_bitmapLabel += " /></p>";
    var startTimeLabel = "<p><span class='fractalInputLabel'>Started:</span><span>" + renderTime(startDate) + "</span></p>";

    //Create Fractal partitions
    var imageElement = "<div class='partialImageWrapper'></div>";
    for (var i = 0; i < p_threads - 1; i++) {
        //Renders all parts except last one
        loadNewPartition(i * partitionWidth, (i + 1) * partitionWidth, serverURLs[(i % serverURLs.length)]);
    }
    //Last image partition compensates for rounding.
    loadNewPartition(partitionWidth * (p_threads - 1), x, serverURLs[(p_threads-1) % serverURLs.length]);
    //Styling
    var wrapperHeight = y + 20 > 340 ? y + 20 : 340;
    //Prepend to DOM
    $("#mandelWrapper").prepend("<div id='fractalId_" + id + "' class='fractalSet' style='min-height: " + wrapperHeight + "px'>" + header +
        min_c_reLabel + min_c_imLabel + max_c_reLabel + max_c_imLabel +
        xLabel + yLabel + inf_nLabel + p_threadsLabel + fast_bitmapLabel + startTimeLabel + imageElement + "</div>");
}