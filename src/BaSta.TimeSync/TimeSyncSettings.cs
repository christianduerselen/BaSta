using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BaSta.TimeSync
{
    internal class TimeSyncSettings
    {
        public TimeSyncSettings(ITimeSyncSettingsGroup inputSettings, IEnumerable<ITimeSyncSettingsGroup> outputSettings)
        {
            Input = inputSettings;
            Outputs = new ReadOnlyCollection<ITimeSyncSettingsGroup>(outputSettings.ToArray());
        }

        public ITimeSyncSettingsGroup Input { get; }

        public IReadOnlyCollection<ITimeSyncSettingsGroup> Outputs { get; }
    }
}