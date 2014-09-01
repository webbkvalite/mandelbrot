using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderModel;
using RenderModel.Abstract;
using System.Drawing;
using System.IO;
using RenderModel.BitmapVariations;

namespace Mandelbrot.Tests
{
    [TestClass]
    public class MandelbrotRenderTests
    {
        [TestMethod]
        public void TestDrawMandelbrot()
        {
            MandelbrotFractal mandelbrot = new MandelbrotFractal(new SafeBitmap(256, 256));
            Bitmap fractal = mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, 256, 256, 8, 0, 256);
            var path = System.Environment.CurrentDirectory + "/fractal.bmp";
            fractal.Save(path);
        }
    }
}
