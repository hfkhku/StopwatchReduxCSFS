using MStopwatch.Actions;
using MStopwatch.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;

namespace Stopwatch.Views
{
    /// <summary>
    /// MainPageView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainPageView : Page
    {
        private IDisposable TimerSubscription;
        public MainPageView()
        {
            InitializeComponent();

            App.Store.ObserveOnDispatcher().Subscribe(state =>
            {
                //Debug.WriteLine($"NowSpan:{state.GetState<TimeSpan>(NowSpan)}");
                txtNowSpan.Text = state.NowSpan.ToString(state.DisplayFormat);
                btnStartStopReset.Content = state.ButtonLabel;
                btnLap.IsEnabled = state.Mode == StopwatchMode.Start;
                lvLap.ItemsSource = state.LapTimeList;
                if (state.Mode == StopwatchMode.Stop)
                {
                    var nowSpan = state.NowSpan;
                    var maxLapTime = state.MaxLapTime;
                    var minLapTime = state.MinLapTime;

                    var r = MessageBox.Show($"All time: {nowSpan.ToString(state.DisplayFormat)}\r\nMax laptime: {maxLapTime.TotalMilliseconds} ms\nMin laptime: { minLapTime.TotalMilliseconds}ms\n\nShow all lap result?", "Confirmation", MessageBoxButton.OKCancel);

                    if (r == MessageBoxResult.OK)
                    {
                        var w = Application.Current.MainWindow as NavigationWindow;
                        w.Source = new Uri("ResultPageView.xaml", UriKind.Relative);
                    }
                }
            });

            //表示切替チェックボックス
            chbIsShowed.Events().Checked.Subscribe(_ => App.Store.Dispatch(new TimeFormatAction() { Format = Constants.TimeSpanFormat }));
            chbIsShowed.Events().Unchecked.Subscribe(_ => App.Store.Dispatch(new TimeFormatAction() { Format = Constants.TimeSpanFormatNoMillsecond }));

            //start,stop,resetボタン
            btnStartStopReset.Events().Click.Subscribe(e =>
            {
                var mode = App.Store.GetState().Mode;
                var scheduler = App.Store.GetState().TimerScheduler;
                if (mode == StopwatchMode.Init)
                {
                    TimerSubscription = Observable.Interval(TimeSpan.FromMilliseconds(10), scheduler)
                    .Subscribe(_ =>
                    {
                        App.Store.Dispatch(new TimerAction() { Now = scheduler.Now.DateTime.ToLocalTime() });
                    });
                }
                else if (mode == StopwatchMode.Start)
                {
                    TimerSubscription.Dispose();
                    TimerSubscription = null;
                }

                App.Store.Dispatch(new ChangeModeAction());

            });

            //lapボタン
            btnLap.Events().Click.Subscribe(_ =>
            {
                App.Store.Dispatch(new LapAction());
            });

        }
    }
}
