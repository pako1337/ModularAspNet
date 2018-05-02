using ModularAspNet.ModuleContracts;

namespace ModularAspnet.Core.MenuLinks
{
    public class AboutMenuLink : IMainMenuLink
    {
        public string DisplayName => "About";

        public string UniqueId => "AboutMenu";

        public string Url => "~/Home/About";
    }
}