using System;

namespace BaSta.Link;

public interface ITimeSyncInputTask : ITimeSyncTask
{
    TimeSpan Pull();

    event EventHandler<DataEventArgs<TimeSpan>> StateChanged;
}