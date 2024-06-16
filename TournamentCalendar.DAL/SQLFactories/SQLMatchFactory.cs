using TournamentCalendar.DAL.Domain;

namespace TournamentCalendar.DAL.SQLFactories;

public class SQLMatchFactory : IMatchFactory
{
    public SQLMatchFactory()
    {
    }

    public IEnumerable<MatchData> GetMatches()
    {
        throw new NotImplementedException();
    }
}