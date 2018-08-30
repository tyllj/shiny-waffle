using System;

namespace Mandelbrot.Distributed.Server
{
    public static class EndianConverter
    {
        public static byte[] FromNetEncoding(byte[] buffer, int offset, int size)
        {
            byte[] result = new byte[size];
            Array.Copy(buffer, offset, result, 0, size);
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