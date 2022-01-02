using System.IO;
using System.Reflection;

namespace BaSta.Tests.Resources;

/// <summary>
/// Helper enabling access to Resources of the containing assembly.
/// </summary>
public static class ResourceHelper
{
    /// <summary>
    /// Extracts the resource from the given location.
    /// </summary>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <returns>The byte-array of the file content or <c>null</c> if no file has been found.</returns>
    public static byte[] ExtractResource(string location)
    {
        return ExtractResource(Assembly.GetCallingAssembly(), location);
    }

    /// <summary>
    /// Extracts the resource from the given assembly and location.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <returns>The byte-array of the file content or <c>null</c> if no file has been found.</returns>
    public static byte[] ExtractResource(Assembly assembly, string location)
    {
        using (Stream stream = assembly.GetManifestResourceStream(location))
        {
            if (stream == null)
                return null;

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }

    /// <summary>
    /// Extracts the resource of the given filename in the given namespace.
    /// </summary>
    /// <param name="nameSpace">The namespace to locate the file in.</param>
    /// <param name="filename">Name of the file.</param>
    /// <returns>The byte-array of the file content or <c>null</c> if no file has been found.</returns>
    public static byte[] ExtractResource(string nameSpace, string filename)
    {
        return ExtractResource(string.Join(".", nameSpace, filename));
    }

    /// <summary>
    /// Extracts the resource of the given assembly, namespace and filename.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="nameSpace">The namespace to locate the file in.</param>
    /// <param name="filename">Name of the file.</param>
    /// <returns>The byte-array of the file content or <c>null</c> if no file has been found.</returns>
    public static byte[] ExtractResource(Assembly assembly, string nameSpace, string filename)
    {
        return ExtractResource(assembly, string.Join(".", nameSpace, filename));
    }

    /// <summary>
    /// Attempts to extract the resource for the given filename.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>The byte-array of the file content or <c>null</c> if no file has been found.</returns>
    public static byte[] TryExtractResource(string fileName)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        string[] availableFiles = assembly.GetManifestResourceNames();

        // Attempt to find the file that has the same name (avoiding namespace problems)
        foreach (string file in availableFiles)
        {
            if (file.EndsWith(fileName))
                return ExtractResource(assembly, file);
        }

        return null;
    }
}