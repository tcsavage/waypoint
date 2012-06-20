using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Device.Location;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Data;

namespace WPSailing
{
	public partial class MainPage : PhoneApplicationPage
	{
		private readonly Random random = new Random();
		private readonly GeoCoordinateWatcher geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			var useEmulation = false;

			var observable = useEmulation ? CreateGeoPositionEmulator() : CreateObservableGeoPositionWatcher();

			observable.ObserveOnDispatcher().Subscribe(OnPositionChanged);

			App.ViewModel.SetMap(map);
			
			App.ViewModel.locationMarker.Content = "Current Location";
			App.ViewModel.locationMarker.Location = GeoCoordinate.Unknown;
			App.ViewModel.locationPushPinLayer.AddChild(App.ViewModel.locationMarker, GeoCoordinate.Unknown, PositionOrigin.Center);

			// Set the data context of the listbox control to the sample data
			DataContext = App.ViewModel;

			this.Loaded +=new RoutedEventHandler(MainPage_Loaded);
		}

		// Load data for the ViewModel Items
		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (!App.ViewModel.IsDataLoaded)
			{
				App.ViewModel.LoadData();
				mapItems.ItemsSource = App.ViewModel.Waypoints;

				var waypointCollectionView = this.Resources["FilteredWaypointList"] as CollectionViewSource;
				if (waypointCollectionView != null)
				{
					waypointCollectionView.Source = App.ViewModel.Waypoints;
					waypointCollectionView.View.Filter = new Predicate<object>(o => ((WaypointViewModel)o).InFilter && App.ViewModel.WaypointTypeInFilter(((WaypointViewModel)o).Type));
					NavigationService.Navigating += new NavigatingCancelEventHandler(NavigationService_Navigating);
				}
				else
				{
					MessageBox.Show("Something went horribly wrong.");
				}
			}
		}

		void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			var waypointCollectionView = this.Resources["FilteredWaypointList"] as CollectionViewSource;
			if (waypointCollectionView != null)
			{
				waypointCollectionView.View.Refresh();
			}
		}

		private IObservable<GeoCoordinate> CreateObservableGeoPositionWatcher()
		{
			var observable = Observable.FromEvent<GeoPositionChangedEventArgs<GeoCoordinate>>(
				e => geoCoordinateWatcher.PositionChanged += e,
				e => geoCoordinateWatcher.PositionChanged -= e)
				.Select(e => e.EventArgs.Position.Location);

			geoCoordinateWatcher.Start();

			return observable;
		}

		private IObservable<GeoCoordinate> CreateGeoPositionEmulator()
		{
			//return Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2)).Select(l => CreateRandomCoordinate());
			return Observable.Return<GeoCoordinate>(new GeoCoordinate(50.78174, -1.10996));
		}

		private GeoCoordinate CreateRandomCoordinate()
		{
			var latitude = (random.NextDouble() * 180.0) - 90.0;
			var longitude = (random.NextDouble() * 360.0) - 180.0;

			return new GeoCoordinate(latitude, longitude);
		}

		private void OnPositionChanged(GeoCoordinate location)
		{
			App.ViewModel.Location.Latitude = new DegreesFractionalMinutes(location.Latitude);
			App.ViewModel.Location.Longitude = new DegreesFractionalMinutes(location.Longitude);
			App.ViewModel.Location.Speed = location.Speed;
			App.ViewModel.Location.Course = location.Course;

			foreach (var wpt in App.ViewModel.Waypoints)
			{
				wpt.NotifyPropertyChanged("RangeAndBearing");
			}

			map.Center = location;
			App.ViewModel.locationMarker.Location = location;
		}

		private void btnAddWaypoint_Click(object sender, System.EventArgs e)
		{
			App.ViewModel.EditingWaypoint = new WaypointViewModel();
			App.ViewModel.EditingWaypoint.Location = App.ViewModel.Location.Location; // Use current location .
			App.ViewModel.EditingWaypoint.New = true;
			NavigationService.Navigate(new Uri("/EditWaypointPage.xaml", UriKind.Relative));
		}

		private void btnSettings_Click(object sender, System.EventArgs e)
		{
			NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
		}
		
		private void cxtWaypoint_TapAndHold(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = (MenuItem) sender;
			WaypointViewModel waypoint = (WaypointViewModel)menuItem.Tag;
			//MessageBox.Show("Action: " + menuItem.Header.ToString() + "\nTarget: " + waypoint.Name);
			switch (menuItem.Header.ToString())
			{
				case "set active":
					App.ViewModel.ActiveWaypoint = waypoint;
					//appPivot.SelectedItem = piLocation;
					break;
				case "edit details":
					App.ViewModel.EditingWaypoint = waypoint;
					NavigationService.Navigate(new Uri("/EditWaypointPage.xaml", UriKind.Relative));
					break;
				case "edit tags":
					App.ViewModel.EditingWaypoint = waypoint;
					NavigationService.Navigate(new Uri("/EditTagsPage.xaml", UriKind.Relative));
					break;
			}
		}

		private void btnMapZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			App.ViewModel.Zoom += 1;
		}

		private void btnMapZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			App.ViewModel.Zoom -= 1;
		}

		private void mnuManageTags_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/ManageTagsPage.xaml", UriKind.Relative));
		}

		private void btnFilter_Click(object sender, System.EventArgs e)
		{
			NavigationService.Navigate(new Uri("/FilterPage.xaml", UriKind.Relative));
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			var waypointCollectionView = this.Resources["FilteredWaypointList"] as CollectionViewSource;
			if (waypointCollectionView != null)
			{
				waypointCollectionView.View.Refresh();
			}
		}

		private void mnuCallibrateCompass_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/CompassCalibrationPage.xaml", UriKind.Relative));
		}
	}
}