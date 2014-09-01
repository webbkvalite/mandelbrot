﻿"use strict";

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
        mandelbrot.fast_bitmap = $("#fast_bitmap");

        //Set default values
        mandelbrot.min_c_re.val(-2.5);
        mandelbrot.min_c_im.val(-1.0);
        mandelbrot.max_c_re.val(1);
        mandelbrot.max_c_im.val(1);
        mandelbrot.x.val(256);
        mandelbrot.y.val(256);
        mandelbrot.inf_n.val(255);
        mandelbrot.p_threads.val(4);
        mandelbrot.fast_bitmap.val(1);


        //Set event handlers
        $("#renderMandelbrot").click(function () {
            //Validate input
            $("#fractalForm").validate({
                rules: {
                    x: {
                        min: 256
                    },
                    y: {
                        min: 256
                    },
                    inf_n: {
                        min: 1,
                        max: 255
                    },
                    p_threads: {
                        min: 1,
                        max: 255
                    }
                }
            });
            if ($("#fractalForm").valid()) {
                mandelbrot.fractals.push(new Fractal(mandelbrot.fractals.length, mandelbrot.min_c_re.val(), mandelbrot.min_c_im.val(),
                mandelbrot.max_c_re.val(), mandelbrot.max_c_im.val(), mandelbrot.x.val(), mandelbrot.y.val(),
                 mandelbrot.inf_n.val(), mandelbrot.p_threads.val(), mandelbrot.fast_bitmap.is(':checked')));
            } else {

            }
            return false;
        });

    });