using Xamarin.Forms;

namespace Mandelbrot.Distributed.Client

{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = new MandelbrotViewModel();
            BindingContext = ViewModel;
            InitializeComponent();
        }
        
        public MandelbrotViewModel ViewModel { get; }
    }
}
