using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderModel;
using System.Drawing;
using System.IO;

namespace Mandelbrot.Tests
{
    [TestClass]
    public class MandelbrotRenderTests
    {
        [TestMethod]
        public void TestDrawMandelbrot()
        {
            Bitmap fractal = RenderMandelbrot.DrawMandelbrot(-2.5, -1.0, 1.0, 1.0, 256, 256,64,127);
            var path = System.Environment.CurrentDirectory + "/fractal.bmp";
            fractal.Save(path);
        }
    }
}
