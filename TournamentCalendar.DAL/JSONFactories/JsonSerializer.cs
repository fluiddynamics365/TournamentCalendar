using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
