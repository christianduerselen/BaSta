using System;

namespace BaSta.TimeSync.Input
{
    public class DataEventArgs<TType> : EventArgs
    {
        public DataEventArgs(TType data)
        {
            Data = data;
        }

        public TType Data { get; }
    }
}   