using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ModularAspnet.Core;
using ModularAspNet.ModuleContracts;
using ModularAspNet.ModuleContracts.ViewEngine;

namespace ModularAspNet.UI.App_Start
{
    public class Bootstrap
    {
        private static List<IModule> modules = new List<IModule>
        {
            new CoreModule(),
        };

        public void Initialise()
        {
            var containerBuuilder = new ContainerBuilder();

            containerBuuilder.RegisterControllers(typeof(Bootstrap).Assembly);
            containerBuuilder.RegisterType<Controllers.MenuController>();
            containerBuuilder.RegisterType<ModuleContracts.ViewEngine.VirtualPathProvider>();

            foreach (var module in modules)
            {
                module.RegisterDependencies(containerBuuilder);
            }

            IContainer container = containerBuuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            HostingEnvironment.RegisterVirtualPathProvider(container.Resolve<ModuleContracts.ViewEngine.VirtualPathProvider>());
        }
    }
}