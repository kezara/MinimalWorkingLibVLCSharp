using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
