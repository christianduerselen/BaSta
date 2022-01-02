using System;

namespace BaSta.Link
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