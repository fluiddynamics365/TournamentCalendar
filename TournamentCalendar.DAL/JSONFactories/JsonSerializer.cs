using System.Text.Json;

namespace TournamentCalendar.DAL.JSONFactories
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);
    }

    public class JsonSerializerWrapper : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
