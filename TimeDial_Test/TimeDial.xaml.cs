using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeDial_Test
{
    /// <summary>
    /// Interaction logic for TimeDial.xaml
    /// </summary>
    public partial class TimeDial : UserControl, INotifyPropertyChanged
    {

        public const double MAX_MARGIN = 330;
        public const double MAX_MINUTE = 1770;
        public const double HOUR_MARGIN = 30;

        public TimeDial()
        {

            Hours = new List<int>
            {
                1,2,3,4,5,6,7,8,9,10,11,12
            };

            Minutes = new List<int>();

            for (int i = 1; i <= 60; i++)
            {
                Minutes.Add(i);
            }

            Hour = 1;
            Minute = 1;

            InitializeComponent();
        }



        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); OnPropertyChanged(nameof(HourMargin)); }
        }

        // Using a DependencyProperty as the backing store for Hour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register(nameof(Hour), typeof(int), typeof(TimeDial), new PropertyMetadata(0));



        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); OnPropertyChanged(nameof(MinuteMargin)); }
        }

        // Using a DependencyProperty as the backing store for Minute.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register(nameof(Minute), typeof(int), typeof(TimeDial), new PropertyMetadata(0));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public Thickness HourMargin
        {
            get
            {

                double Top = (Hour - 1) * 30;

                //if (Top > 120) Top = 120;

                double Bot = MAX_MARGIN - (Hour - 1) * 30;

                //if (Bot > 120) Bot = 120;


                return new Thickness(0, -Top , 0, -Bot);
            }
        }

        public Thickness MinuteMargin
        {
            get
            {
                double Top = (Minute - 1) * 30;

                //if (Top > 120) Top = 120;

                double Bot = MAX_MINUTE - (Minute - 1) * 30;

                //if (Bot > 120) Bot = 120;


                return new Thickness(0, -Top, 0, -Bot);
            }
        }

        public List <int> Hours { get; set; }
        public List <int> Minutes { get; set; }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {



        }

        private void ItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0 && Hour < 12)
            {
                Hour += 1;
            }

            else if (e.Delta > 0 && Hour > 1)
            {
                Hour -= 1;

            }

        }

        private void ItemsControl_MouseWheel1(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0 && Minute < 60)
            {
                Minute += 1;
            }

            else if (e.Delta > 0 && Minute > 1)
            {
                Minute -= 1;
            }
        }
    }
}
