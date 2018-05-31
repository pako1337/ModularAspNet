using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.WebPages;
using Autofac;
using ModularAspnet.Core.Controllers;
using ModularAspnet.Core.MenuLinks;
using ModularAspNet.ModuleContracts;
using ModularAspNet.ModuleContracts.ViewEngine;

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

            container.RegisterInstance(CreateCorePathProvider()).AsImplementedInterfaces();
            RegisterViewEngine();
        }

        private void RegisterViewEngine()
        {
            var engine = new PhysicalWrapperEngine(GetType().Assembly, CreateCorePathProvider());

            engine.ViewLocationFormats = engine.ViewLocationFormats.Concat(new[]
            {
                "~/{1}Module/{0}.cshtml",
            }).ToArray();

            engine.PartialViewLocationFormats = engine.PartialViewLocationFormats.Concat(new string[]
            {
                "~/{1}Module/{0}.cshtml",
            }).ToArray();
            
            ViewEngines.Engines.Insert(0, engine);

            // StartPage lookups are done by WebPages.
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }

        private IPhysicalPathProvider CreateCorePathProvider()
        {
            return new PhysicalPathProvider(typeof(CoreModule).Assembly,
                new VirtualPathMap(new Regex(@"~?\/Views/Home/(.*).cshtml"), @"{0}\Views\Home\{1}.cshtml"));
        }
    }
}