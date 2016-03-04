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
			int imgMaxWidth = (int)(App.screenWidth * App.screenDensity);
			int imgMaxHeight = (int)(App.screenHeight * .50 * App.screenDensity);
//			int imgMaxWidth = (int)(App.screenWidth);
//			int imgMaxHeight = (int)(App.screenHeight);
			int streamLength = 0;
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

							BitmapFactory.Options imgOptions = new BitmapFactory.Options();
							imgOptions.InJustDecodeBounds = true;
							MemoryStream memStream = null;
							await BitmapFactory.DecodeFileAsync(App.DownloadsPath + fileName,imgOptions);

							if(imgOptions.OutHeight <= 5500 && imgOptions.OutWidth <= 5500 )
							{
								using (FileStream fs = File.OpenRead(App.DownloadsPath + fileName))
								{
									streamLength = (int)fs.Length;
									if(streamLength < 5242880) // 5MB = 5242880 byts, 2.5 MB = 2621440 byts
									{
										memStream = new MemoryStream();
										fs.CopyTo(memStream);
									}
									fs.Close();
									fs.Dispose();
								}

								if (memStream != null &&  memStream.ToArray().Length > 0) 
								{
									Bitmap originalImage = null;
									streamLength = (int)memStream.ToArray().Length;
									if(streamLength < 5242880 ) // 5MB = 5242880 byts, 2.5 MB = 2621440 byts
									{
										try {
											originalImage  = BitmapFactory.DecodeByteArray(memStream.ToArray(), 0, memStream.ToArray().Length);

//												imgOptions.InSampleSize = calculateInSampleSize(imgOptions,imgMaxWidth,imgMaxHeight);
//												imgOptions.InJustDecodeBounds = false;
//		
//												originalImage  = BitmapFactory.DecodeFile(App.DownloadsPath + fileName, imgOptions);
//											
										} 
										catch (Exception ex) 
										{
											var test = ex.Message;
											FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
											fstream.Close();
											fstream.Dispose();
											fstream = null;
											break;
										}
										if(originalImage.Height > originalImage.Width)
										{
											originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxHeight, true);
										}
										else
										{
											originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxWidth, true);
										}


										int compressionRate  = 100;

										#region compression ratio
										if (streamLength < 20000) {
											compressionRate = 100;
										}
										else if (streamLength < 40000) {
											compressionRate = App.screenDensity > 2 ? 100: 80;
										}
										else if (streamLength < 50000) {
											compressionRate = App.screenDensity > 2 ? 100: 70;
										}
										else if (streamLength < 100000) {
											compressionRate = App.screenDensity > 2 ? 100: 60;
										}
										else if (streamLength < 200000) {
											compressionRate = App.screenDensity > 2 ? 99: 30;
										}
										else if (streamLength <300000) {
											compressionRate = App.screenDensity > 2 ? 98: 25;
										}
										else if (streamLength < 400000) {
											compressionRate = App.screenDensity > 2 ? 97: 20;
										}
										else if (streamLength < 500000) {
											compressionRate = App.screenDensity > 2 ? 96: 15;
										}
										else {
											compressionRate = App.screenDensity > 2 ? 95: 10;
										}
										#endregion
										FileStream stream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
										originalImage.Compress(Bitmap.CompressFormat.Jpeg, compressionRate, stream);

										stream.Close();
										stream.Dispose();
										stream = null;
										memStream.Dispose();
										memStream = null;
										//resizedImage.Dispose();
										//resizedImage = null;
										originalImage.Recycle();
										originalImage.Dispose();
										originalImage = null;
									}
									else
									{
										FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
										fstream.Close();
										fstream.Dispose();
										fstream = null;
									}
								}
							}
							else
							{
								FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
								fstream.Close();
								fstream.Dispose();
								fstream = null;
							}
						} catch (Exception ex) {
							var test = ex.Message;
							FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
							fstream.Close();
							fstream.Dispose();
							fstream = null;
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


		public int calculateInSampleSize( BitmapFactory.Options options, int reqWidth, int reqHeight) 
		{
			// Raw height and width of image
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {

				int halfHeight = height / 2;
				int halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) > reqHeight
					&& (halfWidth / inSampleSize) > reqWidth) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}
	}
}

