using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ModularAspnet.Core;
using ModularAspNet.ModuleContracts;

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
            var container = new ContainerBuilder();

            container.RegisterControllers(typeof(Bootstrap).Assembly);
            container.RegisterType<Controllers.MenuController>();

            foreach (var module in modules)
            {
                module.RegisterDependencies(container);
            }

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container.Build()));
        }
    }
}