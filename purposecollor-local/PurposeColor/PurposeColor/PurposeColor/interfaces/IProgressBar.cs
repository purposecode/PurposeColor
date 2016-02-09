using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace PurposeColor.interfaces
{
    public interface IProgressBar
    {
        void ShowProgressbar( string text );
		void ShowProgressbarWithCancel( string text, Action cancelAction  );
		void HideProgressbarWithCancel ();
        void HideProgressbar();
        void ShowToast(string messege);
    }
}
