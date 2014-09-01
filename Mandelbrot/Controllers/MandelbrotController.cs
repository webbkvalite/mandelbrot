using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RenderModel;
using System.Drawing;
using RenderModel.BitmapVariations;

namespace Mandelbrot.Controllers
{
    public class MandelbrotController : Controller
    {
        //
        // GET: /Mandelbrot/
        /// <summary>
        /// Renders mandelbrot fractal
        /// </summary>
        /// <param name="min_c_re">Minimum Real</param>
        /// <param name="min_c_im">Minimum Imaginary</param>
        /// <param name="max_c_re">Max Real</param>
        /// <param name="max_c_im">Max Imaginary</param>
        /// <param name="x">Width in pixels of entire fractal</param>
        /// <param name="y">Height in pixels of entire fractal</param>
        /// <param name="inf_n">Maximum number of iterations</param>
        /// <param name="start_x">Starting X-coordinate for this partition of the fractal (optional)</param>
        /// <param name="end_x">Ending X-coordinate for this partition of the fractal (optional)</param>
        /// <param name="fast_bitmap">For using byte pointers in bitmap setter method. Faster rendering, may crash server instance.</param>
        /// <returns>Image in BMP-format</returns>
        public ActionResult Index(double min_c_re, double min_c_im, double max_c_re, double max_c_im, int x, int y
            ,int inf_n=255, int start_x=0, int? end_x=null, bool fast_bitmap = false)
        {
            //Create Mandelbrot class with choosen bitmap
            end_x = end_x == null ? x : end_x;
            MandelbrotFractal mandelbrot;
            try
            {
                if (fast_bitmap)
                {
                    mandelbrot = new MandelbrotFractal(new FastBitmap((int)end_x - start_x, y));
                }
                else
                {
                    mandelbrot = new MandelbrotFractal(new SafeBitmap((int)end_x - start_x, y));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Response.Write(ex.Message);
                return new HttpStatusCodeResult(422);
            }

            catch (Exception ex)
            {
                Response.Write("The bitmap could not be created by the server, due to the following exception: " + ex.Message);
                return new HttpStatusCodeResult(500);
            }

            //Render bitmap
            Bitmap resultImg;
            try
            {
                resultImg = mandelbrot.Draw(min_c_re, min_c_im, max_c_re, max_c_im,
                    x, y, inf_n, start_x, end_x);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Response.Write(ex.Message);
                return new HttpStatusCodeResult(422);
            }

            catch (Exception ex)
            {
                Response.Write("The fractal could not be rendered, due to the following exception: " + ex.Message);
                return new HttpStatusCodeResult(500);
            }

            //Prepare result
            FileContentResult result;
            try
            {
                using (var memStream = new System.IO.MemoryStream())
                {
                    resultImg.Save(memStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    result = this.File(memStream.GetBuffer(), "image/bmp");
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred when creating the image:" + ex.Message);
                return new HttpStatusCodeResult(500);
            }

            return result;
        }
	}
}