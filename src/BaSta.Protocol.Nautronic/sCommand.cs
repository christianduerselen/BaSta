using System.Collections.Generic;

namespace BaSta.Protocol.Nautronic;

public class sCommand
{
    public int Destination;
    public int ControlByte;
    public int Retries;
    public List<byte> Data;

    public sCommand()
    {
        Data = new List<byte>();
        Retries = 3;
    }
}