using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using TournamentCalendar.DAL.Domain;
using TournamentCalendar.DAL.FileAccess;
using TournamentCalendar.DAL.JSONFactories;

namespace TournamentCalendar.Tests
{
    [TestFixture]
    public class JSONMatchFactoryTests
    {
        private Mock<ILogger<JSONMatchFactory>> _mockLogger;
        private Mock<IJsonSerializer> _mockJsonSerializer;
        private Mock<IFileAccessor> _mockFileAccessor;
        private JSONMatchFactory _factory;
        private string _teamsJsonPath;
        private string _matchesJsonPath;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<JSONMatchFactory>>();
            _mockJsonSerializer = new Mock<IJsonSerializer>();
            _mockFileAccessor = new Mock<IFileAccessor>();
            _teamsJsonPath = "uefa_euro_2024_teams.json";
            _matchesJsonPath = "uefa_euro_2024_matches.json";
            _factory = new JSONMatchFactory(_teamsJsonPath, _matchesJsonPath, _mockLogger.Object, _mockJsonSerializer.Object, _mockFileAccessor.Object);
        }

        [Test]
        public void GetMatches_ShouldReturnMatches_WhenJsonIsValid()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { team_name = "Team A", emoji = "🏆" }
            };
            var matches = new List<Fixture>
            {
                new Fixture { MatchNumber = 1, Round = "Group Stage", Date = "2024-06-14", Location = "City A", HomeTeam = "Team A", AwayTeam = "Team B", Group = "A", ID = Guid.NewGuid() }
            };

            _mockJsonSerializer.Setup(js => js.Deserialize<List<Team>>(It.IsAny<string>())).Returns(teams);
            _mockJsonSerializer.Setup(js => js.Deserialize<List<Fixture>>(It.IsAny<string>())).Returns(matches);

            var factory = new JSONMatchFactory(_teamsJsonPath, _matchesJsonPath, _mockLogger.Object, _mockJsonSerializer.Object, _mockFileAccessor.Object);
            // Act
            var result = factory.GetMatches();

            // Assert
            var matchList = new List<Fixture>(result);
            Assert.That(matchList, Has.Count.EqualTo(1));
            Assert.That(matchList[0].HomeTeam, Is.EqualTo("Team A"));
            Assert.That(matchList[0].AwayTeam, Is.EqualTo("Team B"));
            Assert.That(matchList[0].HomeTeamEmoji, Is.EqualTo("🏆"));
        }

        [Test]
        public void GetMatches_ShouldThrowException_WhenTeamsJsonIsInvalid()
        {
            // Arrange
            _mockJsonSerializer.Setup(js => js.Deserialize<List<Team>>(It.IsAny<string>())).Throws<JsonException>();

            // Act & Assert
            Assert.Throws<JsonException>(() => {
                var result = _factory.GetMatches();
                _ = new List<Fixture>(result);
            });
            _mockLogger.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }

        [Test]
        public void GetMatches_ShouldThrowException_WhenMatchesJsonIsInvalid()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { team_name = "Team A", emoji = "🏆" }
            };

            _mockJsonSerializer.Setup(js => js.Deserialize<List<Team>>(It.IsAny<string>())).Returns(teams);
            _mockJsonSerializer.Setup(js => js.Deserialize<List<Fixture>>(It.IsAny<string>())).Throws<JsonException>();

            // Act & Assert
            Assert.Throws<JsonException>(() => {
                var result = _factory.GetMatches();
                _ = new List<Fixture>(result);
            });
            _mockLogger.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }
    }
}
