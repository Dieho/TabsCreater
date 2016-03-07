using System;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using SoundCapture;

namespace FftGuitarTuner
{
    public partial class SelectDeviceForm : Form
    {
        SoundCaptureDevice[] _devices;

        public SoundCaptureDevice SelectedDevice
        {
            get { return _devices[deviceNamesListBox.SelectedIndex]; }
        }

        public SelectDeviceForm()
        {
            InitializeComponent();
        }

        private void SelectDeviceForm_Load(object sender, EventArgs e)
        {
            LoadDevices();
        }

        //private DevicesCollection _myDevices = null;

        private void LoadDevices()
        {
            deviceNamesListBox.Items.Clear();

            int defaultDeviceIndex = 0;
            
            _devices = SoundCaptureDevice.GetDevices();
            for (int i = 0; i < _devices.Length; i++)
            {
                deviceNamesListBox.Items.Add(_devices[i].Name);
                if (_devices[i].IsDefault)
                    defaultDeviceIndex = i;
            }

            deviceNamesListBox.SelectedIndex = defaultDeviceIndex;
        }

        private void deviceNamesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (deviceNamesListBox.SelectedIndex < 0) return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
