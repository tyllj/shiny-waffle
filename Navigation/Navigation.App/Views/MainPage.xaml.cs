using Xamarin.Forms;

namespace Navigation.App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MapViewModel mapViewModel)
        {
            ViewModel = mapViewModel;
            BindingContext = ViewModel;
            InitializeComponent();
            
            ViewModel.RenderCommand.Execute(_resultView);
        }
        
        public MapViewModel ViewModel { get; }
    }
}
