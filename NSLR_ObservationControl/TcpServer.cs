

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InternalProtocol
{
    public class TcpServer
    {
        private TcpListener listener;
        private Thread acceptThread;
        private Thread receiveThread;
        private bool isRunning;
        private StringBuilder logBuilder;

        public event EventHandler<TcpClientEventArgs> ClientConnected;
        public event EventHandler<TcpPacketEventArgs> PacketReceived;
        public event EventHandler<TcpClientEventArgs> ClientDisconnected; 

        public TcpServer(int PORT)
        {
            listener = new TcpListener(System.Net.IPAddress.Any, PORT);

            acceptThread = new Thread(AcceptThreadWorker);
            logBuilder = new StringBuilder();
           // Start();
        }

        public void Start()
        {
            listener.Start();
            acceptThread.Start();
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            acceptThread.Abort();
            receiveThread?.Abort();
        }

        public string GetLog()
        {
            lock (logBuilder)
            {
                string log = logBuilder.ToString();
                logBuilder.Clear();
                return log;
            }
        }
        private void AcceptThreadWorker()
        {
            while (isRunning)
            {
                TcpClient client1 = listener.AcceptTcpClient();
                OnClientConnected(client1);
                receiveThread = new Thread(() => ReceiveThreadWorker(client1));
                receiveThread.Start();

            }
        }


        public void ReceiveThreadWorker(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            while (isRunning)
            {
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        byte[] receivedData = new byte[bytesRead];
                        Array.Copy(buffer, receivedData, bytesRead);
                        OnPacketReceived(client, receivedData);
                    }
                    else
                    {

                        //OnClientDisconnected(client);
                    }
                }
                catch (Exception)
                {
                    OnClientDisconnected(client);
                    break;
                }
                
            }

            stream.Close();
            client.Close();
        }
        private void OnClientDisconnected(TcpClient client)
        {
            ClientDisconnected?.Invoke(this, new TcpClientEventArgs(client));
            AddLog($"Client disconnected: {client.Client.RemoteEndPoint}");
        }

        private void OnClientConnected(TcpClient client)
        {
            ClientConnected?.Invoke(this, new TcpClientEventArgs(client));
            AddLog($"Client connected: {client.Client.RemoteEndPoint}");
        }
        int currentCount;

        private void OnPacketReceived(TcpClient client, byte[] data)
        {
            PacketReceived?.Invoke(this, new TcpPacketEventArgs(client, data));
            AddLog($"Received from {client.Client.RemoteEndPoint}");
            
        }
        private void AddLog(string log)
        {
            lock (logBuilder)
            {
                logBuilder.AppendLine(log);
            }
        }

    }

    public class TcpClientEventArgs : EventArgs
    {
        public TcpClient Client { get; }

        public TcpClientEventArgs(TcpClient client)
        {
            Client = client;
        }
    }

    public class TcpPacketEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public byte[] Data { get; }

        public TcpPacketEventArgs(TcpClient client, byte[] data)
        {
            Client = client;
            Data = data;
        }
    }
}
