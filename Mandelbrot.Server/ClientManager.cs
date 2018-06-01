using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mandelbrot.Server
{
    public class ClientManager
    {
        private TcpListener _listener;

        private List<Client> _clients;
        
        public ClientManager(int port)
        {
            _listener = new TcpListener(IPAddress.Any,5555);
            _clients = new List<Client>();
        }

        public async Task Listen()
        {
            _listener.Start();
            for (;;)
            {
                var mandelbrotClient = await AquireNext();
                _clients.Add(mandelbrotClient);
                ClientAquired?.Invoke(this, mandelbrotClient);
            }      
        }
        
        private async Task<Client> AquireNext()
        {
            var tcpClient = await _listener.AcceptTcpClientAsync();
            return new Client(tcpClient);
        }

        public event EventHandler<Client> ClientAquired;
    }
}