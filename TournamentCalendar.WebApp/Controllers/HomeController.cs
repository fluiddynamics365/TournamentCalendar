using Microsoft.AspNetCore.Mvc;
using TournamentCalendar.Services;

namespace TournamentCalendar.WebApp.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
		{
			_logger = logger;
            _configuration = configuration;
        }

        public IActionResult GetCalendar()
		{
			return RedirectToAction("GetCalendar", "Api");
		}

		 public ActionResult Index()
        {
            var providerType = _configuration["DataSource:Provider"];
            IMatchService matchService = new MatchService(providerType, _logger);
			var matches = matchService.GetMatches();

            return View(matches);
        }
	}
}
