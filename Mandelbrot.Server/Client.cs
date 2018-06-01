using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Mandelbrot.Server
{
    public class Client
    {
        private readonly TcpClient _tcpClient;

        public Client(TcpClient client)
        {
            _tcpClient = client;
            _networkStream = _tcpClient.GetStream();
            _writer = new BinaryWriter(_networkStream);
        }

        private NetworkStream _networkStream;

        private BinaryWriter _writer;

        public string Host => _tcpClient.Client.RemoteEndPoint.ToString();
        
        public async Task<Request> ReadRequest()
        {
            var buffer = new Byte[52];
            try
            {
                await _tcpClient.GetStream().ReadAsync(buffer, 0, 52);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debugger.Break();
            }

            var request = Request.ParseFromBytes(buffer);
            request.Client = this;
            return request;
        }

        public async Task SendResult(Result result)
        {
            await Task.Run(() =>
            {
                _writer.Write(result.OriginatingRequest.Id);
                _writer.Write(result.Data);
                _writer.Flush();
            });
            
        }
    }
}