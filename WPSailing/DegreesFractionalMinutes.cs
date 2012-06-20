using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPSailing
{
    public class DegreesFractionalMinutes
    {
        public DegreesFractionalMinutes(double fracdegrees)
        {
            Degrees = (int)fracdegrees;
            Minutes = Math.Abs(60 * (fracdegrees - ((double)Degrees)));
        }

        public DegreesFractionalMinutes(int degrees, double minutes)
        {
            Degrees = degrees;
            Minutes = minutes;
        }

        public DegreesFractionalMinutes()
        {
            Degrees = 0;
            Minutes = 0;
        }

        private int _degrees;
        public int Degrees
        {
            get
            {
                return _degrees;
            }
            set
            {
                _degrees = value;
            }
        }

        private double _minutes;
        public double Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = Math.Abs(value);
            }
        }

        public double FracDegrees
        {
            get
            {
                double posdeg = (Degrees < 0) ? 0 : 1;
                double mod = ((posdeg) - 0.5) * 2;
                return ((double)Degrees) + (mod * (Minutes / 60));
            }
        }

        public override string ToString()
        {
            String dfm = Degrees + "° " + Minutes.ToString("N2") + "'";
            return dfm;
        }
    }
}
