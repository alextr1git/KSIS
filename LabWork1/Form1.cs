using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int z = 0;
            String remH = textBox1.Text;
            richTextBox1.Text += "Трассировка маршрута к " + remH + "\nc максимальным число прыжков 30: \n\n";
            byte[] data = new byte[1024];
            int recv = 0;
            Socket host = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp); //ipv4/raw=icmp/icmp
            IPHostEntry iphe = Dns.GetHostEntry(remH); // использует DNS-сервер для получения деталей хоста по заданному его имени
            IPEndPoint iep = new IPEndPoint(iphe.AddressList[0], 0);
            EndPoint ep = (EndPoint)iep;
            ICMP packet = new ICMP();

            UInt16 chcksum = packet.CalcCheckSum();
            packet.CheckSum = chcksum;

            host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 300); // Socket options apply to all sockets/Receive a time-out./300ms


            for (int i = 1; i < 30; i++)
            {if (z == 1)
                    break;
                host.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, i); //Socket options apply only to IP sockets./Set the IP header Time-to-Live field. by value of i
                int badcount = 0; //counter of unsuccessful atempts
                for (int count = 1; count <= 3; count++)
                {
                    packet.SeqNum += 1;
                    packet.UpdateCheckSum();
                    DateTime timestart = DateTime.Now;
                    host.SendTo(packet.CreateByteArray(), SocketFlags.None, iep); //Sends data to a specific endpoint. : A span of bytes that contains the data to be sent./A bitwise combination of the SocketFlags values(Use no flags for this call.)/ ep

                    try
                    {
                        data = new byte[1024];
                        recv = host.ReceiveFrom(data, ref ep);
                        TimeSpan timestop = DateTime.Now - timestart;
                        ICMP response = new ICMP(data, recv);

                        if (response.Type == 11) //icmp response type 11 - Time Exceeded
                        {
                           
                            switch (count) {

                            case 1:
                                if (i < 10) {
                                        if (timestop.Milliseconds < 10)
                                             richTextBox1.Text += " " + i + ".     " + (timestop.Milliseconds.ToString()) + "мс";
                                        else
                                            richTextBox1.Text += " " + i + ".    " + (timestop.Milliseconds.ToString()) + "мс";
                                }
                                else {
                                        if (timestop.Milliseconds < 10)
                                            richTextBox1.Text += " " + i + ".   " + (timestop.Milliseconds.ToString()) + "мс";
                                        else
                                            richTextBox1.Text += " " + i + ".  " + (timestop.Milliseconds.ToString()) + "мс";
                                }
                                    break;
                            case 2:
                             richTextBox1.Text += "   " + (timestop.Milliseconds.ToString()) + "мс";
                                break;
                            case 3:
                             richTextBox1.Text += "   " + (timestop.Milliseconds.ToString()) + "мс" + "   " + ep + "\n";
                                break;
                            }

                           
                        }

                        if (response.Type == 0)
                        {
                           richTextBox1.Text += "\n" + " Трассировка завершена\n";
                           richTextBox1.Text += " " + ep.ToString() + " достигнут за " + i + " прыжков, " + (timestop.Milliseconds.ToString()) + "мс\n"; // 0 - Echo Reply
                            z = 1;
                            break;
                        }

                        badcount = 0;

                    }
                    catch (SocketException)
                    {
                        richTextBox1.Text += " " + i + ": нет ответа от " + ep + " (" + iep + ") - " + Convert.ToString(host.Ttl) + "\n";
                        badcount++;

                        if (badcount == 3) // if 3 times no reply
                        {
                            richTextBox1.Text += "Cancelled \n";
                            break;
                        }
                    }

                }
                
            }
            host.Close();
            textBox1.Text = "";
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            radioButton1.Checked = false;
        }
    }

    class ICMP
    {
        private byte[] _header;

        private byte[] _message;

        public byte Type // 1 byte
        {
            get { return _header[0]; }
            set { _header[0] = value; }
        }

        public byte Code // 1 byte
        {
            get { return _header[1]; }
            set { _header[1] = value; }
        }

        public byte[] Message // 1 byte
        {
            get { return _message; }
            set { _message = value; }
        }
        public ushort CheckSum // 2 bytess
        {
            get { return BitConverter.ToUInt16(_header, 2); }
            set { BitConverter.GetBytes(value).CopyTo(_header, 2); }
        }

        public ushort Idnum // 2 bytes
        {
            get { return BitConverter.ToUInt16(_header, 4); }
            set { BitConverter.GetBytes(value).CopyTo(_header, 4); }
        }

        public ushort SeqNum // 2 bytes
        {
            get { return BitConverter.ToUInt16(_header, 6); }
            set { BitConverter.GetBytes(value).CopyTo(_header, 6); } // returns a specified 16-bit uint value as an array of bytes/copy it to header start with p6
        }

       


        public ICMP(byte[] frame, int totalLen)
        {
            _header = new byte[8];
            Buffer.BlockCopy(frame, 20, _header, 0, 8); //Copies a specified number of bytes from a source array starting at a particular offset to a destination array starting at a particular offset.

            _message = new byte[totalLen - 20 - 8];
            Buffer.BlockCopy(frame, 28, _message, 0, _message.Length);
        }

        public ICMP()
        {
            _header = new byte[8];
            Type = 8;
            Code = 0;
            Idnum = 111;
            _message = Encoding.Unicode.GetBytes("ABCD");
            SeqNum = 0;

            UpdateCheckSum();
        }


        public byte[] CreateByteArray()
        {
            byte[] res = new byte[_header.Length + _message.Length];

            _header.CopyTo(res, 0);
            _message.CopyTo(res, _header.Length);

            return res;
        }

        public ushort CalcCheckSum()
        {
            BitConverter.GetBytes(0).CopyTo(_header, 2);

            
            uint res = 0;

            for (int i = 0; i < _header.Length; i += 2)
                res += Convert.ToUInt32(BitConverter.ToUInt16(_header, i));

            for (int i = 0; i < _message.Length; i += 2)
                res += Convert.ToUInt32(BitConverter.ToUInt16(_message, i));

            res = (res >> 16) + (res & 0xffff);
            res += (res >> 16);

            return (ushort)(~res);

        }

        public void UpdateCheckSum()
        {
            ushort sum = CalcCheckSum();
            BitConverter.GetBytes(sum).CopyTo(_header, 2);
        }
    }
}