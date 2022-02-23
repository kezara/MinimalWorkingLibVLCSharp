using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using MinimalWorkingLibVLCSharp.ViewModel;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public class CameraStreamsGridViewModel : Screen, IHandle<RemoveFromGrid>
    {
        private IEventAggregator _events;
        public BindableCollection<StreamGridModel> CameraStreams { get; set; }
        private double _secondaryScreenStart;
        public double SecondaryScreenStart
        {
            get => _secondaryScreenStart;
            set
            {
                SetAndNotify(ref _secondaryScreenStart, value);
            }
        }

        private WindowViewManager _windowViewManager;
        StreamGridModel _sgm;
        Camera _camera;


        public CameraStreamsGridViewModel(WindowViewManager windowViewManager, IEventAggregator events)
        {
            CameraStreams = new BindableCollection<StreamGridModel>();
            _windowViewManager = windowViewManager;
            _events = events;
            _events.Subscribe(this);
            DisplayName = "Video wall";
        }

        /// <summary>
        /// Add camera stream to collection for display
        /// </summary>
        /// <param name="streams">ViewModel</param>
        /// <param name="camera">camera</param>
        public void AddToCollection(CameraStreamGridViewModel streams, Camera camera)
        {
            _sgm = new StreamGridModel();
            _sgm.CameraStreamGridViewModel = streams;
            _sgm.CameraStreamGridViewModel.Cam = camera;
            CameraStreams.Add(_sgm);
        }

        public void WindowClosed()
        {
            foreach (var stream in CameraStreams)
            {
                stream.CameraStreamGridViewModel.VlcPlayer.vvPlayer.Dispose();
            }
            CameraStreams = new BindableCollection<StreamGridModel>();
            _events.Publish(new WindowClosed { ClosedWindow = this });
        }

        /// <summary>
        /// removes stream from video wall grid
        /// </summary>
        /// <param name="message">contains viewModel for removing from grid</param>
        public void Handle(RemoveFromGrid message)
        {
            var rem = CameraStreams.Where(cs => cs.CameraStreamGridViewModel == message.ToRemove).FirstOrDefault();
            if (rem != null)
            {
                CameraStreams.Remove(rem);
                rem.CameraStreamGridViewModel.Dispose();
            }
            if (CameraStreams.Count == 0)
            {
                RequestClose();
            }
        }
    }


    /// <summary>
    /// Helper class for bindable collection and video wall display
    /// </summary>
    public class StreamGridModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private CameraStreamGridViewModel _cameraStreamGridViewModel;

        public CameraStreamGridViewModel CameraStreamGridViewModel
        {
            get { return _cameraStreamGridViewModel; }
            set
            {
                if (value != _cameraStreamGridViewModel)
                {
                    _cameraStreamGridViewModel = value;
                }
                OnPropertyChanged();
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
