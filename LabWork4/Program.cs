using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proxy
{
    static class Program
    {
        static void Main()
        {
            try
            {
                TcpListener Candidate = new TcpListener(IPAddress.Parse("127.0.0.1"), 8009  ); //candidate to listen for
                                                                                             //and accept incoming connection requests in blocking synchronous mode.

                Candidate.Start(); //open connection to listen
                
                while (true)
                {
                    TcpClient Accepted = Candidate.AcceptTcpClient(); //provides simple methods for connecting, sending, and receiving stream data
                                                                      //over a network in synchronous blocking mode.

                    Task Listento = new Task(() => Listen(Accepted));
                    Listento.Start();
                }
            }
            catch (Exception excep)
            {
                Console.WriteLine(excep.Message);
            }
        }

        private static void Listen(TcpClient clientTCP)
        {
            NetworkStream BrowserNS = clientTCP.GetStream();  //to send and recieve Data
            byte[] Buffer = new byte[65536];

            while (BrowserNS.CanRead)                      // Gets a value that indicates
                                                           // whether the NetworkStream supports reading.
            {
                if (BrowserNS.DataAvailable)               //Gets a value that indicates
                                                           //whether data is available on the NetworkStream to be read.  
                {
                    try
                    {
                        int MessageLen = BrowserNS.Read(Buffer, 0, Buffer.Length);// Stream->Buffer | return number of read bytes

                        Request(Buffer, MessageLen, BrowserNS);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured: " + ex.Message);
                        return;
                    }
                }
            }
            clientTCP.Close();
        }

        private static byte[] PathIs(byte[] Data)
        {
            string Buf = Encoding.UTF8.GetString(Data);

            Regex Header_RegExp = new Regex(@"http:\/\/[a-z0-9а-яё\:\.]*");
            MatchCollection Headers = Header_RegExp.Matches(Buf);

            Buf = Buf.Replace(Headers[0].Value, "");

            Data = Encoding.UTF8.GetBytes(Buf);

            return Data;
        }

        private static void Request(byte[] Buffer, int Buf_Length, NetworkStream Browser)
        {
            try
            {
                char[] IFS = {'\r', '\n'};

                string[] buffer = Encoding.UTF8.GetString(Buffer).Trim().Split(IFS);

                string Host = buffer.FirstOrDefault(x => x.Contains("Host")); //Host:  8000

                if (Host != null)
                {
                    Host = Host.Substring(Host.IndexOf(":", StringComparison.Ordinal) + 2); // get Hostname and so on

                    string[] Info_Req = Host.Trim().Split(new char[] {':'}); //Get string from req data
                    string NameofHost = Info_Req[0]; //Host: NameofHost => [0]

                    var Sender = Info_Req.Length == 2 ? new TcpClient(NameofHost, int.Parse(Info_Req[1])) : new TcpClient(NameofHost, 80); //80-HTTP (HyperText Transfer Protocol) reply;

                    NetworkStream ServerNS = Sender.GetStream();

                    ServerNS.Write(PathIs(Buffer), 0, Buf_Length);

                    byte[] Reply = new byte[65536];
                    int Length = ServerNS.Read(Reply, 0, Reply.Length);

                    string[] Head = Encoding.UTF8.GetString(Reply).Split(IFS);
                    string StateCode = Head[0].Substring(Head[0].IndexOf(" ") + 1);

                    Console.WriteLine(Host + "  " + StateCode);
                    
                    Browser.Write(Reply, 0, Length);

                    ServerNS.CopyTo(Browser);

                    ServerNS.Close();
                }
            }
            catch
            {
                return;
            }
            finally
            {
                Browser.Close();
            }
        }

       
    }
}