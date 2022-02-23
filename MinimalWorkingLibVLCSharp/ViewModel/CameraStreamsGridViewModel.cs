using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using Stylet;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="events"></param>
        public CameraStreamsGridViewModel(IEventAggregator events)
        {
            CameraStreams = new BindableCollection<StreamGridModel>();
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
            var _sgm = new StreamGridModel();
            _sgm.CameraStreamGridViewModel = streams;
            _sgm.CameraStreamGridViewModel.Cam = camera;
            CameraStreams.Add(_sgm);
        }

        /// <summary>
        /// Should dispose all open streams and players
        /// </summary>
        public void WindowClosed()
        {
            foreach (var stream in CameraStreams)
            {
                stream.CameraStreamGridViewModel.Dispose();
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
