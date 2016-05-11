using System;
using System.Windows;
using System.Windows.Input;
using SoundCapture;

namespace TabsCreator.Windows
{
    /// <summary>
    /// Interaction logic for SelectDeviceWindow.xaml
    /// </summary>
    public partial class SelectDeviceWindow : Window
    {
        SoundCaptureDevice[] _devices;

        public SoundCaptureDevice SelectedDevice => _devices[listBox.SelectedIndex];

        public SelectDeviceWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDevices();
        }

        private void LoadDevices()
        {
            listBox.Items.Clear();

            int defaultDeviceIndex = 0;

            _devices = SoundCaptureDevice.GetDevices();
            for (int i = 0; i < _devices.Length; i++)
            {
                listBox.Items.Add(_devices[i].Name);
                if (_devices[i].IsDefault)
                    defaultDeviceIndex = i;
            }

            listBox.SelectedIndex = defaultDeviceIndex;
        }

        private void ListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            DialogResult = true;
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
