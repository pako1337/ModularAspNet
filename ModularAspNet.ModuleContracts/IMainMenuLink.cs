namespace ModularAspNet.ModuleContracts
{
    public interface IMainMenuLink
    {
        string DisplayName { get; }
        string UniqueId { get; }
        string Url { get; }
    }
}
