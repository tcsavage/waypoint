using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Devices.Sensors;
using System.Windows.Navigation;

namespace WPSailing
{
	public class LocationViewModel : INotifyPropertyChanged
	{
		public LocationViewModel()
		{
			this._compass = new Compass();
			if (Compass.IsSupported)
			{
				this.UseCompass = false;
			}
			else
			{
				this.UseCompass = false;
			}
		}

		void _compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
		{
			//Bearing = e.SensorReading.MagneticHeading;
			if (compassCounter == 0)
			{
				lock (this)
				{
					lastBearing = _bearing;
					_bearing = e.SensorReading.MagneticHeading;
					Deployment.Current.Dispatcher.BeginInvoke(() =>
					{
						NotifyPropertyChanged("Bearing");
						NotifyPropertyChanged("BearingString");
					});
				}
			}
			compassCounter++;
			if (compassCounter == 5) compassCounter = 0;
		}

		private Position _location = new Position();
		public Position Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
				NotifyPropertyChanged("Location");
				NotifyPropertyChanged("Latitude");
				NotifyPropertyChanged("Longitude");
				NotifyPropertyChanged("LatLong");
			}
		}

		public DegreesFractionalMinutes Latitude
		{
			get 
			{
				return Location.Latitude;
			}
			set
			{
				Location.Latitude = value;
				NotifyPropertyChanged("Location");
				NotifyPropertyChanged("Latitude");
				NotifyPropertyChanged("LatLong");
			}
		}

		public DegreesFractionalMinutes Longitude
		{
			get
			{
				return Location.Longitude;
			}
			set
			{
				Location.Longitude = value;
				NotifyPropertyChanged("Location");
				NotifyPropertyChanged("Longitude");
				NotifyPropertyChanged("LatLong");
			}
		}
		
		public string LatLong
		{
			get
			{
				return Latitude.ToString() + " N, " + Longitude.ToString() + " E";
			}
		}

		private double _speed;
		/// <summary>
		/// Speed (over ground).
		/// </summary>
		public double Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
				NotifyPropertyChanged("Speed");
				NotifyPropertyChanged("SpeedString");
			}
		}
		
		public string SpeedString
		{
			get
			{
				return _speed.MetersPerSecondToKnots().ToString("N2") + "kn";
			}
		}

		private double _course;
		/// <summary>
		/// Heading.
		/// </summary>
		public double Course
		{
			get
			{
				return _course;
			}
			set
			{
				_course = value;
				NotifyPropertyChanged("Course");
				NotifyPropertyChanged("CourseString");
			}
		}
		
		public string CourseString
		{
			get
			{
				return (Speed > 0) ? _course.ToString("N2") + "°" : "--°";
			}
		}

		private volatile Compass _compass;
		public Compass Compass { get { return _compass; } }
		private bool _useCompass;
		public bool UseCompass
		{
			get
			{
				return _useCompass;
			}
			set
			{
				if (value)
				{
					this._compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(_compass_CurrentValueChanged);
					this._compass.Start();
				}
				else
				{
					this._compass.CurrentValueChanged -= new EventHandler<SensorReadingEventArgs<CompassReading>>(_compass_CurrentValueChanged);
					this._compass.Stop();
				}
				_useCompass = value;
			}
		}
		private double lastBearing = 0;
		private int compassCounter = 0;
		private double _bearing = 0;
		public double Bearing
		{
			get
			{
				double b;
				lock (this)
				{
					b = _bearing;
				}
				return b;
			}
			/*set
			{
				_bearing = value;
				NotifyPropertyChanged("Bearing");
				NotifyPropertyChanged("BearingString");
			}*/
		}

		public string BearingString
		{
			get
			{
				string b;
				lock (this)
				{
					b = _bearing.ToString("N2") + "°";
				}
				return b;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			if (null != PropertyChanged)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}