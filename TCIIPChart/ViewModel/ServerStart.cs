using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCIIPChart.ViewModel
{
    public class ServerStart
    {
        ClientManager _clientManager = new ClientManager();

        public ServerStart()
        {
            Task serverStart = Task.Run(() =>
            {
                ServerRun();
            });
        }

        private void ServerRun()
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 708));
            
            listener.Start();
            
            while (true)
            {
                Task<TcpClient> acceptTask = listener.AcceptTcpClientAsync();

                acceptTask.Wait();

                TcpClient client = acceptTask.Result;

                _clientManager.AddClient(client);

            }
        }
    }
}
