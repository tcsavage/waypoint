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
    public partial class ManageTagsPage : PhoneApplicationPage
    {
        public ManageTagsPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selected = sender as TextBlock;
            if (selected != null)
            {
                foreach (var tag in App.ViewModel.WaypointTags)
                {
                    if (tag.Name.Equals(selected.Text))
                    {
                        App.ViewModel.EditingTag = tag;
                        NavigationService.Navigate(new Uri("/EditTagPage.xaml", UriKind.Relative));
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            App.ViewModel.EditingTag = new WaypointTagViewModel();
            App.ViewModel.EditingTag.New = true;
            NavigationService.Navigate(new Uri("/EditTagPage.xaml", UriKind.Relative));
        }
    }
}
