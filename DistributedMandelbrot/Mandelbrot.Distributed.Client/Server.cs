using System.Net.Sockets;
using System.Threading.Tasks;
using Mandelbrot.Distributed.Server;

namespace Mandelbrot.Distributed.Client
{
    public class Server
    {
        #region Private fields

        private readonly EndPoint _endPoint;

        #endregion
        
        public Server(string host, int port)
        {
            _endPoint = EndPoint.Connect(host, port);
        }

        public async Task SendRequest(Request request)
        {
            await _endPoint.Send(RequestSerializer.Serialize(request));
        }
        
        
    }
}