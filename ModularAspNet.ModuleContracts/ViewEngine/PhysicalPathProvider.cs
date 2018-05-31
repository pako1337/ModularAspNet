using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModularAspNet.ModuleContracts.ViewEngine
{
    public class PhysicalPathProvider : IPhysicalPathProvider
    {
        private string baseDir;
        private VirtualPathMap[] virtualPaths;

        public PhysicalPathProvider(Assembly assembly, params VirtualPathMap[] virtualPaths)
        {
            baseDir = GetAssemblyPath(assembly);
            this.virtualPaths = virtualPaths;
        }

        public string GetPhysicalPath(string virtualPath)
        {
            var path = virtualPaths
                .Select(p => new { Match = p.Regex.Match(virtualPath), p.PhysicalPathFormat })
                .Where(x => x.Match.Success)
                .Select(x => string.Format(x.PhysicalPathFormat, baseDir, x.Match.Groups[1].Value))
                .FirstOrDefault(p => File.Exists(p));

            return path;
        }

        private string GetAssemblyPath(Assembly assembly, [CallerFilePath] string physicalPath = "")
        {
            string assemblyName = assembly.GetName().Name;
            var thisAssembly = typeof(PhysicalPathProvider).Assembly.GetName().Name;
            var thisAssemblyNameIndex = physicalPath.IndexOf(thisAssembly);
            return Path.Combine(physicalPath.Substring(0, thisAssemblyNameIndex), assemblyName);
        }
    }
}
