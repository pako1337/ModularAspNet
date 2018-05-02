using Autofac;

namespace ModularAspNet.ModuleContracts
{
    public interface IModule
    {
        void RegisterDependencies(ContainerBuilder container);
    }
}
