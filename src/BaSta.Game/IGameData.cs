using System.Collections;

namespace BaSta.Game
{
    public interface IGameData
    {
        event GameTimeChangedDelegate GameTimeChanged;

        event GameTimeEndReachedDelegate GameTimeEnded;

        event BreakTimeChangedDelegate BreakTimeChanged;

        event BreakTimeEndReachedDelegate BreakTimeEnded;

        event ShotTimeChangedDelegate ShotTimeChanged;

        event ShotTimeEndReachedDelegate ShotTimeEnded;

        event TimeoutTimeChangedDelegate TimeoutTimeChanged;

        event TimeoutTimeEndReachedDelegate TimeoutTimeEnded;

        DataBaseFunctions DbFunctions { set; }

        Hashtable GameParameters { get; set; }

        KindOfGame GameKind { get; set; }

        Team HomeTeam { get; set; }

        Team GuestTeam { get; set; }

        int Period { get; set; }

        bool ServiceHome { get; set; }

        bool ServiceGuest { get; set; }

        long GameTimeMilliseconds { get; set; }

        long ShotTimeMilliseconds { get; set; }

        int ScoreHome { get; set; }

        int ScoreGuest { get; set; }

        int SetScoreHome { get; set; }

        int SetScoreGuest { get; set; }

        int[] SetResultHome { get; set; }

        int[] SetResultGuest { get; set; }

        int TimeoutsHome { get; set; }

        int TimeoutsGuest { get; set; }

        long TimeoutTimeMillisecondsHome { get; set; }

        long TimeoutTimeMillisecondsGuest { get; set; }

        int TeamFoulsHome { get; set; }

        int TeamFoulsGuest { get; set; }

        int PlayerChangesHome { get; set; }

        int PlayerChangesGuest { get; set; }

        int[] PenaltyNumberHome { get; set; }

        long[] PenaltyMillisecondsHome { get; set; }

        int[] PenaltyNumberGuest { get; set; }

        long[] PenaltyMillisecondsGuest { get; set; }

        Player[] Referees { get; set; }

        Player[] PlayerHome { get; set; }

        Player[] PlayerGuest { get; set; }

        ITimeCounter GameTimer { get; set; }

        DownCounter ShotTimer { get; set; }

        DownCounter TimeoutTimerHome { get; set; }

        DownCounter TimeoutTimerGuest { get; set; }

        DownCounter[] PenaltyTimerHome { get; set; }

        DownCounter[] PenaltyTimerGuest { get; set; }

        DownCounter BreakTimer { get; set; }

        void Dispose();
    }
}