using System;
using System.IO;
using Mandelbrot.Common;

namespace Mandelbrot.Offline
{
    class Program
    {
        private static void Main(string[] args)
        {
            Log.Info($"Application started with args: {string.Join(',', args)}");
            var mandelbrot = new MandelbrotProcessor();
            Log.Info($"Calculating mandelbrot set...");
            var fractalArray = mandelbrot.DrawFractal(-2, 3, -1, 2, 1000, 2, 85);
            Log.Info($"Converting to portable gray map...");
            var fractalGrayMap = BitmapFormatter.GeneratePortableGraymap(fractalArray, 85);
            Log.Info($"Writing to file...");
            using (var fs = new FileStream(args[Array.IndexOf(args, "-o") + 1], FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(fractalGrayMap);
                    bw.Flush();
                }
            }
        }
    }
}