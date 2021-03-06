﻿using System;

namespace SoundAnalysis
{
    /// <summary>
    /// Complex number.
    /// </summary>
    internal struct ComplexNumber
    {
        public double Re;
        public double Im;

        public ComplexNumber(double re)
        {
            Re = re;
            Im = 0;
        }

        public ComplexNumber(double re, double im)
        {
            Re = re;
            Im = im;
        }

        public static ComplexNumber operator *(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re * n2.Re - n1.Im * n2.Im,
                n1.Im * n2.Re + n1.Re * n2.Im);
        }

        public static ComplexNumber operator +(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re + n2.Re, n1.Im + n2.Im);
        }

        public static ComplexNumber operator -(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re - n2.Re, n1.Im - n2.Im);
        }

        public static ComplexNumber operator -(ComplexNumber n)
        {
            return new ComplexNumber(-n.Re, -n.Im);
        }

        public static implicit operator ComplexNumber(double n)
        {
            return new ComplexNumber(n, 0);
        }

        public ComplexNumber PoweredE()
        {
            var e = Math.Exp(Re);
            return new ComplexNumber(e * Math.Cos(Im), e * Math.Sin(Im));
        }

        public double Power2()
        {
            return Re * Re - Im * Im;
        }

        public double AbsPower2()
        {
            return Re * Re + Im * Im;
        }

        public override string ToString()
        {
            return $"{Re}+i*{Im}";
        }
    }
}
