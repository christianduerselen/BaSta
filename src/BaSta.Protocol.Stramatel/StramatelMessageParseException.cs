using System;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessageParseException : Exception
{
    public StramatelMessageParseException()
    {
    }

    public StramatelMessageParseException(string message)
        : base(message)
    {
    }

    public StramatelMessageParseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}