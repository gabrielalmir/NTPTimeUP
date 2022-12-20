using System.Net.Sockets;

namespace NTPTimeUP.Views
{
    public class SocketPool
    {
        private readonly Stack<Socket> sockets = new();
        private readonly object syncRoot = new();

        public Socket GetSocket()
        {
            lock (syncRoot)
            {
                if (sockets.Count > 0)
                {
                    return sockets.Pop();
                }
                else
                {
                    return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                }
            }
        }

        public void ReleaseSocket(Socket socket)
        {
            lock (syncRoot)
            {
                sockets.Push(socket);
            }
        }
    }
}