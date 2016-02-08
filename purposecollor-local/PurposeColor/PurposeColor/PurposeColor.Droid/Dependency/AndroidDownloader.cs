using System;
using PurposeColor.Droid.Dependency;
using PurposeColor.Droid;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDownloader))]
namespace PurposeColor.Droid
{
	public class AndroidDownloader : IDownload
	{
		public AndroidDownloader ()
		{
		}

		public string GetLocalFileName (string path)
		{
			return "";
		}

		public async  Task<bool> DownloadFiles ( List<string> downloadUrlList )
		{

			try 
			{
				foreach (var item in downloadUrlList)
				{
					string fileName = Path.GetFileName(item);
					WebClient webClient = new WebClient();
					if( !File.Exists( App.DownloadsPath + fileName ) )
						await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
					webClient.Dispose();
				}
				return true;
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ( ex.Message );
				return false;
			}

		}
	}
}

