using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

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
        string FileName = string.Empty;

        public AudioRecorder()
        {
            microphone = Microphone.Default;
        }

        public bool RecordAudio()
        {
            try
            {
                //                Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                //                string dataFolder = local.CreateFolderAsync("Purposecolor/Audio", Windows.Storage.CreationCollisionOption.OpenIfExists);
                ////.......................................//
                StopRequested = false;
                if (microphone == null)
                {
                    microphone = Microphone.Default;
                }
                sampleRate = microphone.SampleRate;
                FileName = string.Format("Audio{0}.3gpp", DateTime.Now.ToString("yyyyMMddHHmmss"));

                microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
                buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];

                microphone.BufferReady += new EventHandler<EventArgs>(Microphone_BufferReady);
                stream = new MemoryStream(1048576);
                stream.SetLength(0);

                Microsoft.Xna.Framework.FrameworkDispatcher.Update();
                microphone.Start();
                
                
                //System.Diagnostics.Debug.WriteLine("Started Recording");
                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RecordAudio :" + ex.Message);
                return false;
            }
        }

        public void StopRecording()
        {
            try
            {
                StopRequested = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("StopRecording :" + ex.Message);
            }
        }

        public void PlayAudio()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("PlayAudio");
                if (stream != null)
                {
                    if (stream.Length < 1 || microphone.SampleRate <= 0)
                    {
                        return;
                    }

                    SoundEffect sound = new SoundEffect(stream.ToArray(), microphone.SampleRate, AudioChannels.Stereo);
                    SoundEffectInstance isound = sound.CreateInstance();
                    isound.Pitch = 0f;
                    isound.Volume = 3f;
                    isound.Play();
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PlayAudio :" + ex.Message);
            }
        }

        private void Microphone_BufferReady(object sender, EventArgs e)
        {
            try
            {
                //Microsoft.Xna.Framework.FrameworkDispatcher.Update();

                microphone.GetData(buffer);
                stream.Write(buffer, 0, buffer.Length);

                if (!StopRequested)
                {
                    return;
                }

                microphone.Stop();
                microphone.BufferReady -= new EventHandler<EventArgs>(Microphone_BufferReady);
                
                var isoStore = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

                using (var targetFile = isoStore.CreateFile(FileName))
                {
                    // WaveHeaderWriter.WriteHeader(targetFile, (int)stream.Length, 1, );
                    var dataBuffer = stream.GetBuffer();
                    targetFile.Write(dataBuffer, 0, (int)stream.Length);
                    targetFile.Flush();
                    targetFile.Close();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Microphone_BufferReady :" + ex.Message);
            }
        }

        /*
        private async System.Threading.Tasks.Task WriteToFile()
        {
        
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(this.textBox1.Text.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("DataFile.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        
        }
        */
    }
}
