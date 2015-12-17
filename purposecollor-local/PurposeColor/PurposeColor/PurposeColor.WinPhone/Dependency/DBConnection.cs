using PurposeColor.interfaces;
using PurposeColor.WinPhone.Dependency;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;
using System.Windows;
using System.Threading.Tasks;

[assembly: Dependency(typeof(DBConnection))]
namespace PurposeColor.WinPhone.Dependency
{
    public class DBConnection : IDBConnection
    {
        public DBConnection()
        {
        }

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            try
            {
                var fileName = "PurposeColorDB.db3";
                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
                var platform = new SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8(); //wp 8.0
                var connection = new SQLite.Net.SQLiteConnection(platform, path);
                return connection;
            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
                return null;
            }
            
        }
    }
}
