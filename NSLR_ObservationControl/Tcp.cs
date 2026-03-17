using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NSLR_ObservationControl.Module.GroundCalibration_Info;
using static NSLR_ObservationControl.ControlCommand;

namespace NSLR_ObservationControl
{
    internal class Tcp
    {
        const string THAT = "TcpServer";

        /* Socket Component */
        Socket TCP_Server;
        Socket TCP_Client;

        EndPoint TCP_Endpoint;
        IPAddress TCP_IPAddress;
        const string sTCP_IPAddress = "0,0,0,0";
        const int TCP_Port = 1111;

        Thread TCP_Recv_Thread;

        const int MAX_BUFFER_SIZE = 1024 * 1024;
        byte[] TCP_recvBuffer;

        public delegate void OnPacketReceivedEventHandler(byte[] recvData, byte[] STX, byte[] msgID, byte[] dataCNT, byte[] data, byte[] ETX);
        public event OnPacketReceivedEventHandler OnPacketReceivedEvent;

        public delegate void OnConnectedEventHandler(bool value);
        public event OnConnectedEventHandler OnConnectedEvent;

        private bool bDisposed = false;

        //bool Connected = false;

        #region 생성자/소멸자
        public Tcp(string sIP, int iPort)
        {
            // Parse IPAddress
            if (sIP != null && sIP != "")
                TCP_IPAddress = IPAddress.Parse(sIP);
            else
                TCP_IPAddress = IPAddress.Parse(sTCP_IPAddress);

            //// Create Endpoint
            if (iPort != 0)
                TCP_Endpoint = new IPEndPoint(TCP_IPAddress, iPort);
            else
                TCP_Endpoint = new IPEndPoint(TCP_IPAddress, TCP_Port);
            Initialize();
        }


        public Tcp(int iPort)
        {
            /* initial socket component */

            // Parse IPAddress
            string MyIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(); // 현재 접속한 PC - IP 프로토콜 할당

            IPAddress ipAddress = IPAddress.Parse(sTCP_IPAddress); // IP 주소프로토콜에 접근
            //// Create Endpoint
            if (iPort != 0)
                TCP_Endpoint = new IPEndPoint(ipAddress, iPort);
            else
                TCP_Endpoint = new IPEndPoint(TCP_IPAddress, TCP_Port);
            Initialize();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool bManage)
        {
            if (!bDisposed)
            {
                bDisposed = true;

                if (bManage)
                {
                }

                if (TCP_Recv_Thread != null && TCP_Recv_Thread.IsAlive)
                    TCP_Recv_Thread.Abort();

                if (TCP_Server != null)
                {
                    if (TCP_Server.Connected)
                        TCP_Server.Disconnect(false);

                    TCP_Server.Close();
                }
                if (TCP_Client != null)
                {
                    if (TCP_Client.Connected)
                        TCP_Client.Disconnect(false);

                    TCP_Client.Close();
                }
                OnConnectedEvent?.Invoke(false);
            }
        }

        ~Tcp()
        {
            Dispose(false);
        }
        #endregion

        // 초기화 및 서버 작동 개시
        private void Initialize()
        {
            #region Socket Creation
            try
            {
                // Create Socket
                TCP_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Set Socket Option
                TCP_Server.ReceiveBufferSize = 0x80000;
                BackgroundWorker TcpSocketBind = new BackgroundWorker();
                TcpSocketBind.DoWork += TcpSocketBind_DoWork;
                TcpSocketBind.RunWorkerCompleted += TcpSocketBind_RunWorkerCompleted;
                TcpSocketBind.RunWorkerAsync();
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            #endregion
        }

        private void TcpSocketBind_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    TCP_Server.Bind(TCP_Endpoint);

                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }

        private void TcpSocketBind_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TCP_Server.Listen(1);

            // Wait for Client Connect
            TCP_Server.BeginAccept(new AsyncCallback(AcceptConnect), TCP_Server);
        }

        // 클라이언트 접속시 수신 쓰레드 시작
        private void AcceptConnect(IAsyncResult iar)
        {
            Socket server = (Socket)iar.AsyncState;

            try
            {
                // Accept Connection
                TCP_Client = server.EndAccept(iar);
                TCP_Client.ReceiveTimeout = 20000; // Set receive timeout to 10 Seconds

                // Create TCP recv thread
                TCP_Recv_Thread = new Thread(new ThreadStart(TCP_Receive_Process));

                // Start TCP recv thread
                TCP_Recv_Thread.Start();

                OnConnectedEvent?.Invoke(true);
                //Connected = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        // TCP recv process
        private void TCP_Receive_Process()
        {
            int recvLength = 0;

            while (true)
            {
                TCP_recvBuffer = new byte[MAX_BUFFER_SIZE];
                try
                {
                    // recv from SMCU TCP client
                    recvLength = TCP_Client.Receive(TCP_recvBuffer);
                    byte[] recvData = new byte[recvLength];
                    Array.Copy(TCP_recvBuffer, 0, recvData, 0, recvLength);

                    // Socket error
                    if (recvLength == 0)
                    {
                        //Close TCP Client
                        OnConnectedEvent?.Invoke(false);
                        TCP_Client.Close(1);
                        break;
                    }
                    else
                    {
                        CheckPacket(recvData, recvLength);
                    }
                }
                catch (SocketException ex)
                {
                    //Debug.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.ToString());
                    // Close TCP Client
                    OnConnectedEvent?.Invoke(false);
                    TCP_Client.Close(1);
                    break;
                }
            }

            // Wait for new client Connet
            TCP_Server.BeginAccept(new AsyncCallback(AcceptConnect), TCP_Server);
        }

        private void CheckPacket(byte[] recvData, int recvLength)
        {
            byte[] SOM = new byte[4];
            byte[] msgID = new byte[4];
            byte[] data = new byte[6];
            byte[] dataCNT = new byte[4];
            //string data_hex = BitConverter.ToString(recvData).Replace("-", " "/*string.Empty*/);
            //Log(LOG.D, THAT, $"[{recvLength}]  {data_hex}");
            byte[] EOM = new byte[4];
            try
            {
                Array.Copy(recvData, 0, SOM, 0, 4);
                Array.Copy(recvData, recvLength - 4, EOM, 0, 4); //recvData[16~19]                
                if ((BitConverter.ToString(SOM)).Replace("-", string.Empty) == MSG_SOM && (BitConverter.ToString(EOM)).Replace("-", string.Empty) == MSG_EOM)
                {
                    Array.Copy(recvData, 4, msgID, 0, 4);
                    Array.Copy(recvData, 8, dataCNT, 0, 4);
                    Array.Copy(recvData, 12, data, 0, dataCNT[3]);
                    //Log(LOG.D, THAT, $"dataCNT [{dataCNT[3]}]");//
                    OnPacketReceivedEvent?.Invoke(recvData, SOM, msgID, dataCNT, data, EOM);
                }
            }
            catch (Exception e) //크로스 스레드
            {
                Debug.WriteLine(e.Message);
            }
        }
        public string InsertCharAtDividedPosition(string str, int count, string character)
        {
            var i = 0;
            while (++i * count + (i - 1) < str.Length)
            {
                str = str.Insert((i * count + (i - 1)), character);
            }
            return str;
        }
        public void SendData(string sendData)
        {
            byte[] xbytes = new byte[sendData.Length / 2];
            for (int i = 0; i < xbytes.Length; i++)
            {
                xbytes[i] = Convert.ToByte(sendData.Substring(i * 2, 2), 16);
            }

            if (TCP_Client != null)
            {
                TCP_Client.Send(xbytes, 0, xbytes.Length, SocketFlags.None);
                //Log(LOG.D, THAT, " \n\r");
                var readablity = BitConverter.ToString(xbytes).Replace("-", "");

                readablity = readablity.Insert(8, "   ");
                readablity = readablity.Insert(18, "         ");
                readablity = readablity.Insert(34, "          ");
                readablity = readablity.Insert(54, "             ");
                //var pattern = @"(\d{ 4})(\d{ 4})(\d{ 4})(\d{ 4})";
                //var placed = Regex.Replace(readablity, pattern, "$1 -$2 -$3 -$4");
                //Log(LOG.D, THAT, " \n\r");
            }
        }
    }
}
