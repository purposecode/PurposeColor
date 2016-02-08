using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PurposeColor
{
	public interface IDownload
	{
		Task<bool> DownloadFiles ( List<string> downloadUrlList );
		string GetLocalFileName (string path);
	}
}

