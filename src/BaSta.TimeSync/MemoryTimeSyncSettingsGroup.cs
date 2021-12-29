using System.Collections.Generic;

namespace BaSta.TimeSync;

internal class MemoryTimeSyncSettingsGroup : ITimeSyncSettingsGroup
{
    private readonly IDictionary<string, string> _settings = new Dictionary<string, string>();

    public string Name => string.Empty;

    public void AddSetting(string name, string value)
    {
        _settings.Add(name, value);
    }

    public string? GetSetting(string name)
    {
        return _settings.TryGetValue(name, out string value) ? value : null;
    }
}