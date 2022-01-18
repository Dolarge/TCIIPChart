using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using TCIIPChart.Model;

namespace TCIIPChart.ViewModel
{

    public interface IViewModel { }

    public class SeverViewModel : BindableBase, IViewModel
    {

        public ObservableCollection<MessageModel> ServerChart { get; set; } = new ObservableCollection<MessageModel>();

        public ICommand ConnectBtnClick { get; private set; }


        public SeverViewModel()
        {

            this.ConnectBtnClick = new DelegateCommand<object>(this.ServerStart, Cansubmit);
        }


        private bool Cansubmit(object arg) { return true; }


        public static string? stringData = null;

        //Creat TCP/IP socket
        private Socket listener;
        private Socket handler;
        private TcpListener server;
        private TcpClient client;
        private NetworkStream ns;


        public byte[] byteData = new byte[1024];
        public void ServerStart(object obj)
        {
            //9999포트에 ip주소다 받기
            server = new TcpListener(IPAddress.Any, 9999);
            // Server 시작
            server.Start();
            ServerChart.Add(new MessageModel
            {
                Time = DateTime.Now,
                message = "Server Start"
            });

            ServerChart.Add(new MessageModel
            {
                Time = DateTime.Now,
                message = "Client wait..."
            });

            //클라이언트 객체를 만들어 9999에 연결한 client 받기
            //client가 접속할때까지 서버는 해당 구문에서 블락
            client = server.AcceptTcpClient();

            //client에서 받은 데이터를 받을 객체 생성
            ns = client.GetStream();


            ns.Read(byteData, 0, byteData.Length);

            string stringData = Encoding.Default.GetString(byteData);

            ServerChart.Add(new MessageModel
            {
                Time = DateTime.Now,
                message = stringData
            });




        }

        public void CloseServer(object obj)
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

        public void connectionListen(object sender)
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

        public Action CloseAction { get; set; }

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
