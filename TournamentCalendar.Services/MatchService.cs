namespace TournamentCalendar.Services;

using TournamentCalendar.DAL;
using TournamentCalendar.DAL.Domain;
using TournamentCalendar.DAL.JSONFactories;
using TournamentCalendar.DAL.SQLFactories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TournamentCalendar.Services.Domain;

public class MatchService : IMatchService
{
    private readonly string _providerType;
    private readonly ILogger _logger;

    public MatchService(string providerType, ILogger logger)
    {
        _providerType = providerType;
        _logger = logger;
    }

    public IEnumerable<Match> GetMatches()
    {
        IMatchFactory matchFactory = _providerType switch
        {
            "JSON" => new JSONMatchFactory("uefa_euro_2024_teams.json", "uefa_euro_2024_matches.json", _logger),
            "Sql" => new SQLMatchFactory(),
            _ => throw new NotImplementedException(),
        };

        var matches = matchFactory.GetMatches();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MatchData, Match>();
        }
        );

        IMapper mapper = config.CreateMapper();
        return mapper.Map<IEnumerable<Match>>(matches);

    }
}

