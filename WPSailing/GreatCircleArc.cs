using System;
using System.Device.Location;

namespace WPSailing
{
    public static class GreatCircleArc
    {
        private const double MEAN_EARTH_RADIUS_IN_METERS = 6372797.560856;

        private static double ArcInRadians(Position pos1, Position pos2)
        {
            double latArc = (pos1.Latitude.FracDegrees - pos2.Latitude.FracDegrees).DtoR();
            double longArc = (pos1.Longitude.FracDegrees - pos2.Longitude.FracDegrees).DtoR();

            double latH = Math.Sin(latArc * 0.5);
            latH *= latH;

            double longH = Math.Sin(longArc * 0.5);
            longH *= longH;

            double tmp = Math.Cos(pos1.Latitude.FracDegrees.DtoR()) * Math.Cos(pos2.Latitude.FracDegrees.DtoR());

            return 2.0 * Math.Asin(Math.Sqrt(latH + tmp * longH));
        }

        public static double DistanceInMeters(Position pos1, Position pos2)
        {
            return MEAN_EARTH_RADIUS_IN_METERS * ArcInRadians(pos1, pos2);
        }
    }
}
