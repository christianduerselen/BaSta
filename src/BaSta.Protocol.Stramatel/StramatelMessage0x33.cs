using System;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel;

public class StramatelMessage0x33 : IStramatelMessage
{
    private StramatelMessage0x33()
    {
    }

    public Possession? Possession { get; private set; }
    public TimeSpan? GameClock { get; private set; }
    public int? PointsHome { get; private set; }
    public int? PointsGuest { get; private set; }
    public Period? Period { get; private set; }
    public int? TeamFoulsHome { get; private set; }
    public int? TeamFoulsGuest { get; private set; }
    public int? TimeoutsHome { get; private set; }
    public int? TimeoutsGuest { get; private set; }
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

    public static IStramatelMessage Parse(byte[] message)
    {
        return null;
    }
}