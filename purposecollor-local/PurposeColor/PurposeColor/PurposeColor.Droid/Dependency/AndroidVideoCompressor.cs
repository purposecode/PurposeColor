using System;
using System.Windows.Input;
using XamarinAndroidFFmpeg;
using PurposeColor.WinPhone.Dependency;
using PurposeColor.Droid;
using System.Threading.Tasks;
using System.IO;
using Android.Media;
using Android.Graphics;
using Java.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidVideoCompressor))]
namespace PurposeColor.Droid
{
	public class MyCommand : ICommand
	{
		public delegate void ICommandOnExecute(object parameter = null);
		public delegate bool ICommandOnCanExecute(object parameter);

		private ICommandOnExecute _execute;
		private ICommandOnCanExecute _canExecute;

		public MyCommand(ICommandOnExecute onExecuteMethod)
		{
			_execute = onExecuteMethod;
		}

		public MyCommand(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod)
		{
			_execute = onExecuteMethod;
			_canExecute = onCanExecuteMethod;
		}

		#region ICommand Members

		public event EventHandler CanExecuteChanged
		{
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null && _execute != null)
				return true;

			return _canExecute.Invoke(parameter);
		}

		public void Execute(object parameter)
		{	
			if (_execute == null)
				return;

			_execute.Invoke(parameter);
		}

		#endregion
	}

	public class AndroidVideoCompressor : IVideoCompressor
	{

		public  MemoryStream CompressVideo( string sourceFilePath, string destinationFilePath, bool deleteSourceFile )
		{
			if (sourceFilePath == null || destinationFilePath == null)
				return null;

			MediaMetadataRetriever media = new MediaMetadataRetriever ();
			media.SetDataSource ( sourceFilePath );
			string videoRotation = media.ExtractMetadata ( MetadataKey.VideoRotation );


			XamarinAndroidFFmpeg.FFMpeg ffmpeg = new FFMpeg ( MainApplication.Context, App.DownloadsPath);
			var onComplete = new MyCommand ((_) => 
				{
					
				});

			var onMessage = new MyCommand ((message) => 
				{
					System.Diagnostics.Debug.WriteLine(  "---" + message);
				});


			if (videoRotation != null && videoRotation == "90")
			{
				string[] cmds = new string[] 
				{
					"-i",
					sourceFilePath,
					"-vcodec",
					"mpeg4",
					"-acodec",
					"aac",
					"-strict",
					"-2",
					"-ac",
					"1",
					"-ar",
					"16000",
					"-r",
					"13",
					"-ab",
					"32000",
					"-vf",
					"transpose=1",
					"-y",
					destinationFilePath
				};
				var callbacks = new FFMpegCallbacks (onComplete, onMessage);
				ffmpeg.Execute (cmds, callbacks);
			} 
			else 
			{
				string[] cmds = new string[] 
				{
					"-i",
					sourceFilePath,
					"-vcodec",
					"mpeg4",
					"-acodec",
					"aac",
					"-strict",
					"-2",
					"-ac",
					"1",
					"-ar",
					"16000",
					"-r",
					"13",
					"-ab",
					"32000",
					"-y",
					destinationFilePath
				};
				var callbacks = new FFMpegCallbacks (onComplete, onMessage);
				ffmpeg.Execute (cmds, callbacks);
			}
		

			if (deleteSourceFile) 
			{
				Java.IO.File toDel = new Java.IO.File ( sourceFilePath );
				toDel.Delete ();
			}


			MemoryStream ms = new MemoryStream();    
			FileStream file = new FileStream(  destinationFilePath, FileMode.Open, FileAccess.Read);
			file.CopyTo ( ms );
			file.Close();
			return ms;

		}

		public MemoryStream CreateVideoThumbnail ( string inputVideoPath, string outputImagePath )
		{

			MediaMetadataRetriever media = new MediaMetadataRetriever ();
			media.SetDataSource ( inputVideoPath );
			string videoRotation = media.ExtractMetadata ( MetadataKey.VideoRotation );

			XamarinAndroidFFmpeg.FFMpeg ffmpeg = new FFMpeg ( MainApplication.Context, App.DownloadsPath);
			var onComplete = new MyCommand ((_) => 
				{

				});

			var onMessage = new MyCommand ((message) => 
				{
					System.Diagnostics.Debug.WriteLine(  "---" + message);
				});

			var callbacks = new FFMpegCallbacks (onComplete, onMessage);

			if (videoRotation != null && videoRotation == "90") 
			{
				string[] cmds = new string[] {
					"-i",
					inputVideoPath,
					"-ss",
					"00:00:01.000",
					"-vf",
					"transpose=1",
					outputImagePath
				};
				ffmpeg.Execute (cmds, callbacks);
			} 
			else
			{
				string[] cmds = new string[]
				{
					"-i",
					inputVideoPath,
					"-ss",
					"00:00:01.000",
					outputImagePath
				};
				ffmpeg.Execute (cmds, callbacks);
			}

			MemoryStream ms = new MemoryStream ();
			FileStream stream = new FileStream (outputImagePath, FileMode.Open);
			stream.CopyTo (ms);
			return ms;

		}


		private static void CreateFile(Bitmap bitmap, String outFilePath ) 
		{
			//Java.IO.File imageFile = new Java.IO.File(App.DownloadsPath, outFilePath  );

			try 
			{
				FileStream os = new FileStream( outFilePath, FileMode.Create );
				bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
				os.Flush();
				os.Close();
			} 
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine ( "Error in creating file.." );
			}
		}
			
	}


}

