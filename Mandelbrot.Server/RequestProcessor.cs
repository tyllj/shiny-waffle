using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Server
{
    public class RequestProcessor
    {
        private MandelbrotProcessor _mandelbrotProcessor;

        public RequestProcessor(MandelbrotProcessor mandelbrotProcessor)
        {
            _mandelbrotProcessor = mandelbrotProcessor;
        }

        public async Task ServeNewClient(Client client)
        {
            Log.Info($"Client connected: {client.Host}." );
            Log.Info("Awaiting request message...");
            var request = await client.ReadRequest();
            
            Log.Info("Request received.");
            var fractal = await Task.Run(() => _mandelbrotProcessor.DrawFractal(
                request.RealLowerBound,
                request.Width, 
                request.ImaginaryLowerBound,
                request.Height, 1/request.Resolution));

            var data = fractal.SelectMany(values => values)
                .SelectMany(value => BitConverter.GetBytes(value))
                .ToArray();

            Log.Info("Calculated data:");
            Log.Info(string.Join(',', data));

            var result = new Result(request, data);
            Log.Info($"Sending... result for {result.OriginatingRequest.Id} to client {client.Host}.");

            try
            {
                await client.SendResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Debugger.Break();
            }
        } 
    }
}