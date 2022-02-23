namespace MinimalWorkingLibVLCSharp.Events
{
    public class PlayerEventArgs
    {
        public bool CloseRequested { get; set; }

        public PlayerEventArgs(bool closeRequested)
        {
            CloseRequested = closeRequested;
        }
    }
}
