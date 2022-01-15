using System;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessage0x38 : IStramatelMessage
{
    private StramatelMessage0x38()
    {
    }

    public TimeSpan? GameClock { get; private set; }
    public bool? Horn { get; private set; }
    public bool? GameClockRunning { get; private set; }
    public int? PointsHomePlayer1 { get; private set; }
    public int? PointsHomePlayer2 { get; private set; }
    public int? PointsHomePlayer3 { get; private set; }
    public int? PointsHomePlayer4 { get; private set; }
    public int? PointsHomePlayer5 { get; private set; }
    public int? PointsHomePlayer6 { get; private set; }
    public int? PointsHomePlayer7 { get; private set; }
    public int? PointsHomePlayer8 { get; private set; }
    public int? PointsHomePlayer9 { get; private set; }
    public int? PointsHomePlayer10 { get; private set; }
    public int? PointsHomePlayer11 { get; private set; }
    public int? PointsHomePlayer12 { get; private set; }
    public TimeSpan? ShotClock { get; private set; }
    public bool? ShotClockHorn { get; private set; }
    public bool? ShotClockRunning { get; private set; }
    public bool? ShotClockVisible { get; private set; }

    public static IStramatelMessage Parse(byte[] messageData)
    {
        StramatelMessage0x38 message = new()
        {
            GameClock = StramatelMessagePartParser.ParseStramatelGameClock(messageData[4..8]),
            Horn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[19]),
            GameClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[20]),
            PointsHomePlayer1 = StramatelMessagePartParser.ParseStramatelNumber(messageData[22..24]),
            PointsHomePlayer2 = StramatelMessagePartParser.ParseStramatelNumber(messageData[24..26]),
            PointsHomePlayer3 = StramatelMessagePartParser.ParseStramatelNumber(messageData[26..28]),
            PointsHomePlayer4 = StramatelMessagePartParser.ParseStramatelNumber(messageData[28..30]),
            PointsHomePlayer5 = StramatelMessagePartParser.ParseStramatelNumber(messageData[30..32]),
            PointsHomePlayer6 = StramatelMessagePartParser.ParseStramatelNumber(messageData[32..34]),
            PointsHomePlayer7 = StramatelMessagePartParser.ParseStramatelNumber(messageData[34..36]),
            PointsHomePlayer8 = StramatelMessagePartParser.ParseStramatelNumber(messageData[36..38]),
            PointsHomePlayer9 = StramatelMessagePartParser.ParseStramatelNumber(messageData[38..40]),
            PointsHomePlayer10 = StramatelMessagePartParser.ParseStramatelNumber(messageData[40..42]),
            PointsHomePlayer11 = StramatelMessagePartParser.ParseStramatelNumber(messageData[42..44]),
            PointsHomePlayer12 = StramatelMessagePartParser.ParseStramatelNumber(messageData[44..46]),
            ShotClock = StramatelMessagePartParser.ParseStramatelShotClock(messageData[48..50]),
            ShotClockHorn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[50]),
            ShotClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[51]),
            ShotClockVisible = StramatelMessagePartParser.ParseStramatelBoolean(messageData[52])
        };

        return message;
    }

    public void ApplyTo(IGame game)
    {
        NullableHelper.SetValueIfNotNull(GameClock, game.GameClock.SetGameClock);
        NullableHelper.SetValueIfNotNull(GameClockRunning, game.GameClock.SetGameClockRunning);
        NullableHelper.SetValueIfNotNull(Horn, game.Horn.SetHorn);
        NullableHelper.SetValueIfNotNull(PointsHomePlayer1, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(0), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer2, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(1), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer3, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(2), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer4, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(3), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer5, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(4), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer6, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(5), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer7, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(6), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer8, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(7), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer9, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(8), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer10, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(9), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer11, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(10), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(PointsHomePlayer12, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(11), y => y.SetPoints(x)));
        NullableHelper.SetValueIfNotNull(ShotClock, game.ShotClock.SetShotClock);
        NullableHelper.SetValueIfNotNull(ShotClockHorn, game.ShotClock.SetShotClockHorn);
        NullableHelper.SetValueIfNotNull(ShotClockRunning, game.ShotClock.SetShotClockRunning);
        NullableHelper.SetValueIfNotNull(ShotClockVisible, game.ShotClock.SetShotClockVisible);
    }
}