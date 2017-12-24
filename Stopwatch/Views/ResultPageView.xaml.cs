﻿using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;

namespace Stopwatch.Views
{
    /// <summary>
    /// ResultPageView.xaml の相互作用ロジック
    /// </summary>
    public partial class ResultPageView : Page
    {
        public ResultPageView()
        {
            InitializeComponent();

            App.Store.ObserveOnDispatcher().Subscribe(state =>
            {
                //Debug.WriteLine($"NowSpan:{state.GetState<TimeSpan>(NowSpan)}");
                lvLap.ItemsSource = state.LapTimeList;

            });

            //back
            btnBack.Events().Click.Subscribe(_ =>
            {
                var w = Application.Current.MainWindow as NavigationWindow;
                w.Source = new Uri("MainPageView.xaml", UriKind.Relative);

            });
        }
    }
}
