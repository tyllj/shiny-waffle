using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mandelbrot.Common;
using Mandelbrot.Distributed.Server;

namespace Mandelbrot.Distributed.Client
{
    /// <summary>
    /// Provides means of interaction with a Mandelbrot server. 
    /// </summary>
    public class Server : IDisposable
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
            var requestId = BitConverter.ToInt32(RequestSerializer.FromNetEncoding(await _endPoint.Receive(sizeof(int)), 0, sizeof(int)), 0);
            var request = _pendingRequests.FirstOrDefault(r => r.Id == requestId);
            if (request is null)
            {
                throw new IndexOutOfRangeException($"Request with id {requestId} cannot be resolved.");
            }
            
            var resultSize = request.ResultPixelCount * sizeof(int);
            Log.Debug($"Receiving, expect {resultSize} bytes");
            var stopwatch = Stopwatch.StartNew();
            
            var resultBuffer = await _endPoint.Receive(request.WidthPixels * request.HeightPixels * sizeof(int));
            Log.Info($"Received result in {stopwatch.ElapsedMilliseconds} ms.");
            stopwatch.Restart();

            var resultSet = DeserializeResult(request, resultBuffer);
            Log.Info($"Deserialized result in {stopwatch.ElapsedMilliseconds} ms.");
            stopwatch.Stop();
            
            return resultSet;
        }

        private static int[][] DeserializeResult(Request request,byte[] resultBuffer)
        {
            var resultSet = new int[request.HeightPixels][];
            int IndexInRawBuffer(int x, int y) => y * request.WidthPixels * sizeof(int) + x * sizeof(int);
            for (var y = 0; y < resultSet.Length; y++)
            {
                resultSet[y] = new int[request.WidthPixels];
                for (var x = 0; x < resultSet[y].Length; x++)
                {
                    try
                    {
                        var value = BitConverter.ToInt32(RequestSerializer.FromNetEncoding(resultBuffer, IndexInRawBuffer(x, y), sizeof(int)), 0);
                        resultSet[y][x] = value;
                    }
                    catch (Exception e)
                    {
                        Debugger.Break();
                    }
                }
            }

            return resultSet;
        }

        public void Dispose()
        {
            _endPoint?.Dispose();
        }
    }
}