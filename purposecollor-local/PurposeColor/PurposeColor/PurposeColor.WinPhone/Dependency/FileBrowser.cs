using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.WinPhone.Dependency.FileBrowser))]
namespace PurposeColor.WinPhone.Dependency
{
    public class FileBrowser : PurposeColor.interfaces.IFileBrowser
    {

        public FileBrowser()
        {

        }

        public async Task<List<string>> GetVideoFileList()
        {
            try
            {
                List<string> fileList = null;
                var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
                if (files != null)
                {
                    fileList = new List<string>();
                    foreach (var file in files)
                    {
                        string fileType = System.IO.Path.GetExtension(file.Path);
                        //fileType = fileType.Replace(".", "");

                        if (string.Compare( fileType, ".mp4", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.CompareOptions.IgnoreCase) == 0 ||
                            string.Compare( fileType, ".avi", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.CompareOptions.IgnoreCase) == 0
                            )
                        {
                            fileList.Add(file.Name);
                        }
                    }
                }

                files = null;
                return fileList;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return null;
        }

        public async Task<System.IO.MemoryStream> GetVideostream(string fileName)
        {
            try
            {
                var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
                System.IO.MemoryStream videoStream = null;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (string.Compare( file.Name, fileName, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.CompareOptions.IgnoreCase) == 0  )
                        {
                            videoStream = new System.IO.MemoryStream();
                            using (var item = System.IO.File.OpenRead(file.Path))
                            {
                                item.CopyTo(videoStream);
                            }
                            return videoStream;
                        }
                    }
                }
                if (videoStream == null)
                {
                    /// chk file in Video library//
                   // Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                    //var dataFolder = await local.CreateFolderAsync("Purposecolor", Windows.Storage.CreationCollisionOption.OpenIfExists);
               //     var dataFolder = await local.CreateFolderAsync("Videos", Windows.Storage.CreationCollisionOption.OpenIfExists);
                }
                return videoStream;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return null;
        }


    }
}
