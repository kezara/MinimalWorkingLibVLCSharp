using LibVLCSharp.Shared;
using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MinimalWorkingLibVLCSharp.View
{
    /// <summary>
    /// Interaction logic for VLCPlayer.xaml
    /// </summary>
    public partial class VLCPlayer : UserControl//, IDisposable
    {
        public event EventHandler<PlayerEventArgs> ClosePlayer;
        public event EventHandler<PlayerEventArgs> RestorePlayer;
        public event EventHandler<PlayerEventArgs> MinimizePlayer;
        public event EventHandler<PlayerEventArgs> SendToGrid;

        private static WindowViewManager _windowViewManager;


        public static readonly DependencyProperty CameraProperty = DependencyProperty
            .Register
            (
                nameof(Camera),
                typeof(Camera),
                typeof(VLCPlayer),
                new PropertyMetadata(new PropertyChangedCallback(OnCameraPropertyChange))
            );


        private static void OnCameraPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VLCPlayer vLCPlayer = d as VLCPlayer;
            Core.Initialize();

            if (vLCPlayer.Camera != null)
            {
                var libVLC = _windowViewManager.Container.Get<LibVLCSingleton>();


                var mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(libVLC);

                vLCPlayer.vvPlayer.MediaPlayer = mediaPlayer;

                var rtspUri = new UriBuilder(vLCPlayer.Camera.ProfileLive);

                var options = new List<string>();
                options.Add(":rtsp-http");
                options.Add(":rtsp-http-port=" + vLCPlayer.Camera.CameraAccess.Port);
                options.Add(":rtsp-user=" + vLCPlayer.Camera.CameraAccess.Username);
                options.Add(":rtsp-pwd=" + vLCPlayer.Camera.CameraAccess.Password);

                vLCPlayer.vvPlayer.MediaPlayer.Play(new Media(libVLC, rtspUri.Uri, options.ToArray()));
            }
        }

        public Camera Camera
        {
            get => (Camera)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        private DoubleAnimation _animation;
        private Storyboard sb;

        public VLCPlayer(WindowViewManager windowViewManager)
        {
            InitializeComponent();
            _windowViewManager = windowViewManager;
        }

        private DoubleAnimation FadeOutAnimation()
        {
            var animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
            return animation;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePlayer?.Invoke(this, new PlayerEventArgs(true));
            //Dispose();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            RestorePlayer?.Invoke(this, new PlayerEventArgs(true));
        }

        public void HideControls()
        {
            buttonsPanel.Visibility = Visibility.Collapsed;
        }

        public void ShowControls()
        {
            buttonsPanel.Visibility = Visibility.Visible;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            MinimizePlayer?.Invoke(this, new PlayerEventArgs(true));
        }

        private void MoveToGrid_Click(object sender, RoutedEventArgs e)
        {
            SendToGrid?.Invoke(this, new PlayerEventArgs(true));
        }

        //public void Dispose()
        //{
        //    vvPlayer.MediaPlayer.Stop();
        //    vvPlayer.MediaPlayer.Dispose();
        //    DataContext = null;
        //    vvPlayer.Dispose();
        //    GC.Collect();
        //}

        ~VLCPlayer()
        {

        }
    }
}
