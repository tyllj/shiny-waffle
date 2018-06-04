using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot.Offline {
    public static class BitmapFormatter
    {
        private const string PGM_MAGIC_NUMBER = "P5";
        private const byte PGM_MAXVALUE = 255;
        private const byte PGM_MINVALUE = 0;
        
        public static byte[] GeneratePortableGraymap(int[][] intGrid, int valueScale) {
            var imageWidth = intGrid[0].Length;
            var imageHeight = intGrid.Length;
            var header = Encoding.UTF8.GetBytes($"{PGM_MAGIC_NUMBER} {imageWidth} {imageHeight} {PGM_MAXVALUE}\n");
            var data = new List<byte>();
            data.AddRange(header);
            data.AddRange(
                intGrid.SelectMany(
                rows => rows.Select(v => v == -1 ? PGM_MINVALUE
                                                 : RescaleInverted(v, valueScale))));
            return data.ToArray();
        }

        public static byte RescaleValue(int value, int valueScale)
        {
            return (byte) ((double) value / valueScale * PGM_MAXVALUE);
        }
        
        public static byte RescaleInverted(int value, int valueScale)
        {
            return (byte) (PGM_MAXVALUE - RescaleValue(value, valueScale));
        }
    }
}