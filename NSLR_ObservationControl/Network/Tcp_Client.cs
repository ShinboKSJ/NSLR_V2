using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl.Network
{
    public class Tcp_Client
    {
        Socket mainSock;
        //public event deleLogger Log;
        public delegate void OnConnectedEventHandler(bool value);
        public event OnConnectedEventHandler OnConnectedEvent;


        public void Connect(string address, int m_port)
        {
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverAddr = IPAddress.Parse(address);
            IPEndPoint clientEP = new IPEndPoint(serverAddr, m_port);
            IAsyncResult result = mainSock.BeginConnect(clientEP, new AsyncCallback(ConnectCallback), mainSock);
        }
        public void Close()
        {
            if (mainSock != null)
            {
                mainSock.Close();
                mainSock.Dispose();
            }
        }
        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[(long)BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }
        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                AsyncObject obj = new AsyncObject(4096);
                obj.WorkingSocket = mainSock;
                mainSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
                //Log(LOG.I, "[TcpClient]", $"ConnectCallback");
                OnConnectedEvent?.Invoke(true);
            }
            catch (Exception e)
            {
                OnConnectedEvent?.Invoke(false);
            }
        }
        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            int received = obj.WorkingSocket.EndReceive(ar);
            byte[] buffer = new byte[received];
            Array.Copy(obj.Buffer, 0, buffer, 0, received);
            //Log(LOG.I, "[TcpClient]", $"DataReceived : [{received}] {string.Join(" ", buffer)}");
        }
        public void Send(byte[] msg)
        {
            mainSock.Send(msg);
            //Log(LOG.I, "[TcpClient]", $"DataReceived : [{msg.Length}] {string.Join(" ", msg)}");
        }
        public void Send(string msg)
        {
            mainSock.Send(Encoding.Default.GetBytes(msg));
        }
    }

}
