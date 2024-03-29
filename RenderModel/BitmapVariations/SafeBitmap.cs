﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using RenderModel.Abstract;

namespace RenderModel.BitmapVariations
{
    /// <summary>
    /// Standard .net Bitmap handling.
    /// SetPixel method will suffer performance on large bitmaps.
    /// </summary>
    public class SafeBitmap : IFractalBitmap
    {
        public Bitmap Bitmap { get; set; }

        public SafeBitmap(int width, int height)
        {
            //Check input
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width", "Width must be a positive number.");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height", "Height must be a positive number.");
            }
            this.Bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public void SetPixel(int x, int y, Color color)
        {
            this.Bitmap.SetPixel(x, y, color);
        }

        public Bitmap getBitmap()
        {
            return Bitmap;
        }
    }
}
