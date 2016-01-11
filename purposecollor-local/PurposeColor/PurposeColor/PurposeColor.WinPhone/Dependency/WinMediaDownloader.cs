using Microsoft.Phone.Controls;
using PurposeColor.WinPhone.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WinMediaDownloader))]
namespace PurposeColor.WinPhone.Dependency
{

    class WinMediaDownloader : IVideoDownloader
    {
        private PhoneApplicationFrame frame;
        string downloadedDirectory;
        ProgressBarImpl progress;

        public void PlayVideo(string path)
        {
            MediaElement player = new MediaElement();
            player.Source = new Uri(path);
            player.AutoPlay = true;
            player.Play();
        }

        public async Task<bool> Download(string uri, string filename)
        {

            frame = (PhoneApplicationFrame)(System.Windows.Application.Current.RootVisual);
            await DownloadFileFromWeb(new Uri(uri), filename, CancellationToken.None);



            return true;
        }

        // first define Cancellation Token Source - I've made it global so that CancelButton has acces to it
        CancellationTokenSource cts = new CancellationTokenSource();
        public enum Problem { Ok, Cancelled, Other }; // results of my Task

        // cancelling button event
        private void CancellButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.cts != null)
                this.cts.Cancel();
        }


        public Task<Stream> DownloadFile(Uri url)
        {
            var tcs = new TaskCompletionSource<Stream>();
            var wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.TrySetException(e.Error);
                    return;
                }
                else if (e.Cancelled)
                {
                    tcs.TrySetCanceled();
                    return;
                }
                else tcs.TrySetResult(e.Result);
            };
            wc.OpenReadAsync(url);
            MessageBoxResult result = MessageBox.Show("Started downloading media. Do you like to stop the download ?", "Purpose Color", MessageBoxButton.OKCancel);
            if( result == MessageBoxResult.OK )
            {
                progress.HideProgressbar();
                wc.CancelAsync();
                return null;
            }
            return tcs.Task;
        }

        // the main method - I've described it a little below in the text
        public async Task<Problem> DownloadFileFromWeb(Uri uriToDownload, string fileName, CancellationToken cToken)
        {

            progress = new ProgressBarImpl();
            try
            {
                progress.ShowProgressbar("Downloading media....");

            

                using (IsolatedStorageFileStream testfile = IsolatedStorageFile.GetUserStoreForApplication().CreateFile("test.txt"))
                {
                    downloadedDirectory = Path.GetDirectoryName(testfile.Name);
                   
                }

                if (!string.IsNullOrWhiteSpace(downloadedDirectory) && !string.IsNullOrEmpty(downloadedDirectory))
                {
                    string filePath = downloadedDirectory +"\\" + fileName;
                    if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(filePath))
                    {
                        PurposeColor.App.WindowsDownloadedMedia = filePath;
                        progress.HideProgressbar();
                        frame.Navigate(new Uri("/sample.xaml", UriKind.Relative));
                        return Problem.Other;
                    }
                }

                Stream mystr = await DownloadFile(uriToDownload);
                if( mystr == null )
                {
                    progress.HideProgressbar();
                    return Problem.Cancelled;
                }
                using (IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (IsolatedStorageFileStream file = ISF.CreateFile(fileName))
                    {
                        downloadedDirectory = Path.GetDirectoryName( file.Name );
                        const int BUFFER_SIZE = 2048;
                        byte[] buf = new byte[BUFFER_SIZE];

                        int bytesread = 0;
                        while ((bytesread = await mystr.ReadAsync(buf, 0, BUFFER_SIZE)) > 0)
                        {
                            cToken.ThrowIfCancellationRequested();
                            file.Write(buf, 0, bytesread);
                        }

                        PurposeColor.App.WindowsDownloadedMedia = file.Name;
                        progress.HideProgressbar();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                            try
                            {
                                frame.Navigate(new Uri("/sample.xaml", UriKind.Relative));
                                // frame.Navigate(new Uri("/WinVideoPlayer.xaml?msg=" + file.Name, UriKind.Relative));
                            }
                            catch (Exception ex)
                            {
                                tcs.SetException(ex);
                            }
                        });

                        mystr = null;
                    }
                }
                progress.HideProgressbar();
                return Problem.Ok;
            }
            catch (Exception exc)
            {

                if (exc is OperationCanceledException)
                    return Problem.Cancelled;
                else return Problem.Other;
            }
        }

    }
}






// and download
/*private async void Downlaod_Click(object sender, RoutedEventArgs e)
{
   cts = new CancellationTokenSource();
   Problem fileDownloaded = await DownloadFileFromWeb(new Uri(@"http://filedress/myfile.txt", UriKind.Absolute), "myfile.txt", cts.Token);
   switch(fileDownloaded)
   {
      case Problem.Ok:
           MessageBox.Show("File downloaded");
           break;
      case Problem.Cancelled:
           MessageBox.Show("Download cancelled");
           break;
      case Problem.Other:
      default:
           MessageBox.Show("Other problem with download");
           break;
    }
}*/