using System;
using System.Linq;
using FftGuitarTuner.Data;

namespace FftGuitarTuner
{
    public class Notes
    {
        private static Notes notes;
        private static IRepository _noteRepo = new RepositoryService();
        private static readonly string[] NoteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
       
        protected Notes()
        {
        }

        public static Notes Instance()
        {
            if(notes == null)
                notes = new Notes();

            return notes;
        }

        public double GetFrequency(string note)
        {
            return _noteService.GetAll().FirstOrDefault(n => n.Note == note).Frequency;
        }

        public string GetClosestNote(double freq)
        {
            var listOfNotes = _noteService.GetAll().Select(n => n.Frequency).ToList();
            var closest = listOfNotes.OrderBy(frequency => Math.Abs(freq - frequency)).First();
            return _noteService.GetAll().FirstOrDefault(n => n.Frequency == closest).Note;
        }

        public string GetClosestNote2(double freq)
        {
            double toneStep = Math.Pow(2, 1.0 / 12);
            const double aFrequency = 440.0;
            const int toneIndexOffsetToPositives = 120;
            int toneIndex = (int)Math.Round(Math.Log(freq / aFrequency, toneStep));
            var noteName = NoteNames[(toneIndexOffsetToPositives + toneIndex) % NoteNames.Length];
            if (freq < 31.78)
            {
                return noteName + "0";
            }
            if (freq < 63.57)
            {
                return noteName + "1";  
            }
            if (freq < 127.15)
            {
                return noteName + "2";
            }
            if (freq < 254.29)
            {
                return noteName + "3";
            }
            if (freq < 508.56)
            {
                return noteName + "4";
            }
            if (freq < 1017.12)
            {
                return noteName + "5";
            }
            if (freq < 2034.25)
            {
                return noteName + "6";
            }
            if (freq < 4068.5)
            {
                return noteName + "7";
            }
            return noteName + "8";
        }
    }
}
