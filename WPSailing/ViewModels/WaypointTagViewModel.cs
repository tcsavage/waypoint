using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows;

namespace WPSailing
{
    /// <summary>
    /// Holds data about a waypoint tag.
    /// </summary>
	public class WaypointTagViewModel : INotifyPropertyChanged 
	{
        [XmlIgnore]
		private string _name;
        /// <summary>
        /// The tag's name as presented to the user and as filtered in the database. Must be unique.
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
                foreach (WaypointTagViewModel tag in App.ViewModel.WaypointTags)
                {
                    if (tag.Name == value)
                    {
                        MessageBox.Show("Tag name must be unique.");
                        return;
                    }
                }
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [XmlIgnore]
        private string _description;
        /// <summary>
        /// Description of the tag.
        /// </summary>
        [XmlElement("description")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }
		
		[XmlIgnore]
		private bool _inFilter = true;
		[XmlElement("infilter")]
		public bool InFilter
		{
			get
			{
				return _inFilter;
			}
			set
			{
				_inFilter = value;
				NotifyPropertyChanged("InFilter");
			}
		}

        [XmlIgnore]
        public bool New = false;
		
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