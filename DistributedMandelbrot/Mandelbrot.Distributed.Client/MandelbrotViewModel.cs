using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Mandelbrot.Common;
using Mandelbrot.Common.Gui;
using Mandelbrot.Distributed.Client.Annotations;
using Mandelbrot.Distributed.Server;
using Mandelbrot.Offline;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace Mandelbrot.Distributed.Client
{
    public class MandelbrotViewModel : INotifyPropertyChanged
    {
        #region Private Fields


        private int[][] _cachedData;
        private readonly Random _random = new Random();

        #endregion
        
        #region Constructors

        public MandelbrotViewModel()
        {
            RenderCommand = new YaCommand(async _ => await AquireMandelbrotSet());
        }
        
        #endregion
        
        #region Properties

        [UsedImplicitly]
        public string Host { get; set; } = "localhost";

        [UsedImplicitly]
        public int Port { get; set; } = 5555;
        
        [UsedImplicitly]
        public double CenterX { get; set; } = -.5;

        [UsedImplicitly]
        public double CenterY { get; set; } = 0.0;

        [UsedImplicitly]
        public double DistanceX { get; set; } = 3;

        [UsedImplicitly]
        public double DistanceY { get; set; } = 2;
        
        [UsedImplicitly]
        public double Resolution { get; set; } = .01;
        
        [UsedImplicitly]
        public int Threshold { get; set; } = 2;
        
        [UsedImplicitly]
        public int MaxIterations { get; set; } = 85;
        
        #endregion
        
        #region Commands
        
        public ICommand RenderCommand { get; }
        
        #endregion

        #region Public Methods
       
        public Stream ProvideBitmap()
        {
            var stream = new MemoryStream();

            if (_cachedData != null)
            {
                var height = _cachedData.Length;
                var width = _cachedData[0].Length;
                var bitmap = new Bitmap(width, height);
                Log.Info("Rendering bitmap");

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var c = _cachedData[y][x] == -1
                            ? 0
                            : BitmapFormatter.RescaleValue(_cachedData[y][x], MaxIterations);
                        bitmap.SetPixel(x, y, Color.FromRgb(c,c,c));
                    }
                }

                Log.Info("Copying bitmap to output stream");
                bitmap.Save(stream, ImageFormat.Png);
                stream.Flush();
                stream.Position = 0;
            }

            return stream;
        }
        
        #endregion

        #region Non-Public Methods

        private async Task AquireMandelbrotSet()
        {
            using (var server = new Server(Host, Port))
            {
                var request = new Request(
                    _random.Next(),
                    CenterX,
                    CenterY,
                    DistanceX,
                    DistanceY,
                    Resolution,
                    MaxIterations,
                    Threshold
                );
                try
                {
                    await server.SendRequest(request);
                    _cachedData = await server.ReceiveResult();
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    Debugger.Break();
                }

                ResultReady?.Invoke(this, EventArgs.Empty);
            }
        } 

        #endregion

        public event EventHandler ResultReady;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}