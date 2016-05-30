using System;

namespace UdpTcpClientServer
{
    public class NetworkEventArgs<T> : EventArgs
    {
        public NetworkEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    public interface INetworkService
    {
        string SendAndReceiveMessage(string message);

        void Subscribe(string message);

        event EventHandler<NetworkEventArgs<string>> ReceiveOnSubscribed;

    }
}