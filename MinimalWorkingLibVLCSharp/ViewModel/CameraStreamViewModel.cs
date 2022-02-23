using Stylet;

namespace MinimalWorkingLibVLCSharp.ViewModel
{
    public class CameraStreamViewModel : CameraStreamViewModelAbstract
    {
        public CameraStreamViewModel(IEventAggregator events, WindowViewManager windowViewManager) : base(events, windowViewManager)
        {
        }
    }
}
