using System;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessage0x33 : IStramatelMessage
{
    private StramatelMessage0x33()
    {
    }

    public PossessionState Possession { get; private set; }
    public TimeSpan? GameClock { get; private set; }
    public int? PointsHome { get; private set; }
    public int? PointsGuest { get; private set; }
    public int? Period { get; private set; }
    public int? TeamFoulsHome { get; private set; }
    public int? TeamFoulsGuest { get; private set; }
    public int? TimeoutsHome { get; private set; }
    public int? TimeoutsGuest { get; private set; }
    public bool? Horn { get; private set; }
    public bool? GameClockRunning { get; private set; }
    public TimeSpan? TimeoutClock { get; private set; }
    public int? FoulsHomePlayer1 { get; private set; }
    public int? FoulsHomePlayer2 { get; private set; }
    public int? FoulsHomePlayer3 { get; private set; }
    public int? FoulsHomePlayer4 { get; private set; }
    public int? FoulsHomePlayer5 { get; private set; }
    public int? FoulsHomePlayer6 { get; private set; }
    public int? FoulsHomePlayer7 { get; private set; }
    public int? FoulsHomePlayer8 { get; private set; }
    public int? FoulsHomePlayer9 { get; private set; }
    public int? FoulsHomePlayer10 { get; private set; }
    public int? FoulsHomePlayer11 { get; private set; }
    public int? FoulsHomePlayer12 { get; private set; }
    public int? FoulsGuestPlayer1 { get; private set; }
    public int? FoulsGuestPlayer2 { get; private set; }
    public int? FoulsGuestPlayer3 { get; private set; }
    public int? FoulsGuestPlayer4 { get; private set; }
    public int? FoulsGuestPlayer5 { get; private set; }
    public int? FoulsGuestPlayer6 { get; private set; }
    public int? FoulsGuestPlayer7 { get; private set; }
    public int? FoulsGuestPlayer8 { get; private set; }
    public int? FoulsGuestPlayer9 { get; private set; }
    public int? FoulsGuestPlayer10 { get; private set; }
    public int? FoulsGuestPlayer11 { get; private set; }
    public int? FoulsGuestPlayer12 { get; private set; }
    public TimeSpan? ShotClock { get; private set; }
    public bool? ShotClockHorn { get; private set; }
    public bool? ShotClockRunning { get; private set; }
    public bool? ShotClockVisible { get; private set; }

    public static IStramatelMessage Parse(byte[] messageData)
    {
        // The parsing algorithm intends to extract as many information as possible and set non-parseable data to null.
        StramatelMessage0x33 message = new()
        {
            Possession = StramatelMessagePartParser.ParseStramatelPossession(messageData[3]),
            GameClock = StramatelMessagePartParser.ParseStramatelGameClock(messageData[4..8]),
            PointsHome = StramatelMessagePartParser.ParseStramatelNumber(messageData[8..11]),
            PointsGuest = StramatelMessagePartParser.ParseStramatelNumber(messageData[11..14]),
            Period = StramatelMessagePartParser.ParseStramatelNumber(messageData[14]),
            TeamFoulsHome = StramatelMessagePartParser.ParseStramatelNumber(messageData[15]),
            TeamFoulsGuest = StramatelMessagePartParser.ParseStramatelNumber(messageData[16]),
            TimeoutsHome = StramatelMessagePartParser.ParseStramatelNumber(messageData[17]),
            TimeoutsGuest = StramatelMessagePartParser.ParseStramatelNumber(messageData[18]),
            Horn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[19]),
            GameClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[20]),
            TimeoutClock = StramatelMessagePartParser.ParseStramatelShotClock((new[] {messageData[21], messageData[46], messageData[48]})),
            FoulsHomePlayer1 = StramatelMessagePartParser.ParseStramatelNumber(messageData[22]),
            FoulsHomePlayer2 = StramatelMessagePartParser.ParseStramatelNumber(messageData[23]),
            FoulsHomePlayer3 = StramatelMessagePartParser.ParseStramatelNumber(messageData[24]),
            FoulsHomePlayer4 = StramatelMessagePartParser.ParseStramatelNumber(messageData[25]),
            FoulsHomePlayer5 = StramatelMessagePartParser.ParseStramatelNumber(messageData[26]),
            FoulsHomePlayer6 = StramatelMessagePartParser.ParseStramatelNumber(messageData[27]),
            FoulsHomePlayer7 = StramatelMessagePartParser.ParseStramatelNumber(messageData[28]),
            FoulsHomePlayer8 = StramatelMessagePartParser.ParseStramatelNumber(messageData[29]),
            FoulsHomePlayer9 = StramatelMessagePartParser.ParseStramatelNumber(messageData[30]),
            FoulsHomePlayer10 = StramatelMessagePartParser.ParseStramatelNumber(messageData[31]),
            FoulsHomePlayer11 = StramatelMessagePartParser.ParseStramatelNumber(messageData[32]),
            FoulsHomePlayer12 = StramatelMessagePartParser.ParseStramatelNumber(messageData[33]),
            FoulsGuestPlayer1 = StramatelMessagePartParser.ParseStramatelNumber(messageData[34]),
            FoulsGuestPlayer2 = StramatelMessagePartParser.ParseStramatelNumber(messageData[35]),
            FoulsGuestPlayer3 = StramatelMessagePartParser.ParseStramatelNumber(messageData[36]),
            FoulsGuestPlayer4 = StramatelMessagePartParser.ParseStramatelNumber(messageData[37]),
            FoulsGuestPlayer5 = StramatelMessagePartParser.ParseStramatelNumber(messageData[38]),
            FoulsGuestPlayer6 = StramatelMessagePartParser.ParseStramatelNumber(messageData[39]),
            FoulsGuestPlayer7 = StramatelMessagePartParser.ParseStramatelNumber(messageData[40]),
            FoulsGuestPlayer8 = StramatelMessagePartParser.ParseStramatelNumber(messageData[41]),
            FoulsGuestPlayer9 = StramatelMessagePartParser.ParseStramatelNumber(messageData[42]),
            FoulsGuestPlayer10 = StramatelMessagePartParser.ParseStramatelNumber(messageData[43]),
            FoulsGuestPlayer11 = StramatelMessagePartParser.ParseStramatelNumber(messageData[44]),
            FoulsGuestPlayer12 = StramatelMessagePartParser.ParseStramatelNumber(messageData[45]),
            ShotClock = StramatelMessagePartParser.ParseStramatelShotClock(messageData[48..50]),
            ShotClockHorn = StramatelMessagePartParser.ParseStramatelBoolean(messageData[50]),
            ShotClockRunning = StramatelMessagePartParser.ParseStramatelBoolean(messageData[51]),
            ShotClockVisible = StramatelMessagePartParser.ParseStramatelBoolean(messageData[52])
        };

        return message;
    }

    public void ApplyTo(IGame game)
    {
        game.Possession.SetPossession(Possession);
        NullableHelper.SetValueIfNotNull(GameClock, game.GameClock.SetGameClock);
        NullableHelper.SetValueIfNotNull(GameClockRunning, game.GameClock.SetGameClockRunning);
        NullableHelper.SetValueIfNotNull(Horn, game.Horn.SetHorn);
        NullableHelper.SetValueIfNotNull(PointsHome, game.Home.SetPoints);
        NullableHelper.SetValueIfNotNull(PointsGuest, game.Guest.SetPoints);
        NullableHelper.SetValueIfNotNull(Period, game.Period.SetPeriod);
        NullableHelper.SetValueIfNotNull(TimeoutsHome, game.Home.SetTimeouts);
        NullableHelper.SetValueIfNotNull(TimeoutsGuest, game.Guest.SetTimeouts);
        game.TimeoutClock.SetTimeoutClock(TimeoutClock);
        NullableHelper.SetValueIfNotNull(TeamFoulsHome, game.Home.SetFouls);
        NullableHelper.SetValueIfNotNull(TeamFoulsGuest, game.Guest.SetFouls);
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer1, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(0), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer2, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(1), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer3, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(2), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer4, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(3), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer5, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(4), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer6, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(5), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer7, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(6), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer8, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(7), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer9, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(8), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer10, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(9), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer11, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(10), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsHomePlayer12, x => NullableHelper.SetValueIfNotNull(game.Home.GetPlayer(11), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer1, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(0), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer2, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(1), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer3, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(2), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer4, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(3), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer5, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(4), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer6, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(5), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer7, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(6), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer8, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(7), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer9, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(8), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer10, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(9), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer11, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(10), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(FoulsGuestPlayer12, x => NullableHelper.SetValueIfNotNull(game.Guest.GetPlayer(11), y => y.SetFouls(x)));
        NullableHelper.SetValueIfNotNull(ShotClock, game.ShotClock.SetShotClock);
        NullableHelper.SetValueIfNotNull(ShotClockHorn, game.ShotClock.SetShotClockHorn);
        NullableHelper.SetValueIfNotNull(ShotClockRunning, game.ShotClock.SetShotClockRunning);
        NullableHelper.SetValueIfNotNull(ShotClockVisible, game.ShotClock.SetShotClockVisible);
    }
}