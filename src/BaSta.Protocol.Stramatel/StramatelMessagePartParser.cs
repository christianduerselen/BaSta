using System;
using System.Linq;
using System.Text;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

internal static class StramatelMessagePartParser
{
    public static PossessionState? ParseStramatelPossession(byte value)
    {
        return value switch
        {
            0x31 => PossessionState.Home,
            0x32 => PossessionState.Guest,
            0x20 => PossessionState.None,
            _ => null
        };
    }

    public static TimeSpan? ParseStramatelGameClock(byte[] values)
    {
        // Sanity check - time bytes should either be 0x20 (Space), 0x3A (:) or 0x30 (0) to 0x39 (8)
        if (!values.All(value => value is 0x20 or 0x3A or >= 0x30 and <= 0x39))
            return null;

        string timeText = Encoding.ASCII.GetString(values);

        // Everything after a ':' must be zeroed
        if (timeText.IndexOf(':') >= 0)
            timeText = timeText.Substring(0, timeText.IndexOf(':') + 1).PadRight(4, '0');

        // If the last char is a space (0x20 | " "), shift the text by 2 digits because of sub-minute display
        if (timeText[^1] != ' ')
            timeText += "  ";

        // If a character is a ':' this means first two digits are *10 minutes
        timeText = timeText.Replace(":", "00");

        // Pad to 7 digits (3x minutes, 2x seconds, 2x milliseconds)
        timeText = timeText.PadLeft(7, ' ');

        // Spaces are zeros
        timeText = timeText.Replace(' ', '0');

        int minutes = int.Parse(timeText.Substring(0, 3));
        int seconds = int.Parse(timeText.Substring(3, 2));
        int milliseconds = int.Parse(timeText.Substring(5, 2)) * 10;
        return new TimeSpan(0, 0, minutes, seconds, milliseconds);
    }

    public static TimeSpan? ParseStramatelShotClock(byte[] values)
    {
        // TODO
        return null;
    }

    public static bool? ParseStramatelBoolean(byte value)
    {
        return value == 0x31;
    }

    public static int? ParseStramatelNumber(byte[] values)
    {
        // Sanity check - numbers should be 0x20 (Space) or 0x30 (0) to 0x39 (8)
        if (!values.All(value => value is 0x20 or >= 0x30 and <= 0x39))
            return null;

        string numberText = Encoding.ASCII.GetString(values);
        return int.TryParse(numberText, out int number) ? number : null;
    }

    public static int? ParseStramatelNumber(byte value)
    {
        // Sanity check - numbers should be 0x20 (Space) or 0x30 (0) to 0x39 (8)
        if (value is not (0x20 or >= 0x30 and <= 0x39))
            return null;

        string numberText = Encoding.ASCII.GetString(new[] { value });
        return int.TryParse(numberText, out int number) ? number : null;
    }
}