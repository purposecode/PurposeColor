using System;
using Xamarin.Forms;
using CustomControls;
using PurposeColor.interfaces;
using PurposeColor.CustomControls;

namespace PurposeColor
{
    public class AudioRecorderPage : ContentPage
    {
        CustomLayout masterLayout;
        IAudioRecorder audioRecorder;
        public AudioRecorderPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.HorizontalOptions = LayoutOptions.Center;
            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Audio recorder", false);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(2, 4, 4));

            PurposeColor.CustomControls.PurposeColorTitleBar titleBar = new PurposeColor.CustomControls.PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");
            masterLayout.AddChildToLayout(titleBar, 0, 0);

            Button recordBtn = new Button
            {
                Text = "Record",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            recordBtn.Clicked += RecordBtn_Clicked;
            masterLayout.AddChildToLayout(recordBtn, 10, 20);

            Button stopRecordBtn = new Button
            {
                Text = "Stop Recording",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            stopRecordBtn.Clicked += StopRecordBtn_Clicked;
            masterLayout.AddChildToLayout(stopRecordBtn, 10, 30);


            Button PlaybackBtn = new Button
            {
                Text = "Playback",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            PlaybackBtn.Clicked += PlaybackBtn_Clicked;
            masterLayout.AddChildToLayout(PlaybackBtn, 10, 40);




            Content = masterLayout;
        }

        void StopRecordBtn_Clicked(object sender, EventArgs e)
        {
            audioRecorder.StopRecording();
        }

        void RecordBtn_Clicked(object sender, EventArgs e)
        {
            if (audioRecorder == null)
            {
                audioRecorder = DependencyService.Get<IAudioRecorder>();
            }
            bool isAudioRecording = audioRecorder.RecordAudio();
            if (isAudioRecording == false)
            {
                DisplayAlert("Audio recording", "Sorry the audio recording service is not available now, please try again later", "OK");
            }
            else
            {
                DisplayAlert("Audio recording", "Audio recording stared", "OK");
            }
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            //Navigation.PushAsync( new GraphPage() );
        }
        void PlaybackBtn_Clicked(object sender, System.EventArgs e)
        {
            audioRecorder.PlayAudio();
        }
    }
}

