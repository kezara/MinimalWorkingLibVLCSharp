using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using MinimalWorkingLibVLCSharp.View;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public abstract class CameraStreamViewModelAbstract : Screen, IDisposable
    {
        protected IEventAggregator _events;
        protected WindowViewManager _windowViewManager;
        private string _stramPath;
        WindowState _windowState;
        WindowStyle _windowStyle;
        private Camera _camera;
        private bool _windowTopMost;

        public bool WindowTopMost
        {
            get { return _windowTopMost; }
            set
            {
                SetAndNotify(ref _windowTopMost, value);
            }
        }


        public Camera Camera
        {
            get { return _camera; }
            set
            {
                SetAndNotify(ref _camera, value);
                //Player.GMarker = _camera;
            }
        }


        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                SetAndNotify(ref _windowState, value);
            }
        }
        public ResizeMode ResizeMode
        {
            get => _resizeMode;
            set
            {
                SetAndNotify(ref _resizeMode, value);
            }
        }
        public WindowStyle WindowStyle
        {
            get => _windowStyle;
            set
            {
                SetAndNotify(ref _windowStyle, value);
            }
        }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }

        public string StreamPath
        {
            get { return _stramPath; }
            set { _stramPath = value; }
        }

        private VLCPlayer _vlcPlayer;

        public VLCPlayer VlcPlayer
        {
            get { return _vlcPlayer; }
            set
            {
                SetAndNotify(ref _vlcPlayer, value);
            }
        }


        public double SecondaryScreenStart { get; set; }

        public CameraStreamViewModelAbstract(IEventAggregator events, WindowViewManager windowViewManager)
        {
            //_events = events;
            //_events.Subscribe(this);
            //_cameraViewOverlayViewModel = cameraViewOverlayViewModel;
            _windowViewManager = windowViewManager;
            //can be removed use instead WindowViewManager
            //_cameraStreamRepository = cameraStreamRepository;


            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            //SecondaryScreenStart = Helpers.CalculateSecondaryDisplay();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            VlcPlayer = _windowViewManager.Container.Get<VLCPlayer>();//new VLCPlayer();
            VlcPlayer.ClosePlayer += ClosePlayer;
            
            VlcPlayer.SendToGrid += Player_SendToGrid;
            DisplayName = Camera.Name;
            //VlcPlayer.DisplayName = Camera.Name;
            VlcPlayer.Camera = Camera;
        }

        protected void Player_SendToGrid(object sender, PlayerEventArgs e)
        {
            MoveToGrid();
        }

        protected virtual void ClosePlayer(object sender, PlayerEventArgs e)
        {
            //_events.Publish(new WindowClosed { ClosedWindow = this });
            RequestClose();
        }

        public virtual void OnWindowStateChanged(object sender, EventArgs e)
        {
            var csw = sender as CameraStreamView;
            if (csw.IsLoaded)
            {
                ResizeMode = ResizeMode.NoResize;
                if (WindowState == WindowState.Maximized)
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    WindowTopMost = true;
                    WindowTopMost = false;
                    VlcPlayer.ShowControls();
                    HideCameraOverlayButtons(false);
                }
            }
        }


        bool _buttonVisibility;
        int _buttonOpacity;
        private ResizeMode _resizeMode;
        protected bool disposedValue;

        public int Height { get; set; }
        public int Width { get; set; }
        public Brush ButtonBackground { get; set; }
        public bool ButtonVisibility
        {
            get => _buttonVisibility;
            set
            {
                SetAndNotify(ref _buttonVisibility, value);
            }
        }

        public int ButtonOpacity
        {
            get => _buttonOpacity;
            set
            {
                SetAndNotify(ref _buttonOpacity, value);
            }
        }

        public void ChangeButtonBackground()
        {
            ButtonBackground = Brushes.Red;
            ButtonOpacity = 100;
        }

        public void Restore()
        {
            _events.Publish(new CameraStreamRestoreWindow { CanRestore = true });
        }

        public void MoveToGrid()
        {
            var window = _windowViewManager.Container.Get<CameraStreamsGridViewModel>();
            _windowViewManager.EnsureVisible(window);
            var csvm = _windowViewManager.Container.Get<CameraStreamGridViewModel>();

            window.AddToCollection(csvm, VlcPlayer.Camera);
            //csvm.Camera = cam;
            //VlcPlayer = null;
            ClosePlayer(this, new PlayerEventArgs(true));
        }

        public void HideCameraOverlayButtons(bool canHide)
        {
            if (canHide)
            {
                ButtonVisibility = false;
            }
            else
            {
                ButtonVisibility = true;
            }
        }

        public void WindowClosed()
        {
            Dispose();
        }

        

        public void Dispose()
        {
           
            //_events.Unsubscribe(this);
            VlcPlayer.ClosePlayer -= ClosePlayer;
            //VlcPlayer.RestorePlayer -= RestorePlayer;
            //VlcPlayer.MinimizePlayer -= MinimizePlayer;
            //VlcPlayer.Camera = null;
            //VlcPlayer.DataContext = null;
            VlcPlayer.vvPlayer.MediaPlayer.Stop();
            VlcPlayer.vvPlayer.MediaPlayer.Media.Dispose();

            VlcPlayer.vvPlayer.MediaPlayer.Dispose();
            VlcPlayer.vvPlayer.Dispose();
        }

        ~CameraStreamViewModelAbstract()
        {

        }
    }
}
