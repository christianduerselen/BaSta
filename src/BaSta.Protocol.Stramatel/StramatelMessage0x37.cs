using System;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessage0x37 : IStramatelMessage
{
    private StramatelMessage0x37()
    {
    }

    public PossessionState? Possession { get; private set; }
    public TimeSpan? GameClock { get; private set; }
    public bool? Horn { get; private set; }
    public bool? GameClockRunning { get; private set; }
    public int? PointsGuestPlayer1 { get; private set; }
    public int? PointsGuestPlayer2 { get; private set; }
    public int? PointsGuestPlayer3 { get; private set; }
    public int? PointsGuestPlayer4 { get; private set; }
    public int? PointsGuestPlayer5 { get; private set; }
    public int? PointsGuestPlayer6 { get; private set; }
    public int? PointsGuestPlayer7 { get; private set; }
    public int? PointsGuestPlayer8 { get; private set; }
    public int? PointsGuestPlayer9 { get; private set; }
    public int? PointsGuestPlayer10 {get; private set; }
    public int? PointsGuestPlayer11{ get; private set; }
    public int? PointsGuestPlayer12 { get; private set; }
    public TimeSpan? ShotClock { get; private set; }
    public bool? ShotClockHorn { get; private set; }
    public bool? ShotClockRunning { get; private set; }
    public bool? ShotClockVisible { get; private set; }

    public static IStramatelMessage Parse(byte[] messageData)
    {
        StramatelMessage0x37 message = new()
        {
            Possession = StramatelMessagePartParser.ParseStramatelPossession(messageData[3]),
            GameClock = StramatelMessagePartParser.ParseStramatelGameClock(messageData[4..8]),
            Horn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[19]),
            GameClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[20]),
            PointsGuestPlayer1 = StramatelMessagePartParser.ParseStramatelNumber(messageData[22..24]),
            PointsGuestPlayer2 = StramatelMessagePartParser.ParseStramatelNumber(messageData[24..26]),
            PointsGuestPlayer3 = StramatelMessagePartParser.ParseStramatelNumber(messageData[26..28]),
            PointsGuestPlayer4 = StramatelMessagePartParser.ParseStramatelNumber(messageData[28..30]),
            PointsGuestPlayer5 = StramatelMessagePartParser.ParseStramatelNumber(messageData[30..32]),
            PointsGuestPlayer6 = StramatelMessagePartParser.ParseStramatelNumber(messageData[32..34]),
            PointsGuestPlayer7 = StramatelMessagePartParser.ParseStramatelNumber(messageData[34..36]),
            PointsGuestPlayer8 = StramatelMessagePartParser.ParseStramatelNumber(messageData[36..38]),
            PointsGuestPlayer9 = StramatelMessagePartParser.ParseStramatelNumber(messageData[38..40]),
            PointsGuestPlayer10 = StramatelMessagePartParser.ParseStramatelNumber(messageData[40..42]),
            PointsGuestPlayer11 = StramatelMessagePartParser.ParseStramatelNumber(messageData[42..44]),
            PointsGuestPlayer12 = StramatelMessagePartParser.ParseStramatelNumber(messageData[44..46]),
            ShotClock = StramatelMessagePartParser.ParseStramatelShotClock(messageData[48..50]),
            ShotClockHorn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[50]),
            ShotClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[51]),
            ShotClockVisible = StramatelMessagePartParser.ParseStramatelBoolean(messageData[52])
        };

        return message;
    }

    public void ApplyTo(IGame game)
    {
        game.Possession.SetPossession(Possession ?? PossessionState.None);
        NullableHelper.SetValueIfNotNull(GameClock, game.GameClock.SetGameClock);
        NullableHelper.SetValueIfNotNull(GameClockRunning, game.GameClock.SetGameClockRunning);
        NullableHelper.SetValueIfNotNull(Horn, game.Horn.SetHorn);
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer1, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(0), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer2, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(1), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer3, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(2), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer4, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(3), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer5, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(4), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer6, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(5), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer7, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(6), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer8, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(7), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer9, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(8), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer10, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(9), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer11, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(10), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsGuestPlayer12, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(11), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(ShotClock, game.ShotClock.SetShotClock);
        NullableHelper.SetValueIfNotNull(ShotClockHorn, game.ShotClock.SetShotClockHorn);
        NullableHelper.SetValueIfNotNull(ShotClockRunning, game.ShotClock.SetShotClockRunning);
        NullableHelper.SetValueIfNotNull(ShotClockVisible, game.ShotClock.SetShotClockVisible);
    }
}