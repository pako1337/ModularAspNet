namespace ModularAspNet.ModuleContracts.ViewEngine
{
    public interface IPhysicalPathProvider
    {
        string GetPhysicalPath(string virtualPath);
    }
}