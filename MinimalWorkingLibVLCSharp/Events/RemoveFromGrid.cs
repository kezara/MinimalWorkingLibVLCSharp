﻿using MinimalWorkingLibVLCSharp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalWorkingLibVLCSharp.Events
{
    public class RemoveFromGrid
    {
        public CameraStreamGridViewModel ToRemove { get; set; }
    }
}