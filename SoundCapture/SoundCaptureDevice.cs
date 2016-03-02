using System;
using System.Collections.Generic;
using Microsoft.DirectX.DirectSound;

namespace SoundCapture
{
    /// <summary>
    /// Capture device.
    /// </summary>
    public class SoundCaptureDevice
    {
        readonly Guid _id;

        readonly string _name;

        public bool IsDefault
        {
            get { return _id == Guid.Empty; }
        }

        /// <summary>
        /// Name of the device.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        internal Guid Id
        {
            get { return _id; }
        }

        internal SoundCaptureDevice(Guid id, string name)
        {
            _id = id;
            _name = name;
        }

        public static SoundCaptureDevice[] GetDevices()
        {
            CaptureDevicesCollection captureDevices = new CaptureDevicesCollection();
            List<SoundCaptureDevice> devices = new List<SoundCaptureDevice>();
            foreach (DeviceInformation captureDevice in captureDevices)
            {
                devices.Add(new SoundCaptureDevice(captureDevice.DriverGuid, captureDevice.Description));
            }
            return devices.ToArray();
        }

        public static SoundCaptureDevice GetDefaultDevice()
        {
            CaptureDevicesCollection captureDevices = new CaptureDevicesCollection();
            SoundCaptureDevice device = null;
            foreach (DeviceInformation captureDevice in captureDevices)
            {
                if(captureDevice.DriverGuid == Guid.Empty)
                {
                    device = new SoundCaptureDevice(captureDevice.DriverGuid, captureDevice.Description);
                    break;
                }
            }
            if (device == null)
                throw new SoundCaptureException("Default capture device is not found");
            return device;
        }
    }
}
