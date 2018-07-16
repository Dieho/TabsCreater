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
        public bool IsDefault => Id == Guid.Empty;

        /// <summary>
        /// Name of the device.
        /// </summary>
        public string Name { get; }

        internal Guid Id { get; }

        internal SoundCaptureDevice(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static SoundCaptureDevice[] GetDevices()
        {
            var captureDevices = new CaptureDevicesCollection();
            var devices = new List<SoundCaptureDevice>();
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
