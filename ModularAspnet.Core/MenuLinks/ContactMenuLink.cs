using ModularAspNet.ModuleContracts;

namespace ModularAspnet.Core.MenuLinks
{
    public class ContactMenuLink : IMainMenuLink
    {
        public string DisplayName => "Contact";

        public string UniqueId => "ContactMenu";

        public string Url => "~/Home/Contact";
    }
}