using System;
using Mandelbrot.Distributed.Client;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Application = Gtk.Application;

namespace Mandelbrot.Client.Gtk
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
            window.Title = "Mandelbrot Plotter";
            window.Show();

            Application.Run();
        }
    }
}
