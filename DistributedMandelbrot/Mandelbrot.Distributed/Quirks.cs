namespace Mandelbrot.Distributed.Server
{
    public static class Quirks
    {
        public static double FromMirkosResolution(double resolution)
        {
            return 1 / (1 / resolution + 1);
        }

        public static double ToMirkosResolution(double resolution)
        {
            return  1 / (1 / resolution - 1);
        }
    }
}