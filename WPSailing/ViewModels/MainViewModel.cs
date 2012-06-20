using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Phone.Controls.Maps;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Data;

namespace WPSailing
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public MainViewModel()
		{
			this.locationPushPinLayer = new MapLayer();
			this.locationMarker = new Pushpin();
			this.WaypointTags = new ObservableCollection<WaypointTagViewModel>();
			this.Waypoints = new ObservableCollection<WaypointViewModel>();
			this.Location = new LocationViewModel();
			this._activeWaypoint = null;
			this._editingWaypoint = null;
			this._zoom = DefaultZoomLevel;

			this.Location.PropertyChanged += new PropertyChangedEventHandler(Location_PropertyChanged);
		}

		void Location_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_activeWaypoint != null)
			{
				this.ActiveWaypoint.NotifyPropertyChanged("Bearing");
				this.ActiveWaypoint.NotifyPropertyChanged("BearingString");
			}
		}
		
		public void SetMap(Map map)
		{
			this.mapControl = map;
			this.mapControl.Children.Add(this.locationPushPinLayer);
		}

		public ObservableCollection<WaypointTagViewModel> WaypointTags { get; private set; }

		public ObservableCollection<WaypointViewModel> Waypoints { get; private set; }
		
		[XmlIgnore]
		public Map mapControl;

		[XmlIgnore]
		public LocationViewModel Location { get; private set; }
		
		[XmlIgnore]
		public MapLayer locationPushPinLayer;
		
		[XmlIgnore]
		public Pushpin locationMarker;

		[XmlIgnore]
		private readonly CredentialsProvider _credentialsProvider = new ApplicationIdCredentialsProvider(App.Id);
		[XmlIgnore]
		public CredentialsProvider CredentialsProvider
		{
			get { return _credentialsProvider; }
		}
		
		[XmlIgnore]
		private WaypointViewModel _activeWaypoint;
		[XmlIgnore]
		public WaypointViewModel ActiveWaypoint
		{
			get
			{
				return _activeWaypoint;
			}
			set
			{
				_activeWaypoint = value;
				NotifyPropertyChanged("ActiveWaypoint");
				ActiveWaypointOpacity = (value == null) ? 0 : 1;
			}
		}
		
		[XmlIgnore]
		private WaypointViewModel _editingWaypoint;
		[XmlIgnore]
		public WaypointViewModel EditingWaypoint
		{
			get
			{
				return _editingWaypoint;
			}
			set
			{
				_editingWaypoint = value;
				NotifyPropertyChanged("EditingWaypoint");
			}
		}
		
		[XmlIgnore]
		private WaypointTagViewModel _editingTag;
		[XmlIgnore]
		public WaypointTagViewModel EditingTag
		{
			get
			{
				return _editingTag;
			}
			set
			{
				_editingTag = value;
				NotifyPropertyChanged("EditingTag");
			}
		}

		[XmlIgnore]
		public ObservableCollection<WaypointTagListViewModel> EditingTagList
		{
			get
			{
				if (_editingWaypoint == null)
				{
					return null;
				}
				else
				{
					ObservableCollection<WaypointTagListViewModel> list = new ObservableCollection<WaypointTagListViewModel>();
					foreach (WaypointTagViewModel tag in WaypointTags)
					{
						list.Add(new WaypointTagListViewModel() { Name = tag.Name, Set = EditingWaypoint.Tags.Contains(tag.Name) });
					}
					return list;
				}
			}
		}
		
		[XmlIgnore]
		private double _zoom;
		[XmlIgnore]
		private const double DefaultZoomLevel = 15.0;
		[XmlIgnore]
		private const double MaxZoomLevel = 21.0;
		[XmlIgnore]
		private const double MinZoomLevel = 1.0;
		[XmlIgnore]
		public double Zoom
		{
			get { return _zoom; }
			set
			{
				var coercedZoom = Math.Max(MinZoomLevel, Math.Min(MaxZoomLevel, value));
				if (_zoom != coercedZoom)
				{
					_zoom = value;
					NotifyPropertyChanged("Zoom");
				}
			}
		}

		[XmlIgnore]
		public bool IsDataLoaded
		{
			get;
			private set;
		}

		[XmlIgnore]
		private float _activeWaypointOpacity = 0;
		[XmlIgnore]
		public float ActiveWaypointOpacity
		{
			get
			{
				return _activeWaypointOpacity;
			}
			set
			{
				_activeWaypointOpacity = value;
				NotifyPropertyChanged("ActiveWaypointOpacity");
			}
		}

		public ObservableCollection<WaypointTypeViewModel> WaypointTypeFilter { get; set; }

		public bool WaypointTypeInFilter(WaypointTypeEnum type)
		{
			if (WaypointTypeFilter == null)
			{
				// Setup type filter.
				this.WaypointTypeFilter = new ObservableCollection<WaypointTypeViewModel>();
				this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.ArbitraryPoint, true));
				this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.Buoy, true));
				this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.MOB, true));
				this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.Wreck, true));
			}

			foreach (var t in WaypointTypeFilter)
			{
				if (t.Type == type) return t.InFilter;
			}
			return false;
		}

		/// <summary>
		/// Creates and adds a few ItemViewModel objects into the Items collection.
		/// </summary>
		public void LoadData()
		{
			try
			{
				using (var store =
				  IsolatedStorageFile.GetUserStoreForApplication())
				{
					string offlineData =
					  Path.Combine("WPSailing", "Offline");
					//string offlineDataFile = Path.Combine(offlineData, "offline.xml");
					string offlineDataFile = "waypoint.xml";

					IsolatedStorageFileStream dataFile = null;

					if (store.FileExists(offlineDataFile))
					{
						dataFile =
						  store.OpenFile(offlineDataFile, FileMode.Open);
						XmlSerializer x = new XmlSerializer(typeof(MainViewModel));
						MainViewModel data = (MainViewModel)x.Deserialize(dataFile);
						dataFile.Close();

						this.WaypointTags = data.WaypointTags;
						this.Waypoints = data.Waypoints;
						this.WaypointTypeFilter = data.WaypointTypeFilter;
					}
					else
					{
						this.WaypointTags.Add(new WaypointTagViewModel() { Name = "Solent" });
						this.WaypointTags.Add(new WaypointTagViewModel() { Name = "Salcombe" });

						this.Waypoints.Add(new WaypointViewModel() { Name = "Calshot Spit", Latitude = new DegreesFractionalMinutes(50, 48.33979), Longitude = new DegreesFractionalMinutes(-1, 17.65075) });
						this.Waypoints.Add(new WaypointViewModel() { Name = "Thorn Channel", Latitude = new DegreesFractionalMinutes(50, 47.84878), Longitude = new DegreesFractionalMinutes(-1, 18.17573) });
						this.Waypoints.Add(new WaypointViewModel() { Name = "South Bramble", Latitude = new DegreesFractionalMinutes(50, 47.43856), Longitude = new DegreesFractionalMinutes(-1, 17.84913) });

						foreach (WaypointViewModel wpt in this.Waypoints)
						{
							wpt.Tags.Add("Solent");
						}

						this.Waypoints[1].Tags.Add("Salcombe");

						// Setup type filter.
						this.WaypointTypeFilter = new ObservableCollection<WaypointTypeViewModel>();
						this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.ArbitraryPoint, true));
						this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.Buoy, true));
						this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.MOB, true));
						this.WaypointTypeFilter.Add(new WaypointTypeViewModel(WaypointTypeEnum.Wreck, true));

						SaveData();

						MessageBox.Show("Some example waypoints have been loaded for you and saved.", "New Waypoint Directory", MessageBoxButton.OK);
					}
				}
			}

			catch (IsolatedStorageException)
			{
				// Fail.
			}

			this.IsDataLoaded = true;
		}

		public void SaveData()
		{
			try
			{
				using (var store =
				  IsolatedStorageFile.GetUserStoreForApplication())
				{

					// Create three directories in the root.
					//store.CreateDirectory("WPSailing");

					// Create three subdirectories under app.
					//string offlineData = Path.Combine("WPSailing", "Offline");

					//if (!store.DirectoryExists(offlineData)) { store.CreateDirectory(offlineData); }
					//string offlineDataFile = Path.Combine(offlineData, "offline.xml");
					IsolatedStorageFileStream dataFile = dataFile = store.OpenFile("waypoint.xml", FileMode.OpenOrCreate);

					XmlSerializer x = new XmlSerializer(typeof(MainViewModel));
					x.Serialize(dataFile, this);

					dataFile.Close();
				}
			}

			catch (IsolatedStorageException)
			{
				// Fail gracefully
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