using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace PurposeColor
{
	public interface IDownload
	{
		Task<bool> DownloadFiles (List<string> downloadUrlList, CancellationToken cancelToken);
		Task<bool> DownloadFiles ( List<string> downloadUrlList );
		string GetLocalFileName (string path);
	}
}

