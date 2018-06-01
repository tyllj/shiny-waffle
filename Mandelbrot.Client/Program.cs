using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security;

namespace Mandelbrot.Client
{
    class Program
    
    
    {
        static void Main(string[] args)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 5555);
            var tcpStream = tcpClient.GetStream();
            var streamWriter = new BinaryWriter(tcpStream);
            streamWriter.Write(0);
            streamWriter.Write(-2.0);
            streamWriter.Write(-1.0);
            streamWriter.Write(3.0);
            streamWriter.Write(2.0);
            streamWriter.Write(.1);
            streamWriter.Write(85);
            streamWriter.Write(2);
            streamWriter.Flush();

            var buffer = new byte[4];

            try
            {
                for (;;)
                {
                    tcpClient.GetStream().Read(buffer, 0, 4);
                    Console.WriteLine(BitConverter.ToInt32(buffer, 0));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}