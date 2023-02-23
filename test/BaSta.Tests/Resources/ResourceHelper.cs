using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BaSta.Tests.Resources;

/// <summary>
/// Helper enabling access to Resources of the containing assembly.
/// </summary>
public static class ResourceHelper
{
    /// <summary>
    /// Extracts the resource from the calling assembly and the given location.
    /// </summary>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <returns>The byte-array of the file content.</returns>
    /// <exception cref="ArgumentNullException">Argument <see cref="location"/> is not usable.</exception>
    /// <exception cref="FileNotFoundException">The embedded resource with the given location couldn't be found in the assembly.</exception>
    public static byte[] ExtractResource(string location)
    {
        return ExtractResource(Assembly.GetCallingAssembly(), location);
    }

    /// <summary>
    /// Extracts the resource from the given assembly and location.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <returns>The byte-array of the file content extracted from the embedded resource.</returns>
    /// <exception cref="ArgumentNullException">Argument <see cref="assembly"/> or <see cref="location"/> are not usable.</exception>
    /// <exception cref="FileNotFoundException">The embedded resource with the given location couldn't be extracted from the assembly.</exception>
    public static byte[] ExtractResource(Assembly assembly, string location)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        if (string.IsNullOrEmpty(location))
            throw new ArgumentNullException(nameof(location));

        try
        {
            using (Stream stream = assembly.GetManifestResourceStream(location))
            {
                if (stream == null)
                    throw new Exception("Manifest resource stream is null!");

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Embedded resource with location '{location}' couldn't be extracted from assembly '{assembly.FullName}'! Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Attempts to extract the resource from the calling assembly and the given location.
    /// </summary>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <param name="fileData">When this method returns, contains an byte-array containing the file data of the embedded resource, if the extraction succeeded, or <c>null</c> if the extraction failed.</param>
    /// <returns><c>true</c> if the resource was found and successfully extracted, otherwise <c>false</c>.</returns>
    public static bool TryExtractResource(string location, out byte[] fileData)
    {
        return TryExtractResource(Assembly.GetCallingAssembly(), location, out fileData);
    }

    /// <summary>
    /// Attempts to extract the resource from the given assembly and location.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="location">The location (including namespace and filename) of the resource.</param>
    /// <param name="fileData">When this method returns, contains an byte-array containing the file data of the embedded resource, if the extraction succeeded, or <c>null</c> if the extraction failed.</param>
    /// <returns><c>true</c> if the resource was found and successfully extracted, otherwise <c>false</c>.</returns>
    public static bool TryExtractResource(Assembly assembly, string location, out byte[] fileData)
    {
        try
        {
            fileData = ExtractResource(assembly, location);
            return true;
        }
        catch
        {
            // Ignored
        }

        fileData = null;
        return false;
    }

    /// <summary>
    /// Finds and extracts the resource from the calling assembly and given filename.
    /// </summary>
    /// <param name="fileName">The filename of the resource to search for.</param>
    /// <returns>The byte-array of the file content extracted from the embedded resource.</returns>
    /// <exception cref="ArgumentNullException">Argument <see cref="fileName"/> is not usable.</exception>
    /// <exception cref="FileNotFoundException">The embedded resource with the given filename couldn't be found in the assembly.</exception>
    public static byte[] ExtractFile(string fileName)
    {
        return ExtractFile(Assembly.GetCallingAssembly(), fileName);
    }

    /// <summary>
    /// Finds and extracts the resource from the given assembly and filename.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="fileName">The filename of the resource to search for.</param>
    /// <returns>The byte-array of the file content extracted from the embedded resource.</returns>
    /// <exception cref="ArgumentNullException">Argument <see cref="assembly"/> or <see cref="fileName"/> are not usable.</exception>
    /// <exception cref="FileNotFoundException">The embedded resource with the given filename couldn't be extracted from the assembly.</exception>
    public static byte[] ExtractFile(Assembly assembly, string fileName)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));

        try
        {
            // Get list of all embedded resouces from assembly
            string[] locations = assembly.GetManifestResourceNames();

            // Attempt to find the resource file that ends with the same name (avoiding namespace problems)
            string location = locations.FirstOrDefault(x => x.EndsWith(fileName));

            if (string.IsNullOrEmpty(location))
                throw new Exception($"No embedded resource location found ending with name '{fileName}'! Available: {string.Join('|', locations)}");

            return ExtractResource(assembly, location);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Embedded resource with filename '{fileName}' couldn't be extracted from assembly '{assembly.FullName}'! Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Attempts to find and extract the resource from the calling assembly and given filename.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="fileData">When this method returns, contains an byte-array containing the file data of the embedded resource, if the extraction succeeded, or <c>null</c> if the extraction failed.</param>
    /// <returns><c>true</c> if the resource was found and successfully extracted, otherwise <c>false</c>.</returns>
    public static bool TryExtractFile(string fileName, out byte[] fileData)
    {
        return TryExtractFile(Assembly.GetCallingAssembly(), fileName, out fileData);
    }

    /// <summary>
    /// Attempts to find and extract the resource from the given assembly and filename.
    /// </summary>
    /// <param name="assembly">The assembly in which to search for the resource.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="fileData">When this method returns, contains an byte-array containing the file data of the embedded resource, if the extraction succeeded, or <c>null</c> if the extraction failed.</param>
    /// <returns><c>true</c> if the resource was found and successfully extracted, otherwise <c>false</c>.</returns>
    public static bool TryExtractFile(Assembly assembly, string fileName, out byte[] fileData)
    {
        try
        {
            fileData = ExtractFile(assembly, fileName);
            return true;
        }
        catch
        {
            // Ignored
        }

        fileData = null;
        return false;
    }
}