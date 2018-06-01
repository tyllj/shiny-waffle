using System;
using System.Linq;

namespace Mandelbrot 
{
    public class MandelbrotProcessor
    {
        private int PointConverges(ComplexDouble c, Double MagnitudeThreshold, int MaxIterations)
        {
            
            ComplexDouble z = c;
            int i;
            for (i = 0; i <= MaxIterations; i++)
            {
                z = z * z + c;
                if (z > MagnitudeThreshold)
                {
                    break;
                }
            }
            if (z <= MagnitudeThreshold)
            {
                return -1;
            }
            return i;
        }

        public int[][] DrawFractal(double rLowerBound, double rDistance, double iLowerBound, double iDistance, double dotsPerUnit){
            var yDots = (int)Math.Ceiling(iDistance*dotsPerUnit);
            var xDots = (int)Math.Ceiling(rDistance*dotsPerUnit);
            var increment = 1/dotsPerUnit;
            var yValues = new int[yDots][];
            for (var y = 0; y < yDots; y++) {
                var xValues = new int[xDots];
                for (var x = 0; x < xDots; x++){
                    var r = rLowerBound + x*increment;
                    var i = iLowerBound + y*increment;
                    xValues[x] = PointConverges(new ComplexDouble(r,i),2,85);
                }
                yValues[y]=xValues;
            }
            return yValues;
        }        
    }
}