using System;

namespace Mandelbrot.Common
{
    public static class Log
    {
        public static void Info(string message)
        {
            Console.WriteLine($"[Info]  {DateTime.Now.ToShortTimeString()} - {message}");
        }
        
        public static void Warn(string message)
        {
            Console.WriteLine($"[Warn]  {DateTime.Now.ToShortTimeString()} - {message}");
        }
        
        public static void Error(string message)
        {
            Console.WriteLine($"[Error] {DateTime.Now.ToShortTimeString()} - {message}");
        }
    }
}