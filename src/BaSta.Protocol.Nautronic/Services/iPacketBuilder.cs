namespace BaSta.Protocol.Nautronic.Services;

internal interface iPacketBuilder
{
    int Run();

    void ClearFlags();

    void NauBeeEvent(int[] data);
}