using System;
using System.Windows.Forms;
using SoundCapture;

namespace FftGuitarTuner
{
    public partial class MainForm : Form
    {
        bool _isListenning;

        public bool IsListenning
        {
            get { return _isListenning; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        FrequencyInfoSource _frequencyInfoSource;

        private void StopListenning()
        {
            _isListenning = false;
            _frequencyInfoSource.Stop();
            _frequencyInfoSource.FrequencyDetected -= frequencyInfoSource_FrequencyDetected;
            _frequencyInfoSource = null;
        }

        private void StartListenning(SoundCaptureDevice device)
        {
            _isListenning = true;
            _frequencyInfoSource = new SoundFrequencyInfoSource(device);
            _frequencyInfoSource.FrequencyDetected += frequencyInfoSource_FrequencyDetected;
            _frequencyInfoSource.Listen();
        }

        void frequencyInfoSource_FrequencyDetected(object sender, FrequencyDetectedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<FrequencyDetectedEventArgs>(frequencyInfoSource_FrequencyDetected), sender, e);
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
                frequenciesScale1.SignalDetected = true;
                frequenciesScale1.CurrentFrequency = frequency;

                frequencyTextBox.Enabled = true;
                frequencyTextBox.Text = frequency.ToString("f3");

                double closestFrequency;
                string noteName;
                FindClosestNote(frequency, out closestFrequency, out noteName);
                closeFrequencyTextBox.Enabled = true;
                closeFrequencyTextBox.Text = closestFrequency.ToString("f3");
                noteNameTextBox.Enabled = true;
                noteNameTextBox.Text = noteName;
                //string path = @"C:\WriteLines1.txt";
                //FileInfo fi1 = new FileInfo(path);
                //if (!fi1.Exists)
                //{
                //    //Create a file to write to.
                //    fi1.CreateText();
                //}
                //using (System.IO.StreamWriter file =
                //    new System.IO.StreamWriter(path, true))
                //{
                //    file.WriteLine(noteName + "Time:"+ DateTime.UtcNow + "\n");
                //}
            }
            else
            {
                frequenciesScale1.SignalDetected = false;

                frequencyTextBox.Enabled = false;
                closeFrequencyTextBox.Enabled = false;
                noteNameTextBox.Enabled = false;
            }

        }

        private void FindClosestNote(double frequency, out double closestFrequency, out string noteName)
        {
            noteName = Notes.GetClosestNote(frequency);
            closestFrequency = Notes.GetFrequency(noteName);
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            SoundCaptureDevice device = null;
            using (SelectDeviceForm form = new SelectDeviceForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    device = form.SelectedDevice;
                }
            }

            if (device != null)
            {
                StartListenning(device);
                UpdateListenStopButtons();
            }
        }

        private void UpdateListenStopButtons()
        {
            listenButton.Enabled = !_isListenning;
            stopButton.Enabled = _isListenning;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopListenning();
            UpdateListenStopButtons();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsListenning)
            {
                StopListenning();
            }
        }
    }
}
