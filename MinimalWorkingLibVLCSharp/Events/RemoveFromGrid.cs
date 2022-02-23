using MinimalWorkingLibVLCSharp.ViewModel;

namespace MinimalWorkingLibVLCSharp.Events
{
    public class RemoveFromGrid
    {
        public CameraStreamGridViewModel ToRemove { get; set; }
    }
}
