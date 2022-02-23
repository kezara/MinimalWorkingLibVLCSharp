using MinimalWorkingLibVLCSharp.Model;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public class ShellViewModel : Conductor<IScreen>
    {
        readonly IContainer _container;
        private IWindowManager _windowManager;
        bool? _isLoggedIn = false;
        Camera _camera;

        public ShellViewModel(IContainer container, IWindowManager windowManager, IEventAggregator events)
        {
            this.DisplayName = "Shell View";
            _container = container;
            _windowManager = windowManager;
            _camera = new Camera
            {
                CameraAccess = new CameraAccess
                {
                    IpAddress = "192.168.25.32",
                    Port = 80,
                    Password = "admin12345",
                    Username = "admin"
                },
                Name = "Camera 1",
                ProfileLive = "rtsp://192.168.25.33:554/Streaming/Channels/1?transportmode=unicast&profile=Profile_1",
                ProfileProcessing = "rtsp://192.168.25.33:554/Streaming/Channels/2?transportmode=unicast&profile=Profile_2"
            };
           // events.Subscribe(this);
        }

        public void OpenStream()
        {
            var window = _container.Get<CameraStreamViewModel>();
            window.Camera = _camera;
            _windowManager.ShowWindow(window);
        }
    }
}
