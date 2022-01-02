using System.Threading;
using BaSta.Core;
using NLog;

namespace BaSta.Link;

public abstract class TimeSyncTaskBase : ProcessTaskBase, ITimeSyncTask
{
    private ExternalActionProcess _process;

    protected string Name { get; private set; }
        
    protected Logger Logger { get; private set; }

    /// <inheritdoc/>
    public void LoadSettings(string name, ITimeSyncSettingsGroup settings)
    {
        Name = name;
        Logger = LogManager.GetLogger(name);

        LoadSettings(settings);
    }

    protected abstract void LoadSettings(ITimeSyncSettingsGroup settings);

    protected abstract void OnStartSync();

    protected abstract void OnProcess(CancellationToken cancellationToken);

    protected abstract void OnStopSync();

    /// <inheritdoc/>
    protected override void OnInitialize()
    {
        OnStartSync();

        _process = new ExternalActionProcess(OnProcess);
        _process.Initialize();
    }

    /// <inheritdoc/>
    protected override void OnTerminate()
    {
        _process?.Terminate();
        _process = null;

        OnStopSync();
    }
}