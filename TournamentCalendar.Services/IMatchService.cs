using TournamentCalendar.Services.Domain;

namespace TournamentCalendar.Services
{
    public interface IMatchService
    {
        IEnumerable<Match> GetMatches();
    }
}