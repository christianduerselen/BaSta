using System.Reflection;

namespace BaSta.TimeSync
{
    internal class ApplicationInfo
    {
        public ApplicationInfo(Assembly assembly)
        {
            Version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            ProductName = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        }

        public string ProductName { get; }

        public string Version { get; }
    }
}