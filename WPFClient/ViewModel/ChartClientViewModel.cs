using Prism.Commands;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFClient.ViewModel
{
    public class ChartClientViewModel
    {
        private TcpClient client = null;

        public ICommand ConnectClient { get; private set; }
        public ICommand SendmessageBtn { get; private set; }
        public ICommand CloseClientBtn { get; private set; }
        private bool Cansubmit(object arg) { return true; }

        public ChartClientViewModel()
        {
            ConnectClient = new DelegateCommand<object>(this.SetupClient, Cansubmit);
            SendmessageBtn = new DelegateCommand<object>(this.SendMessage, Cansubmit);
            CloseClientBtn = new DelegateCommand<object>(this.CloseClient, Cansubmit);
        }

        public void SetupClient(object obj)
        {
            //Server가 동일한 pc에서 돌아가므로
            //자기자신을 나타내는 루프백ip (127.0.0.1) 사용
            //두번째 변수는 서버에서 설정한 포트번호 입력
            if (client == null)
            {
                client = new TcpClient();
            }
            client.Connect("127.0.0.1", 9999);


        }

        public void SendMessage(object obj)
        {
            if (client == null)
            {
                MessageBox.Show("Connect Client First", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (obj == null)
            {
                MessageBox.Show("empty message", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string message = obj as string;
            byte[] byteData = Encoding.Default.GetBytes(message);
            client.GetStream().Write(byteData, 0, byteData.Length);

        }

        public void CloseClient(object obj)
        {
            client.Close();
        }
    }
}
