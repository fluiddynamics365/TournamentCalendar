using TournamentCalendar.DAL.Domain;

namespace TournamentCalendar.DAL
{
    public interface IMatchFactory
    {
        IEnumerable<MatchData> GetMatches();
    }
}