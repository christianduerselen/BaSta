namespace BaSta.Protocol.Nautronic.Services;

internal struct T_BOARD_ASSIGNMENT
{
    public T_BOARD_ASSIGNMENT(int index, int table, int command, int destinationAddress, int dotMergeAddress, int dotMergeBit)
    {
        OriginalIndex = index;
        OriginalTable = table;
        DestinationCommand = command;
        DestinationAddress = destinationAddress;
        DotMergeAddress = dotMergeAddress;
        DotMergeBit = dotMergeBit;
    }

    public int OriginalIndex;
    public int OriginalTable;
    public int DestinationCommand;
    public int DestinationAddress;
    public int DotMergeAddress;
    public int DotMergeBit;
}