using System;
using System.IO;

namespace Mandelbrot.Distributed.Server
{
    public static class RequestSerializer
    {
        public static Request Deserialize(byte[] data)
        {
            var reqId = BitConverter.ToInt32(EndianConverter.FromNetEncoding(data, 0, sizeof(int)), 0);
            var realCenter = BitConverter.ToDouble(EndianConverter.FromNetEncoding(data, 4 , sizeof(double)), 0);
            var imaginaryCenter = BitConverter.ToDouble(EndianConverter.FromNetEncoding(data, 12, sizeof(double)), 0);
            var width = BitConverter.ToDouble(EndianConverter.FromNetEncoding(data, 20, sizeof(double)), 0);
            var height = BitConverter.ToDouble(EndianConverter.FromNetEncoding(data, 28, sizeof(double)), 0);
            var resolution = Quirks.FromMirkosResolution(BitConverter.ToDouble(EndianConverter.FromNetEncoding(data, 36, sizeof(double)), 0));
            var maxIterations = BitConverter.ToInt32(EndianConverter.FromNetEncoding(data, 44, sizeof(int)), 0);
            var maxMagnitude = BitConverter.ToInt32(EndianConverter.FromNetEncoding(data, 48, sizeof(int)), 0);  
            
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
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.Id));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.RealCenter));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.ImaginaryCenter));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.Width));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.Height));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(Quirks.ToMirkosResolution(request.Resolution)));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.MaxIterations));
                    binaryWriter.Write(EndianConverter.ToNetEncoding(request.MaxMagnitude));
                    binaryWriter.Flush();
                }
                memoryStream.GetBuffer().CopyTo(buffer, 0);
            }

            return buffer;
        }
    }
}