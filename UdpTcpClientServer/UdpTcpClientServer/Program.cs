using System;
using UdpTcpClientServer.Udp;

namespace UdpTcpClientServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var server = new UdpServer(54000);
            server.Start();

            INetworkService client = new UdpNetworkClient("127.0.0.1",54000);
            var messageFromServer = client.SendAndReceiveMessage("This is test message!");

            Console.WriteLine(messageFromServer);

            client.Subscribe("subscribe");

            client.ReceiveOnSubscribed += (sender, eventArgs) =>
            {
                var message = eventArgs.Value;
                Console.WriteLine($"Message for on subscription: {message}");
            };


            Console.ReadKey();
        } 
    }
}