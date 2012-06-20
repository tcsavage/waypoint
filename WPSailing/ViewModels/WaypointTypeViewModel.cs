using System.ComponentModel;
using System;
using System.Xml.Serialization;

namespace WPSailing
{
    public class WaypointTypeViewModel : INotifyPropertyChanged 
    {
        [XmlIgnore]
        private WaypointTypeEnum _type;

        [XmlIgnore]
        private bool _inFilter;

        public WaypointTypeViewModel()
        {
        }

        public WaypointTypeViewModel(WaypointTypeEnum type, bool defaultFilterState)
        {
            _type = type;
            _inFilter = defaultFilterState;
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                return _type.ToString().FromPascalCase();
            }
        }

        public WaypointTypeEnum Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
                NotifyPropertyChanged("Name");
            }
        }

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
