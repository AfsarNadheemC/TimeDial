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

namespace TimeDialControl
{
    /// <summary>
    /// Interaction logic for TimeDial.xaml
    /// </summary>
    public partial class TimeDial : UserControl, INotifyPropertyChanged
    {

        public double MAX_MARGIN = 690;
        public const double MAX_MINUTE = 1770;

        private bool isTimeUpdatingFlag;

        public TimeDial()
        {

            Minutes = new List<int>();
            Hours = new ObservableCollection<int>();

            for (int i = 0; i < 60; i++)
            {
                Minutes.Add(i);
            }



            var v = Is24HourFormat;

            InitializeComponent();
        }

        public void Initiate()
        {
            if (TimeTypeValue == TimeType.PM)
            {
                if (Hour > 12)
                {
                    Hour -= 12;
                }
            }
            else if (TimeTypeValue == TimeType.AM)
            {
                if (Hour == 0)
                {
                    Hour = 12;
                }
            }
        }

        private static void OnHourChanged(
DependencyObject d,
DependencyPropertyChangedEventArgs e)
        {
            if (d is TimeDial TD)
            {
                TD.UpdateHour();
            }
        }

        private void UpdateHour()
        {
            IsLiveTime = false;
            OnPropertyChanged(nameof(HourMargin));
        }

        private static object IsValidHour(DependencyObject d, object baseValue)
        {
            if (d is TimeDial TD)
            {
                return TD.GetHour((int)baseValue);
            }
            return 0;

        }

        private int GetHour(int hour)
        {
            bool IsSet;

            if (TimeTypeValue == TimeType.H24)
            {
                IsSet = (hour >= 0 && hour <= 23);
            }
            else
            {
                IsSet = (hour >= 1 && hour <= 12);
            }

            return IsSet ? hour : Hour;
        }


        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register(nameof(Hour), typeof(int), typeof(TimeDial), new PropertyMetadata(DateTime.Now.Hour, OnHourChanged, IsValidHour));



        private static void OnMinuteChanged(
DependencyObject d,
DependencyPropertyChangedEventArgs e)
        {
            if (d is TimeDial TD)
            {
                TD.UpdateMinute();
            }
        }

        private void UpdateMinute()
        {
            IsLiveTime = false;
            OnPropertyChanged(nameof(MinuteMargin));
        }

        private static object IsValidMinute(DependencyObject d, object baseValue)
        {
            if (baseValue is int minute && minute >= 0 && minute <= 59)
            {
                return minute;
            }
            else if (d is TimeDial TD)
            {
                return TD.Minute;
            }
            return 0;
        }


        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); OnPropertyChanged(nameof(MinuteMargin)); }
        }

        // Using a DependencyProperty as the backing store for Minute.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register(nameof(Minute), typeof(int), typeof(TimeDial), new PropertyMetadata(DateTime.Now.Minute, OnMinuteChanged, IsValidMinute));

        public event PropertyChangedEventHandler? PropertyChanged;


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

        private static void OnFormatChanged(
DependencyObject d,
DependencyPropertyChangedEventArgs e)
        {
            if (d is TimeDial TD)
            {
                TD.UpdateTimeTypeValue(TD.Is24HourFormat);
            }
        }

        public void UpdateTimeTypeValue(bool? is24HourFormat)
        {
            if (is24HourFormat is true)
            {

                TimeTypeValue = TimeType.H24;

                switch (_TimeType)
                {

                    case TimeType.AM:
                        if (Hour == 12)
                        {
                            Hour = 0;
                        }
                        else
                        {
                            OnPropertyChanged(nameof(HourMargin));
                        }
                        break;

                    case TimeType.PM:
                        if (Hour != 12)
                        {
                            Hour += 12;
                        }
                        else
                        {
                            OnPropertyChanged(nameof(HourMargin));
                        }
                        break;

                }


            }
            else if (is24HourFormat is false)
            {
                if (Hour > 11)
                {
                    TimeTypeValue = TimeType.PM;
                }
                else
                {
                    TimeTypeValue = TimeType.AM;
                }
            }

        }


        public bool? Is24HourFormat
        {
            get { return (bool?)GetValue(Is24HourFormatProperty); }
            set
            {
                SetValue(Is24HourFormatProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Is24HourFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Is24HourFormatProperty =
            DependencyProperty.Register(nameof(Is24HourFormat), typeof(bool?), typeof(TimeDial), new PropertyMetadata(null, OnFormatChanged));



        private TimeType _TimeType;

        public TimeType TimeTypeValue
        {
            get { return _TimeType; }
            private set
            {

                isTimeUpdatingFlag = true;

                Hours.Clear();

                TimeType OldTimeType = _TimeType;
                _TimeType = value;


                switch (value)
                {

                    case TimeType.AM:
                        for (int i = 1; i <= 12; i++)
                        {
                            Hours.Add(i);
                        }
                        HourItemsControl?.Height = 360;
                        MAX_MARGIN = 330;

                        if (OldTimeType == TimeType.H24)
                        {
                            if (Hour == 0)
                            {
                                Hour = 12;
                            }
                            else if (Hour > 12)
                            {
                                Hour -= 12;
                            }
                            else
                            {
                                OnPropertyChanged(nameof(HourMargin));

                            }
                        }

                        break;


                    case TimeType.PM:

                        for (int i = 1; i <= 12; i++)
                        {
                            Hours.Add(i);
                        }
                        HourItemsControl?.Height = 360;
                        MAX_MARGIN = 330;


                        if (OldTimeType == TimeType.H24)
                        {
                            if (Hour == 0)
                            {
                                Hour = 12;
                            }
                            else if (Hour > 12)
                            {
                                Hour -= 12;
                            }
                            else
                            {
                                OnPropertyChanged(nameof(HourMargin));

                            }
                        }
                        break;

                    case TimeType.H24:

                        for (int i = 0; i <= 23; i++)
                        {
                            Hours.Add(i);
                        }

                        HourItemsControl?.Height = 720;
                        MAX_MARGIN = 690;


                        OnPropertyChanged(nameof(HourMargin));

                        break;

                }


                OnPropertyChanged(nameof(TimeTypeValue));
                OnPropertyChanged(nameof(TimeTypeMargin));

                isTimeUpdatingFlag = false;
            }
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
            private set { _IsLiveTime = value; OnPropertyChanged(nameof(IsLiveTime)); }
        }

        public ObservableCollection<int> Hours { get; private set; }

        public List<int> Minutes { get; private set; }

        private void HourItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            isTimeUpdatingFlag = true;

            IsLiveTime = false;
            if (e.Delta < 0 && (Hour < 12 && TimeTypeValue != TimeType.H24 || Hour < 23 && TimeTypeValue == TimeType.H24))
            {
                Hour += 1;
            }

            else if (e.Delta > 0 && (Hour > 1 && TimeTypeValue != TimeType.H24 || Hour > 0 && TimeTypeValue == TimeType.H24))
            {
                Hour -= 1;

            }

            isTimeUpdatingFlag = false;
        }

        private void MinuteItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            isTimeUpdatingFlag = true;

            IsLiveTime = false;
            if (e.Delta < 0 && Minute < 59)
            {
                Minute += 1;
            }

            else if (e.Delta > 0 && Minute > 0)
            {
                Minute -= 1;
            }

            isTimeUpdatingFlag = false;
        }

        public void ResetOperation()
        {
            if (Minute != Minute)
            {
                Minute = DateTime.Now.Minute;
            }

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
                    if (Hour != H - 12)
                    {
                        Hour = H - 12;
                    }
                    TimeTypeValue = TimeType.PM;
                }
                else
                {
                    if (Hour != H)
                    {
                        Hour = H;
                    }
                    TimeTypeValue = TimeType.AM;
                }
            }
            else
            {
                if (Hour != H)
                {
                    Hour = H;
                }
            }
        }

        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetOperation();
            IsLiveTime = true;
        }

        private void NowGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsLiveTime)
            {
                ResetOperation();
            }
        }

        private void TimeTypeBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            IsLiveTime = false;

            switch (TimeTypeValue)
            {
                case TimeType.AM:
                    if (e.Delta < 0)
                    {
                        TimeTypeValue = TimeType.PM;
                    }
                    break;

                case TimeType.PM:
                    if (e.Delta < 0)
                    {
                        TimeTypeValue = TimeType.H24;
                    }
                    else
                    {
                        TimeTypeValue = TimeType.AM;
                    }
                    break;

                case TimeType.H24:
                    if (e.Delta > 0)
                    {
                        TimeTypeValue = TimeType.PM;
                    }
                    break;
            }
        }

        private void Hour_Mousedown(object sender, MouseButtonEventArgs e)
        {
            isTimeUpdatingFlag = true;

            if (sender is TextBlock tb && tb.DataContext is int h)
            {

                if (Hour == h)
                {
                    OnPropertyChanged(nameof(HourMargin));
                    HourTextBox.Visibility = Visibility.Visible;
                    HourTextBox.Focus();
                    HourTextBox.SelectAll();

                }
                else
                {
                    Hour = h;
                    IsLiveTime = false;
                }
            }

            isTimeUpdatingFlag = false;
        }

        private void HourTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateHourByTextBox();
        }
        private void HourTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateHourByTextBox();
        }
        private void HourTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            IsLiveTime = false;

            if (sender is TextBox tb)
            {
                if (e.Key == Key.Enter)
                {
                    UpdateHourByTextBox();
                    e.Handled = true;
                }
            }
        }
        public void UpdateHourByTextBox()
        {
            HourTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            OnPropertyChanged(nameof(HourMargin));
            OnPropertyChanged(nameof(HourMargin));
            HourTextBox.Visibility = Visibility.Collapsed;
        }

        private void Minute_Mousedown(object sender, MouseButtonEventArgs e)
        {
            isTimeUpdatingFlag = true;

            if (sender is TextBlock tb && tb.DataContext is int m)
            {
                if (Minute == m)
                {
                    MinuteTextBox.Visibility = Visibility.Visible;
                    MinuteTextBox.Focus();
                    OnPropertyChanged(nameof(Minute));
                    MinuteTextBox.SelectAll();
                }
                else
                {
                    Minute = m;
                    IsLiveTime = false;
                }
            }

            isTimeUpdatingFlag = false;
        }
        private void MinuteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (isTimeUpdatingFlag) return;

            if (int.TryParse(MinuteTextBox.Text, out int newMinute))
            {
                if (newMinute < 0 || newMinute > 59)
                {
                    MinuteTextBox.Text = Minute.ToString();
                }
            }
            else
            {
                MinuteTextBox.Text = Minute.ToString();
            }
        }
        private void MinuteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            IsLiveTime = false;
            if (e.Key == Key.Enter)
            {
                UpdateMinuteByTextBox();
                e.Handled = true;
            }
        }
        private void MinuteTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateMinuteByTextBox();
        }
        private void MinuteTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateMinuteByTextBox();
        }
        public void UpdateMinuteByTextBox()
        {
            MinuteTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            OnPropertyChanged(nameof(Minute));
            OnPropertyChanged(nameof(MinuteMargin));
            MinuteTextBox.Visibility = Visibility.Collapsed;
        }

        private void TimeTypeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label tb)
            {
                switch (tb.Content)
                {
                    case "AM":
                        if (TimeTypeValue != TimeType.AM) IsLiveTime = false;
                        TimeTypeValue = TimeType.AM;
                        break;

                    case "PM":
                        if (TimeTypeValue != TimeType.PM) IsLiveTime = false;
                        TimeTypeValue = TimeType.PM;
                        break;

                    case "24h":
                        if (TimeTypeValue != TimeType.H24) IsLiveTime = false;
                        TimeTypeValue = TimeType.H24;
                        break;
                }
            }
        }

        public string GetTimeString
        {
            get
            {
                string hourStr = Hour.ToString("D2");
                string minuteStr = Minute.ToString("D2");
                if (TimeTypeValue == TimeType.H24)
                {
                    return $"{hourStr}:{minuteStr}";
                }
                else
                {
                    string amPm = TimeTypeValue == TimeType.AM ? "AM" : "PM";
                    return $"{hourStr}:{minuteStr} {amPm}";
                }
            }
        }
        public TimeOnly GetTime
        {
            get
            {
                if (IsLiveTime)
                {
                    ResetOperation();
                }
                return new TimeOnly(Hour, Minute);
            }
        }

        public string GetMeridiem
        {
            get
            {
                if (TimeTypeValue == TimeType.H24)
                {
                    return "";
                }
                else
                {
                    return TimeTypeValue == TimeType.AM ? "AM" : "PM";
                }
            }
        }

        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var h = Hour;
            var m = Minute;
            Is24HourFormat ??= false;

            if (Is24HourFormat == false)
            {
                if (Hour == 0)
                {
                    Hour = 12;
                }
                else if (Hour > 12)
                {
                    Hour -= 12;
                }
            }

            IsLiveTime = true;
        }

        private void Container_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
        }

        private void Container_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Popup.IsMouseOver) return;
            Popup.IsOpen = false;
        }
    }

    public enum TimeType
    {
        H24,
        AM,
        PM

    }
}
