﻿using System.Windows;
using TCIIPChart.ViewModel;

namespace TCIIPChart
{
    /// <summary>
    /// 간단한 tcp/ip로 비동기 통신 예제
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            ViewModel.ChartServer severViewModel = new ViewModel.ChartServer();
            this.DataContext = severViewModel;

           
        }



    }
}
