using System.Threading.Tasks;
using Mandelbrot.Common;

namespace Mandelbrot.Distributed.Server
{
    static class Program
    {
        private static ClientManager _clientManager;

        private static MandelbrotProcessor _mandelbrotProcessor;

        private static RequestProcessor _requestProcessor;

        private static void Bootstrap(string[] args)
        {
            _clientManager = new ClientManager(5555);
            _mandelbrotProcessor = new MandelbrotProcessor();
            _requestProcessor = new RequestProcessor(_mandelbrotProcessor);
        }
        
        private static async Task Main(string[] args)
        {
            Bootstrap(args);
            _clientManager.ClientAquired += async (sender, client) => await _requestProcessor.ServeClient(client);
            
            Log.Info("Server is starting, press Ctrl+C to terminate.");
            var listenerTask = Task.Run(() => _clientManager.Listen());

            await listenerTask;
        }
    }
}