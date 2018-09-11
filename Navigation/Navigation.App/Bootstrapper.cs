using System;
using System.Dynamic;
using Autofac;
using Autofac.Core;
using Navigation.App;
using Navigation.Routing;

namespace Navigation
{
    public class Bootstrapper
    {
        private const string MAP_XML_PATH =
            @"/media/tyll/personal/DEV/Navigation/Navigation/daten_hl_altstadt_routenplaner_koordinaten.xml";
        
        private static Bootstrapper _instance;
        private IContainer _container;
        
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
                .As<IMapProvider>()
                .WithParameter((p, c) => p.Name == "filePath",
                    (p, c) => MAP_XML_PATH);
            builder.RegisterType<Router>()
                .As<IRouter>();
            builder.RegisterType<MapViewModel>();
        }
        

    }
}