using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mandelbrot.Server
{
    public class Request
    {      
        public Client Client { get; set; }
        public int Id { get; }
        public double RealCenter { get; }
        public double ImaginaryCenter { get; }
        public double Width { get; }
        public double Height { get; }
        public double Resolution { get; }
        public int MaxIterations { get; }
        public int MaxMagnitude { get; }

        public double RealLowerBound => RealCenter - Width / 2;
        public double ImaginaryLowerBound => ImaginaryCenter - Height / 2;

        public Request(
            int id,
            double realCenter,
            double imaginaryCenter,
            double width,
            double height,
            double resolution,
            int maxIterations,
            int maxMagnitude)
        {
            Id = id;
            RealCenter = realCenter;
            ImaginaryCenter = imaginaryCenter;
            Width = width;
            Height = height;
            Resolution = resolution;
            MaxIterations = maxIterations;
            MaxMagnitude = maxMagnitude;
        }
        
        public static Request ParseFromBytes(byte[] buffer)
        {
            var reqId = BitConverter.ToInt32(buffer, 0);
            var realCenter = BitConverter.ToDouble(buffer, 4);
            var imaginaryCenter = BitConverter.ToDouble(buffer, 12);
            var width = BitConverter.ToDouble(buffer, 20);
            var height = BitConverter.ToDouble(buffer, 28);
            var resolution = BitConverter.ToDouble(buffer, 36);
            var maxIterations = BitConverter.ToInt32(buffer, 44);
            var maxMagnitude = BitConverter.ToInt32(buffer, 48);  
            
            return new Request(
                reqId,
                realCenter,
                imaginaryCenter,
                width,
                height,
                resolution,
                maxIterations,
                maxMagnitude
            );
        }
    }
}