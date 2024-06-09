using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TournamentCalendarFunctionApp
{
	public class Team
	{
		public int team_id { get; set; }
		public string team_name { get; set; }
		public string emoji { get; set; }
	}

	public class Match
	{
		public int MatchNumber { get; set; }
		public string Round { get; set; }
		public string Date { get; set; }
		public string Location { get; set; }

		[JsonProperty("Home Team")]
		public string HomeTeam { get; set; }


		[JsonProperty("Away Team")]
		public string AwayTeam { get; set; }
		public string Group { get; set; }
		public string Result { get; set; }
		public string HomeTeamEmoji { get; set; }
		public string AwayTeamEmoji { get; set; }
		public Guid ID { get; set; }
	}

	internal class MatchFactory
	{
		private const string _teamsJsonPath = "uefa_euro_2024_teams.json";
		private const string _matchesJsonPath = "uefa_euro_2024_matches.json";

		private readonly ILogger _logger;

		public MatchFactory(ILogger logger)
		{
			_logger = logger;
		}

		public IList<Match> GetMatches()
		{
			List<Team> teams;
			List<Match> matches;

			_logger.LogInformation("Retrieving teams...");
			try
			{
				teams = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(_teamsJsonPath));
			}
			catch (Exception e)
			{
				_logger.LogError(e,"Error retrieving teams from JSON!");
				throw;
			}

			_logger.LogInformation("Retrieving matches...");
			try
			{
				matches = JsonConvert.DeserializeObject<List<Match>>(File.ReadAllText(_matchesJsonPath));
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error retrieving matches from JSON!");
				throw;
			}


			Dictionary<string, string> teamEmojis = new();
			foreach (Team team in teams)
			{
				teamEmojis[team.team_name] = team.emoji;
			}

			var matchDetailsList = new List<Match>();

			foreach (var match in matches)
			{
				var homeEmoji = teamEmojis.ContainsKey(match.HomeTeam) ? teamEmojis[match.HomeTeam] : "";
				var awayEmoji = teamEmojis.ContainsKey(match.AwayTeam) ? teamEmojis[match.AwayTeam] : "";

				matchDetailsList.Add(new Match
				{
					MatchNumber = match.MatchNumber,
					Date = match.Date,
					Location = match.Location,
					HomeTeam = match.HomeTeam,
					HomeTeamEmoji = homeEmoji,
					AwayTeam = match.AwayTeam,
					AwayTeamEmoji = awayEmoji,
					Round = match.Round,
					Group = match.Group,
					ID = match.ID
				});
			}

			return matchDetailsList;
		}
	}
}
