using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace RenderModel
{
    public static class RenderMandelbrot
    {
        public static List<Color> GenerateColorPalette()
        {
            List<Color> retVal = new List<Color>();
            for (int i = 0; i <= 255; i++)
            {
                retVal.Add(Color.FromArgb(255, i, i, i));
            }
            return retVal;
        }

        public static Bitmap DrawMandelbrot(double rMin, double iMin, double rMax, double iMax, int width, int height)
        {
            List<Color> Palette = GenerateColorPalette();
            FastBitmap img = new FastBitmap(width, height); // Bitmap to contain the set

            double rScale = (Math.Abs(rMin) + Math.Abs(rMax)) / width; // Amount to move each pixel in the real numbers
            double iScale = (Math.Abs(iMin) + Math.Abs(iMax)) / height; // Amount to move each pixel in the imaginary numbers

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Complex c = new Complex(x * rScale + rMin, y * iScale + iMin); // Scaled complex number
                    Complex z = c;
                    for (int i = 0; i < Palette.Count; i++) // 255 iterations with the method we already wrote
                    {
                        if (z.Magnitude >= 2.0)
                        {
                            img.SetPixel(x, y, Palette[i]); // Set the pixel if the magnitude is greater than two
                            break; // We're done with this loop
                        }
                        else
                        {
                            z = c + Complex.Pow(z, 2); // Z = Zlast^2 + C
                        }
                    }
                }
            }

            return img.Bitmap;
        }
    }
}
