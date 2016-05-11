using System;
using System.Linq;
using System.Windows;
using FftGuitarTuner;
using System.Threading.Tasks;
using SoundAnalysis;
using SoundCapture;

namespace TabsCreator.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TunerWindow _tuner = new TunerWindow();
        
        private bool _isListenning;
        

        public bool IsListenning => _isListenning;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new SelectDeviceWindow();
            
                if (form.ShowDialog() == true)
                {
                Listener.Instance.device = form.SelectedDevice;
                }

            if (Listener.Instance.device != null)
            {
                Listener.Instance.StartListenning(frequencyInfoSource_FrequencyDetected);
                UpdateListenStopButtons();
            }
        }

        private void UpdateListenStopButtons()
        {
            SelectDeviceButton.IsEnabled = !_isListenning;
            StopButton.IsEnabled = _isListenning;
        }

        private void TunerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_tuner.IsVisible)
            {
                if (Listener.Instance.device == null)
                {
                    Listener.Instance.device = SoundCaptureDevice.GetDevices().FirstOrDefault();
                }
                _tuner.Show();
            }
            else
            {
                _tuner.Hide();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Listener.Instance.StopListenning(frequencyInfoSource_FrequencyDetected);
            UpdateListenStopButtons();
        }

        private void frequencyInfoSource_FrequencyDetected(object sender, FrequencyDetectedEventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new EventHandler<FrequencyDetectedEventArgs>(frequencyInfoSource_FrequencyDetected), sender, e);
            }
            else
            {
                UpdateFrequecyDisplays(e.Frequency);
            }
        }

        private void UpdateFrequecyDisplays(double frequency)
        {
            if (frequency > 0)
            {
                //frequenciesScale1.SignalDetected = true;
                //frequenciesScale1.CurrentFrequency = frequency;

                //frequencyTextBox.Enabled = true;
                //frequencyTextBox.Text = frequency.ToString("f3");

                //var noteName = NotesOperations.Instance().GetClosestNote(frequency);
                //noteNameTextBox.Enabled = true;
                //noteNameTextBox.Text = noteName;
            }
            else
            {
                //frequenciesScale1.SignalDetected = false;

                //frequencyTextBox.Enabled = false;
                //closeFrequencyTextBox.Enabled = false;
                //noteNameTextBox.Enabled = false;
            }
        }
    }
}
