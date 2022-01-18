using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tcp
{
    //test Listener
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Console Message");

            TcpListener serverSocket = null;
            TcpClient clientSocket = null;
            try
            {
                serverSocket = new TcpListener(IPAddress.Any, 9999);
                clientSocket = default(TcpClient);

                serverSocket.Start();
                Console.WriteLine("Server Started");

                Thread threadhandler = new Thread(new ParameterizedThreadStart(OnAccepted));
                threadhandler.IsBackground = true;
                threadhandler.Start(clientSocket);

            }
            catch (SocketException SE)
            {
                Console.WriteLine("InitSocke : SocketException ",SE );
                throw;
            }
            catch (Exception E)
            {
                Console.WriteLine("InitSocke : Eexception ", E);
            }




        }

        private static void OnAccepted(object sender)
        {
            TcpClient clientsocket = sender as TcpClient;
            while (true)
            {
                try
                {
                    NetworkStream stream = clientsocket.GetStream();
                    byte[] buffer = new byte[1024];

                    stream.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.Unicode.GetString(buffer);
                    msg = msg.Substring(0, msg.IndexOf("$"));
                    Console.WriteLine(" >> Data from client - " + msg);

                    string response = "Last Message from client - " + msg;
                    byte[] sbuffer = Encoding.Unicode.GetBytes(response);

                    stream.Write(sbuffer, 0, sbuffer.Length);
                    stream.Flush();

                    Console.WriteLine(" >> " + response);


                }
                catch (SocketException se)
                {
                    Console.WriteLine("OnAccepted : SocketException : ", se.Message);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("OnAccepted : Exception : ", ex.Message);
                    break;
                }


            }
        }
    }
}
