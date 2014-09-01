﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using RenderModel.Abstract;

namespace RenderModel
{
    public class MandelbrotFractal
    {

        IFractalBitmap image;

        public MandelbrotFractal(IFractalBitmap renderBitmap)
        {
            image = renderBitmap;
        }

        public List<Color> GenerateColorPalette()
        {
            List<Color> retVal = new List<Color>();
            for (int i = 0; i <= 255; i++)
            {
                retVal.Add(Color.FromArgb(255, i, i, i));
            }
            return retVal;
        }

        public Bitmap Draw(double rMin, double iMin, double rMax, double iMax, int width, int height,
            int inf_n = 255, int startX = 0, int? end_x = null)
        {
            //Check input
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("Width must be a positive number");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("Height must be a positive number");
            }

            if (inf_n < 1 || inf_n > 255)
            {
                throw new ArgumentOutOfRangeException("inf_n must be within the interval 1 - 255");
            }

            end_x = end_x == null ? width : end_x;

            //Apply modulo 256
            width = width - width % 256;
            height = height - height % 256;

            if (startX < 0 || startX > width)
            {
                throw new ArgumentOutOfRangeException("StartX must be within the width");
            }


            List<Color> Palette = GenerateColorPalette();
            double rScale = (Math.Abs(rMin) + Math.Abs(rMax)) / width; // Amount to move each pixel in the real numbers
            double iScale = (Math.Abs(iMin) + Math.Abs(iMax)) / height; // Amount to move each pixel in the imaginary numbers

            for (int x = startX; x < end_x; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Complex c = new Complex(x * rScale + rMin, y * iScale + iMin); // Scaled complex number
                    Complex z = c;
                    for (int i = 255; i > 0; i = i - 255/inf_n) //Vary decrements with total number of iterations
                    {
                        if (z.Magnitude >= 2.0)
                        {
                            image.SetPixel(x - startX, y, Palette[i]); // Set the pixel if the magnitude is greater than two
                            break; // We're done with this loop
                        }
                        else
                        {
                            z = c + Complex.Pow(z, 2); // Z = Zlast^2 + C
                        }
                    }
                }
            }

            return image.getBitmap();
        }
    }
}
