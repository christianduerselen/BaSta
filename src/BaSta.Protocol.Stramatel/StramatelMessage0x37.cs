using System;
using System.Linq;
using System.Text;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessage0x37 : IStramatelMessage
{
    private StramatelMessage0x37()
    {
    }

    public Possession? Possession { get; private set; }
    public TimeSpan? GameClock { get; private set; }
    public bool? Horn { get; private set; }
    public bool? GameClockRunning { get; private set; }
    public int? GuestPlayer1Points { get; private set; }
    public int? GuestPlayer2Points { get; private set; }
    public int? GuestPlayer3Points { get; private set; }
    public int? GuestPlayer4Points { get; private set; }
    public int? GuestPlayer5Points { get; private set; }
    public int? GuestPlayer6Points { get; private set; }
    public int? GuestPlayer7Points { get; private set; }
    public int? GuestPlayer8Points { get; private set; }
    public int? GuestPlayer9Points { get; private set; }
    public int? GuestPlayer10Points { get; private set; }
    public int? GuestPlayer11Points { get; private set; }
    public int? GuestPlayer12Points { get; private set; }
    public TimeSpan? ShotClock { get; private set; }
    public bool? ShotClockHorn { get; private set; }
    public bool? ShotClockRunning { get; private set; }
    public bool? ShotClockVisible { get; private set; }

    public static IStramatelMessage Parse(byte[] messageData)
    {
        StramatelMessage0x37 message = new StramatelMessage0x37();
        message.Possession = messageData[3].ParseStramatelPossession();
        message.GameClock = messageData[4..8].ParseStramatelGameClock();
        message.Horn = messageData[19].ParseStramatelBoolean();
        message.GameClockRunning = messageData[20].ParseStramatelBoolean();
        message.GuestPlayer1Points = messageData[22..24].ParseStramatelNumber();
        message.GuestPlayer2Points = messageData[24..26].ParseStramatelNumber();
        message.GuestPlayer3Points = messageData[26..28].ParseStramatelNumber();
        message.GuestPlayer4Points = messageData[28..30].ParseStramatelNumber();
        message.GuestPlayer5Points = messageData[30..32].ParseStramatelNumber();
        message.GuestPlayer6Points = messageData[32..34].ParseStramatelNumber();
        message.GuestPlayer7Points = messageData[34..36].ParseStramatelNumber();
        message.GuestPlayer8Points = messageData[36..38].ParseStramatelNumber();
        message.GuestPlayer9Points = messageData[38..40].ParseStramatelNumber();
        message.GuestPlayer10Points = messageData[40..42].ParseStramatelNumber();
        message.GuestPlayer11Points = messageData[42..44].ParseStramatelNumber();
        message.GuestPlayer12Points = messageData[44..46].ParseStramatelNumber();
        message.ShotClock = messageData[48..50].ParseStramatelShotClock();
        message.ShotClockHorn = messageData[50].ParseStramatelBoolean();
        message.ShotClockRunning = messageData[51].ParseStramatelBoolean();
        message.ShotClockVisible = messageData[52].ParseStramatelBoolean();

        return message;
    }
}

internal static class StramatelProtocolExtensions
{
    public static Possession? ParseStramatelPossession(this byte value)
    {
        return value switch
        {
            0x31 => Possession.Home,
            0x32 => Possession.Guest,
            _ => null
        };
    }

    public static TimeSpan? ParseStramatelGameClock(this byte[] values)
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

    public static TimeSpan? ParseStramatelShotClock(this byte[] values)
    {
        // TODO
        return null;
    }

    public static bool? ParseStramatelBoolean(this byte value)
    {
        return value == 0x31;
    }

    public static int? ParseStramatelNumber(this byte[] values)
    {
        // Sanity check - numbers should be 0x20 (Space) or 0x30 (0) to 0x39 (8)
        if (!values.All(value => value is 0x20 or >= 0x30 and <= 0x39))
            return null;

        string numberText = Encoding.ASCII.GetString(values);
        return int.TryParse(numberText, out int number) ? number : null;
    }
}