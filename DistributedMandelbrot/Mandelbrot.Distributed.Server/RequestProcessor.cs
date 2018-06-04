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
            //while (client.EndPoint.IsAvailable) // There _is_ no reliable way to check the status of a tcp connection.
                                                  // This is kept for debugging purpurse
            for (;;)
            {
                try
                {
                    await ProcessNextRequest(client);
                }
                catch (OperationCanceledException e)
                {
                    Log.Warn($"Stop listining due to cancel signal: {e.GetType().FullName} - {e.Message}");
                    break;
                }
            }
            Log.Info($"Connection to {client.EndPoint.Host} was closed.");
        }

        private async Task ProcessNextRequest(Client client)
        {
            Log.Info($"Awaiting request message from {client.EndPoint.Host}.");
            Request request;
            try
            {
                request = await client.ReadRequest();
            }
            catch (ObjectDisposedException e)
            {
                throw new OperationCanceledException(e.Message, e);
            }
            catch (IOException e)
            {
                throw new OperationCanceledException(e.Message, e);
            }
            catch (InvalidDataException e)
            {
                throw new OperationCanceledException(e.Message, e);
            }
            
            Log.Info($"Request received from {client.EndPoint.Host}: Id: {request.Id}, R: {request.RealCenter}, I: {request.ImaginaryCenter}");
            Log.Info($"Will send {request.HeightPixels * request.WidthPixels * sizeof(int)} bytes.");
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