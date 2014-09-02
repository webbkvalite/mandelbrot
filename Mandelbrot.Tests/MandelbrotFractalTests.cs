using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderModel;
using RenderModel.Abstract;
using System.Drawing;
using System.IO;
using RenderModel.BitmapVariations;
using Moq;
using System.Drawing.Imaging;

namespace Mandelbrot.Tests
{
    [TestClass]
    public class MandelbrotFractalTests
    {
        [TestMethod]
        public void TestDrawMandelbrot()
        {
            //Arrange
            int width = 256;
            int height = 256;
            Mock<IFractalBitmap> mockedBitmap = new Mock<IFractalBitmap>();
            mockedBitmap.Setup(m => m.SetPixel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Color>()));
            mockedBitmap.Setup(m => m.getBitmap()).Returns(new Bitmap(1, 1, PixelFormat.Format24bppRgb));
            MandelbrotFractal mandelbrot = new MandelbrotFractal(mockedBitmap.Object);

            //Act
            Bitmap fractal = mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 8);

            //Assert
            //SetPixel should be called at least once.
            mockedBitmap.Verify(m => m.SetPixel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Color>()),
                Times.AtLeastOnce());
            //getBitmap should be called exactly once.
            mockedBitmap.Verify(m => m.getBitmap(), Times.Once());

            //**************** Test validations ****************
            int invalidLowParam = 0;

            //Test low width
            string paramName = String.Empty;
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, invalidLowParam, height);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("width", paramName);

            //Test low height
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, invalidLowParam);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("height", paramName);

            //Test low inf_n
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, invalidLowParam);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("inf_n", paramName);

            //Test low startX
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 8, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("startX", paramName);

            //Test low end_x
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 8, 0, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("end_x", paramName);

            //Test high inf_n
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 257);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("inf_n", paramName);

            //Test high startX
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 8, width + 1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("startX", paramName);

            //Test high end_x
            try
            {
                mandelbrot.Draw(-2.5, -1.0, 1.0, 1.0, width, height, 8, 0, width + 1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                paramName = ex.ParamName;
            }
            Assert.AreEqual("end_x", paramName);

        }
    }
}
