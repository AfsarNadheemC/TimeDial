using System.ComponentModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _Hour;

        public int Hour
        {
            get { return _Hour; }
            set { _Hour = value;  }
        }

        private int _Minute;

        public int Minute
        {
            get { return _Minute; }
            set { _Minute = value;  }
        }

        public MainWindow()
        {
            InitializeComponent();

            TimeOnly Time = TimeDial.GetTime;                 // {06:04}
            string TimeString = TimeDial.GetTimeString;       // "06:04 AM"
            string Meridiem = TimeDial.GetMeridiem;           // "AM"
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show($"Hour : {TimeDial.Hour}\nHour Margin : {TimeDial.HourMargin}\nHour Max Margin : {TimeDial.MAX_MARGIN}\nHour Items Control Height : {TimeDial.HourItemsControl.Height}\nMinute : {TimeDial.Minute}\nMinute Margin : {TimeDial.MinuteMargin} Time Type : {TimeDial.TimeTypeValue}"                );
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{TimeDial.IsLiveTime}");
        }
    }
}
