using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IReminderService
    {
        bool Remind(DateTime startDate, DateTime endtDate, string title, string message, int reminder);

		Task<bool> RequestAccessAsync ();
    }
}
