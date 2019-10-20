using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace BaSta.Layout.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }

    [DefaultSettingValue("384, 256")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public Size SCB_Default_Size
    {
      get
      {
        return (Size) this[nameof (SCB_Default_Size)];
      }
      set
      {
        this[nameof (SCB_Default_Size)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("0, 0")]
    public Point DefaultFieldLocation
    {
      get
      {
        return (Point) this[nameof (DefaultFieldLocation)];
      }
      set
      {
        this[nameof (DefaultFieldLocation)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DefaultSettingValue("100, 20")]
    [DebuggerNonUserCode]
    public Size DefaultFieldSize
    {
      get
      {
        return (Size) this[nameof (DefaultFieldSize)];
      }
      set
      {
        this[nameof (DefaultFieldSize)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Arial, 12pt, style=Bold")]
    public Font DefaultFont
    {
      get
      {
        return (Font) this[nameof (DefaultFont)];
      }
      set
      {
        this[nameof (DefaultFont)] = (object) value;
      }
    }

    [DefaultSettingValue("White")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public Color DefaultForeColor
    {
      get
      {
        return (Color) this[nameof (DefaultForeColor)];
      }
      set
      {
        this[nameof (DefaultForeColor)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("White")]
    [UserScopedSetting]
    public Color DefaultAltColor
    {
      get
      {
        return (Color) this[nameof (DefaultAltColor)];
      }
      set
      {
        this[nameof (DefaultAltColor)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string DefaultText
    {
      get
      {
        return (string) this[nameof (DefaultText)];
      }
      set
      {
        this[nameof (DefaultText)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DefaultSettingValue("TopLeft")]
    [DebuggerNonUserCode]
    public ContentAlignment DefaultTextAlign
    {
      get
      {
        return (ContentAlignment) this[nameof (DefaultTextAlign)];
      }
      set
      {
        this[nameof (DefaultTextAlign)] = (object) value;
      }
    }

    [DefaultSettingValue("c:\\")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string DefaultWorkingDirectory
    {
      get
      {
        return (string) this[nameof (DefaultWorkingDirectory)];
      }
      set
      {
        this[nameof (DefaultWorkingDirectory)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("Label")]
    public string DefaultFieldName
    {
      get
      {
        return (string) this[nameof (DefaultFieldName)];
      }
      set
      {
        this[nameof (DefaultFieldName)] = (object) value;
      }
    }
  }
}
