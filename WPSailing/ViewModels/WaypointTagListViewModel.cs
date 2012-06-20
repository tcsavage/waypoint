using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WPSailing
{
    /// <summary>
    /// Holds waypoint relevance data for the waypoint being edited.
    /// </summary>
    public class WaypointTagListViewModel : INotifyPropertyChanged
    {
        private string _name;
        /// <summary>
        /// The tag's name as presented to the user and as filtered in the database. Must be unique.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private bool _set;
        /// <summary>
        /// True if the tag is set to the current waypoint.
        /// </summary>
        public bool Set
        {
            get
            {
                return _set;
            }
            set
            {
                _set = value;
                NotifyPropertyChanged("Set");
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
