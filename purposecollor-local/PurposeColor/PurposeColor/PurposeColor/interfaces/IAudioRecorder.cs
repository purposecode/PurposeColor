using System;
using System.IO;

namespace PurposeColor.interfaces
{
	public interface IAudioRecorder
	{
        string AudioPath { get; }
		bool RecordAudio();
        MemoryStream StopRecording();
		void PlayAudio();
	}
}