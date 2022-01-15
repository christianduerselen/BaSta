namespace BaSta.Protocol.Stramatel;

public interface IStramatelMessageParser
{
    void Parse(params byte[] data);

    int MessagesAvailable { get; }

    IStramatelMessage? Dequeue();
}