namespace Mandelbrot.Distributed.Server
{
    public class Result
    {
        public Result(Request originatingRequest, byte[] data = null)
        {
            OriginatingRequest = originatingRequest;
            Data = data ?? new byte[0];
        }

        public Request OriginatingRequest { get; private set; }
        public byte[] Data { get; set; }
    }
}