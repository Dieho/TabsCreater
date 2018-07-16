using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FftGuitarTuner;
using Hellpers;
using SoundAnalysis;
using SoundCapture;

namespace TabsCreator.Windows
{
    /// <summary>
    /// Interaction logic for TunerWindow.xaml
    /// </summary>
    public partial class TunerWindow : Window
    {
        public TunerWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
            var closestNote = NotesOperations.Instance().GetClosestNote(frequency);
            var bc = new BrushConverter();
            var brush2 = (Brush)bc.ConvertFrom("White");
            Dispatcher.Invoke(() =>
            {
                Dispatcher.Invoke(() => { FrequencyScale.Lowest.Fill = brush2; });
                Dispatcher.Invoke(() => { FrequencyScale.Lower.Fill = brush2; });
                Dispatcher.Invoke(() => { FrequencyScale.Main.Fill = brush2; });
                Dispatcher.Invoke(() => { FrequencyScale.Higher.Fill = brush2; });
                Dispatcher.Invoke(() => { FrequencyScale.Highest.Fill = brush2; });
                Dispatcher.Invoke(() => { FrequencyScale.NoteName.Text = closestNote.Note; });
            });
            
            var loverNote = NotesOperations.Instance().GetNoteById(closestNote.Id - 1);
            var higherNote = NotesOperations.Instance().GetNoteById(closestNote.Id + 1);

            double? loverDiff = null;
            double? higherDiff = null;
            if (loverNote != null)
            {
                loverDiff = (closestNote.Frequency - loverNote.Frequency)/2;
            }
            if (higherNote != null)
            {
                higherDiff = (closestNote.Frequency - higherNote.Frequency)/2;
            }

            
            var brush = (Brush)bc.ConvertFrom("Green");
            
            var noteDiff = closestNote.Frequency - frequency;
            if (noteDiff < loverDiff/3 && noteDiff > higherDiff/3)
            {
                Dispatcher.Invoke(() => { FrequencyScale.Main.Fill = brush; });
                return;
            }
            if (noteDiff > loverDiff/1.5)
            {
                Dispatcher.Invoke(() => { FrequencyScale.Lowest.Fill = brush; });
                return;
            }
            if (noteDiff < loverDiff / 1.5 && noteDiff > loverDiff / 3)
            {
                Dispatcher.Invoke(() => { FrequencyScale.Lower.Fill = brush; });
                return;
            }
            if (noteDiff < higherDiff/1.5)
            {
                Dispatcher.Invoke(() => { FrequencyScale.Higher.Fill = brush; });
                return;
            }
            if (noteDiff > higherDiff/1.5 && noteDiff < higherDiff / 3)
            {
                Dispatcher.Invoke(() => { FrequencyScale.Highest.Fill = brush; });
                return;
            }
        }

        private void TunerWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Listener.Instance.StartListenning(frequencyInfoSource_FrequencyDetected, GetType());
            }
            else
            {
                Listener.Instance.StopListenning(frequencyInfoSource_FrequencyDetected, GetType());
            }
        }
    }
}
