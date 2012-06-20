using System;
using System.Windows;
using System.Windows.Controls;

namespace WPSailing
{
	public partial class LatLongControl : UserControl
	{		
		public LatLongControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		/// <summary>
		/// Gets the REAL value of LatLong (if it hasn't updated). Horrible but it works.
		/// </summary>
		public DegreesFractionalMinutes LatLongFoSho
		{
			get
			{
				int deg;
				try
				{
					deg = Int32.Parse(txtDegrees.Text);
				}
				catch
				{
					deg = 0;
				}

				double min;
				try
				{
					min = Double.Parse(txtMinutes.Text);
				}
				catch
				{
					min = 0;
				}
				return new DegreesFractionalMinutes(deg, min);
			}
		}
		
		public static DependencyProperty LatLongProperty = DependencyProperty.Register("LatLong", typeof(DegreesFractionalMinutes), typeof(LatLongControl), new PropertyMetadata(OnLatLongPropertyChanged));
		public DegreesFractionalMinutes LatLong
		{
			get
			{
				return (DegreesFractionalMinutes)GetValue(LatLongProperty);
			}
			set
			{
				SetValue(LatLongProperty, value);
				SetValue(DegreesProperty, value.Degrees);
				SetValue(MinutesProperty, value.Minutes);
			}
		}

		public static DependencyProperty DegreesProperty = DependencyProperty.Register("Degrees", typeof(int), typeof(LatLongControl), new PropertyMetadata(OnDegreesPropertyChanged));
		public int Degrees
		{
			get
			{
				return (int)GetValue(DegreesProperty);
			}
			set
			{
				SetValue(DegreesProperty, value);
				SetValue(LatLongProperty, new DegreesFractionalMinutes(value, Minutes));
			}
		}

		public static DependencyProperty MinutesProperty = DependencyProperty.Register("Minutes", typeof(Double), typeof(LatLongControl), new PropertyMetadata(OnMinutesPropertyChanged));
		public Double Minutes
		{
			get
			{
				return (Double)GetValue(MinutesProperty);
			}
			set
			{
				SetValue(MinutesProperty, value);
				SetValue(LatLongProperty, new DegreesFractionalMinutes(Degrees, value));
			}
		}

		private static void OnLatLongPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			var ob = s as LatLongControl;
			DegreesFractionalMinutes value = (DegreesFractionalMinutes)e.NewValue;
			ob.LatLong = value;
		}

		private static void OnDegreesPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			var ob = s as LatLongControl;
			int value = (int)e.NewValue;
			ob.Degrees = value;
		}

		private static void OnMinutesPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			var ob = s as LatLongControl;
			double value = (double)e.NewValue;
			ob.Minutes = value;
		}

		private void Degrees_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			/*TextBox tb = sender as TextBox;
			try
			{
				Degrees = Double.Parse(tb.Text);
			}
			catch (FormatException ex)
			{
				return;
			}*/
		}

		private void Minutes_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			/*TextBox tb = sender as TextBox;
			try
			{
				Minutes = Double.Parse(tb.Text);
			}
				catch (FormatException ex)
			{
				return;
			}*/
		}
	}
}