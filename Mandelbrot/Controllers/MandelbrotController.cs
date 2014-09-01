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
                return new HttpStatusCodeResult(422, ex.Message);
            }

            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "The bitmap could not be created by the server, due to the following exception: " + ex.Message);
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
                return new HttpStatusCodeResult(422, ex.Message);
            }

            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "The fractal could not be rendered, due to the following exception: " + ex.Message);
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
                return new HttpStatusCodeResult(500, "An error occurred when creating the image: " + ex.Message);
            }

            return result;
        }
	}
}