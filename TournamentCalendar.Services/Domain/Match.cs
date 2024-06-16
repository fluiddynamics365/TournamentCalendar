namespace TournamentCalendar.Services.Domain
{
    public class Match
	{
        public Guid ID { get; set; }

        public int MatchNumber { get; set; }

        public string? Round { get; set; }
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }

        public string? Group { get; set; }
        public string? Result { get; set; }
        public string? HomeTeamEmoji { get; set; }
        public string? AwayTeamEmoji { get; set; }
    }
}

