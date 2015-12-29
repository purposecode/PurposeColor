using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class GlobalSettings
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int ID { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool ShowRegistrationScreen { get; set; }
        public bool IsFirstLogin { get; set; }

    }
}
