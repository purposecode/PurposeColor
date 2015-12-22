using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class VideoPlayerView : ContentPage
    {
        public VideoPlayerView( string path )
        {
            CrossVideoPlayer.FormsPlugin.Abstractions.CrossVideoPlayerView videoPlayer = new CrossVideoPlayer.FormsPlugin.Abstractions.CrossVideoPlayerView();
            videoPlayer.VideoSource = path;
            videoPlayer.HeightRequest = 400;
            videoPlayer.WidthRequest = 400;

            Content = new StackLayout
            {
                Children = {
					videoPlayer
				}
            };
        }
    }
}
