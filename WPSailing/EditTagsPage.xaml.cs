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
    public partial class EditTagsPage : PhoneApplicationPage
    {
        public EditTagsPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            this.Loaded += new RoutedEventHandler(EditTagsPage_Loaded);
        }

        void EditTagsPage_Loaded(object sender, RoutedEventArgs e)
        {
            int i = VisualTreeHelper.GetChildrenCount(tagListBox);
            for (int j = 0; j < i; j++)
            {
                StackPanel sp1 = VisualTreeHelper.GetChild(tagListBox, j) as StackPanel;
                if (sp1 != null)
                {
                    foreach (var item2 in sp1.Children)
                    {
                        StackPanel sp2 = item2 as StackPanel;
                        if (sp2 != null)
                        {
                            foreach (var item3 in sp2.Children)
                            {
                                CheckBox cb = item3 as CheckBox;
                                if (cb != null)
                                {
                                    if (App.ViewModel.EditingWaypoint.Tags.Contains((string)cb.Tag))
                                    {
                                        cb.IsChecked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void cxtTag_TapAndHold(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            WaypointTagViewModel tag = (WaypointTagViewModel)menuItem.Tag;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            App.ViewModel.EditingWaypoint.Tags.Add((string)cb.Tag);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            App.ViewModel.EditingWaypoint.Tags.Remove((string)cb.Tag);
        }
    }
}
