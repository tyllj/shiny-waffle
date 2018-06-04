using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Distributed.Server
{
    public class Client : IDisposable
    {
        #region Private Fields

        private EndPoint _endPoint;
        
        #endregion

        #region Constructors
        
        public Client(TcpClient tcpClient)
        {
            _endPoint = new EndPoint(tcpClient);
        }

        #endregion

        #region Properties

        public EndPoint EndPoint => _endPoint;

        #endregion
        
        
        #region Public Methods
        
        public async Task<Request> ReadRequest()
        {
            var rawRequest = await _endPoint.Receive(52);
            Log.Info($"Data received from {_endPoint.Host}");
            var request = RequestSerializer.Deserialize(rawRequest);
            return request;
        }

        public async Task SendResult(Result result)
        {
            await _endPoint.Send(result.OriginatingRequest.Id);
            await _endPoint.Send(result.Data);
        }
        
        #endregion

        public void Dispose()
        {
            _endPoint?.Dispose();
        }
    }
}