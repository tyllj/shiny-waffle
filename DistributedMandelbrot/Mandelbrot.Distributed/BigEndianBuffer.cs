using System;

namespace Mandelbrot.Distributed.Server
{
    public class BigEndianBuffer
    {
        private byte[] _internalBuffer;

        public BigEndianBuffer(byte[] buffer)
        {
            _internalBuffer = new byte[buffer.LongLength];
            Array.Copy(buffer, _internalBuffer, buffer.LongLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
        }
    }
}