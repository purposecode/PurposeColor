using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IShareVia
    {
        void ShareMedia( string text, string path, PurposeColor.Constants.MediaType type );
    }
}
