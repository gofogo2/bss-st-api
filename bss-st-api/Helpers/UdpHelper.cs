using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_SE.Helpers
{
    public class UdpHelper
    {
        public string _server_host = "127.0.0.1";
        public string _server_broadcast = "255.255.255.255";
        public int _receiver_port = 2923;
        public int _sender_port = 5500;
        private bool disposed = false;
        private UdpClient sender;
        private UdpClient receiver;
        private static UdpHelper _instance { get; set; }
        public static UdpHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new UdpHelper());
            }
        }

        public delegate void PacketReceiveEventHandler(string code);

        public void Receive(object rECEIVER_PORT)
        {
            throw new NotImplementedException();
        }

        public event PacketReceiveEventHandler PacketReceiveEventHandlerEvent;

       public void Setting(string server_host, int receiver_port, int sender_port)
        {
            _server_host = server_host;
            _receiver_port = receiver_port;
            _sender_port = sender_port;
        }

        UdpHelper()
        {
            
        }

        public void Start()
        {
            Receive();
        }

        public void Send(string msg)
        {
            Send(msg, false);
        }


        public void Send(string msg, int port)
        {
            Send(msg, false, port);
        }



        public void Send(string msg, bool isBroadCast)
        {
            Send(msg, isBroadCast, _sender_port);
        }

        public async void Send(string msg, bool isBroadCast, int port)
        {
            try
            {

                sender = new UdpClient(isBroadCast ? _server_broadcast : _server_host, port);
                byte[] msgToByte = ByteConverter.StringToByte(msg);
                _ = await sender.SendAsync(msgToByte, msgToByte.Length);
                sender.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void SendWithIP(string msg, string ip, int port)
        {
            try
            {
                sender = new UdpClient(ip, port);
                byte[] msgToByte = ByteConverter.StringToByte(msg);
                _ = await sender.SendAsync(msgToByte, msgToByte.Length);
                sender.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void SendByte(byte[] msg)
        {
            SendByte(msg, _sender_port);
        }

        public void SendObject(object msg)
        {
            SendObject(msg, _sender_port);
        }

        public void SendObject(object msg, int port)
        {
            var bytes = ByteConverter.ObjectToByte(msg);
            SendByte(bytes, false, port);
        }

        public void SendByte(byte[] msg, int port)
        {
            SendByte(msg, false, port);
        }

        public async void SendByte(byte[] msg, bool isBroadCast, int port)
        {
            try
            {
                sender = new UdpClient(isBroadCast ? _server_broadcast : _server_host, port);
                byte[] msgToByte = msg;
                _ = await sender.SendAsync(msgToByte, msgToByte.Length);
                sender.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void Receive()
        {
            Receive(_receiver_port);
        }

        public async void Receive(int Port)
        {
            try
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                receiver = new UdpClient { Client = socket };
                receiver.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
                var receive = await receiver.ReceiveAsync();

                var msg = ByteConverter.ByteToString(receive.Buffer);
                PacketReceiveEventHandlerEvent?.Invoke(msg);

                receiver.Close();
                await Task.Delay(100);  // Wait for 100 milliseconds.

                Receive(Port);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (receiver != null)
                    {
                        receiver.Close();
                        receiver = null;
                    }

                    if (sender != null)
                    {
                        sender.Close();
                        sender = null;
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
