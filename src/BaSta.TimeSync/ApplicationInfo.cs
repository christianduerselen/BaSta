using System.Diagnostics;
using System.Reflection;

namespace BaSta.TimeSync
{
    internal class ApplicationInfo
    {
        public ApplicationInfo(Assembly assembly)
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = $"{info.ProductMajorPart}.{info.ProductMinorPart}.{info.ProductBuildPart}.{info.ProductPrivatePart}";
            ProductName = info.ProductName;
        }

        public string ProductName { get; }

        public string Version { get; }
    }
}