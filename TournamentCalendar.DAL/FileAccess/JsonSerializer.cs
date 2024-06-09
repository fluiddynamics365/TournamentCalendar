using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TournamentCalendar.DAL.FileAccess
{
    public interface IFileAccessor
    {
        string ReadAllText(string filename);
    }

    public class FileAccessWrapper : IFileAccessor
    {
        public string ReadAllText(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
