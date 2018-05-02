using ModularAspNet.ModuleContracts;

namespace ModularAspnet.Core.MenuLinks
{
    public class HomeMainMenu : IMainMenuLink
    {
        public string DisplayName => "Home";

        public string UniqueId => "HomeMenu";

        public string Url => "~/Home";
    }
}