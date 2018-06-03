using Xamarin.Forms;


namespace Mandelbrot.Distributed.Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = new MandelbrotViewModel();
            ViewModel.ResultReady += (_,__) => UpdateResultView();
            BindingContext = ViewModel;
            InitializeComponent();
        }

        public MandelbrotViewModel ViewModel { get; }
        
        private void UpdateResultView()
        {
            _resultView.Source = ImageSource.FromStream(() => ViewModel.ProvideBitmap());
        }
        
    }
}
