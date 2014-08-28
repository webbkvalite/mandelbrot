using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RenderModel;
using System.Drawing;

namespace Mandelbrot.Controllers
{
    public class MandelbrotController : Controller
    {
        //
        // GET: /Mandelbrot/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min_c_re"></param>
        /// <param name="min_c_im"></param>
        /// <param name="max_c_re"></param>
        /// <param name="max_c_im"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="inf_n"></param>
        /// <returns></returns>
        public ActionResult Index(double min_c_re, double min_c_im, double max_c_re, double max_c_im, int x, int y
            ,int inf_n=255, int start_x=0, int? end_x=null)
        {
            Bitmap resultImg = RenderMandelbrot.DrawMandelbrot(min_c_re, min_c_im, max_c_re, max_c_im,
                x, y, inf_n, start_x, end_x);
            FileContentResult result;

            using (var memStream = new System.IO.MemoryStream())
            {
                resultImg.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/bmp");
            }

            return result;
        }
	}
}