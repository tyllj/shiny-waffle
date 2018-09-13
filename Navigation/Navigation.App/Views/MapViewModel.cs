using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Navigation.App.Annotations;
using Navigation.App.Graphics;
using Navigation.Routing;
using Xamarin.Forms;
using Forms = Xamarin.Forms;

namespace Navigation.App.Views
{
    public class MapViewModel : INotifyPropertyChanged
    {
        #region private fields
        
        private readonly IRouter _router;
        private readonly IMapRenderer _mapRenderer;

        #endregion
        
        public MapViewModel(IRouter router,
                            IMapRenderer mapRenderer)
        {
            _router = router;
            _mapRenderer = mapRenderer;
            RenderCommand = new YaCommand(imageView => DrawMap((Forms.Image)imageView));
        }
        
        public ICommand RenderCommand { get; }

        #region non-public methods
        
        private void DrawMap(Forms.Image imageView)
        {
            var imageStream = _mapRenderer.Render();
            imageView.Source = ImageSource.FromStream(() => imageStream);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}