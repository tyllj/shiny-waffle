using System;
using System.Diagnostics;
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
                catch (OperationCanceledException)
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
            Request request;
            try
            {
                request = await client.ReadRequest();
            }
            catch (ObjectDisposedException)
            {
                throw new OperationCanceledException();
            }
            catch (IOException)
            {
                throw new OperationCanceledException();
            }
            catch (InvalidDataException)
            {
                throw new OperationCanceledException();
            }
            
            Log.Info("Request received.");

            var stopwatch = Stopwatch.StartNew();
            var fractal = await Task.Run(() => _mandelbrotProcessor.DrawFractal(
                request.RealLowerBound,
                request.Width,
                request.ImaginaryLowerBound,
                request.Height,
                1 / request.Resolution,
                request.MaxMagnitude,
                request.MaxIterations));
            Log.Info($"Calculated mandelbrot set in {stopwatch.ElapsedMilliseconds} ms.");
            stopwatch.Restart();
            
            var data = fractal.SelectMany(values => values)
                .SelectMany(value => BitConverter.GetBytes(value))
                .ToArray();
            Log.Info($"Serialized mandelbrot set in {stopwatch.ElapsedMilliseconds} ms.");
            stopwatch.Stop();

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