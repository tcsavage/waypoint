using System;
using System.ComponentModel;
using System.Device.Location;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Specialized;

namespace WPSailing
{
	/// <summary>
	/// Holds data about a waypoint.
	/// </summary>
	public class WaypointViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public WaypointViewModel()
		{
			_location.Latitude = new DegreesFractionalMinutes(0.0);
			_location.Longitude = new DegreesFractionalMinutes(0.0);
			Tags = new ObservableCollection<string>();
		}

		/// <summary>
		/// Shortcut to the users current location.
		/// </summary>
		[XmlIgnore]
		public static LocationViewModel CurrentLocation
		{
			get
			{
				return App.ViewModel.Location;
			}
		}
		
		/// <summary>
		/// Set when the waypoint is newly created.
		/// </summary>
		[XmlIgnore]
		public bool New = false;
		
		[XmlIgnore]
		private string _name;
		/// <summary>
		/// The waypoint's name as presented to the user and as filtered in the database. Must be unique.
		/// </summary>
		[XmlElement("name")]
		public string Name 
		{
			get 
			{
				return _name;
			}
			set 
			{
				foreach (WaypointViewModel wpt in App.ViewModel.Waypoints)
				{
					if (wpt.Name == value && _name != value)
					{
						MessageBox.Show("Waypoint name must be unique.");
						return;
					}
				}
				_name = value;
				NotifyPropertyChanged("Name");
			}
		}

		[XmlIgnore]
		private WaypointTypeEnum _type = WaypointTypeEnum.ArbitraryPoint;
		[XmlElement("type")]
		public WaypointTypeEnum Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		[XmlIgnore]
		private string _notes;
		/// <summary>
		/// Notes about the waypoint.
		/// </summary>
		[XmlElement("notes")]
		public string Notes
		{
			get
			{
				return _notes;
			}
			set
			{
				_notes = value;
				NotifyPropertyChanged("Notes");
			}
		}
		
		[XmlIgnore]
		private string _source;
		/// <summary>
		/// The waypoint's origin.
		/// </summary>
		[XmlElement("source")]
		public string Source 
		{
			get 
			{
				return _source;
			}
			set 
			{
				_source = value;
				NotifyPropertyChanged("Source");
			}
		}

		/// <summary>
		/// List of tag names.
		/// </summary>
		public ObservableCollection<string> Tags;

		/// <summary>
		/// Calculates if the filter includes this waypoint.
		/// </summary>
		[XmlIgnore]
		public bool InFilter
		{
			get
			{
				// Return true if:
				// - No tags are assigned
				// - At least one of the assigned tags is in the filter
				if (Tags.Count == 0)
				{
					return true;
				}
				else
				{
					foreach (WaypointTagViewModel tag in App.ViewModel.WaypointTags)
					{
						if (tag.InFilter && Tags.Contains(tag.Name))
						{
							return true;
						}
					}
					return false;
				}
			}
		}

		/// <summary>
		/// Returns visibility.
		/// </summary>
		[XmlIgnore]
		public Visibility IsVisible
		{
			get
			{
				return (InFilter) ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		/// <summary>
		/// Latitude position.
		/// </summary>
		[XmlElement("latitude")]
		public DegreesFractionalMinutes Latitude
		{
			get 
			{
				return _location.Latitude;
			}
			set 
			{
				_location.Latitude = value;
				NotifyPropertyChanged("Location");
				NotifyPropertyChanged("Latitude");
				NotifyPropertyChanged("LatLong");
				NotifyPropertyChanged("DevGeoCoordinate");
				NotifyPropertyChanged("Range");
				NotifyPropertyChanged("Bearing");
				NotifyPropertyChanged("RangeAndBearing");
			}
		}

		/// <summary>
		/// Longitude position.
		/// </summary>
		[XmlElement("longitude")]
		public DegreesFractionalMinutes Longitude
		{
			get
			{
				return _location.Longitude;
			}
			set
			{
				_location.Longitude = value;
				NotifyPropertyChanged("Location");
				NotifyPropertyChanged("Longitude");
				NotifyPropertyChanged("LatLong");
				NotifyPropertyChanged("DevGeoCoordinate");
				NotifyPropertyChanged("Range");
				NotifyPropertyChanged("Bearing");
				NotifyPropertyChanged("RangeAndBearing");
			}
		}

		[XmlIgnore]
		private Position _location = new Position();
		/// <summary>
		/// Full location of the waypoint.
		/// </summary>
		[XmlIgnore]
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
				NotifyPropertyChanged("Longitude");
				NotifyPropertyChanged("LatLong");
				NotifyPropertyChanged("DevGeoCoordinate");
				NotifyPropertyChanged("Range");
				NotifyPropertyChanged("Bearing");
				NotifyPropertyChanged("BearingRelCompass");
				NotifyPropertyChanged("RangeAndBearing");
			}
		}

		/// <summary>
		/// Gets location as a System.Device.Location.GeoCoordinate.
		/// </summary>
		[XmlIgnore]
		public GeoCoordinate DevGeoCoordinate
		{
			get
			{
				return new GeoCoordinate(Latitude.FracDegrees, Longitude.FracDegrees);
			}
		}
		
		/// <summary>
		/// Gets location latlong as a string.
		/// </summary>
		[XmlIgnore]
		public string LatLong
		{
			get
			{
				return Latitude.ToString() + " N, " + Longitude.ToString() + " E";
			}
		}
		
		/// <summary>
		/// Gets range to waypoint in nautical miles.
		/// </summary>
		[XmlIgnore]
		public double Range
		{
			get
			{
				return GreatCircleArc.DistanceInMeters(new Position(CurrentLocation.Latitude, CurrentLocation.Longitude), new Position(Latitude, Longitude)).MetersToNauticalMiles();
			}
		}
		
		/// <summary>
		/// Gets bearing to waypoint in degrees.
		/// </summary>
		[XmlIgnore]
		public double Bearing
		{
			get
			{
				Position pos = new Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
				Position target = new Position(Latitude, Longitude);
				return pos.BearingTo(target);
			}
		}

		/// <summary>
		/// Gets bearing to waypoint in degrees relative to compass.
		/// </summary>
		[XmlIgnore]
		public double BearingRelCompass
		{
			get
			{
				Position pos = new Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
				Position target = new Position(Latitude, Longitude);
				return pos.BearingTo(target) + CurrentLocation.Bearing;
			}
		}
		
		/// <summary>
		/// Gets range and bearing in a formatted string.
		/// </summary>
		[XmlIgnore]
		public string RangeAndBearing
		{
			get
			{
				return Range.ToString("F") + "NM, " + Bearing.ToString("F") + "°";
			}
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(String propertyName) 
		{
			if (null != PropertyChanged) 
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}