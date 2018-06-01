using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot {
    public static class BitmapHelper
    {
        public static byte[] GeneratePortableGraymap(bool[][] bitmap) {
            var imageWidth = bitmap[0].Length;
            var imageHeight = bitmap.Length;
            var data = new List<byte>();
            var header = $"P5 {imageWidth} {imageHeight} 255\n";
            data.AddRange(Encoding.UTF8.GetBytes(header));
            data.AddRange(bitmap.SelectMany(x => x.Select(v => v ? Convert.ToByte(0) : Convert.ToByte(255))));
            return data.ToArray();
        }
        
        public static byte[] GeneratePortableGraymap(int[][] bitmap) {
            var imageWidth = bitmap[0].Length;
            var imageHeight = bitmap.Length;
            var data = new List<byte>();
            var header = $"P5 {imageWidth} {imageHeight} 255\n";
            data.AddRange(Encoding.UTF8.GetBytes(header));
            data.AddRange(bitmap.SelectMany(x => x.Select(v => v == -1 ? Convert.ToByte(0) : Convert.ToByte(v/85*254))));
            return data.ToArray();
        }
    }
}