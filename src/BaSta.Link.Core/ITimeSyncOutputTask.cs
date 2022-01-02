using System;

namespace BaSta.Link;

public interface ITimeSyncOutputTask : ITimeSyncTask
{
    void Push(TimeSpan time);
}