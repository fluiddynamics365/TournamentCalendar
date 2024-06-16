namespace TournamentCalendar.DAL.FileAccess
{

    public class FileAccessWrapper : IFileAccessor
    {
        public string ReadAllText(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
