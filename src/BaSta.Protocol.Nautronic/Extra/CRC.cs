namespace BaSta.Protocol.Nautronic.Extra;

internal static class CRC
{
    public static int crc8(int crc8In, int dataByte)
    {
        byte num1 = 141;
        int num2 = 8;
        crc8In ^= dataByte;
        do
        {
            if ((uint) (crc8In & 128) > 0U)
            {
                crc8In <<= 1;
                crc8In ^= num1;
            }
            else
                crc8In <<= 1;
        }
        while (--num2 > 0);
        return crc8In;
    }
}