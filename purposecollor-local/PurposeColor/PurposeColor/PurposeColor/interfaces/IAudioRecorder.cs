using System;

namespace PurposeColor.interfaces
{
	public interface IAudioRecorder
	{
		bool RecordAudio();
		void StopRecording();
		void PlayAudio();
	}
}