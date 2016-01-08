using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO;

namespace PurposeColor.WinPhone
{
    public partial class sample : PhoneApplicationPage
    {
        public sample()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
           // videoplayer.RenderTransform = new CompositeTransform() { Rotation = 90, CenterX = 0, CenterY = 0 };
            videoplayer.Source = new Uri(PurposeColor.App.WindowsDownloadedMedia);
            videoplayer.Stretch = Stretch.Fill;
            videoplayer.MediaOpened += videoplayer_MediaOpened;
            videoplayer.Play();
        }

        void videoplayer_MediaOpened(object sender, RoutedEventArgs e)
        {
 
        }

        private void StopMedia(object sender, RoutedEventArgs e)
        {
            videoplayer.Stop();
        }
        private void PauseMedia(object sender, RoutedEventArgs e)
        {
            videoplayer.Pause();
        }
        private void PlayMedia(object sender, RoutedEventArgs e)
        {
            videoplayer.Play();
        } 
    }
}