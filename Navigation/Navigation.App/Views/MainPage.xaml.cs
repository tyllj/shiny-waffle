using System;
using Xamarin.Forms;

namespace Navigation.App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainWindowViewModel mainWindowViewModel)
        {
            ViewModel = mainWindowViewModel;
            BindingContext = ViewModel;
            ViewModel.MapChanged += (_,__) => UpdateMap();
            
            InitializeComponent();
            UpdateMap();
        }

        private void UpdateMap()
        {
            _resultView.Source = ImageSource.FromStream(() => ViewModel.DrawMap());
        }

        public MainWindowViewModel ViewModel { get; }
    }
}
