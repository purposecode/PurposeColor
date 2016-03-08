﻿using System;
using System.Windows.Input;
using XamarinAndroidFFmpeg;
using PurposeColor.WinPhone.Dependency;
using PurposeColor.Droid;
using System.Threading.Tasks;
using System.IO;

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

		public  MemoryStream CompressVideo( string sourceFilePath, string destinationFilePath )
		{
			if (sourceFilePath == null || destinationFilePath == null)
				return null;
			

			XamarinAndroidFFmpeg.FFMpeg ffmpeg = new FFMpeg ( MainApplication.Context, App.DownloadsPath);
			var onComplete = new MyCommand ((_) => 
				{
					
				});

			var onMessage = new MyCommand ((message) => 
				{
					System.Diagnostics.Debug.WriteLine(  "---" + message);
				});

			var callbacks = new FFMpegCallbacks (onComplete, onMessage);
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

			ffmpeg.Execute (cmds, callbacks);

			Java.IO.File toDel = new Java.IO.File ( sourceFilePath );
			toDel.Delete ();

			MemoryStream ms = new MemoryStream();    
			FileStream file = new FileStream(  destinationFilePath, FileMode.Open, FileAccess.Read);
			file.CopyTo ( ms );
			file.Close();
			return ms;

		}
	}
}

