using System;
using Foundation;
using AVFoundation;

[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.iOS.Dependency.AudioRecorder))]
namespace PurposeColor.iOS.Dependency
{
	public class AudioRecorder : PurposeColor.interfaces.IAudioRecorder
	{
		AVFoundation.AVAudioRecorder recorder;
		Foundation.NSError error;
		Foundation.NSUrl url;
		Foundation.NSDictionary settings;
		public AVAudioPlayer player;

		public AudioRecorder ()
		{
		}

		public bool RecordAudio()
		{
			try {
				var audioSession = AVFoundation.AVAudioSession.SharedInstance ();
				var err = audioSession.SetCategory (AVFoundation.AVAudioSessionCategory.PlayAndRecord);
				if (err != null) {
					return false;
				}
				err = audioSession.SetActive (true);
				if (err != null) {
					return false;
				}
				string directoryname = string.Empty;

				try {
					var documents =
						Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
					directoryname = System.IO.Path.Combine (documents, "PurposeColor/Audio");
					System.IO.Directory.CreateDirectory(directoryname);
				} catch (Exception ex) {
					var test = ex.Message;
				}

				string fileName = string.Format ("Audio{0}.wav", DateTime.Now.ToString ("yyyyMMddHHmmss"));
				string audioFilePath = System.IO.Path.Combine (directoryname, fileName);

				url = Foundation.NSUrl.FromFilename (audioFilePath);

				//set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
				Foundation.NSObject[] values = new NSObject[] {
					NSNumber.FromFloat (44100.0f), //Sample Rate
					NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat
					NSNumber.FromInt32 (1), //Channels
					NSNumber.FromInt32 (16) //PCMBitDepth
				};
				
				//Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
				NSObject[] keys = new NSObject[] {
					AVAudioSettings.AVSampleRateKey,
					AVAudioSettings.AVFormatIDKey,
					AVAudioSettings.AVNumberOfChannelsKey,
					AVAudioSettings.AVLinearPCMBitDepthKey
				};
				//Set Settings with the Values and Keys to create the NSDictionary
				settings = NSDictionary.FromObjectsAndKeys (values, keys);
				var error  = new NSError();
				//Set recorder parameters
				recorder = AVAudioRecorder.Create (url, new AudioSettings (settings), out error);
				//Set Recorder to Prepare To Record
				recorder.PrepareToRecord ();
				recorder.Record ();

				return true;
			}
			catch (Exception ex) 
			{
				var tt = ex.Message;
				return false;
			}

		}

		public void StopRecording()
		{
			recorder.Stop ();
		}

		public void PlayAudio()
		{
			try {
				var error  = new NSError();
				if (player != null) {
					
				}
				player = AVAudioPlayer.FromUrl (url);
				player.NumberOfLoops = 1;
				player.Volume = 1.0f;
				player.FinishedPlaying += DidFinishPlaying;
				player.PrepareToPlay ();
				player.Play ();
			} 
			catch (Exception ex) 
			{
				var tt = ex.Message;
			}
		}

		public static void DidFinishPlaying(object sender , AVStatusEventArgs e)
		{
			if (e.Status)
			{
				GC.Collect ();
			}
		}
	}
}

