using System.Net;
using System.Text;
using Messages;
using Sockets;

namespace SocketServerTestApp
{
    public class Form1
    {
        static int m_Count = 0;
        private readonly MessageFactory MessageFactory = new();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate a CSocketServer object
            SocketServer socketServer = new();
            // Start listening for connections
            socketServer.Start(IPAddress.Parse("127.0.0.1"), 9000, 1024,
                new MessageHandler(MessageHandlerServer),
                new SocketServer.AcceptHandler(AcceptHandler),
                new CloseHandler(CloseHandler),
                new ErrorHandler(ErrorHandler));
            Console.WriteLine("Waiting for a client connection on Machine: {0} Port: {1}", Environment.MachineName, 9000);
            // Stay here until you are ready to shutdown the server    
            Console.ReadLine();
            socketServer.Dispose();
        }

        public static void ErrorHandler(SocketBase socket, Exception pException)
        {
            Console.WriteLine(pException.Message);
        }
        public static void CloseHandler(SocketBase socket)
        {
            Console.WriteLine("Close Handler");
            Console.WriteLine("IpAddress: " + socket.IpAddress);
        }

        public static void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append('}');
            Console.WriteLine(sb.ToString());
        }

        /// <summary> Called when a message is extracted from the socket </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        /// <param name="iNumberOfBytes"> The number of bytes in the RawBuffer inside the SocketClient </param>
        static public void MessageHandlerServer(SocketBase socket, int iNumberOfBytes)
        {
            try
            {
                SocketClient pSocket = (SocketClient)socket;
                // Find a complete message
                // PrintByteArray(pSocket.RawBuffer);
                byte[] message = pSocket.RawBuffer[0..iNumberOfBytes];

                PrintByteArray(message);

                Message msg = MessageFactory.CreateMessage(message);

                Console.WriteLine("Message=<{1}> Received {0} messages", m_Count++, ((ClientConnectionMessage)msg).Username);
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
        }

        /// <summary> Called when a socket connection is accepted </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        static public void AcceptHandler(SocketClient pSocket)
        {
            Console.WriteLine("Accept Handler");
            Console.WriteLine("IpAddress: " + pSocket.IpAddress);
        }


    }
}