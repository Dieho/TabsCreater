using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FftGuitarTuner
{
    public partial class FrequenciesScale : UserControl
    {
        const double MinFrequency = 70;
        const double MaxFrequency = 1200;
        const double AFrequency = 440;
        static readonly double _toneStep = Math.Pow(2, 1.0 / 12);
        static readonly ScaleLabel[] _labels = 
        {
             new ScaleLabel() { Title = "E", Frequency =  82.4069, Color=Color.LightGreen},
             new ScaleLabel() { Title = "A", Frequency = 110.0000, Color=Color.LightGreen},
             new ScaleLabel() { Title = "D", Frequency = 146.8324, Color=Color.LightGreen},
             new ScaleLabel() { Title = "G", Frequency = 195.9977, Color=Color.LightGreen},
             new ScaleLabel() { Title = "B", Frequency = 246.9417, Color=Color.LightGreen},
             new ScaleLabel() { Title = "E", Frequency = 329.6276, Color=Color.LightGreen},
             new ScaleLabel() { Title = "",  Frequency = 440.0000, Color=Color.Silver}
        };        

        double _currentFrequency;

        [DefaultValue(0.0)]
        public double CurrentFrequency
        {
            get { return _currentFrequency; }
            set
            {
                if (_currentFrequency != value)
                { 
                    _currentFrequency = value; Invalidate(); 
                }
            }
        }

        bool _signalDetected;

        [DefaultValue(false)]
        public bool SignalDetected
        {
            get { return _signalDetected; }
            set
            {
                if (_signalDetected != value)
                {
                    _signalDetected = value; Invalidate();
                }
            }
        }

        public FrequenciesScale()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        public FrequenciesScale(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        static readonly Pen _markerPen = new Pen(Color.Black);
        static readonly Brush _activeSliderBrush1 = new SolidBrush(Color.GreenYellow);
        static readonly Brush _activeSliderBrush2 = new SolidBrush(Color.Green);
        static readonly Brush _inactiveSliderBrush1 = new SolidBrush(Color.FromArgb(70, Color.Gray));
        static readonly Brush _inactiveSliderBrush2 = new SolidBrush(Color.FromArgb(50, Color.Black));
        const int DisplayPadding = 5;
        const int MarkWidth = 6;
        const int LabelMarkSize = 9;

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            int minStep = (int)Math.Floor(GetToneStep(MinFrequency));
            int maxStep = (int)Math.Ceiling( GetToneStep(MaxFrequency) );

            int center = Width / 2;

            int totalSteps = maxStep - minStep;
            float stepSize = (float)(Height - 2 * DisplayPadding) / totalSteps;

            for (int i = 0; i <= totalSteps; i++)
            {
                float y = stepSize * i + DisplayPadding;

                e.Graphics.DrawLine(_markerPen, center - MarkWidth / 2, y, center + MarkWidth / 2, y);
            }

            float maxTextWidth = e.Graphics.MeasureString("W", Font).Width;
            foreach (ScaleLabel label in _labels)
            {
                Brush labelBrush = new SolidBrush(label.Color);
                double labelStep = GetToneStep(label.Frequency);
                float labelPosition = (float)(stepSize * (maxStep - labelStep) + DisplayPadding);

                RectangleF labelMarkPosition = new RectangleF(DisplayPadding, labelPosition - LabelMarkSize / 2,
                    LabelMarkSize, LabelMarkSize);
                e.Graphics.FillEllipse(labelBrush, labelMarkPosition);
                e.Graphics.DrawEllipse(_markerPen, labelMarkPosition);
                e.Graphics.FillEllipse(Brushes.White, DisplayPadding + LabelMarkSize / 5, labelPosition - LabelMarkSize / 3,
                    LabelMarkSize / 3, LabelMarkSize / 3);

                SizeF titleSize = e.Graphics.MeasureString(label.Title, Font);

                e.Graphics.DrawString(label.Title, Font, Brushes.Black,
                    new PointF(Width - DisplayPadding - maxTextWidth / 2 - titleSize.Width / 2 , 
                        labelPosition - titleSize.Height / 2));
            }

            if (CurrentFrequency > 0)
            {
                Brush sliderBrush1, sliderBrush2;
                if (!SignalDetected)
                {
                    sliderBrush1 = _inactiveSliderBrush1;
                    sliderBrush2 = _inactiveSliderBrush2;
                }
                else
                {
                    sliderBrush1 = _activeSliderBrush1;
                    sliderBrush2 = _activeSliderBrush2;
                }

                double sliderStep = GetToneStep(CurrentFrequency);
                float sliderPosition = (float)(stepSize * (maxStep - sliderStep) + DisplayPadding);

                e.Graphics.FillPolygon(sliderBrush1, new PointF[] 
                {
                    new PointF(center - 10, sliderPosition),
                    new PointF(center, sliderPosition - 5),
                    new PointF(center, sliderPosition + 5),
                    new PointF(center + 10, sliderPosition)
                });
                e.Graphics.FillPolygon(sliderBrush2, new PointF[] 
                {
                    new PointF(center - 10, sliderPosition),
                    new PointF(center, sliderPosition + 5),
                    new PointF(center, sliderPosition - 5),
                    new PointF(center + 10, sliderPosition)
                });
            }

        }

        private double GetToneStep(double frequency)
        {
            return Math.Log(frequency / AFrequency, _toneStep);
        }

        class ScaleLabel
        {
            public string Title;
            public double Frequency;
            public Color Color;
        }
    }
}
