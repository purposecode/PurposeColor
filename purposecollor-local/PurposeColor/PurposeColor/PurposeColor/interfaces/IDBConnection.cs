
namespace PurposeColor.interfaces
{
    public interface IDBConnection
    {
        SQLite.Net.SQLiteConnection GetConnection();
    }
}