using Redux;
using System;
using System.Windows;
using System.Reactive.Concurrency;
using System.Collections.ObjectModel;
using MStopwatch.States;
using MStopwatch.Models;
using MStopwatch;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        private IScheduler scheduler = Scheduler.Default;
        public App()
        {
            var initialState = new ApplicationState(
                timerScheduler: scheduler,
                displayFormat: Constants.TimeSpanFormatNoMillsecond,
                nowSpan: TimeSpan.Zero,
                mode: StopwatchMode.Init,
                buttonLabel: Constants.StartLabel,
                startTime: new DateTime(),
                now: new DateTime(),
                lapTimeList: new ObservableCollection<LapTime>(),
                maxLapTime: TimeSpan.Zero,
                minLapTime: TimeSpan.Zero
                );

            Store = new Store<ApplicationState>(Reducers.ReduceApplication, initialState);

        }
    }
}
