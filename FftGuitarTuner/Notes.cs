using System;
using System.Linq;
using System.Threading.Tasks;
using FftGuitarTuner.Data;
using System.Collections.Generic;

namespace FftGuitarTuner
{
    public class Notes
    {
        private static Notes notes;
        private static IRepository _noteRepo = new RepositoryService();
        private IList<FftGuitarTuner.Data.Entities.Notes> _notesList = _noteRepo.GetAll().ToList();
        private static readonly string[] NoteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
       
        protected Notes()
        {
        }

        public static Notes Instance()
        {
            return notes ?? (notes = new Notes());
        }

        public double GetFrequency(string note)
        {
            return _notesList.FirstOrDefault(n => n.Note == note).Frequency;
        }

        public string GetClosestNote(double freq)
        {
            var closest = _notesList.Select(n => n.Frequency).OrderBy(frequency => Math.Abs(freq - frequency)).First();
            return _notesList.FirstOrDefault(n => n.Frequency == closest).Note;
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
