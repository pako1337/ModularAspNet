using Autofac;
using ModularAspnet.Core.Controllers;
using ModularAspnet.Core.MenuLinks;
using ModularAspNet.ModuleContracts;

namespace ModularAspnet.Core
{
    public class CoreModule : IModule
    {
        public void RegisterDependencies(ContainerBuilder container)
        {   
            container.RegisterType<HomeMainMenu>().As<IMainMenuLink>();
            container.RegisterType<ContactMenuLink>().As<IMainMenuLink>();
            container.RegisterType<AboutMenuLink>().As<IMainMenuLink>();

            container.RegisterType<HomeController>();
        }
    }
}