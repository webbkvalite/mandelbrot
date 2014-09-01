using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RenderModel.Abstract
{
    /// <summary>
    /// Handels polymorphism and protected variations of Fractal bitmaps
    /// </summary>
    public interface IFractalBitmap
    {
        void SetPixel(int x, int y, Color color);

        Bitmap getBitmap();
    }
}
