using Autofac;
using Navigation.App.Graphics;
using Navigation.App.Views;
using Navigation.Routing;

namespace Navigation.App
{
    public class Bootstrapper
    {
        private const string MAP_XML_PATH =
            @"/media/tyll/personal/DEV/shiny-waffle/Navigation/Navigation/daten_hl_altstadt_routenplaner_koordinaten.xml";
        
        private static Bootstrapper _instance;
        private readonly IContainer _container;
        
        public static Bootstrapper Create()
        {
            return _instance ?? (_instance = new Bootstrapper());
        }

        public MainPage GetMainPage()
        {
            return _container.Resolve<MainPage>();
        }
        
        private Bootstrapper()
        {
            var builder = new ContainerBuilder();
            SetUp(builder);
            
            _container = builder.Build();
        }

        private void SetUp(ContainerBuilder builder)
        {
            builder.RegisterType<MainPage>();
            builder.RegisterType<XmlMapParser>()
                .WithParameter((p, c) => p.Name == "filePath",
                               (p, c) => MAP_XML_PATH)
                .As<IMapProvider>();
            builder.RegisterType<Router>()
                .As<IRouter>();
            builder.RegisterType<MapRenderer>()
                .As<IMapRenderer>();
            builder.RegisterType<MainWindowViewModel>();
        }
        

    }
}