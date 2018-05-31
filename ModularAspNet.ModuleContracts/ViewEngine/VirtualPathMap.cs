using System.Text.RegularExpressions;

namespace ModularAspNet.ModuleContracts.ViewEngine
{
    public class VirtualPathMap
    {
        public VirtualPathMap(Regex regex, string physicalPathFormat)
        {
            Regex = regex;
            PhysicalPathFormat = physicalPathFormat;
        }

        public Regex Regex { get; }
        public string PhysicalPathFormat { get; }
    }
}