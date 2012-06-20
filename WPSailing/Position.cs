using System;

namespace WPSailing
{
    public class Position
    {
        public DegreesFractionalMinutes Latitude;
        public DegreesFractionalMinutes Longitude;

        public Position(DegreesFractionalMinutes latitude, DegreesFractionalMinutes longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Position(double latitude, double longitude)
        {
            Latitude = new DegreesFractionalMinutes(latitude);
            Longitude = new DegreesFractionalMinutes(longitude);
        }

        public Position()
        {
            Latitude = new DegreesFractionalMinutes();
            Longitude = new DegreesFractionalMinutes();
        }

        public double DistanceInMetersTo(Position target)
        {
            return GreatCircleArc.DistanceInMeters(this, target);
        }

        public double BearingTo(Position target)
        {
            double lat1 = this.Latitude.FracDegrees.DtoR();
            double long1 = this.Longitude.FracDegrees.DtoR();
            double lat2 = target.Latitude.FracDegrees.DtoR();
            double long2 = target.Longitude.FracDegrees.DtoR();
            double bearing = Math.Atan2(Math.Sin(long2 - long1) * Math.Cos(lat2), (Math.Cos(lat1) * Math.Sin(lat2)) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(long2 - long1)));
            bearing = bearing.RtoD();
            bearing = (bearing + 360.0) % 360;
            return bearing;
        }
    }
}
