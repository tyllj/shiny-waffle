using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Server
{
    public class Client
    {
        #region Private Fields

        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _networkStream;
        private readonly BinaryWriter _writer;

        #endregion

        #region Constructors
        
        public Client(TcpClient client)
        {
            _tcpClient = client;
            _networkStream = _tcpClient.GetStream();
            _writer = new BinaryWriter(_networkStream);
        }
        
        #endregion

        #region Properties

        public string Host => _tcpClient.Client.RemoteEndPoint.ToString();
        public bool IsAvailable => _tcpClient.GetState() == TcpState.Established;
        #endregion
        
        #region Public Methods
        
        public async Task<Request> ReadRequest()
        {
            var buffer = new Byte[52];

            await _tcpClient.GetStream().ReadAsync(buffer, 0, 52);
            if (!IsAvailable) // NetworkStream reads zero for closed connections.
            {
                throw new IOException("Connection was closed unexpectedly.");
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
        
        #endregion
    }
}