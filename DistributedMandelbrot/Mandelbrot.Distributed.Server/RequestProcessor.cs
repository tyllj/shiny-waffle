using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Distributed.Server
{
    public class RequestProcessor
    {
        private MandelbrotProcessor _mandelbrotProcessor;

        public RequestProcessor(MandelbrotProcessor mandelbrotProcessor)
        {
            _mandelbrotProcessor = mandelbrotProcessor;
        }

        /// <summary>
        /// Repeatatly  wait for requests from a client and process them until connection is closed.
        /// </summary>
        public async Task ServeClient(Client client)
        {
            while (client.EndPoint.IsAvailable)
            {
                try
                {
                    await ProcessNextRequest(client);
                }
                catch (ObjectDisposedException e)
                {
                    break;
                }
                catch (IOException e)
                {
                    break;
                }
            }
            Log.Info($"Connection to {client.EndPoint.Host} was closed.");
        }

        private async Task ProcessNextRequest(Client client)
        {
            Log.Info($"Client connected: {client.EndPoint.Host}.");
            Log.Info("Awaiting request message...");
            var request = await client.ReadRequest();

            Log.Info("Request received.");
            var fractal = await Task.Run(() => _mandelbrotProcessor.DrawFractal(
                request.RealLowerBound,
                request.Width,
                request.ImaginaryLowerBound,
                request.Height,
                1 / request.Resolution,
                request.MaxMagnitude,
                request.MaxIterations));

            var data = fractal.SelectMany(values => values)
                .SelectMany(value => BitConverter.GetBytes(value))
                .ToArray();

            Log.Info("Calculated data:");
            Log.Info(string.Join(',', data));

            var result = new Result(request, data);
            Log.Info($"Sending result for {result.OriginatingRequest.Id} to client {client.EndPoint.Host}.");

            try
            {
                await client.SendResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}