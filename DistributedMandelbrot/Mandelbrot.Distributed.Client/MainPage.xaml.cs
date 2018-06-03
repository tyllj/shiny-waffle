using System;
using System.Diagnostics;
using Mandelbrot.Common;
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
            try
            {
                _image.Source = ImageSource.FromStream(() => ViewModel.ProvideBitmap());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                Debugger.Break();
            }

        }

        public MandelbrotViewModel ViewModel { get; }
        
        
    }
}
