using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandelbrot.Distributed.Server;

namespace Mandelbrot.Distributed.Client
{
    public class Server
    {
        #region Private fields

        private readonly EndPoint _endPoint;
        private readonly List<Request> _pendingRequests;
        
        #endregion
        
        public Server(string host, int port)
        {
            _pendingRequests = new List<Request>();
            _endPoint = EndPoint.Connect(host, port);
        }
        
        public async Task SendRequest(Request request)
        {
            _pendingRequests.Add(request);
            await _endPoint.Send(RequestSerializer.Serialize(request));
        }

        public async Task<int[][]> ReceiveResult()
        {
            var requestId = BitConverter.ToInt32(await _endPoint.Receive(sizeof(int)), 0);
            var request = _pendingRequests.FirstOrDefault(r => r.Id == requestId);
            if (request is null)
            {
                throw new IndexOutOfRangeException($"Request with id {requestId} cannot be resolved.");
            }
            

            var resultSet = new int[request.HeightPixels][];
            for (var y = 0; y < resultSet.Length; y++)
            {
                resultSet[y] = new int[request.WidthPixels];
                for (var x = 0; x < resultSet[y].Length; x++)
                {
                    var rawValue = await _endPoint.Receive(sizeof(int));
                    var value = BitConverter.ToInt32(rawValue, 0);
                    resultSet[y][x] = value;
                }
            }

            return resultSet;
        }

        

    }
}