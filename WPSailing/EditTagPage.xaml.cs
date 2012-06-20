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
    public partial class EditTagPage : PhoneApplicationPage
    {
        public EditTagPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel.EditingTag;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (App.ViewModel.EditingTag.Name != null)
            {
                if (!App.ViewModel.EditingTag.Name.Equals(txtName.Text))
                {
                    foreach (var wpt in App.ViewModel.Waypoints)
                    {
                        for (int i = 0; i < wpt.Tags.Count; i++)
                        {
                            if (wpt.Tags[i] != null && wpt.Tags[i].Equals(App.ViewModel.EditingTag.Name))
                            {
                                wpt.Tags[i] = txtName.Text;
                            }
                        }
                    }
                }
            }
            App.ViewModel.EditingTag.Name = txtName.Text;
            App.ViewModel.EditingTag.Description = txtDescription.Text;
            if (App.ViewModel.EditingTag.New)
            {
                App.ViewModel.EditingTag.New = false;
                App.ViewModel.WaypointTags.Add(App.ViewModel.EditingTag);
            }
            App.ViewModel.EditingTag = null;
            NavigationService.GoBack();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!App.ViewModel.EditingTag.New)
            {
                MessageBoxResult res = MessageBox.Show("Are you sure you want to delete this tag?", App.ViewModel.EditingTag.Name, MessageBoxButton.OKCancel);
                switch (res)
                {
                    case MessageBoxResult.OK:
                        foreach (var wpt in App.ViewModel.Waypoints)
                        {
                            if (wpt.Tags.Contains(App.ViewModel.EditingTag.Name))
                            {
                                wpt.Tags.Remove(App.ViewModel.EditingTag.Name);
                            }
                        }
                        App.ViewModel.WaypointTags.Remove(App.ViewModel.EditingTag);
                        App.ViewModel.EditingTag = null;
                        NavigationService.GoBack();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
        }
    }
}