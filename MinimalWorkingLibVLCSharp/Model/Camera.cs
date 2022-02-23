using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalWorkingLibVLCSharp.Model
{
    public class Camera
    {
        public string Id { get; set; }
        public int VideoDeviceInterfaceTypeID { get; set; }
        public string Name { get; set; }
        public string DrawingId { get; set; }
        public bool UseGps { get; set; }
        public string ProfileLive { get; set; }
        public string ProfileProcessing { get; set; }
        public bool HasGpsIntegrated { get; set; }
        public string SerialNumber { get; set; }
        public CameraAccess CameraAccess { get; set; }
    }

    public class CameraAccess
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
