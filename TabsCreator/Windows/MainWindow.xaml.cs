﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FftGuitarTuner;
using System.Threading.Tasks;
using Hellpers;
using SoundAnalysis;
using SoundCapture;

namespace TabsCreator.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly TunerWindow _tuner = new TunerWindow();

        private readonly ListeningChangedEventHandler _listeningChangedEventHandler = new ListeningChangedEventHandler();
        public bool IsListenning { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Listener.Instance.Device = SoundCaptureDevice.GetDevices().FirstOrDefault();
            _listeningChangedEventHandler.ListneningStatusChangedEventHandler += ListneningStatusChaged;
            UpdateListenButtons();
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
        }

        private void SelectDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new SelectDeviceWindow();
            
                if (form.ShowDialog() == true)
                {
                Listener.Instance.Device = form.SelectedDevice;
                }

            if (Listener.Instance.Device != null)
            {
                Listener.Instance.StartListenning(frequencyInfoSource_FrequencyDetected, GetType());
                _listeningChangedEventHandler.OnListneningStatusChanged(new ListeningChangedEventArgs(true));
            }
        }

        private void UpdateListenButtons()
        {
            SelectDeviceButton.IsEnabled = !IsListenning;
            StopButton.IsEnabled = IsListenning;
            StartButton.IsEnabled = !IsListenning;
        }

        private void TunerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_tuner.IsVisible)
            {
                //if (Listener.Instance.Device == null)
                //{
                //    Listener.Instance.Device = SoundCaptureDevice.GetDevices().FirstOrDefault();
                //}
                _tuner.Show();
            }
            else
            {
                _tuner.Hide();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Listener.Instance.StopListenning(frequencyInfoSource_FrequencyDetected, GetType());
            _listeningChangedEventHandler.OnListneningStatusChanged(new ListeningChangedEventArgs(false));
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

        private void ListneningStatusChaged(object sender, ListeningChangedEventArgs e)
        {
            //if (Dispatcher.CheckAccess())
            //{
            //    Dispatcher.BeginInvoke(new EventHandler<ListeningChangedEventArgs>(ListneningStatusChaged), sender, e);
            //}
            //else
            //{
                IsListenning = e.IsListnening;
                UpdateListenButtons();
            //}
        }

        private void UpdateFrequecyDisplays(double frequency)
        {
            Dispatcher.Invoke(() => {NoteName.Text = frequency.ToString(CultureInfo.InvariantCulture); });
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Listener.Instance.StartListenning(frequencyInfoSource_FrequencyDetected, GetType());
            _listeningChangedEventHandler.OnListneningStatusChanged(new ListeningChangedEventArgs(true));
            //UpdateListenButtons();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
