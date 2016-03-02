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
					if(  !File.Exists( App.DownloadsPath + fileName ))
					{	
						await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
						webClient.Dispose();

						try {
							MemoryStream memStream = new MemoryStream();
							using (FileStream fs = File.OpenRead(App.DownloadsPath + fileName))
							{
								fs.CopyTo(memStream);
								fs.Close();
								fs.Dispose();
							}
							int streamLength = (int)memStream.ToArray().Length;

							if (memStream != null &&  memStream.ToArray().Length > 0) {
								Bitmap originalImage = BitmapFactory.DecodeByteArray(memStream.ToArray(), 0, memStream.ToArray().Length);
								Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage,  imgWidth, imgHeight, false);
								FileStream stream= null;
								if(originalImage.Width < originalImage.Height)
								{
									resizedImage = Bitmap.CreateScaledBitmap(originalImage,  imgWidth, imgHeight, false);
								}
								else
								{
									resizedImage = Bitmap.CreateScaledBitmap(originalImage, imgHeight, imgWidth, false);
								}

								stream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
								//int compressionRate = streamLength < 25000 ? 100: (streamLength < 100000 ? 80 : (streamLength < 200000 ? 60): (streamLength < 300000? 50: (streamLength < 400000? 50 :(streamLength < 500000? 40: 30))));

								//int compressionRate = streamLength < 25000 ? 100 :(streamLength < 50000? 90: (streamLength < 100000? 80: (streamLength < 200000? 70: (streamLength < 300000? 60:(streamLength < 400000? 50: (streamLength < 500000: 40:30))))))
								int compressionRate  = 100;

								if (streamLength < 20000) {
									compressionRate = 100;
								}
								else if (streamLength < 40000) {
									compressionRate = App.screenDensity > 2 ? 90: 80;
								}
								else if (streamLength < 50000) {
									compressionRate = App.screenDensity > 2 ? 85: 70;
								}
								else if (streamLength < 100000) {
									compressionRate = App.screenDensity > 2 ? 80: 60;
								}
								else if (streamLength < 200000) {
									compressionRate = App.screenDensity > 2 ? 70: 30;
								}
								else if (streamLength <300000) {
									compressionRate = App.screenDensity > 2 ? 65: 25;
								}
								else if (streamLength < 400000) {
									compressionRate = App.screenDensity > 2 ? 60: 20;
								}
								else if (streamLength < 500000) {
									compressionRate = App.screenDensity > 2 ? 50: 15;
								}
								else {
									compressionRate = App.screenDensity > 2 ? 40: 10;
								}

								resizedImage.Compress(Bitmap.CompressFormat.Jpeg, compressionRate, stream);

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

