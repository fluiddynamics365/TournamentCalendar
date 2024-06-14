using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TournamentCalendar.WebApp.Models;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net;
using System.Text;
using TournamentCalendar.DAL;
using TournamentCalendar.DAL.JSONFactories;

namespace TournamentCalendar.WebApp.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
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

			IMatchFactory matchFactory = new JSONMatchFactory("uefa_euro_2024_teams.json", "uefa_euro_2024_matches.json", _logger);


			_logger.LogInformation("Using MatchFactory to get matches");
			var matches = matchFactory.GetMatches();
			_logger.LogInformation("{matchCount} matches retrieved", matches.Count());


			_logger.LogInformation("Processing matches into calendar");
			foreach (var match in matches)
			{

				_logger.LogInformation("Processing match {team1} vs {team2} into calendar", match.HomeTeam, match.AwayTeam);
				DateTime matchDate = DateTime.Parse(match.Date);
				var duration = new TimeSpan(2, 0, 0); // Default duration 2 hours
				if (match.Round != "1" && match.Round != "2" && match.Round != "3")
				{
					duration = new TimeSpan(3, 0, 0); // Knockout matches duration 3 hours
				}

				string summary = $"{match.HomeTeamEmoji} {match.HomeTeam} vs {match.AwayTeam} {match.AwayTeamEmoji} ({match.Group ?? match.Round.ToString()})";

				var e = new CalendarEvent
				{
					Start = new CalDateTime(matchDate, "UTC"),
					End = new CalDateTime(matchDate.Add(duration), "UTC"),
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

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
