using System.Text.Json.Serialization;

namespace TournamentCalendar.DAL.Domain
{
    public class MatchData
    {
        public Guid ID { get; set; }

        [JsonPropertyName("Match Number")]
        public int MatchNumber { get; set; }

        public string Round { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        [JsonPropertyName("Home Team")]
        public string HomeTeam { get; set; }

        [JsonPropertyName("Away Team")]
        public string AwayTeam { get; set; }

        public string Group { get; set; }
        public string Result { get; set; }
        public string HomeTeamEmoji { get; set; }
        public string AwayTeamEmoji { get; set; }

    }
}
