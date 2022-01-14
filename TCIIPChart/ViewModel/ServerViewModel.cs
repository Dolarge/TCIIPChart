using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using TCIIPChart.Model;

namespace TCIIPChart.ViewModel
{

    public interface IViewModel { }

    public partial class ServerViewModel : BindableBase, IViewModel
    {

        ClientManager _clientManager = new ClientManager();
        public ObservableCollection<string> AccessLogList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ClientChart { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> userList { get; set; } = new ObservableCollection<string>();

        private bool Cansubmit(object arg) { return true; }
        private object lockObj = new object();
        Task conntectCheckThread = null;

        public ServerViewModel()
        {


            MainServerStart();
            ClientManager.messageParsingAction += messageParsing;
            ClientManager.ChangeListViewAction += ChangeListView;
            conntectCheckThread = new Task(ConnectCheckLoop);
            conntectCheckThread.Start();
            //test
        }
        private void ConnectCheckLoop()
        {
            while (true)
            {
                foreach (var item in ClientManager.clientDic)
                {
                    try
                    {
                        string sendStringData = "관리자<TEST>";
                        byte[] sendByteData = new byte[sendStringData.Length];
                        sendByteData = Encoding.Default.GetBytes(sendStringData);

                        item.Value.tcpClient.GetStream().Write(sendByteData, 0, sendByteData.Length);
                    }
                    catch (Exception e)
                    {
                        RemoveClient(item.Value);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void RemoveClient(ClientData targetClient)
        {
            ClientData result = null;
            ClientManager.clientDic.TryRemove(targetClient.clientNumber, out result);
            string leaveLog = string.Format("[{0}] {1} Leave Server", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), result.clientName);
            ChangeListView(leaveLog, StaticDefine.ADD_ACCESS_LIST);
            ChangeListView(result.clientName, StaticDefine.REMOVE_USER_LIST);
        }

        public void messageParsing(string sender, string message)
        {
            lock (lockObj)
            {
                List<string> msgList = new List<string>();
                string[] msgArray = message.Split('>');
                foreach (var item in msgArray)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    msgList.Add(item);
                }
                SendMsgToClient(msgList, sender);

            }
        }

        public void SendMsgToClient(List<string> msgList, string sender)
        {
            string parsedMessage = "";
            string receiver = "";

            int senderNumber = -1;
            int receiverNumber = -1;

            foreach (var item in msgList)
            {
                string[] splitedMsg = item.Split('<');

                receiver = splitedMsg[0];
                parsedMessage = string.Format("{0}<{1}>", sender, splitedMsg[1]);

                if (parsedMessage.Contains("<GroupChattingStart>"))
                {
                    string[] groupSplit = receiver.Split('#');

                    foreach (var el in groupSplit)
                    {
                        if (string.IsNullOrEmpty(el))
                            continue;
                        string groupLogMessage = string.Format(@"[{0}] [{1}] -> [{2}] , {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), groupSplit[0], el, splitedMsg[1]);
                        ChangeListView(groupLogMessage, StaticDefine.ADD_CHATTING_LIST);

                        receiverNumber = GetClinetNumber(el);

                        parsedMessage = string.Format("{0}<GroupChattingStart>", receiver);
                        byte[] sendGroupByteData = Encoding.Default.GetBytes(parsedMessage);
                        ClientManager.clientDic[receiverNumber].tcpClient.GetStream().Write(sendGroupByteData, 0, sendGroupByteData.Length);
                    }
                    return;
                }

                if (receiver.Contains('#'))
                {
                    string[] groupSplit = receiver.Split('#');

                    foreach (var el in groupSplit)
                    {
                        if (string.IsNullOrEmpty(el))
                            continue;
                        if (el == groupSplit[0])
                            continue;
                        string groupLogMessage = string.Format(@"[{0}] [{1}] -> [{2}] , {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), groupSplit[0], el, splitedMsg[1]);
                        ChangeListView(groupLogMessage, StaticDefine.ADD_CHATTING_LIST);

                        receiverNumber = GetClinetNumber(el);

                        parsedMessage = string.Format("{0}<{1}>", receiver, splitedMsg[1]);
                        byte[] sendGroupByteData = Encoding.Default.GetBytes(parsedMessage);
                        ClientManager.clientDic[receiverNumber].tcpClient.GetStream().Write(sendGroupByteData, 0, sendGroupByteData.Length);
                    }
                    return;
                }


                senderNumber = GetClinetNumber(sender);
                receiverNumber = GetClinetNumber(receiver);


                if (senderNumber == -1 || receiverNumber == -1)
                {
                    return;
                }

                byte[] sendByteData = new byte[parsedMessage.Length];
                sendByteData = Encoding.Default.GetBytes(parsedMessage);

                if (parsedMessage.Contains("<GiveMeUserList>"))
                {
                    string userListStringData = "관리자<";
                    foreach (var el in userList)
                    {
                        userListStringData += string.Format("${0}", el);
                    }
                    userListStringData += ">";
                    byte[] userListByteData = new byte[userListStringData.Length];
                    userListByteData = Encoding.Default.GetBytes(userListStringData);
                    ClientManager.clientDic[receiverNumber].tcpClient.GetStream().Write(userListByteData, 0, userListByteData.Length);
                    return;
                }




                string logMessage = string.Format(@"[{0}] [{1}] -> [{2}] , {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sender, receiver, splitedMsg[1]);
                ChangeListView(logMessage, StaticDefine.ADD_CHATTING_LIST);

                if (parsedMessage.Contains("<ChattingStart>"))
                {
                    parsedMessage = string.Format("{0}<ChattingStart>", receiver);
                    sendByteData = Encoding.Default.GetBytes(parsedMessage);
                    ClientManager.clientDic[senderNumber].tcpClient.GetStream().Write(sendByteData, 0, sendByteData.Length);

                    parsedMessage = string.Format("{0}<ChattingStart>", sender);
                    sendByteData = Encoding.Default.GetBytes(parsedMessage);
                    ClientManager.clientDic[receiverNumber].tcpClient.GetStream().Write(sendByteData, 0, sendByteData.Length);

                    return;
                }



                if (parsedMessage.Contains(""))

                    ClientManager.clientDic[receiverNumber].tcpClient.GetStream().Write(sendByteData, 0, sendByteData.Length);
            }
        }
        public void ChangeListView(string Message, int key)
        {
            switch (key)
            {
                case StaticDefine.ADD_ACCESS_LIST:
                    {
                       Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            AccessLogList.Add(Message);
                        }));
                        break;
                    }
                case StaticDefine.ADD_CHATTING_LIST:
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            ClientChart.Add(Message);
                        }));
                        break;
                    }
                case StaticDefine.ADD_USER_LIST:
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            userList.Add(Message);
                        }));
                        break;
                    }
                case StaticDefine.REMOVE_USER_LIST:
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            userList.Remove(Message);
                        }));
                        break;
                    }
                default:
                    break;
            }
        }

        private int GetClinetNumber(string targetClientName)
        {
            foreach (var item in ClientManager.clientDic)
            {
                if (item.Value.clientName == targetClientName)
                {
                    return item.Value.clientNumber;
                }
            }
            return -1;
        }

        private void MainServerStart()
        {
            ServerStart serverStart = new ServerStart();
        }

        public ICommand ConnectBtnClick { get; private set; }



    }
}
