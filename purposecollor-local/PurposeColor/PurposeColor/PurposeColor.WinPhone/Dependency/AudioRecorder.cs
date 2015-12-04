using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using Microsoft.Phone.BackgroundAudio;
using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.WinPhone.Dependency.AudioRecorder))]
namespace PurposeColor.WinPhone.Dependency
{
    public class AudioRecorder : PurposeColor.interfaces.IAudioRecorder
    {
        Microphone microphone = null;
        byte[] buffer;
        MemoryStream stream = null;
        bool StopRequested;
        int sampleRate;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        string fileName = string.Empty;

        public string AudioPath
        {
            get
            {
                return fileName;
            }
        }

        public AudioRecorder()
        {
            microphone = Microphone.Default;
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
        }

        public bool RecordAudio()
        {
            try
            {
                StopRequested = false;
                if (microphone == null)
                {
                    microphone = Microphone.Default;
                }
                sampleRate = microphone.SampleRate;
                microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
                buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];

                microphone.BufferReady += new EventHandler<EventArgs>(Microphone_BufferReady);
                stream = new MemoryStream(1048576);
                stream.SetLength(0);
                WriteWavHeader(microphone.SampleRate);

                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(33);
                dispatcherTimer.Tick += delegate
                {
                    try
                    {
                        Microsoft.Xna.Framework.FrameworkDispatcher.Update();
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("FrameworkDispatcher :" + ex.Message);
                    }
                };

                dispatcherTimer.Start();
                Microsoft.Xna.Framework.FrameworkDispatcher.Update();
                microphone.Start();

                System.Diagnostics.Debug.WriteLine("Started Recording");
                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RecordAudio :" + ex.Message);
                return false;
            }
        }

        public MemoryStream StopRecording()
        {
            try
            {
                StopRequested = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("StopRecording :" + ex.Message);
            }

            return null; //for testing only
        }

        public void PlayAudio()
        {
            try
            {
                var fname = fileName;
                Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var fileUrl = local.Path + @"\Purposecolor\Audio\" + fileName;//test.wav
                AudioTrack audioTrack = new AudioTrack(new Uri(fileUrl, UriKind.Relative), string.Empty, string.Empty, string.Empty, null);
                BackgroundAudioPlayer.Instance.Track = audioTrack;
                BackgroundAudioPlayer.Instance.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PlayAudio :" + ex.Message);
            }
        }

        private async void Microphone_BufferReady(object sender, EventArgs e)
        {
            try
            {
                microphone.GetData(buffer);
                stream.Write(buffer, 0, buffer.Length);

                if (!StopRequested)
                {
                    return;
                }

                microphone.Stop();
                microphone.BufferReady -= new EventHandler<EventArgs>(Microphone_BufferReady);
                dispatcherTimer.Stop();
                microphone.GetData(buffer);
                UpdateWavHeader();
                stream.Write(buffer, 0, buffer.Length);

                fileName = string.Format("Audio{0}.wav", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var dataFolder = await local.CreateFolderAsync("Purposecolor", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var audioFolder = await dataFolder.CreateFolderAsync("Audio", Windows.Storage.CreationCollisionOption.OpenIfExists);
                using (var audioFile = File.Create(audioFolder.Path + "\\" + fileName))
                {
                    var dataBuffer = stream.GetBuffer();
                    audioFile.Write(dataBuffer, 0, (int)stream.Length);
                    audioFile.Flush();
                    audioFile.Close();
                }

                audioFolder = null;
                dataFolder = null;
                buffer = null;
                microphone = null;
                stream.Close();
                stream = null;
                local = null;
                GC.Collect();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Microphone_BufferReady :" + ex.Message);
            }
        }

        public void WriteWavHeader(int sampleRate)
        {
            const int bitsPerSample = 16;
            const int bytesPerSample = bitsPerSample / 8;
            var encoding = System.Text.Encoding.UTF8;
            // ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
            stream.Write(encoding.GetBytes("RIFF"), 0, 4);

            // NOTE this will be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);

            // Format Contains the letters "WAVE"(0x57415645 big-endian form).
            stream.Write(encoding.GetBytes("WAVE"), 0, 4);

            // Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
            stream.Write(encoding.GetBytes("fmt "), 0, 4);

            // Subchunk1Size 16 for PCM.  This is the size of therest of the Subchunk which follows this number.
            stream.Write(BitConverter.GetBytes(16), 0, 4);

            // AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // NumChannels Mono = 1, Stereo = 2, etc.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // SampleRate 8000, 44100, etc.
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // ByteRate =  SampleRate * NumChannels * BitsPerSample/8
            stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample), 0, 4);

            // BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);

            // BitsPerSample    8 bits = 8, 16 bits = 16, etc.
            stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

            // Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
            stream.Write(encoding.GetBytes("data"), 0, 4);

            // NOTE to be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);

        }

        public void UpdateWavHeader()
        {
            try
            {

                //if (!stream.CanSeek) throw new Exception("Can't seek stream to update wav header");

                var oldPos = stream.Position;

                // ChunkSize  36 + SubChunk2Size
                stream.Seek(4, SeekOrigin.Begin);
                stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);

                // Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
                stream.Seek(40, SeekOrigin.Begin);
                stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);

                stream.Seek(oldPos, SeekOrigin.Begin);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateWavHeader :" + ex.Message);
            }
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    break;

                case PlayState.Paused:
                case PlayState.Stopped:
                    BackgroundAudioPlayer.Instance.Track = null;
                    GC.Collect();
                    break;
            }

            if (null != BackgroundAudioPlayer.Instance.Track)
            {

            }
        }
    }
}