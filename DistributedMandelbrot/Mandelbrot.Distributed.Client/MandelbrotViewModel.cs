using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Mandelbrot.Common;
using Mandelbrot.Common.Gui;
using Mandelbrot.Distributed.Client.Annotations;
using Mandelbrot.Distributed.Server;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace Mandelbrot.Distributed.Client
{
    public class MandelbrotViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private Server _server;
        private int[][] _cachedData;
        private readonly ImageSource _plotBitmapSource;

        #endregion
        
        #region Constructors

        public MandelbrotViewModel()
        {
            RenderCommand = new YaCommand(async _ => await AquireAndPresentMandelbrotSet());
            _plotBitmapSource = ImageSource.FromStream(() => ProvideBitmap());
        }

        public Stream ProvideBitmap()
        {
            var bitmap = new Bitmap(480, 320);
            try
            {
                for (int x = 0; x < _cachedData[0].Length; x++)
                {
                    for (int y = 0; y < _cachedData.Length; y++)
                    {
                        var c = _cachedData[y][x] == -1
                            ? Color.White
                            : Color.Black;
                        bitmap.SetPixel(x, y, c);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Debugger.Break();
            }

            Stream stream = new MemoryStream();

            try
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Flush();
                stream.Position = 0;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Debugger.Break();
            }

            return stream;
        }

        #endregion
        
        #region Public Properties

        public ImageSource PlotBitmapSource => _plotBitmapSource;

        public double CenterX { get; set; } = -0.5;

        public double CenterY { get; set; } = 0.0;

        public double DistanceX { get; set; } = 3;

        public double DistanceY { get; set; } = 2;
        
        public double Resolution { get; set; } = .01;

        public int Threshold { get; set; } = 2;

        public int MaxIterations { get; set; } = 85;
        
        #endregion
        
        #region Commands
        
        public ICommand RenderCommand { get; }
        
        #endregion

        #region Public Methods

        

        #endregion

        #region Non-Public Methods

        private async Task AquireAndPresentMandelbrotSet()
        {
            if (_server == null)
                _server = new Server("127.0.0.1", 5555);
            
            var request = new Request(
                0,
                CenterX,
                CenterY,
                DistanceX,
                DistanceY,
                Resolution,
                MaxIterations,
                Threshold
                );
            await _server.SendRequest(request);
            _cachedData = await _server.ReceiveResult();
            ImageLoaded?.Invoke(this, EventArgs.Empty);
        } 

        #endregion

        public event EventHandler ImageLoaded;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}