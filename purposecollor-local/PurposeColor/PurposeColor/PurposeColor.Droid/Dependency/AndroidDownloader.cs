using System;
using PurposeColor.Droid.Dependency;
using PurposeColor.Droid;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Graphics;

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
			int imgWidth = (int)(App.screenWidth * App.screenDensity);
			int imgHeight = (int)(App.screenHeight * App.screenDensity);
			try 
			{
				foreach (var item in downloadUrlList)
				{
					string fileName = System.IO.Path.GetFileName(item);
					WebClient webClient = new WebClient();
					if( !File.Exists( App.DownloadsPath + fileName ) )
					{	await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
						webClient.Dispose();

						try {
							MemoryStream memStream = new MemoryStream();
							using (FileStream fs = File.OpenRead(App.DownloadsPath + fileName))
							{
								fs.CopyTo(memStream);
								fs.Close();
								fs.Dispose();
							}
							if (memStream != null &&  memStream.ToArray().Length > 0) {
								Bitmap originalImage = BitmapFactory.DecodeByteArray(memStream.ToArray(), 0, memStream.ToArray().Length);
								Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage,  imgWidth, imgHeight, false);

								FileStream stream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
								resizedImage.Compress(Bitmap.CompressFormat.Png, 100, stream);

								stream.Close();
								stream.Dispose();
								stream = null;
								memStream.Dispose();
								memStream = null;
								resizedImage.Dispose();
								resizedImage = null;
								originalImage.Dispose();
								originalImage = null;
							}

						} catch (Exception ex) {
							var test = ex.Message;
						}
					}// if file exists

				} //foreach
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

