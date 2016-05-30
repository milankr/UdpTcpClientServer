using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpTcpClientServer.Udp
{
    public class UdpServer
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint _ipEndPoint;

        public UdpServer(int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            _udpClient = new UdpClient(_ipEndPoint);
        }

        public void Start()
        {
            _udpClient.BeginReceive(RequestCallback, this);
        }

        private void RequestCallback(IAsyncResult ar)
        {
            var result = _udpClient.EndReceive(ar, ref _ipEndPoint);
            var resultMessage = Encoding.UTF8.GetString(result, 0, result.Length);

            if (resultMessage == "subscribe")
            {
                for (var i = 0; i < 10; i++)
                {
                    var messageSubscribed = "Subscribed!";
                    var message = Encoding.UTF8.GetBytes(messageSubscribed);
                    _udpClient.Send(message, message.Length, _ipEndPoint);
                }
            }
            else
            {
                var messageReceived = "Message received!";
                var message = Encoding.UTF8.GetBytes(messageReceived);

                _udpClient.Send(message, message.Length, _ipEndPoint);
            }

            _udpClient.BeginReceive(RequestCallback, this);
        }
    }
}