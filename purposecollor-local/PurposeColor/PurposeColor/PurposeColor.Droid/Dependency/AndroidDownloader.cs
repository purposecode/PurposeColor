using System;
using PurposeColor.Droid.Dependency;
using PurposeColor.Droid;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Graphics;
using Android.Media;
using System.Threading;

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

		public async  Task<bool> DownloadFiles ( List<string> downloadUrlList, CancellationToken cancelToken )
		{
			int imgMaxWidth = (int)(App.screenWidth * App.screenDensity);
			int imgMaxHeight = (int)(App.screenHeight * .50 * App.screenDensity);
			int streamLength = 0;
			try 
			{
				foreach (var item in downloadUrlList)
				{
					cancelToken.ThrowIfCancellationRequested();

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

							if(imgOptions.OutHeight <= 5000 && imgOptions.OutWidth <= 5000 )
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
									if(streamLength < 4242880 ) // 5MB = 5242880 byts, 2.5 MB = 2621440 byts
									{
										try {
											originalImage  = BitmapFactory.DecodeByteArray(memStream.ToArray(), 0, memStream.ToArray().Length);
										} 
										catch (Exception ex) 
										{
											var test = ex.Message;
											FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
											fstream.Close();
											fstream.Dispose();
											fstream = null;
											continue;
										}
										if(originalImage.Height > originalImage.Width)
										{
											if (originalImage.Height > 300 || originalImage.Width > 300) {
												originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxHeight, true);
											}
										}
										else
										{
											if (originalImage.Width > imgMaxHeight || originalImage.Height > imgMaxWidth) {
												originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxWidth, true);
											}
										}


										int compressionRate  = 100;

										#region compression ratio
										if (streamLength < 20000) {
											compressionRate = 100;
										}
										else if (streamLength < 40000) {
											compressionRate = App.screenDensity > 2 ? 100: 95;
										}
										else if (streamLength < 50000) {
											compressionRate = App.screenDensity > 2 ? 100: 91;
										}
										else if (streamLength < 100000) {
											compressionRate = App.screenDensity > 2 ? 100: 90;
										}
										else if (streamLength < 200000) {
											compressionRate = App.screenDensity > 2 ? 99: 89;
										}
										else if (streamLength <300000) {
											compressionRate = App.screenDensity > 2 ? 98: 88;
										}
										else if (streamLength < 400000) {
											compressionRate = App.screenDensity > 2 ? 97: 87;
										}
										else if (streamLength < 500000) {
											compressionRate = App.screenDensity > 2 ? 96: 86;
										}
										else if (streamLength < 600000) {
											compressionRate = App.screenDensity > 2 ? 95: 86;
										}
										else if (streamLength < 700000) {
											compressionRate = App.screenDensity > 2 ? 94: 86;
										}
										else if (streamLength < 900000) {
											compressionRate = App.screenDensity > 2 ? 92: 86;
										}
										else if (streamLength < 1000000) {
											compressionRate = App.screenDensity > 2 ? 90: 86;
										}
										else if (streamLength < 2000000) {
											compressionRate = App.screenDensity > 2 ? 85: 86;
										}
										else {
											compressionRate = App.screenDensity > 2 ? 80: 85;
										}
										#endregion

										ExifInterface exif = new ExifInterface(App.DownloadsPath+fileName);
										var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);
										int orientationP = exif.GetAttributeInt (ExifInterface.TagOrientation, 0);
										switch (orientation) 
										{
										case (int)Android.Media.Orientation.Rotate180:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										case (int) Android.Media.Orientation.Rotate270:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										case (int)Android.Media.Orientation.Rotate90:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										default:
											break;
										}
										exif.Dispose();
										exif = null;

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




		public async  Task<bool> DownloadFiles ( List<string> downloadUrlList )
		{
			int imgMaxWidth = (int)(App.screenWidth * App.screenDensity);
			int imgMaxHeight = (int)(App.screenHeight * .50 * App.screenDensity);
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

							if(imgOptions.OutHeight <= 5000 && imgOptions.OutWidth <= 5000 )
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
									if(streamLength < 4242880 ) // 5MB = 5242880 byts, 2.5 MB = 2621440 byts
									{
										try {
											originalImage  = BitmapFactory.DecodeByteArray(memStream.ToArray(), 0, memStream.ToArray().Length);
										} 
										catch (Exception ex) 
										{
											var test = ex.Message;
											FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
											fstream.Close();
											fstream.Dispose();
											fstream = null;
											continue;
										}
										if(originalImage.Height > originalImage.Width)
										{
											if (originalImage.Height > 300 || originalImage.Width > 300) {
												originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxHeight, true);
											}
										}
										else
										{
											if (originalImage.Width > imgMaxHeight || originalImage.Height > imgMaxWidth) {
												originalImage = Bitmap.CreateScaledBitmap(originalImage,  imgMaxWidth, imgMaxWidth, true);
											}
										}


										int compressionRate  = 100;

										#region compression ratio
										if (streamLength < 20000) {
											compressionRate = 100;
										}
										else if (streamLength < 40000) {
											compressionRate = App.screenDensity > 2 ? 100: 95;
										}
										else if (streamLength < 50000) {
											compressionRate = App.screenDensity > 2 ? 100: 91;
										}
										else if (streamLength < 100000) {
											compressionRate = App.screenDensity > 2 ? 100: 90;
										}
										else if (streamLength < 200000) {
											compressionRate = App.screenDensity > 2 ? 99: 89;
										}
										else if (streamLength <300000) {
											compressionRate = App.screenDensity > 2 ? 98: 88;
										}
										else if (streamLength < 400000) {
											compressionRate = App.screenDensity > 2 ? 97: 87;
										}
										else if (streamLength < 500000) {
											compressionRate = App.screenDensity > 2 ? 96: 86;
										}
										else if (streamLength < 600000) {
											compressionRate = App.screenDensity > 2 ? 95: 86;
										}
										else if (streamLength < 700000) {
											compressionRate = App.screenDensity > 2 ? 94: 86;
										}
										else if (streamLength < 900000) {
											compressionRate = App.screenDensity > 2 ? 92: 86;
										}
										else if (streamLength < 1000000) {
											compressionRate = App.screenDensity > 2 ? 90: 86;
										}
										else if (streamLength < 2000000) {
											compressionRate = App.screenDensity > 2 ? 85: 86;
										}
										else {
											compressionRate = App.screenDensity > 2 ? 80: 85;
										}
										#endregion

										ExifInterface exif = new ExifInterface(App.DownloadsPath+fileName);
										var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);
										int orientationP = exif.GetAttributeInt (ExifInterface.TagOrientation, 0);
										switch (orientation) 
										{
										case (int)Android.Media.Orientation.Rotate180:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										case (int) Android.Media.Orientation.Rotate270:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										case (int)Android.Media.Orientation.Rotate90:
											originalImage = changeOrientation (App.DownloadsPath + fileName, originalImage, orientationP);
											break;
										default:
											break;
										}
										exif.Dispose();
										exif = null;

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



		public async  Task<bool> DownloadFilesWithoutResize ( List<string> downloadUrlList )
		{
			int imgMaxWidth = (int)(App.screenWidth * App.screenDensity);
			int imgMaxHeight = (int)(App.screenHeight * .50 * App.screenDensity);
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

						/*BitmapFactory.Options imgOptions = new BitmapFactory.Options();
						imgOptions.InJustDecodeBounds = true;
						MemoryStream memStream = null;
						await BitmapFactory.DecodeFileAsync(App.DownloadsPath + fileName,imgOptions);
						if( imgOptions.OutWidth > 5000 || imgOptions.OutHeight > 5000 )
						{
							
						}*/
				
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




		Bitmap changeOrientation (string filePath, Bitmap bitmap, int orientation)
		{
			var matrix = new Matrix ();
			switch (orientation) {
			case 2:
				matrix.SetScale (-1, 1);
				break;
			case 3:
				matrix.SetRotate (180);
				break;
			case 4:
				matrix.SetRotate (180);
				matrix.PostScale (-1, 1);
				break;
			case 5:
				matrix.SetRotate (90);
				matrix.PostScale (-1, 1);
				break;
			case 6:
				matrix.SetRotate (90);
				break;
			case 7:
				matrix.SetRotate (-90);
				matrix.PostScale (-1, 1);
				break;
			case 8:
				matrix.SetRotate (-90);
				break;
			default:
				return bitmap;
			}

			try {
				Bitmap oriented = Bitmap.CreateBitmap (bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
				bitmap.Recycle ();
				return oriented;
			} catch (Exception e) {
				return bitmap;
			}
		}

	}
}

