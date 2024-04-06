using System.Net;
using System.Text;

using Sockets;
using Messages;

namespace SocketClientTestApp
{
    class TestApp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Test the SocketClient class
            TestClient();
        }

        static public void MessageHandlerClient(SocketBase pSocket, int iNumberOfBytes)
        {
            try
            {
                // Convert the message from a byte array to a string
                string strMessage = Encoding.ASCII.GetString(pSocket.RawBuffer, 0, iNumberOfBytes);

                // Display the string to the console window
                Console.WriteLine(strMessage);
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
        }
        //*********************************************
        /// <summary> Called when a socket connection is closed </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        static public void CloseHandler(SocketBase pSocket)
        {
            Console.WriteLine("Close Handler");
            Console.WriteLine("IpAddress: " + pSocket.IpAddress);
        }
        //**************************************************
        /// <summary> Called when a socket error occurs </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        /// <param name="pException"> The reason for the error </param>
        static public void ErrorHandler(SocketBase pSocket, Exception pException)
        {
            Console.WriteLine(pException.Message);
        }
        static void TestClient()
        {
            try
            {
                // Instantiate a SocketClient object
                SocketClient pSocketClient = new(10240,
                    new MessageHandler(MessageHandlerClient),
                    new CloseHandler(CloseHandler),
                    new ErrorHandler(ErrorHandler));

                pSocketClient.Connect(IPAddress.Parse("127.0.0.1"), 9000);


                while (true) {
                    // Send a message to the server
                    // pSocketClient.Send("Hello from the client");
                    string username = "TestUser" + new Random().Next(1000);

                    pSocketClient.Send(new ClientConnectionMessage(username).Serialize());

                    Console.WriteLine("Press Enter to send another message or 'Q' to quit");
                    string? strInput = Console.ReadLine();
                    if (strInput != null && strInput.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
                        break;
                }

                pSocketClient.Disconnect();
                pSocketClient.Dispose();
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
        }

    }
}