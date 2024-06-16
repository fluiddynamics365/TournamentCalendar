using Microsoft.Extensions.Logging;
using TournamentCalendar.DAL.Domain;
using TournamentCalendar.DAL.FileAccess;

namespace TournamentCalendar.DAL.JSONFactories
{

    public class JSONMatchFactory : IMatchFactory
    {
        private readonly string _teamsJsonPath;
        private readonly string _matchesJsonPath;
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileAccessor _fileAccessor;

        public JSONMatchFactory(string teamsJsonPath, string matchesJsonPath, ILogger logger, IJsonSerializer jsonSerializer, IFileAccessor fileAccessor)
        {
            _teamsJsonPath = teamsJsonPath;
            _matchesJsonPath = matchesJsonPath;
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _fileAccessor = fileAccessor;
        }
        public JSONMatchFactory(string teamsJsonPath, string matchesJsonPath, ILogger logger) 
            : this(teamsJsonPath, matchesJsonPath, logger, new JsonSerializerWrapper(), new FileAccessWrapper())
        {
        }


        public IEnumerable<MatchData> GetMatches()
        {
            List<TeamData> teams;
            List<MatchData> matches;

            _logger.LogInformation("Retrieving teams...");
            try
            {
                teams = _jsonSerializer.Deserialize<List<TeamData>>(_fileAccessor.ReadAllText(_teamsJsonPath));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving teams from JSON!");
                throw;
            }

            _logger.LogInformation("Retrieving matches...");
            try
            {
                matches = _jsonSerializer.Deserialize<List<MatchData>>(_fileAccessor.ReadAllText(_matchesJsonPath));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving matches from JSON!");
                throw;
            }


            Dictionary<string, string> teamEmojis = new();
            foreach (TeamData team in teams)
            {
                teamEmojis[team.team_name] = team.emoji;
            }


            foreach (var match in matches)
            {
                var matchWithEmojis = match;
                var homeEmoji = teamEmojis.ContainsKey(match.HomeTeam) ? teamEmojis[match.HomeTeam] : "";
                var awayEmoji = teamEmojis.ContainsKey(match.AwayTeam) ? teamEmojis[match.AwayTeam] : "";

                matchWithEmojis.HomeTeamEmoji = homeEmoji;
                matchWithEmojis.AwayTeamEmoji = awayEmoji;

                yield return matchWithEmojis;
            }
        }
    }
}
