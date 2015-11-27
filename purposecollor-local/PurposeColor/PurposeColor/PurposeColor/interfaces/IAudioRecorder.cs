using System;
using System.IO;

namespace PurposeColor.interfaces
{
	public interface IAudioRecorder
	{
		bool RecordAudio();
        MemoryStream StopRecording();
		void PlayAudio();
	}
}