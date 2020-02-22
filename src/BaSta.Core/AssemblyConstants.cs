using System.Runtime.CompilerServices;
using BaSta;

[assembly: InternalsVisibleTo("BaSta.Test, PublicKey=" + AssemblyConstants.PublicKey)]
[assembly: InternalsVisibleTo("BaSta.TimeSync, PublicKey=" + AssemblyConstants.PublicKey)]

namespace BaSta
{
    /// <summary>
    /// Static class containing constants for use within the assembly.
    /// </summary>
    internal static class AssemblyConstants
    {
        /// <summary>
        /// The public key used to sign libraries. Can be used for reference in the <see cref="InternalsVisibleToAttribute"/>.
        /// </summary>
        internal const string PublicKey = "00240000048000009400000006020000002400005253413100040000010001004be7601ce65a0a0260c1cdfe42237ec0c1b73cae3ba4463db05ab6d8fecbf1923ef6fe859b8ccc80de9e8fc4526e972efef3a1fc823a696dcae20b3fdad9424b66317fbb9af2815a3fc5becf0ea2f238b9c8c8dcff7a0f106739bd68dfb2a4a4f264c2916a5551968245460613d58509b20b08935d434186c6dcd2c7ea0e50a9";
    }
}