using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Mandelbrot.Distributed.Server;
using NUnit.Framework;
namespace Mandelbrot.Test
{
    [TestFixture]
    public class ServerTests
    {
        
        [Test]
        [Explicit]
        public void SendClientRequest()
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 5555);
            var tcpStream = tcpClient.GetStream();
            var streamWriter = new BinaryWriter(tcpStream);
            streamWriter.Write(0);    // id
            streamWriter.Write(0.0);  // r center pos
            streamWriter.Write(0.0);  // i center pos
            streamWriter.Write(3.0);  // width
            streamWriter.Write(2.0);  // height
            streamWriter.Write(.1);   // distance of points
            streamWriter.Write(85);   // iterations
            streamWriter.Write(2);    // threshold
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

        [Test]
        public void SerializeDeserializeTest()
        {
            var id = 3;
            var rCenterPos = -0.15;
            var iCenterPos = 0.0;
            var width = 2.75;
            var height = 1.5;
            var pointsDistance = 0.276666;
            var iterations = Int32.MaxValue;
            var threshold = 2;
                
            
            MemoryStream stream = new MemoryStream(52);
            var streamWriter = new BinaryWriter(stream);
            streamWriter.Write(id);    // id
            streamWriter.Write(rCenterPos);  // r center pos
            streamWriter.Write(iCenterPos);  // i center pos
            streamWriter.Write(width);  // width
            streamWriter.Write(height);  // height
            streamWriter.Write(pointsDistance);   // distance of points
            streamWriter.Write(iterations);   // iterations
            streamWriter.Write(threshold);    // threshold
            streamWriter.Flush();

            //stream.Read(buffer, 0, 52);
            var buffer = stream.GetBuffer();
            
            var request = RequestSerializer.Deserialize(buffer);
            Assert.AreEqual(id, request.Id);
            Assert.AreEqual(iCenterPos, request.ImaginaryCenter);
            Assert.AreEqual(rCenterPos, request.RealCenter);
            Assert.AreEqual(width, request.Width);
            Assert.AreEqual(height, request.Height);
            Assert.AreEqual(pointsDistance, request.Resolution);
            Assert.AreEqual(iterations, request.MaxIterations);
            Assert.AreEqual(threshold, request.MaxMagnitude);

        }
    }
}