using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IReminderService
    {
        void Remind(DateTime dateTime, string title, string message);
    }
}
