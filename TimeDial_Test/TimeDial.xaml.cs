using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public double MAX_MARGIN = 690;
        public const double MAX_MINUTE = 1770;
        public const double HOUR_MARGIN = 30;

        public TimeDial()
        {
            Minutes = new List<int>();
            Hours = new ObservableCollection<int>();

            for (int i = 0; i < 60; i++)
            {
                Minutes.Add(i);
            }

            for (int i = 0; i <= 23; i++)
            {
                Hours.Add(i);
            }

            TimeTypeValue = TimeType.H24;
            Hour = DateTime.Now.Hour;
            Minute = DateTime.Now.Minute;


            IsLiveTime = true;

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
                double Top = TimeTypeValue == TimeType.H24 ? (Hour) * 30 : (Hour - 1) * 30;

                double Bot = TimeTypeValue == TimeType.H24 ? MAX_MARGIN - (Hour) * 30 : MAX_MARGIN - (Hour - 1) * 30;

                return new Thickness(0, -Top, 0, -Bot);
            }
        }

        public Thickness MinuteMargin
        {
            get
            {
                double Top = (Minute) * 30;

                double Bot = MAX_MINUTE - (Minute) * 30;

                return new Thickness(0, -Top, 0, -Bot);
            }
        }



        private TimeType _TimeType;

        public TimeType TimeTypeValue
        {
            get { return _TimeType; }
            set { _TimeType = value; OnPropertyChanged(nameof(TimeTypeValue)); OnPropertyChanged(nameof(TimeTypeMargin)); }
        }

        public Thickness TimeTypeMargin
        {
            get
            {
                switch (TimeTypeValue)
                {

                    case TimeType.AM:
                        return new Thickness(0, 30, 0, -30);

                    case TimeType.PM:
                        return new Thickness(0, 0, 0, 0);

                    case TimeType.H24:
                        return new Thickness(0, -30, 0, 30);

                }

                return new Thickness();
            }
        }

        private bool _IsLiveTime;

        public bool IsLiveTime
        {
            get { return _IsLiveTime; }
            set { _IsLiveTime = value; OnPropertyChanged(nameof(IsLiveTime)); }
        }




        public ObservableCollection<int> Hours { get; set; }
        public List<int> Minutes { get; set; }


        private void ItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            IsLiveTime = false;
            if (e.Delta < 0 && (Hour < 12 && TimeTypeValue != TimeType.H24 || Hour < 23 && TimeTypeValue == TimeType.H24))
            {
                Hour += 1;
            }

            else if (e.Delta > 0 && (Hour > 1 && TimeTypeValue != TimeType.H24 || Hour > 0 && TimeTypeValue == TimeType.H24))
            {
                Hour -= 1;

            }
        }

        private void ItemsControl_MouseWheel1(object sender, MouseWheelEventArgs e)
        {
            IsLiveTime = false;
            if (e.Delta < 0 && Minute < 60)
            {
                Minute += 1;
            }

            else if (e.Delta > 0 && Minute > 1)
            {
                Minute -= 1;
            }
        }

        public void ResetOperation()
        {
            Minute = DateTime.Now.Minute;

            int H = DateTime.Now.Hour;
            if (TimeTypeValue != TimeType.H24)
            {

                if (H == 0)
                {
                    Hour = 12;
                    TimeTypeValue = TimeType.AM;
                }
                else if (H > 12)
                {
                    Hour = H - 12;
                    TimeTypeValue = TimeType.PM;
                }
                else
                {
                    Hour = H;
                    TimeTypeValue = TimeType.AM;

                }
            }
            else
            {
                Hour = H;
            }
        }

        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsLiveTime = true;
            ResetOperation();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsLiveTime)
            {
                ResetOperation();
            }
        }

        private void TimeTypeBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            switch (TimeTypeValue)
            {
                case TimeType.AM:
                    if (e.Delta < 0)
                    {
                        TimeTypeValue = TimeType.PM;
                        // No need to change in count
                    }
                    break;

                case TimeType.PM:
                    if (e.Delta < 0)
                    {
                        ItemsControl.Height = 720;
                        MAX_MARGIN = 690;
                        TimeTypeValue = TimeType.H24;

                    }
                    else
                    {
                        TimeTypeValue = TimeType.AM;
                        // No need to change in count
                    }
                    break;

                case TimeType.H24:
                    if (e.Delta > 0)
                    {
                        ItemsControl.Height = 360;
                        MAX_MARGIN = 330;
                        if (Hour == 0)
                        {
                            Hour = 12;
                        }
                        else if (Hour > 12)
                        {
                            Hour -= 12;
                        }
                        TimeTypeValue = TimeType.PM;
                    }
                    break;
            }
        }
    }

    public enum TimeType
    {
        AM, PM, H24
    }
}
