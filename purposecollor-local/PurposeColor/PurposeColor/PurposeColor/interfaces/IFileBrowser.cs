using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IFileBrowser
    {
        Task<List<string>> GetVideoFileList();
        Task<System.IO.MemoryStream> GetVideostream(string fileName);
    }
}
