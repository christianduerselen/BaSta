namespace BaSta.TimeSync;

internal class EmptyTimeSyncSettingsGroup : ITimeSyncSettingsGroup
{
    public string Name => string.Empty;

    public string? GetSetting(string name)
    {
        return null;
    }
}