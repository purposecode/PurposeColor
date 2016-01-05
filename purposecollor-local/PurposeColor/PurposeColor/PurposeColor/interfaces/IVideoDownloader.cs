using System;
using System.Threading.Tasks;

namespace PurposeColor
{
	public interface IVideoDownloader
	{
		Task<bool> Download(string uri, string filename);
	
		void PlayVideo (string path);
	}
}

