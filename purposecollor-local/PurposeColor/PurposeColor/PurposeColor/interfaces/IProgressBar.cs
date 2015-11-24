using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IProgressBar
    {
        void ShowProgressbar( string text );
        void HideProgressbar();
        void ShowToast(string messege);
    }
}
