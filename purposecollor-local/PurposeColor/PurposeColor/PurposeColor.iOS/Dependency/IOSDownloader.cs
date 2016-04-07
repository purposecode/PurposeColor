﻿using System;
using PurposeColor.iOS;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;
using ImageIO;
using PurposeColor.interfaces;
using Foundation;
using UIKit;
using System.Threading;



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



		public async Task<bool> DownloadFilesWithoutResize (List<string> downloadUrlList)
		{
			try 
			{
				int requiredHeight = (int)(App.screenHeight * 0.5 * App.screenDensity);
				int requiredWidth = (int)(App.screenWidth * App.screenDensity);
				int imageHeight = 0;
				int imagewidth = 0;

				foreach (var item in downloadUrlList)
				{
					string fileName = Path.GetFileName(item);

					Uri uri = new Uri( App.DownloadsPath + fileName);

					if( !File.Exists( App.DownloadsPath + fileName ))
					{
						WebClient webClient = new WebClient();
						await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
						webClient.Dispose();

					}
				}
				return true;
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ( ex.Message );
				return false;
			}
		}

		public async  Task<bool> DownloadFiles ( List<string> downloadUrlList, CancellationToken cancelToken  )
		{

			try 
			{
				int requiredHeight = (int)(App.screenHeight * 0.5 * App.screenDensity);
				int requiredWidth = (int)(App.screenWidth * App.screenDensity);
				int imageHeight = 0;
				int imagewidth = 0;

				foreach (var item in downloadUrlList)
				{
					if( cancelToken != null )
						cancelToken.ThrowIfCancellationRequested();

					string fileName = Path.GetFileName(item);

					Uri uri = new Uri( App.DownloadsPath + fileName);

					if( !File.Exists( App.DownloadsPath + fileName ))
					{
						WebClient webClient = new WebClient();
						await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
						webClient.Dispose();

						#region Resize and compression

						try {
							ImageIO.CGImageSource myImageSource = null;
							myImageSource = ImageIO.CGImageSource.FromUrl(uri,null);
							var ns = new Foundation.NSDictionary();
							var imageProperties = myImageSource.CopyProperties(ns, 0);
							myImageSource.Dispose();
							myImageSource = null;

							var iwid = imageProperties[CGImageProperties.PixelWidth];
							var ihei = imageProperties[CGImageProperties.PixelHeight];
							ns.Dispose();
							ns = null;

							bool doResize = false;

							imagewidth = Convert.ToInt32(Convert.ToString(iwid));
							imageHeight = Convert.ToInt32(Convert.ToString(ihei));
							if (imagewidth < 5000 && imageHeight < 5000) {
								if(imageHeight > imagewidth)
								{
									if(imageHeight > requiredHeight || imagewidth > requiredWidth)
									{
										doResize = true;
									}
								}
								else
								{
									if(imageHeight > requiredWidth || imagewidth > requiredHeight)
									{
										doResize = true;
									}
								}

							}

							if(doResize)
							{
								UIImage image = null;

								Byte[] myByteArray = null;
								IResize resize = Xamarin.Forms.DependencyService.Get<IResize>();
								Byte[] myByteA = null;

								image = UIImage.FromFile(App.DownloadsPath +fileName);

								if(image == null)
								{
									continue;
								}

								try {
									NSData imageData = image.AsPNG();
									myByteArray = new Byte[imageData.Length];
									System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
									myByteA = new Byte[imageData.Length];
									myByteA = resize.Resize(myByteArray, requiredWidth, requiredHeight, App.DownloadsPath + fileName);

									try {
										int compress_ratio = 99;
										if(myByteA.Length < 100000) {
											compress_ratio = 99;
										}
										else if(myByteA.Length < 400000) {
											compress_ratio = 98;
										}else if(myByteA.Length < 600000) {
											compress_ratio = 97;
										}
										else if(myByteA.Length < 800000) {
											compress_ratio = 96;
										}else{
											compress_ratio = 90;
										}
										MemoryStream compressedImage = resize.CompessImage(compress_ratio, new MemoryStream(myByteA));
										myByteA = new Byte[compressedImage.ToArray().Length];
										myByteA = compressedImage.ToArray();
									} catch (System.Exception ex) {
										var test = ex.Message;
									}

									imageData.Dispose();
									myByteArray = null;
									imageData.Dispose();
									imageData = null;

								} catch (Exception ex) 
								{
									var test = ex.Message;

									myByteA = null;
									continue;
								}

								if(myByteA != null && myByteA.Length > 0)
								{
									FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create, FileAccess.ReadWrite);
									fstream.Write(myByteA, 0, myByteA.Length);
									fstream.Close();
									fstream.Dispose();
									fstream = null;
								}

								myByteA = null;
								image.Dispose();
								image  = null;
							}
						} catch (Exception ex) {
							var test = ex.Message;

							FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
							fstream.Close();
							fstream.Dispose();
							fstream = null;
						}
						#endregion
					}
				}
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

			try 
			{
				int requiredHeight = (int)(App.screenHeight * 0.5 * App.screenDensity);
				int requiredWidth = (int)(App.screenWidth * App.screenDensity);
				int imageHeight = 0;
				int imagewidth = 0;

				foreach (var item in downloadUrlList)
				{
					string fileName = Path.GetFileName(item);

					Uri uri = new Uri( App.DownloadsPath + fileName);

					if( !File.Exists( App.DownloadsPath + fileName ))
					{
						WebClient webClient = new WebClient();
						await webClient.DownloadFileTaskAsync ( item,  App.DownloadsPath + fileName );
						webClient.Dispose();

						#region Resize and compression

						try {
							ImageIO.CGImageSource myImageSource = null;
							myImageSource = ImageIO.CGImageSource.FromUrl(uri,null);
							var ns = new Foundation.NSDictionary();
							var imageProperties = myImageSource.CopyProperties(ns, 0);
							myImageSource.Dispose();
							myImageSource = null;

							var iwid = imageProperties[CGImageProperties.PixelWidth];
							var ihei = imageProperties[CGImageProperties.PixelHeight];
							ns.Dispose();
							ns = null;

							bool doResize = false;

							imagewidth = Convert.ToInt32(Convert.ToString(iwid));
							imageHeight = Convert.ToInt32(Convert.ToString(ihei));
							if (imagewidth < 5000 && imageHeight < 5000) {
								if(imageHeight > imagewidth)
								{
									if(imageHeight > requiredHeight || imagewidth > requiredWidth)
									{
										doResize = true;
									}
								}
								else
								{
									if(imageHeight > requiredWidth || imagewidth > requiredHeight)
									{
										doResize = true;
									}
								}

							}

							if(doResize)
							{
								UIImage image = null;

								Byte[] myByteArray = null;
								IResize resize = Xamarin.Forms.DependencyService.Get<IResize>();
								Byte[] myByteA = null;

								image = UIImage.FromFile(App.DownloadsPath +fileName);

								if(image == null)
								{
									continue;
								}

								try {
									NSData imageData = image.AsPNG();
									myByteArray = new Byte[imageData.Length];
									System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
									myByteA = new Byte[imageData.Length];
									myByteA = resize.Resize(myByteArray, requiredWidth, requiredHeight, App.DownloadsPath + fileName);

									try {
										int compress_ratio = 99;
										if(myByteA.Length < 100000) {
											compress_ratio = 99;
										}
										else if(myByteA.Length < 400000) {
											compress_ratio = 98;
										}else if(myByteA.Length < 600000) {
											compress_ratio = 97;
										}
										else if(myByteA.Length < 800000) {
											compress_ratio = 96;
										}else{
											compress_ratio = 90;
										}
										MemoryStream compressedImage = resize.CompessImage(compress_ratio, new MemoryStream(myByteA));
										myByteA = new Byte[compressedImage.ToArray().Length];
										myByteA = compressedImage.ToArray();
									} catch (System.Exception ex) {
										var test = ex.Message;
									}

									imageData.Dispose();
									myByteArray = null;
									imageData.Dispose();
									imageData = null;

								} catch (Exception ex) 
								{
									var test = ex.Message;

									myByteA = null;
									continue;
								}

								if(myByteA != null && myByteA.Length > 0)
								{
									FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create, FileAccess.ReadWrite);
									fstream.Write(myByteA, 0, myByteA.Length);
									fstream.Close();
									fstream.Dispose();
									fstream = null;
								}

								myByteA = null;
								image.Dispose();
								image  = null;
							}
						} catch (Exception ex) {
							var test = ex.Message;

							FileStream fstream = new FileStream(App.DownloadsPath + fileName, FileMode.Create);
							fstream.Close();
							fstream.Dispose();
							fstream = null;
						}
						#endregion
					}
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

