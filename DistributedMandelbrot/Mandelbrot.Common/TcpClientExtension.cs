using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Mandelbrot.Common
{
    public static class TcpClientExtension
    {
        public static TcpState GetState(this TcpClient tcpClient)
        {
            var tcpConnections = IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpConnections();
            var tcpConnection = tcpConnections
                .FirstOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
            return tcpConnection?.State ?? TcpState.Unknown;
        }
    }
}