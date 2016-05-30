using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpTcpClientServer.Udp
{
    public class UdpNetworkClient : INetworkService
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint _ipEndPoint;

        public UdpNetworkClient(string remoteIpAddress, int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(remoteIpAddress), port);

            _udpClient = new UdpClient();
            _udpClient.Connect(_ipEndPoint);
        }

        public event EventHandler<NetworkEventArgs<string>> ReceiveOnSubscribed;

        public string SendAndReceiveMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            _udpClient.SendAsync(bytes, bytes.Length);

            var receivedResult = _udpClient.Receive(ref _ipEndPoint);
            var receivedMessage = Encoding.UTF8.GetString(receivedResult, 0, receivedResult.Length);

            return receivedMessage;
        }

        public void Subscribe(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            _udpClient.SendAsync(bytes, bytes.Length);

            _udpClient.BeginReceive(OnBeginReceive, this);
        }

        private void OnBeginReceive(IAsyncResult ar)
        {
            var result = _udpClient.EndReceive(ar, ref _ipEndPoint);
            var resultMessage = Encoding.UTF8.GetString(result, 0, result.Length);

            ReceiveOnSubscribed?.Invoke(this, new NetworkEventArgs<string>(resultMessage));

            _udpClient.BeginReceive(OnBeginReceive, this);
        }
    }
}