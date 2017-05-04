using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.Timers;
using Business;
using System.Data;

namespace PokerTimerClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan _time;
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _currentRound;
        private Configuration _conf;
        private int _count;
        private int _vato;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            RestartTimer();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            TimerStartStop();
        }

        private void TimerStartStop()
        {
            // Verify if the timer is running to stablish the buttons Labels.
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                btnStart.Content = "Start";
            }
            else
            {
                _timer.Start();
                btnStart.Content = "Stop";
            }

        }

        private void Initialize()
        {
            _conf = ConfigurationLoader.GetConfiguration();

            _timer.Interval = new TimeSpan(TimeSpan.TicksPerSecond);
            _timer.Tick += (s, a) =>
            {
                // Substract a second.
                _time = _time.Subtract(new TimeSpan(0, 0, 1));

                if (_time.TotalSeconds.Equals(0))
                {
                    RestartTimer();
                }

                // Draw the current time and color.
                UpdateClockColor();
                lblClock.Content = string.Format(_time.ToString("mm':'ss"));

            };
        }

        private void RestartTimer()
        {
            _time = _conf.RoundTime;
            lblClock.Content = _time.ToString("mm':'ss");
            _currentRound++;
            GetRoundInfo(_currentRound);
        }

        private void GetRoundInfo(int currentRound)
        {
            if (_conf.Blinds.Count() >= currentRound)
            {
                lblSmallBlind.Content = _conf.Blinds[_count].SmallBlind;
                if (!string.IsNullOrEmpty(_conf.Blinds[_count].Ante))
                {
                    lblBig.Content = "Big Blind";
                    lblBigBlind.Content = _conf.Blinds[_count].BigBlind;
                    lblAnteTxt.Content = "Ante";
                    lblAnte.Content = _conf.Blinds[_count].Ante;
                }
                else
                {
                    lblBig.Content = string.Empty;
                    lblAnteTxt.Content = "Big Blind";
                    lblAnte.Content = _conf.Blinds[_count].BigBlind;
                }
                _count++;
            }
            // Get Next Round Info.
            if (_conf.Blinds.Count() > _count)
            {
                lblNextSmall.Content = _conf.Blinds[_count].SmallBlind;
                lblNextBigBlind.Content = _conf.Blinds[_count].BigBlind;
                lblNextAnte.Content = _conf.Blinds[_count].Ante ?? string.Empty;
            }

        }

        private void UpdateClockColor()
        {
            var leftTime = (_conf.RoundTime - _time).TotalSeconds;
            var totalTime = _conf.RoundTime.TotalSeconds;
            var red = (leftTime / totalTime) * 255;
            var green = (totalTime - leftTime) / totalTime * 255;
            var blue = 0;

            var color = Color.FromRgb((byte)red, (byte)green, (byte)blue);
            var brush = new SolidColorBrush(color);

            lblClock.Foreground = brush;


        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            var load = LoadPremios();
            if (load.Count() > 0)
            {
                dataGrid.ItemsSource = load;
            }

        }

        private List<Premios> LoadPremios()
        {
            int entranda = 50;

            int x = 0;
            _vato = _vato + 1;
            lblNumEntra.Content =  _vato;
            lblTotalAcumulado.Content = _vato * entranda;
            int totalAct = _vato * entranda;

            var premios = new List<Premios>();
            while (totalAct >= entranda)
            {
                x++;
                int totalTemp;
                Premios premio = new Premios();

                if (totalAct == entranda)
                {
                    totalTemp = totalAct;
                    premio.Lugar = x;
                    premio.Cantidad = totalAct;

                }
                else
                {
                    totalTemp = totalAct / 2;
                    totalTemp = (int)(Math.Round((totalTemp + 10) / 100d) * 100);
                    premio.Lugar = x;
                    premio.Cantidad = totalTemp;

                }
                totalAct = totalAct - totalTemp;
                premios.Add(premio);
           
            }

            return premios;
        }
    }
}
