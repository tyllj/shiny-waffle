using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Distributed.Server
{
    public interface IEndPoint
    {
        string Host { get; }
        bool IsAvailable { get; }
        Task Send(dynamic data);
        Task<byte[]> Receive(int count);
    }

    public class EndPoint : IEndPoint
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
        public bool IsAvailable
        {
            get { var state = _tcpClient.GetState();
                Log.Debug($"OS connection state for {Host} is {state}");
                return state == TcpState.Established; }
        }

        public virtual async Task Send(dynamic data)
        {
            await Task.Run(() =>
            {
                _writer.Write(data);
                _writer.Flush();
            });
            if (IsAvailable);
        }

        public virtual async Task<byte[]> Receive(int count = 1)
        {
            var buffer = new byte[count];
            await _stream.ReadAsync(buffer, 0, count);
            
            return buffer;
        } 
    }
    
    
}