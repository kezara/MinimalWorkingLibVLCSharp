using MinimalWorkingLibVLCSharp.Events;
using MinimalWorkingLibVLCSharp.Model;
using MinimalWorkingLibVLCSharp.ViewModel;
using Stylet;
using System.Linq;
using System.Windows;

namespace MinimalWorkingLibVLCSharp
{
    public class WindowViewManager : IHandle<WindowClosed>
    {
        readonly IWindowManager _windowManager;
        readonly StyletIoC.IContainer _container;
        BiDictionary<Camera, Screen> _openedWindows;
        //this is only for ensuring stream grid window visible
        readonly Camera _cameraStreamGridMarker;
        public IWindowManager WindowManager { get => _windowManager; }
        public StyletIoC.IContainer Container { get => _container; }

        public WindowViewManager(IWindowManager windowManager, IEventAggregator events, StyletIoC.IContainer container)
        {
            _windowManager = windowManager;
            _container = container;
            _openedWindows = new BiDictionary<Camera, Screen>();
            _cameraStreamGridMarker = new Camera { Id = "default"};
            events.Subscribe(this);
        }

        /// <summary>
        /// only for ensuring stream grid visible
        /// </summary>
        /// <param name="vm">Stream grid view model</param>
        public void EnsureVisible(Screen vm)
        {
            if (_openedWindows.ContainsValue(vm))
            {
                var window = (Window)_openedWindows[_cameraStreamGridMarker].View;

                BringWindowToFront(window);
            }
            else
            {
                _openedWindows.Add(_cameraStreamGridMarker, vm);
                _windowManager.ShowWindow(vm);
            }
        }

        /// <summary>
        /// ensures that only one video of selected camera is displayed
        /// in grid view or single window view
        /// </summary>
        /// <param name="camera">CameraMarker</param>
        public void EnsureVisible(Camera camera)
        {
            Screen vm;
            if (_openedWindows.ContainsKey(camera))
            {
                vm = _openedWindows[camera];
            }
            else
            {
                vm = _openedWindows[_cameraStreamGridMarker];
            }
            var window = (Window)vm.View;
            BringWindowToFront(window);
            //_windowManager.ShowWindow(vm);
        }

        public void EnsureVisible(Screen viewModel, Camera camera)
        {
            if (_openedWindows.ContainsKey(camera))
            {
                var window = (Window)_openedWindows[camera].View;

                BringWindowToFront(window);
            }
            else
            {
                _openedWindows.Add(camera, viewModel);
                _windowManager.ShowWindow(viewModel);
            }
        }

        private static void BringWindowToFront(Window window)
        {
            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.Activate();
            }
        }

        public bool IsCameraOpened(Camera camera)
        {
            if (_openedWindows.ContainsKey(camera))
            {
                return true;
            }
            else if (_openedWindows.ContainsKey(_cameraStreamGridMarker))
            {
                
                var grid = (CameraStreamsGridViewModel)_openedWindows[_cameraStreamGridMarker];
                var cam = grid.CameraStreams.Where(c => c.CameraStreamGridViewModel.Cam.Id == camera.Id).FirstOrDefault();
                if (cam != null)
                {
                    return true;
                }
            }
            return false;
        }

        

        public bool? ShowDialog(object viewModel)
        {
            return _windowManager.ShowDialog(viewModel);
        }

        public void Handle(WindowClosed message)
        {
            var key = _openedWindows[message.ClosedWindow];
            var t = _openedWindows.Remove(key);
        }
    }
}
