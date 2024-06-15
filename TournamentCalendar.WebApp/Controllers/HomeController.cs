using Microsoft.AspNetCore.Mvc;
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
			return RedirectToAction("GetCalendar", "Api");
		}

		 public ActionResult Index()
        {
			IMatchFactory matchFactory = new JSONMatchFactory("uefa_euro_2024_teams.json", "uefa_euro_2024_matches.json", _logger);

			_logger.LogInformation("Using MatchFactory to get matches");
			var matches = matchFactory.GetMatches();

            return View(matches);
        }
	}
}
