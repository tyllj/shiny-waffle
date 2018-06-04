using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Mandelbrot.Distributed.Server
{
    public interface IEndPoint
    {
        string Host { get; }
        Task Send(dynamic data);
        Task<byte[]> Receive(int count);
    }

    public class EndPoint : IEndPoint, IDisposable
    {
        #region Private Fields

        protected readonly TcpClient _tcpClient;
        protected readonly NetworkStream _stream;
        protected readonly BinaryWriter _writer;

        #endregion

        public static EndPoint Connect(string host, int port)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(host, port);
            return new EndPoint(tcpClient);
        }
      
        public EndPoint(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _stream = _tcpClient.GetStream();
            _writer = new BinaryWriter(_stream);
        }

        public string Host => _tcpClient.Client.RemoteEndPoint.ToString();

        public virtual async Task Send(dynamic data)
        {
            await Task.Run(() =>
            {
                _writer.Write(data);
                _writer.Flush();
            });
        }

        public virtual async Task<byte[]> Receive(int count = 1)
        {
            var buffer = new byte[count];
            int readSoFar = 0;
            while(readSoFar < count)
            {
                var read = await _stream.ReadAsync(buffer, readSoFar, count - readSoFar);
                readSoFar += read;
                if (read == 0)
                    throw new IOException("Read zero bytes, assuming connection is closed.");
            }
            
            return buffer;
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
            _stream?.Dispose();
            _writer?.Dispose();
        }
    }
    
    
}