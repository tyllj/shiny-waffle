using System;
using System.IO;

namespace Mandelbrot.Distributed.Server
{
    public static class RequestSerializer
    {
        public static Request Deserialize(byte[] data)
        {
            var reqId = BitConverter.ToInt32(FromNetEncoding(data, 0, sizeof(int)), 0);
            var realCenter = BitConverter.ToDouble(FromNetEncoding(data, 4 , sizeof(double)), 0);
            var imaginaryCenter = BitConverter.ToDouble(FromNetEncoding(data, 12, sizeof(double)), 0);
            var width = BitConverter.ToDouble(FromNetEncoding(data, 20, sizeof(double)), 0);
            var height = BitConverter.ToDouble(FromNetEncoding(data, 28, sizeof(double)), 0);
            var resolution = 1/(1 / BitConverter.ToDouble(FromNetEncoding(data, 36, sizeof(double)), 0) + 1);
            var maxIterations = BitConverter.ToInt32(FromNetEncoding(data, 44, sizeof(int)), 0);
            var maxMagnitude = BitConverter.ToInt32(FromNetEncoding(data, 48, sizeof(int)), 0);  
            
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
                    binaryWriter.Write(ToNetEncoding(request.Id));
                    binaryWriter.Write(ToNetEncoding(request.RealCenter));
                    binaryWriter.Write(ToNetEncoding(request.ImaginaryCenter));
                    binaryWriter.Write(ToNetEncoding(request.Width));
                    binaryWriter.Write(ToNetEncoding(request.Height));
                    binaryWriter.Write(ToNetEncoding(1/(1/request.Resolution-1)));
                    binaryWriter.Write(ToNetEncoding(request.MaxIterations));
                    binaryWriter.Write(ToNetEncoding(request.MaxMagnitude));
                    binaryWriter.Flush();
                }
                memoryStream.GetBuffer().CopyTo(buffer, 0);
            }

            return buffer;
        }

        public static byte[] FromNetEncoding(byte[] buffer, int offset, int size)
        {
            byte[] result = new byte[size];
            Array.ConstrainedCopy(buffer, offset, result, 0, size);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }

        public static byte[] ToNetEncoding(dynamic value)
        {
            var raw = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(raw);
            }

            return raw;
        }
    }
}