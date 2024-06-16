using Microsoft.AspNetCore.Mvc;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net;
using System.Text;
using TournamentCalendar.Services;

namespace TournamentCalendar.WebApp.Controllers
{
    public class ApiController : Controller
	{
		private readonly ILogger<ApiController> _logger;
        private IConfiguration _configuration;

        public ApiController(ILogger<ApiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult GetCalendar()
		{
			// Create the calendar
			var calendar = new Calendar
			{
				Method = "PUBLISH",
				Scale = "GREGORIAN",
				Name = "VCALENDAR"
			};


            var providerType = _configuration["DataSource:Provider"];
            MatchService matchService = new(providerType, _logger);


			_logger.LogInformation("Using matchService to get matches");
			var matches = matchService.GetMatches();
			_logger.LogInformation("{matchCount} matches retrieved", matches.Count());


			_logger.LogInformation("Processing matches into calendar");
			foreach (var match in matches)
			{

				_logger.LogInformation("Processing match {team1} vs {team2} into calendar", match.HomeTeam, match.AwayTeam);

				var duration = new TimeSpan(2, 0, 0); // Default duration 2 hours
				if (match.Round != "1" && match.Round != "2" && match.Round != "3")
				{
					duration = new TimeSpan(3, 0, 0); // Knockout matches duration 3 hours
				}

				string summary = $"{match.HomeTeamEmoji} {match.HomeTeam} vs {match.AwayTeam} {match.AwayTeamEmoji} ({match.Group ?? match.Round.ToString()})";

				var e = new CalendarEvent
				{
					Start = new CalDateTime(match.Date, "UTC"),
					End = new CalDateTime(match.Date.Add(duration), "UTC"),
					Summary = summary,
					Location = match.Location,
					Uid = match.ID.ToString()
				};

				calendar.Events.Add(e);

				_logger.LogInformation("Completed processing of {summary}", summary);
			}


			_logger.LogInformation("All matches processed. Serializing calendar...");
			// Serialize the calendar to string
			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);
			byte[] bytes = Encoding.UTF8.GetBytes(serializedCalendar);

			_logger.LogInformation("Response ready");
			return File(bytes, "text/calendar", "uefa_euro_2024_calendar.ics");
		}
    }
}