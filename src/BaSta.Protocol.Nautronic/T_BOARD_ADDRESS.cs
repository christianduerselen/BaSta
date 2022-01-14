namespace BaSta.Protocol.Nautronic;

public class T_BOARD_ADDRESS
{
    public uint datauint;
    public uint flashuint;
    public uint priority;
    public uint updates;

    public void Touch()
    {
        priority = 1U;
        updates = 3U;
    }
}