using System.Collections.Generic;
using System.Web.Mvc;
using ModularAspNet.ModuleContracts;

namespace ModularAspNet.UI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IEnumerable<IMainMenuLink> menuLinks;

        public MenuController(IEnumerable<IMainMenuLink> menuLinks)
        {
            this.menuLinks = menuLinks;
        }

        [ChildActionOnly]
        public ActionResult _MainMenu()
        {
            return View(menuLinks);
        }
    }
}