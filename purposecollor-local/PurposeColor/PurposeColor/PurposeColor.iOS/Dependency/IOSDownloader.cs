using System;
using PurposeColor.iOS;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;



[assembly: Xamarin.Forms.Dependency(typeof(IOSDownloader))]
namespace PurposeColor.iOS
{
	public class IOSDownloader : IDownload
	{
		public IOSDownloader ()
		{
			
		}

		public string GetLocalFileName ( string path )
		{
			string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string downloadFilePath = Path.Combine(downloadPath, path);

			return downloadFilePath;
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

