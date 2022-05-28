using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Chat
{
    public partial class Chat : Form
    {
        private string _login;
        private string _userIPaddress;

        private IPAddress IPaddress;
        private bool _exist = true;

        private const int UDPPort = 7500;
        private const int TCPPort = 7501;

        private readonly IPAddress broadcastAd = IPAddress.Broadcast;

        private static Task receiveUDPTh;
        private static Task receiveTCPTh;

        private readonly ChatMaintanance _chatMaintain = new ChatMaintanance();
        public Chat()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            using (Authorization AuthForm = new Authorization())
            {
                AuthForm.ShowDialog();

                if (AuthForm.UserName == "")
                    Close();
                else
                {
                    _login = AuthForm.UserName;
                    _userIPaddress = AuthForm.UserIP; 

                    string  MessageLogin = _login;
                    string MessageAddress = _userIPaddress;

                    if (IPAddress.TryParse(MessageAddress, out var adr)) //try to put address->messageIp
                    {
                        IPaddress = adr;
                    }

                    _exist = true;
                    UDPSend("0" + _login);

                    receiveUDPTh = new Task(UDPReceive);
                    receiveUDPTh.Start();
                    
                    tbChatWindow.Text = $"{DateTime.Now.ToShortTimeString()} |  {_login} (You) has just entered the chat\n" + tbChatWindow.Text;
                    receiveTCPTh = new Task(ReceiveTCP);
                    receiveTCPTh.Start();
                    Show();
                }
            }
                
        }
        
        private void UDPSend(string message) //Send broadcast message
        {
            UdpClient sender = new UdpClient(new IPEndPoint(IPaddress, UDPPort));
            try
            {
                byte[] messagedata = Encoding.UTF8.GetBytes(message);
                sender.Send(messagedata, messagedata.Length, broadcastAd.ToString(), UDPPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }
        
        private void UDPReceive()
        {
            IPEndPoint receiverIp = new IPEndPoint(IPAddress.Any, UDPPort);
            UdpClient receiver = new UdpClient(new IPEndPoint(IPaddress, UDPPort));
            while (_exist)
            {
                byte[] messagedata = receiver.Receive(ref receiverIp);
                string message = Encoding.UTF8.GetString(messagedata);

                string toPrint = _chatMaintain.NewChecker(message);

                User newUser = new User(message.Substring(1), receiverIp); // get clear login and create user
                newUser.EstablishConnection();
                _chatMaintain.UsersList.Add(newUser);
                newUser.SendMessage("0" + _login);
                this.Invoke(new MethodInvoker(() =>
                {
                    string time = DateTime.Now.ToShortTimeString();
                    tbChatWindow.Text = $"{time} [{receiverIp.Address}] {toPrint}\n {tbChatWindow.Text}";
                }));
                Task.Factory.StartNew(() => ListenClient(newUser));
            }

            receiver.Close();
            receiver.Dispose();
        }

        private void SendMessageToAllClients(string tcpMessage)
        {
            foreach (var user in _chatMaintain.UsersList)
            {
              
                try
                {
                    user.SendMessage(tcpMessage);
                }
                catch
                {
                    MessageBox.Show($"Could not send message to {user.Name}.");
                }
            }

            if (tcpMessage[0] == '2')
            {
                this.Invoke(new MethodInvoker(() =>
                {          
                    tbChatWindow.Text = $"{DateTime.Now.ToShortTimeString()} | You: {tcpMessage.Substring(1)}\n" + tbChatWindow.Text;
                }));
            }
        }

        private void ReceiveTCP()
        {
            TcpListener tcpListener = new TcpListener(IPaddress, TCPPort);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpNewClient = tcpListener.AcceptTcpClient();
                User newUser = new User(tcpNewClient, TCPPort);

                Task.Factory.StartNew(() => ListenClient(newUser));
            }
        }

        private void ListenClient(User client)
        {
            while (_exist)
            {
                string tcpMessage = client.ReceiveMessage();
                switch (tcpMessage[0])
                {
                    case '0': //fist message
                    {
                        client.Name = tcpMessage.Substring(1);
                        _chatMaintain.UsersList.Add(client);
                        break;
                    }
                    case '1': // last message
                        this.Invoke(new MethodInvoker(() =>
                        {
                            tbChatWindow.Text =
                                $"{DateTime.Now.ToShortTimeString()} | {client.Name} [{client.IP}] has left the chat\n" +
                                tbChatWindow.Text;
                        }));
                        _chatMaintain.UsersList.Remove(client);
                        return;

                    case '2': // ordinary message
                        this.Invoke(new MethodInvoker(() =>
                        {
                            tbChatWindow.Text =
                                $"{DateTime.Now.ToShortTimeString()} |  {client.Name} [{client.IP}]: {tcpMessage.Substring(1)}\n" +
                                tbChatWindow.Text;
                        }));
                        break;
                }
            }
        }

        private void SendMessage()
        {
            SendMessageToAllClients("2" + txtToSend.Text);
            txtToSend.Text = "";
        }

        public void SafeSender() {
            if (txtToSend.Text == "")
            {
                MessageBox.Show("Try to write something first");
            }
            else
            {
                SendMessage();
            }
        }
        private void txtToSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SafeSender();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _exist = false;
            SendMessageToAllClients("1");
            if (_chatMaintain.UsersList.Count != 0)
            {
                foreach (var user in _chatMaintain.UsersList)
                {
                    user.Disconnect();
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SafeSender();
        }
        
    }
}