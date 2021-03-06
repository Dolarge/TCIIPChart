using System.Windows;
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

            ViewModel.SeverViewModel severViewModel = new ViewModel.SeverViewModel();
            this.DataContext = severViewModel;

            //if close window
            if (severViewModel.CloseAction == null)
            {
                //socket dipose 

                //close window
                severViewModel.CloseAction = new System.Action(() => this.Close());

            }

        }



    }
}
