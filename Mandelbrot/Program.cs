using System;
using System.IO;

namespace Mandelbrot
{
    class Program
    {
        static void Main(string[] args)
        {
            var mandelbrot = new MandelbrotProcessor();
            var fractalBoolArray = mandelbrot.DrawFractal(-2,3,-1,2,1000);
            var fractalGrayMap = BitmapHelper.GeneratePortableGraymap(fractalBoolArray);
            var bw = new BinaryWriter(Console.OpenStandardOutput());
            foreach (var b in fractalGrayMap) {
                bw.Write(b);
            }
            bw.Flush();
        }
    }
}