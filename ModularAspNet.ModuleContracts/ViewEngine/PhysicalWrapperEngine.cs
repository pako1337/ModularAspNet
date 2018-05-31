using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web;
using System.Web.Mvc;
using RazorGenerator.Mvc;

namespace ModularAspNet.ModuleContracts.ViewEngine
{
    public class PhysicalWrapperEngine : PrecompiledMvcEngine
    {
        private readonly IPhysicalPathProvider _pathProvider;

        public PhysicalWrapperEngine(Assembly assembly, IPhysicalPathProvider pathProvider) : base(assembly)
        {
#if DEBUG
            UsePhysicalViewsIfNewer = true;
#else
            UsePhysicalViewsIfNewer = false;
#endif
            _pathProvider = pathProvider;
        }

        protected override bool IsPhysicalFileNewer(string virtualPath)
        {
#if DEBUG
            var physicalPath = _pathProvider.GetPhysicalPath(virtualPath);
            if (!string.IsNullOrEmpty(physicalPath) && File.Exists(physicalPath))
                return File.GetLastWriteTimeUtc(physicalPath) > GetLastWriteTimeUtc(GetType().Assembly, DateTime.MinValue);
#endif
            return false;
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
#if DEBUG
            virtualPath = EnsureVirtualPathPrefix(virtualPath);
            if (this.UsePhysicalViewsIfNewer && this.IsPhysicalFileNewer(virtualPath))
                return true;
#endif
            return base.FileExists(controllerContext, virtualPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
#if DEBUG
            viewPath = EnsureVirtualPathPrefix(viewPath);
            if (this.UsePhysicalViewsIfNewer && this.IsPhysicalFileNewer(viewPath))
            {
                var razor = GetFallbackRazorEngine();
                var view = razor.FindView(controllerContext, viewPath, masterPath, useCache: false);
                return view.View;
            }
#endif
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
#if DEBUG
            partialPath = EnsureVirtualPathPrefix(partialPath);
            if (this.UsePhysicalViewsIfNewer && this.IsPhysicalFileNewer(partialPath))
            {
                return GetFallbackRazorEngine()
                    .FindPartialView(controllerContext, partialPath, useCache: false)
                    .View;
            }
#endif
            return base.CreatePartialView(controllerContext, partialPath);
        }

        private static IViewEngine GetFallbackRazorEngine()
        {
            return ViewEngines.Engines.First(e => e.GetType() == typeof(RazorViewEngine));
        }

        private static string EnsureVirtualPathPrefix(string virtualPath)
        {
            if (!string.IsNullOrEmpty(virtualPath) && !virtualPath.StartsWith("~/", StringComparison.Ordinal))
                virtualPath = "~/" + virtualPath.TrimStart('/', '~');
            return virtualPath;
        }

        private static DateTime GetLastWriteTimeUtc(Assembly assembly, DateTime fallback)
        {
            string path = null;
            try
            {
                path = assembly.Location;
            }
            catch (SecurityException)
            {
                if (!string.IsNullOrEmpty(assembly.CodeBase))
                {
                    Uri result;
                    if (Uri.TryCreate(assembly.CodeBase, UriKind.Absolute, out result))
                    {
                        if (result.IsFile)
                            path = result.LocalPath;
                    }
                }
            }
            if (string.IsNullOrEmpty(path))
                return fallback;

            DateTime dateTime;
            try
            {
                dateTime = File.GetLastWriteTimeUtc(path);
            }
            catch (UnauthorizedAccessException)
            {
                dateTime = fallback;
            }
            catch (PathTooLongException)
            {
                dateTime = fallback;
            }
            catch (NotSupportedException)
            {
                dateTime = fallback;
            }

            return dateTime;
        }
    }
}