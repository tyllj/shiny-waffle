using System;
using System.Dynamic;
using Autofac;
using Autofac.Core;

namespace Navigation
{
    public class Bootstrapper
    {
        private static Bootstrapper _instance;
        public static Bootstrapper Create()
        {
            return _instance ?? (_instance = new Bootstrapper());
        }
        
        private Bootstrapper()
        {
            var builder = new ContainerBuilder();
            SetUp(builder);
            builder.Build();
        }

        private void SetUp(ContainerBuilder builder)
        {
            
        }
        

    }
}