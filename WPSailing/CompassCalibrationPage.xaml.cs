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
using System.Windows.Threading;

namespace WPSailing
{
    public partial class CompassCalibrationPage : PhoneApplicationPage
    {
        DispatcherTimer timer;
        double headingAccuracy;

        public CompassCalibrationPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += new EventHandler(timer_Tick);

            App.ViewModel.Location.Compass.CurrentValueChanged += new EventHandler<Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.CompassReading>>(Compass_CurrentValueChanged);
        }

        void Compass_CurrentValueChanged(object sender, Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.CompassReading> e)
        {
            headingAccuracy = Math.Abs(e.SensorReading.HeadingAccuracy);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (headingAccuracy <= 10)
            {
                calibrationTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                calibrationTextBlock.Text = "Complete!";
            }
            else
            {
                calibrationTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                calibrationTextBlock.Text = headingAccuracy.ToString("0.0");
            }
        }

        private void calibrationButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            NavigationService.GoBack();
        }
    }
}