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

namespace WPSailing
{
    public partial class EditWaypointPage : PhoneApplicationPage
    {
        public EditWaypointPage()
        {
			InitializeComponent();
			
			DataContext = App.ViewModel;

            Loaded += new RoutedEventHandler(EditWaypointPage_Loaded);
        }

        void EditWaypointPage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ListPickerItem item in lprType.Items)
            {
                string tag = (string)item.Tag;
                if (tag.Equals(App.ViewModel.EditingWaypoint.Type.ToString()))
                {
                    lprType.SelectedItem = item;
                    break;
                }
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	App.ViewModel.EditingWaypoint = null;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
			if (txtName.Text == String.Empty)
			{
				MessageBox.Show("Name cannot be blank.");
				return;
			}
        	App.ViewModel.EditingWaypoint.Name = txtName.Text;
            ListPickerItem selectedType = lprType.SelectedItem as ListPickerItem;
            if (selectedType != null)
            {
                App.ViewModel.EditingWaypoint.Type = (WaypointTypeEnum)Enum.Parse(typeof(WaypointTypeEnum), (string)selectedType.Tag, true);
            }
			App.ViewModel.EditingWaypoint.Source = txtSource.Text;
            App.ViewModel.EditingWaypoint.Latitude = llLatitude.LatLongFoSho;
            App.ViewModel.EditingWaypoint.Longitude = llLongitude.LatLongFoSho;
			if (App.ViewModel.EditingWaypoint.New)
			{
				App.ViewModel.EditingWaypoint.New = false;
				App.ViewModel.Waypoints.Add(App.ViewModel.EditingWaypoint);
			}
			App.ViewModel.EditingWaypoint = null;
			NavigationService.GoBack();
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
        	if (!App.ViewModel.EditingWaypoint.New)
			{
				MessageBoxResult res = MessageBox.Show("Are you sure you want to delete this waypoint?", App.ViewModel.EditingWaypoint.Name, MessageBoxButton.OKCancel);
				switch (res)
				{
					case MessageBoxResult.OK:
                        if (App.ViewModel.ActiveWaypoint != null && App.ViewModel.ActiveWaypoint.Name.Equals(App.ViewModel.EditingWaypoint.Name))
                        {
                            App.ViewModel.ActiveWaypoint = null;
                        }
						App.ViewModel.Waypoints.Remove(App.ViewModel.EditingWaypoint);
						App.ViewModel.EditingWaypoint = null;
						NavigationService.GoBack();
						break;
					case MessageBoxResult.Cancel:
						break;
				}
			}
        }
    }
}
