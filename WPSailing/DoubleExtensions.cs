using System;

namespace WPSailing
{
    public static class DoubleExtensions
    {
        public static double DtoR(this double angle)
        {
            return angle * (Math.PI / 180);
        }

        public static double RtoD(this double angle)
        {
            return angle * (180 / Math.PI);
        }
		
		public static double MetersToNauticalMiles(this double distance)
		{
			return distance * 0.000539956803;
		}
		
		public static double MetersPerSecondToKnots(this double speed)
		{
			return speed * 1.94384449;
		}

        public static String ToDegFracMinString(this double latlong)
        {
            int deg = (int)latlong;
            double min = Math.Abs((latlong - (double)deg) * 60);
            String dfm = deg + "° " + min.ToString("N5") + "'";
            return dfm;
        }
    }
}
