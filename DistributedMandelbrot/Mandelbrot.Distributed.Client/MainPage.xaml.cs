using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mandelbrot.Distributed.Client

{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = new MandelbrotViewModel();
            ViewModel.ImageLoaded += ViewModelOnImageLoaded;
            BindingContext = ViewModel;
            InitializeComponent();
        }

        private void ViewModelOnImageLoaded (object sender, EventArgs e)
        {            
            
            _image.Source = ImageSource.FromStream(() => ViewModel.ProvideBitmap());

        }

        public MandelbrotViewModel ViewModel { get; }
        
        
    }
}
