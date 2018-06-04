using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Distributed.Server
{
    public class ClientManager
    {
        #region Private Fields
        
        private TcpListener _listener;
        private List<Client> _clients;
        
        #endregion
        
        #region Constructors
        
        public ClientManager(int port)
        {
            _listener = new TcpListener(IPAddress.Any,5555);
            _clients = new List<Client>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts incomming client connections and invokes assigned handlers.
        /// </summary>
        /// <returns></returns>
        public async Task Listen()
        {
            _listener.Start();
            try
            {
                for(;;)
                {
                    var mandelbrotClient = await AquireNext();
                    Log.Info($"Connection stablished with {mandelbrotClient.EndPoint.Host}.");
                    _clients.Add(mandelbrotClient);
                    ClientAquired?.Invoke(this, mandelbrotClient);
                }
            }
            finally
            {
                _listener.Stop();
            }
        }

        #endregion
        
        #region Non-public Methods
        
        private async Task<Client> AquireNext()
        {
            var tcpClient = await _listener.AcceptTcpClientAsync();
            return new Client(tcpClient);
        }

        #endregion
        
        #region Events
        
        public event EventHandler<Client> ClientAquired;
        
        #endregion
    }
}