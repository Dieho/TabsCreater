using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FftGuitarTuner
{
    public static class Notes
    {
        private static readonly string[] NoteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        private static readonly Dictionary<string, double> NotesDict = new Dictionary<string, double>()
        {
                                                                              {"E0",20.61},   {"F0",21.82},   {"F#",23.12},    {"G0",24.50},   {"G#0",25.95},   {"A0",27.50},   {"A#0",29.13},   {"B0",30.87}, 
            {"C1",32.70},   {"C#1",34.65},   {"D1",36.95},   {"D#1",38.88},   {"E1",41.21},   {"F1",43.65},   {"F#1",46.25},   {"G1",49.00},   {"G#1",51.90},   {"A1",55.00},   {"A#1",58.26},   {"B1",61.74},       
            {"C2",65.41},   {"C#2",69.30},   {"D2",73.91},   {"D#2",77.78},   {"E2",82.41},   {"F2",87.31},   {"F#2",92.50},   {"G2",98.00},   {"G#2",103.80},  {"A2",110.00},  {"A#2",116.54},  {"B2",123.48},       
            {"C3",130.82},  {"C#3",138.59},  {"D3",147.83},  {"D#3",155.56},  {"E3",164.81},  {"F3",174.62},  {"F#3",185.00},  {"G3",196.00},  {"G#3",207.00},  {"A3",220.00},  {"A#3",233.08},  {"B3",246.96},    
            {"C4",261.63},  {"C#4",277.18},  {"D4",293.66},  {"D#4",311.13},  {"E4",329.63},  {"F4",349.23},  {"F#4",369.99},  {"G4",392.00},  {"G#4",415.30},  {"A4",440.00},  {"A#4",466.16},  {"B4",493.88},    
            {"C5",523.25},  {"C#5",554.36},  {"D5",587.32},  {"D#5",622.26},  {"E5",659.26},  {"F5",698.46},  {"F#5",739.98},  {"G5",784.00},  {"G#5",830.60},  {"A5",880.00},  {"A#5",932.32},  {"B5",987.75},    
            {"C6",1046.50}, {"C#6",1108.70}, {"D6",1174.60}, {"D#6",1244.50}, {"E6",1318.50}, {"F6",1396.90}, {"F#6",1480.00}, {"G6",1568.00}, {"G#6",1661.20}, {"A6",1720.00}, {"A#6",1864.60}, {"B6",1975.50}, 
            {"C7",2093.00}, {"C#7",2217.40}, {"D7",2349.20}, {"D#7",2489.00}, {"E7",2637.00}, {"F7",2793.80}, {"F#7",2960.00}, {"G7",3136.00}, {"G#7",3332.40}, {"A7",3440.00}, {"A#7",3729.20}, {"B7",3951.00}, 
            {"C8",4186.00}, {"C#8",4434.80}, {"D8",4698.40}, {"D#8",4978.00}, {"E8",5274.00}
        };

        public static double GetFrequency(string note)
        {
            return NotesDict[note];
        }

        public static string GetClosestNote(double freq)
        {
            var listOfNotes = NotesDict.Values.ToList();
            var closest = listOfNotes.OrderBy(frequency => Math.Abs(freq - frequency)).First();
            return NotesDict.FirstOrDefault(x => x.Value == closest).Key;
        }

        public static string GetClosestNote2(double freq)
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
