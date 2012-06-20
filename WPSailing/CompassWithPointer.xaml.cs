using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPSailing
{
	public partial class CompassWithPointer : UserControl
	{
		public CompassWithPointer()
		{
			// Required to initialize variables
			InitializeComponent();
		}
		
		public static DependencyProperty BearingProperty = DependencyProperty.Register("Bearing", typeof(Double), typeof(CompassWithPointer), null);
        public Double Bearing
        {
            get
			{
                return (Double)GetValue(BearingProperty);
			}
            set
			{
                SetValue(BearingProperty, value);
			}
        }
		
		public static DependencyProperty PointerBearingProperty = DependencyProperty.Register("PointerBearing", typeof(Double), typeof(CompassWithPointer), null);
        public Double PointerBearing
        {
            get
			{
                return (Double)GetValue(PointerBearingProperty);
			}
            set
			{
                SetValue(PointerBearingProperty, value);
			}
        }

        public static DependencyProperty PointerOpacityProperty = DependencyProperty.Register("PointerOpacity", typeof(float), typeof(CompassWithPointer), null);
        public float PointerOpacity
        {
            get
            {
                return (float)GetValue(PointerOpacityProperty);
            }
            set
            {
                SetValue(PointerOpacityProperty, value);
            }
        }
	}
}