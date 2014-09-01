# Parallell Mandelbrot Rendering
This project consists of an ASP.net MVC web-server + client for rendering Mandelbrot fractions using parallell
processing. The idea is to instantiate the server on multiple instances and/or domains and let the client distribute the
work before joining the pieces back together.

## Current setup
The webserver is currently setup on two domains: mandelbrot.azurewebsites.net and mandelbrot2.azurewebsites.net

The webserver will automatically assign available threads to incoming requests and put remaining requests on hold.
An optimal performance is therefore achieved by using a proportionate number of parallell requests, weighing in
losses in setting up too many requests versus the strength of parallell processing.

Each domain is currently hosted in Azure's "shared mode", which means a maximum of six instances. The exact amount of available 
cores and threads is therefore difficult to determine, but a good performance is usually achieved with 5 - 20 parallell threads, 
depending on the size of the fractal.

### Limitations
There are memory limitations on Azure instances that can be surpassed with big fractals and that may suddenly stop the instance.
These limitations are also more quickly surpassed with high numbers of iterations.

In order to get best possible control of the performance and limitations, dedicated server instances should be used.

## Quick overview of Classes
### Client Classes
Javascript is used for the client. A "Fractal" class handels all the logic, distribution and rendering. The input validation
and initialization is done in mandelbrotRender.js

All JS-files are found in Mandelbrot/Scripts/Mandelbrot

###Server Classes
Incoming requests are received in the MandelbrotController (Mandelbrot/Controllers), where the model classes are 
instantiated, run and the resulting bitmap is returned to the client.

The model classes are in a separate Class Library "RenderModel" and comprise of an interface for the different bitmap variations 
and a MandelbrotFractal-class that contains server-side validation and render logic.

The FastBitmap-class is a faster version of the standard .net Bitmap-class that suffers in performance for big bitmaps.
However, the majority of time consumption lies in the mathematical calculations. FastBitmap uses byte pointers that may
cause the server to crash without warning if it runs outside of its boundaries. 