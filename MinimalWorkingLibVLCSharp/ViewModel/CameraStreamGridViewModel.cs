using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using MinimalWorkingLibVLCSharp.View;
using Stylet;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public class CameraStreamGridViewModel : CameraStreamViewModelAbstract
    {
        public Camera Cam { get; set; }
        public CameraStreamGridViewModel(IEventAggregator events, WindowViewManager windowViewManager) : base(events, windowViewManager)
        {
        }

        protected override void OnViewLoaded()
        {
            VlcPlayer = _windowViewManager.Container.Get<VLCPlayer>();
            VlcPlayer.ClosePlayer += ClosePlayer;
            VlcPlayer.RestorePlayer += RestorePlayer;
            VlcPlayer.MoveToGrid_btn.Visibility = System.Windows.Visibility.Hidden;
            VlcPlayer.Minimize_btn.Visibility = System.Windows.Visibility.Hidden;
            DisplayName = Cam.Name;
            VlcPlayer.Camera = Cam;
        }

        protected override void ClosePlayer(object sender, PlayerEventArgs e)
        {
            _events.Publish(new RemoveFromGrid { ToRemove = this });
            Cam = null;
        }

        protected void RestorePlayer(object sender, PlayerEventArgs e)
        {
            //var repo = _windowViewManager.Container.Get<MapLocationRepository>();
            //var cameraMarker = repo.GetCameraMarker(VlcPlayer.Camera);
            ClosePlayer(sender, e);
            //if (cameraMarker != null)
            //{
                var window = _windowViewManager.Container.Get<CameraStreamViewModel>();
            _windowViewManager.WindowManager.ShowWindow(window);
                //_windowViewManager.EnsureVisible(window, cameraMarker);
            //}
        }
    }
}
