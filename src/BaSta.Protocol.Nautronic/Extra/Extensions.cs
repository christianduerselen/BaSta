namespace BaSta.Protocol.Nautronic.Extra;

internal static class Extensions
{
    public static bool Between(this int val, int from, int to) => val >= from && val <= to;

    public static int CountSetBits(this int val)
    {
        int num = 0;
        for (; val > 0; val >>= 1)
        {
            if ((val & 1) > 0)
                ++num;
        }
        return num;
    }

    public static int CountSetBits(this byte val)
    {
        int num = 0;
        for (; val > 0; val >>= 1)
        {
            if ((val & 1) > 0)
                ++num;
        }
        return num;
    }
}