using IniParser.Model;

namespace BaSta.Link;

internal class IniTimeSyncSettingsGroup : ITimeSyncSettingsGroup
{
    private readonly KeyDataCollection _settings;

    public IniTimeSyncSettingsGroup(SectionData settings)
    {
        Name = settings.SectionName;
        _settings = settings.Keys;
    }

    public string Name { get; }

    public string? GetSetting(string name)
    {
        return _settings.ContainsKey(name) ?_settings[name] : null;
    }
}