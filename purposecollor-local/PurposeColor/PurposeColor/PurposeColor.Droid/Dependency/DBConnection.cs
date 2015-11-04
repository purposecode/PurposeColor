using PurposeColor.Droid.Dependency;
using PurposeColor.interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DBConnection))]
namespace PurposeColor.Droid.Dependency
{
    public class DBConnection : IDBConnection
    {
        public DBConnection ()
        {
        }

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var fileName = "PurposeColorDB.db3";
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentPath, fileName);
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);

            return connection;
        }

    }
}