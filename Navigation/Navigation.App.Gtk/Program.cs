using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Application = Gtk.Application;

namespace Navigation.App.Gtk
{

    public class GtkApplication
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.Init();
            Forms.Init();

            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.Title = app.MainPage.Title;
            window.Show();

            Application.Run();
        }
    }
    
}