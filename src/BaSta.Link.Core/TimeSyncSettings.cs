using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BaSta.Link
{
    public class TimeSyncSettings
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