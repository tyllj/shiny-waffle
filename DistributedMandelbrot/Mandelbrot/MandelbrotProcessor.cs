using System;
using System.Threading.Tasks;

namespace Mandelbrot 
{
    public class MandelbrotProcessor
    {
        /// <summary>
        /// Returns -1 if sequence zn+1=zn²+c where z0 = 0
        /// </summary>
        private int PointConverges(ComplexDouble c, double magnitudeThreshold, int maxIterations)
        {
            ComplexDouble z = ComplexDouble.Zero; 
            int i;
            for (i = maxIterations; i > 0; i--)
            {
                z = z * z + c;
                if (z > magnitudeThreshold)
                {
                    break;
                }
            }
            if (z <= magnitudeThreshold)
            {
                return -1;
            }
            return i;
        }

        public int[][] DrawFractal(
            double rLowerBound,
            double rDistance,
            double iLowerBound,
            double iDistance,
            double dotsPerUnit,
            double magnitureThreshold,
            int maxIterations)
        {
            var yDots = (int)Math.Ceiling(iDistance*dotsPerUnit);
            var xDots = (int)Math.Ceiling(rDistance*dotsPerUnit);
            var increment = 1/dotsPerUnit;
            var yValues = new int[yDots][];
            Parallel.For(0, yDots, y =>
            //for (var y = 0; y < yDots; y++) 
            {
                var xValues = new int[xDots];
                Parallel.For(0, xDots, x => 
                //for (var x = 0; x < xDots; x++)
                {
                    var r = rLowerBound + x * increment;
                    var i = iLowerBound + y * increment;
                    xValues[x] = PointConverges(new ComplexDouble(r, i), magnitureThreshold, maxIterations);
                });

                yValues[y] = xValues;
            });
            
            return yValues;
        }        
    }
}