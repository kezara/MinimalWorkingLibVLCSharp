using MinimalWorkingLibVLCSharp.Model;
using Stylet;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public class ShellViewModel : Conductor<IScreen>
    {
        Camera _camera;
        WindowViewManager _windowViewManager;

        public ShellViewModel(WindowViewManager windowViewManager)
        {
            this.DisplayName = "Shell View";
            _windowViewManager = windowViewManager;
            _camera = new Camera
            {
                CameraAccess = new CameraAccess
                {
                    IpAddress = "camera ip address",
                    Port = 80,
                    Password = "password",
                    Username = "username"
                },
                Name = "Camera 1",
                ProfileLive = "rtsp:",
                ProfileProcessing = "rtsp:"
            };
        }

        public void OpenStream()
        {
            var window = _windowViewManager.Container.Get<CameraStreamViewModel>();
            window.Camera = _camera;
            _windowViewManager.EnsureVisible(window, _camera);
        }
    }
}
