[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.WinPhone.Dependency.AudioRecorder))]
namespace PurposeColor.WinPhone.Dependency
{
    public class AudioRecorder
    {
        public AudioRecorder()
        {
        }

        public bool RecordAudio()
        {
            try
            {
                Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var dataFolder = local.CreateFolderAsync("Purposecolor", Windows.Storage.CreationCollisionOption.OpenIfExists);


            }
            catch (System.Exception)
            {
                
            }
            

            return false;
        }

        public void StopRecording()
        {
            try
            {

            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void PlayAudio()
        {
            try
            {

            }
            catch (System.Exception)
            {
                
                throw;
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
