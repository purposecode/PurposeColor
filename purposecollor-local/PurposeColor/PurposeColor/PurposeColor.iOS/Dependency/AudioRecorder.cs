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
					Console.WriteLine ("audioSession: {0}", err);
					return false;
				}
				err = audioSession.SetActive (true);
				if (err != null) {
					Console.WriteLine ("audioSession: {0}", err);
					return false;
				}
				// ------done initialising ---
				
				string fileName = string.Format ("Myfile{0}.wav", DateTime.Now.ToString ("yyyyMMddHHmmss"));
				string audioFilePath = System.IO.Path.Combine (System.IO.Path.GetTempPath (), fileName);
				
				Console.WriteLine ("Audio File Path: " + audioFilePath);
				
				url = Foundation.NSUrl.FromFilename (audioFilePath);
				//set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
				Foundation.NSObject[] values = new NSObject[] {
					NSNumber.FromFloat (44100.0f), //Sample Rate
					NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat
					NSNumber.FromInt32 (2), //Channels
					NSNumber.FromInt32 (16), //PCMBitDepth
					NSNumber.FromBoolean (false), //IsBigEndianKey
					NSNumber.FromBoolean (false) //IsFloatKey
				};
				
				//Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
				NSObject[] keys = new NSObject[] {
					AVAudioSettings.AVSampleRateKey,
					AVAudioSettings.AVFormatIDKey,
					AVAudioSettings.AVNumberOfChannelsKey,
					AVAudioSettings.AVLinearPCMBitDepthKey,
					AVAudioSettings.AVLinearPCMIsBigEndianKey,
					AVAudioSettings.AVLinearPCMIsFloatKey
				};
				
				//Set Settings with the Values and Keys to create the NSDictionary
				settings = NSDictionary.FromObjectsAndKeys (values, keys);
				
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
				// var file = Path.Combine("sounds", filename);
				//var Soundurl = NSUrl.FromFilename(file);

				//var Soundurl = NSUrl.FromFilename (url);
				var error  = new NSError();
				player = new AVAudioPlayer(url,null,out error);
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
				// your code
			}
		}

	}
}

