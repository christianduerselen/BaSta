using BaSta.Core;

namespace BaSta.Link;

public interface ITimeSyncTask : IProcessTask
{
    void LoadSettings(string name, ITimeSyncSettingsGroup settings);
}