using System;
using System.IO;

namespace Mandelbrot.Distributed.Server
{
    public static class RequestSerializer
    {
        public static Request Deserialize(byte[] data)
        {
            var reqId = BitConverter.ToInt32(data, 0);
            var realCenter = BitConverter.ToDouble(data, 4);
            var imaginaryCenter = BitConverter.ToDouble(data, 12);
            var width = BitConverter.ToDouble(data, 20);
            var height = BitConverter.ToDouble(data, 28);
            var resolution = BitConverter.ToDouble(data, 36);
            var maxIterations = BitConverter.ToInt32(data, 44);
            var maxMagnitude = BitConverter.ToInt32(data, 48);  
            
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

        public static byte[] Serialize(Request request)
        {
            var buffer = new byte[52];
            using (var memoryStream = new MemoryStream(52))
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(request.Id);
                    binaryWriter.Write(request.RealCenter);
                    binaryWriter.Write(request.ImaginaryCenter);
                    binaryWriter.Write(request.Width);
                    binaryWriter.Write(request.Height);
                    binaryWriter.Write(request.Resolution);
                    binaryWriter.Write(request.MaxIterations);
                    binaryWriter.Write(request.MaxMagnitude);
                    binaryWriter.Flush();
                }
                memoryStream.GetBuffer().CopyTo(buffer, 0);
            }

            return buffer;
        }
    }
}