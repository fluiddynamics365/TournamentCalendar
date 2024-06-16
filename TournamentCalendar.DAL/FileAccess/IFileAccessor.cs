namespace TournamentCalendar.DAL.FileAccess
{
    public interface IFileAccessor
    {
        string ReadAllText(string filename);
    }
}
