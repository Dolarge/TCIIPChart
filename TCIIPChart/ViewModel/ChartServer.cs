using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TCIIPChart.Model;

namespace TCIIPChart.ViewModel
{
    public class ChartServer : BindableBase
    {

        public ChartServer()
        {
            this.ConnectBtnClick = new DelegateCommand<object>(AsyncServerStart, Cansubmit);
        }
        public ICommand ConnectBtnClick { get; private set; }
        public ObservableCollection<MessageModel> ServerChart { get; set; } = new ObservableCollection<MessageModel>();
        private bool Cansubmit(object arg) { return true; }
        private readonly object _lock = new object();


        public static string? stringData = null;

        //Creat TCP/IP socket
        private Socket listener;
        private Socket handler;
        private TcpListener server;
        private TcpClient client;
        private NetworkStream ns;


        public byte[] byteData = new byte[1024];

        private void AsyncServerStart(object obj)
        {
            MessageBox.Show("TEST", "TEST", MessageBoxButton.OK, MessageBoxImage.Warning);
            ServerStart();
        }

        public void ServerStart()
        {
            server = new TcpListener(IPAddress.Any, 9999);
            server.Start();

            while (true)
            {
                var client = server.AcceptTcpClient();
            }


        }

        private static async void getclientData(TcpClient tcpClient)
        {
            using (NetworkStream ns = tcpClient.GetStream())
            {

                byte[] buffer = new byte[1024];

                int nRecv = await ns .ReadAsync(buffer, 0, buffer.Length);

                string message = Encoding.Default.GetString(buffer, 0, nRecv);

                Console.WriteLine(message);
                byte[] sendbuffer = Encoding.UTF8.GetBytes("Test "+ message);
                await ns.WriteAsync(sendbuffer,0,sendbuffer.Length);
                ns.Close();

            }
        }


        private void DataReceived(IAsyncResult ar)
        {
            // 콜백메서드입니다.(피호출자가 호출자의 해당 메서드를 실행시켜줍니다)
            // 즉 데이터를 읽었을때 실행됩니다.

            // 콜백으로 받아온 Data를 ClientData로 형변환 해줍니다.
            ClientData? callbackClient = ar.AsyncState as ClientData;

            //실제로 넘어온 크기를 받아옵니다
            int bytesRead = callbackClient.client.GetStream().EndRead(ar);

            // 문자열로 넘어온 데이터를 파싱해서 출력해줍니다.
            string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);

            Console.WriteLine(readString);

            // 비동기서버에서 가장중요한 핵심입니다. 
            // 비동기서버는 while문을 돌리지않고 콜백메서드에서 다시 읽으라고 비동기명령을 내립니다.
            callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
        }

        public void CloseServer()
        {
            server.Stop();
            ServerChart.Add(new MessageModel
            {
                Time = DateTime.Now,
                message = "Server Stop"
            });

            ns.Close();

        }

        public static string getLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    return localIP;
                }
            }

            return "127.0.0.1";

        }

        public void connectionListen()
        {
            Console.WriteLine("서버콘솔창 \n\n\n");
            ServerChart.Add(new MessageModel
            {
                Time = DateTime.Now,
                message = "TEST"
            });

            // TcpListener 생성자에 붙는 매개변수는 
            // 첫번째는 IP를 두번째는 port 번호입니다.
            TcpListener server = new TcpListener(IPAddress.Any, 9999);

            // 서버를 시작합니다.
            server.Start();


            // 클라이언트 객체를 만들어 9999에 연결한 client를 받아옵니다
            // 클라이언트가 접속할때까지 서버는 해당구문에서 블락됩니다.
            TcpClient client = server.AcceptTcpClient();


            // NetworkStream 객체를 만들어 client에서 보낸 데이터를 받을 객체를 생성합니다.
            NetworkStream ns = client.GetStream();

            // Socket은 byte[] 형식으로 데이터를 주고받으므로 byte[]형 변수를 선언합니다.
            byte[] byteData = new byte[1024];

            // 클라이언트가 write한 데이터를 읽어옵니다.
            // 아래의 작업 이후에 byteData에는 읽어온 데이터가 들어갑니다.
            // 동기서버의 경우 해당코드에서 읽을데이터가 올때까지 대기합니다.
            ns.Read(byteData, 0, byteData.Length);


            // 출력을 위해 byteData를 string형으로 바꿔줍니다.
            string stringData = Encoding.Default.GetString(byteData);

            Console.WriteLine(stringData);

            server.Stop();
            ns.Close();

        }
        public void CloseSocket()
        {
            if (handler != null)
            {
                handler.Close();
                handler.Dispose();
            }
            if (listener != null)
            {
                listener.Close();
                listener.Dispose();
            }

        }
    }


}
