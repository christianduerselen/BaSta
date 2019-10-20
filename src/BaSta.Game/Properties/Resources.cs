using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BaSta.Game.Properties
{
    [DebuggerNonUserCode]
    [CompilerGenerated]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static ResourceManager _resourceManager;
        private static readonly CultureInfo ResourceCulture = CultureInfo.CurrentCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager => _resourceManager ??= new ResourceManager("BaSta.Game.Properties.Resources", typeof(Resources).Assembly);

        internal static Bitmap NextIcon => (Bitmap)ResourceManager.GetObject(nameof(NextIcon), ResourceCulture);

        internal static Bitmap PreviousIcon => (Bitmap)ResourceManager.GetObject(nameof(PreviousIcon), ResourceCulture);
    }
}