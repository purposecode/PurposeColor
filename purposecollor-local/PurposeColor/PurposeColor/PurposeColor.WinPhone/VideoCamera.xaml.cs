using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace PurposeColor.WinPhone
{
    public partial class VideoCamera : PhoneApplicationPage// global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage//PhoneApplicationPage
    {
        // Viewfinder for capturing video.
        private VideoBrush videoRecorderBrush;

        // Source and device for capturing video.
        private CaptureSource captureSource;
        private VideoCaptureDevice videoCaptureDevice;

        // File details for storing the recording.        
        private IsolatedStorageFileStream isoVideoFile;
        private FileSink fileSink;
        private string fileName = "CameraMovie.mp4";

        // For managing button and application state.
        private enum ButtonState { Initialized, Ready, Recording, Playback, Paused, NoChange, CameraNotSupported, Stopped };
        private ButtonState currentAppState;

        public VideoCamera()
        {
            InitializeComponent();
            //PhoneAppBar = (ApplicationBar)ApplicationBar;
            //PhoneAppBar.IsVisible = true;

            InitializeVideoRecorder();
        }

        public void InitializeVideoRecorder()
        {
            try
            {
                //fileName = string.Format(@"\Purposecode\Video{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmmss"));
                fileName = string.Format("Video{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmmss"));

                if (captureSource == null)
                {
                    // Create the VideoRecorder objects.
                    captureSource = new CaptureSource();
                    fileSink = new FileSink();

                    videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

                    // Add eventhandlers for captureSource.
                    captureSource.CaptureFailed += new EventHandler<ExceptionRoutedEventArgs>(OnCaptureFailed);

                    // Initialize the camera if it exists on the device.
                    if (videoCaptureDevice != null)
                    {
                        // Create the VideoBrush for the viewfinder.
                        videoRecorderBrush = new VideoBrush();
                        videoRecorderBrush.SetSource(captureSource);

                        // Display the viewfinder image on the rectangle.
                        viewfinderRectangle.Fill = videoRecorderBrush;
                        StopPlaybackRecording.IsEnabled = false;
                        // Start video capture and display it on the viewfinder.
                        captureSource.Start();

                        // Set the button state and the message.
                        UpdateUI(ButtonState.Initialized, "Tap record to start recording...");
                    }
                    else
                    {
                        // Disable buttons when the camera is not supported by the device.
                        UpdateUI(ButtonState.CameraNotSupported, "Camera is not supported..");
                    }
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        } //InitializeVideoRecorder()

        private void StartVideoRecording()
        {
            try
            {

                // Connect fileSink to captureSource.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Connect the input and output of fileSink.
                    fileSink.CaptureSource = captureSource;
                    fileSink.IsolatedStorageFileName = fileName;
                }

                // Begin recording.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Stopped)
                {

                    captureSource.Start();
                }

                StopPlaybackRecording.IsEnabled = true;
                StartRecording.IsEnabled = false;
                // Set the button states and the message.
                UpdateUI(ButtonState.Recording, "Recording...");
            }
            catch (Exception e)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    txtDebug.Text = "ERROR: " + e.Message.ToString();
                });
            }
        } // StartVideoRecording()

        private void StopVideoRecording()
        {
            try
            {
                // Stop recording.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Disconnect fileSink.
                    fileSink.CaptureSource = null;
                    fileSink.IsolatedStorageFileName = null;
                    
                    // Set the button states and the message.
                    UpdateUI(ButtonState.Stopped, "Recording stopped...");

                    viewfinderRectangle.Fill = null;

                    // Create the file stream 
                    isoVideoFile = new IsolatedStorageFileStream(fileName,
                                            FileMode.Open, FileAccess.Read,
                                            IsolatedStorageFile.GetUserStoreForApplication());

                    MemoryStream videoStream = new MemoryStream();
                    using (isoVideoFile)
                    {
                        isoVideoFile.CopyTo(videoStream);
                    }

                    PurposeColor.screens.AddEventsSituationsOrThoughts.ReceiveVideoFromWindows(videoStream, isoVideoFile.Name);
                    
                    isoVideoFile.Flush();
                    isoVideoFile.Dispose();
                    isoVideoFile = null;
                    //videoStream = null;

                    DisposeVideoRecorder();

                }
            }
            // If stop fails, display an error.
            catch (Exception e)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    txtDebug.Text = "ERROR: " + e.Message.ToString();
                });
            }
        }//StopVideoRecording()

        private void StartRecording_Click(object sender, EventArgs e)
        {
            // Avoid duplicate taps.
            StartRecording.IsEnabled = false;
            InitializeVideoRecorder();
            StartVideoRecording();
        }

        private void StopPlaybackRecording_Click(object sender, EventArgs e)
        {
            // Avoid duplicate taps.
            StopPlaybackRecording.IsEnabled = false;

            // Stop during video recording.
            if (currentAppState == ButtonState.Recording)
            {
                StopVideoRecording();
            }

            DisposeVideoRecorder();

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

        }

        private void DisposeVideoRecorder()
        {
            try
            {
                if (captureSource != null)
                {
                    if (captureSource.VideoCaptureDevice != null
                        && captureSource.State == CaptureState.Started)
                    {
                        captureSource.Stop();
                    }

                    captureSource.CaptureFailed -= OnCaptureFailed;
                    captureSource = null;
                }
                videoCaptureDevice = null;
                fileSink = null;
                videoRecorderBrush = null;
                isoVideoFile = null;

                GC.Collect();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        private void UpdateUI(ButtonState currentButtonState, string statusMessage)
        {
            try
            {

                // Run code on the UI thread.
                Dispatcher.BeginInvoke(delegate
                {

                    switch (currentButtonState)
                    {
                        // When the camera is not supported by the device.
                        case ButtonState.CameraNotSupported:
                            StartRecording.IsEnabled = false;
                            StopPlaybackRecording.IsEnabled = false;
                            //StartPlayback.IsEnabled = false;
                            //PausePlayback.IsEnabled = false;
                            break;

                        // First launch of the application, so no video is available.
                        case ButtonState.Initialized:
                            StartRecording.IsEnabled = true;
                            StopPlaybackRecording.IsEnabled = false;
                            //StartPlayback.IsEnabled = false;
                            //PausePlayback.IsEnabled = false;
                            break;

                        // Ready to record, so video is available for viewing.
                        case ButtonState.Ready:
                            StartRecording.IsEnabled = true;
                            StopPlaybackRecording.IsEnabled = false;
                            //StartPlayback.IsEnabled = true;
                            //PausePlayback.IsEnabled = false;
                            break;

                        // Video recording is in progress.
                        case ButtonState.Recording:
                            StartRecording.IsEnabled = false;
                            StopPlaybackRecording.IsEnabled = true;
                            //StartPlayback.IsEnabled = false;
                            //PausePlayback.IsEnabled = false;
                            break;

                        // Video playback is in progress.
                        case ButtonState.Playback:
                            StartRecording.IsEnabled = false;
                            StopPlaybackRecording.IsEnabled = true;
                            //StartPlayback.IsEnabled = false;
                            //PausePlayback.IsEnabled = true;
                            break;

                        // Video playback has been paused.
                        case ButtonState.Paused:
                            StartRecording.IsEnabled = false;
                            StopPlaybackRecording.IsEnabled = true;
                            //StartPlayback.IsEnabled = true;
                            //PausePlayback.IsEnabled = false;
                            break;

                        default:
                            break;
                    }

                    // Display a message.
                    txtDebug.Text = statusMessage;

                    // Note the current application state.
                    currentAppState = currentButtonState;
                });


            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        private void OnCaptureFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                txtDebug.Text = "ERROR: " + e.ErrorException.Message.ToString();
            });
        }
    }
}