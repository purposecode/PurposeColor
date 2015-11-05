using PurposeColor.interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.iOS.Dependency.DBConnection))]
namespace PurposeColor.iOS.Dependency
{
    public class DBConnection : IDBConnection
    {
        public DBConnection()
        {
        }

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var fileName = "PurposeColorDB.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = System.IO.Path.Combine(documentsPath, "..", "Library");
            var path = System.IO.Path.Combine(libraryPath, fileName);
            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);

            return connection;

        }
    }
}
