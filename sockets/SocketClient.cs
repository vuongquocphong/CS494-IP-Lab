using System.Net;
using System.Net.Sockets;

Console.WriteLine("Press any key to exit...");

namespace Sockets
{
    public class SocketClient : SocketBase
    {
        private readonly Socket clientSocket;

        public SocketClient(IPAddress iPAddress, int port, MessageHandler messageHandler, CloseHandler closeHandler, ErrorHandler errorHandle) : base(iPAddress, port, messageHandler, closeHandler, errorHandle)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = false
            };
        }

        public void Connect()
        {
            clientSocket.BeginConnect(IpAdress, Port, new AsyncCallback(ConnectCallback), clientSocket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            clientSocket.EndConnect(ar);
            if (clientSocket.RemoteEndPoint != null)
            {
                Console.WriteLine("Socket connected to {0}", clientSocket.RemoteEndPoint.ToString());
            }

            Receive();
        }

        public void Receive()
        {
            clientSocket.BeginReceive(RawBuffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), clientSocket);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = clientSocket.EndReceive(ar);

            if (bytesRead > 0)
            {
                // Call the custom message handler
                messageHandler(this, bytesRead);

                // Continue receiving data
                clientSocket.BeginReceive(RawBuffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), clientSocket);
            }
        }

        public void Send(byte[] data)
        {
            clientSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), clientSocket);
        }

        private void SendCallback(IAsyncResult ar)
        {

            // Complete sending the data to the remote device.
            int bytesSent = clientSocket.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to server.", bytesSent);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}