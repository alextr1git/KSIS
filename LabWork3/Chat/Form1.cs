using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Chat
{
    public partial class Form1 : Form
    {
        private string _username; //CH
        private string _userIP; //CH
        private IPAddress Ip; //CH
        private bool _alive = true;
        private const int UDPPort = 7500;
        private const int TCPPort = 7501;
        private readonly IPAddress broadcastAddress = IPAddress.Broadcast;

        private static Task receiveThreadUDP;
        private static Task receiveThreadTCP;

        private readonly SetChat _setChat = new SetChat();
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            using (LoginForm loginForm = new LoginForm())
            {
                loginForm.ShowDialog();

                if (loginForm.UserName == "")
                    Close();
                else
                {
                    _username = loginForm.UserName;
                    _userIP = loginForm.UserIP; 

                    string  MessageUsername = _username;
                    string MessageIp = _userIP;
                    if (IPAddress.TryParse(MessageIp, out var address)) //try to put address->messageIp
                    {
                        Ip = address;
                    }

                    _alive = true;
                    SendMessageUDP("0" + _username);
                    
                    receiveThreadUDP = new Task(ReceiveMessageUDP);
                    receiveThreadUDP.Start();
                    
                    txtChat.Text = $"{DateTime.Now.ToShortTimeString()} : {_username} [{Ip}] (you)  has just entered the chat\n" + txtChat.Text;
                    receiveThreadTCP = new Task(ReceiveTCP);
                    receiveThreadTCP.Start();
                    Show();
                }
            }
                
        }
        
        private void SendMessageUDP(string message) //Send broadcast message
        {
            UdpClient sender = new UdpClient(new IPEndPoint(Ip, UDPPort));
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                sender.Send(data, data.Length, broadcastAddress.ToString(), UDPPort);
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
        
        private void ReceiveMessageUDP()
        {
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, UDPPort);
            UdpClient receiver = new UdpClient(new IPEndPoint(Ip, UDPPort));
            while (_alive)
            {
                byte[] data = receiver.Receive(ref remoteIp);
                string message = Encoding.UTF8.GetString(data);

                string toPrint = _setChat.WhatIsThis(message); // check wherther it's first,last or ordinary message

                User newUser = new User(message.Substring(1), remoteIp); // get clear login and create user
                newUser.EstablishConnection();
                _setChat.UserList.Add(newUser);
                newUser.SendMessage("0" + _username);
                this.Invoke(new MethodInvoker(() =>
                {
                    string time = DateTime.Now.ToShortTimeString();
                    txtChat.Text = $"{time} [{remoteIp.Address}] {toPrint}\n {txtChat.Text}";
                }));
                Task.Factory.StartNew(() => ListenClient(newUser));
            }

            receiver.Close();
            receiver.Dispose();
        }

        private void SendMessageToAllClients(string tcpMessage)
        {
            foreach (var user in _setChat.UserList)
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
                    txtChat.Text = $"{DateTime.Now.ToShortTimeString()} : {_username} [{Ip}] (you):  {tcpMessage.Substring(1)}\n" + txtChat.Text;
                }));
            }
        }

        private void ReceiveTCP()
        {
            TcpListener tcpListener = new TcpListener(Ip, TCPPort);
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
            while (_alive)
            {
                string tcpMessage = client.ReceiveMessage();
                switch (tcpMessage[0])
                {
                    case '0': //fist message
                    {
                        client.Name = tcpMessage.Substring(1);
                        _setChat.UserList.Add(client);
                        break;
                    }
                    case '1': // last message
                        this.Invoke(new MethodInvoker(() =>
                        {
                            txtChat.Text =
                                $"{DateTime.Now.ToShortTimeString()} :  {client.Name} [{client.IP}] has left the chat\n" +
                                txtChat.Text;
                        }));
                        _setChat.UserList.Remove(client);
                        return;

                    case '2': // ordinary message
                        this.Invoke(new MethodInvoker(() =>
                        {
                            txtChat.Text =
                                $"{DateTime.Now.ToShortTimeString()} :  {client.Name} [{client.IP}]: {tcpMessage.Substring(1)}\n" +
                                txtChat.Text;
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
            _alive = false;
            SendMessageToAllClients("1");
            if (_setChat.UserList.Count != 0)
            {
                foreach (var user in _setChat.UserList)
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