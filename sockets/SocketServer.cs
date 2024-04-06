using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Sockets
{
    /// <summary> 
    /// This class abstracts a socket server 
    /// </summary>
    public class SocketServer : SocketBase
    {
        /// <summary> 
        /// Called when a socket connection is accepted 
        /// </summary>
        public delegate void AcceptHandler(SocketClient socket);
        /// <summary>
        /// A reference to a user supplied function to be called when a socket connection is accepted
        /// </summary>
        private AcceptHandler acceptHandler = null!;
        /// <summary> 
        /// A TcpListener object to accept socket connections 
        /// </summary>
        private TcpListener tcpListener = null!;
        /// <summary>
        /// A thread to process accepting socket connections
        /// </summary>
        private Thread? acceptThread;
        /// <summary>
        /// An Array of SocketClient objects 
        /// </summary>
        private readonly ArrayList socketClientList = [];
        public ArrayList SocketClientList
        {
            get { return socketClientList; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SocketServer()
        {
            // Init the dispose flag
            disposed = false;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~SocketServer()
        {
            // If this object has not been disposed yet
            if (!disposed)
                Stop();
        }

        /// <summary>
        /// Release objects
        /// </summary>
        public override void Dispose()
        {
            try
            {
                // Mark the object as disposed
                disposed = true;

                // Stop the server if the thread is running
                if (acceptThread != null)
                    Stop();
            }
            catch
            {
            }
            base.Dispose();
        }


        protected virtual SocketClient AcceptedSocketClient(SocketServer socketServer,
            Socket clientSocket, IPAddress ipAddress, int port,
            int sizeOfRawBuffer, MessageHandler messageHandler,
            CloseHandler closeHandler, ErrorHandler errorHandler)
        {
            return new SocketClient(socketServer, clientSocket,
                ipAddress, port, sizeOfRawBuffer, messageHandler,
                closeHandler, errorHandler);
        }
        /// <summary>
        /// Function to process and accept socket connection requests
        /// </summary>
        private void AcceptThread()
        {
            Socket? clientSocket = null;
            try
            {
                // Create a new TCPListner and start it up
                tcpListener = new TcpListener(IpAddress, Port);
                tcpListener.Start();
                for (; ; )
                {
                    // If a client connects, accept the connection
                    clientSocket = tcpListener.AcceptSocket();
                    if (clientSocket.Connected)
                    {
                        IPAddress Addr = ((IPEndPoint)clientSocket.RemoteEndPoint!).Address;

                        // Create a SocketClient object
                        SocketClient socket = AcceptedSocketClient(this,
                            clientSocket,                                           // The socket object for the connection
                            Addr,                                                   // The IpAddress of the client
                            Port,                                                 // The port the client connected to
                            SizeOfRawBuffer,                                      // The size of the byte array for storing messages
                            new MessageHandler(messageHandler),    // Application developer Message Handler
                            new CloseHandler(closeHandler),        // Application developer Close Handler
                            new ErrorHandler(errorHandler));       // Application developer Error Handler

                        socketClientList.Add(socket);
                        // Call the Accept Handler
                        acceptHandler(socket);
                    }
                }
            }
            catch (SocketException e)
            {
                // Did we stop the TCPListener
                if (e.ErrorCode != 10004)
                {
                    // Call the error handler
                    errorHandler(null!, e);
                    // Close the socket down if it exists
                    if (clientSocket != null)
                        if (clientSocket.Connected)
                            clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                // Call the error handler
                errorHandler(null!, e);
                // Close the socket down if it exists
                if (clientSocket != null)
                    if (clientSocket.Connected)
                        clientSocket.Close();
            }
        }

        public void RemoveSocket(SocketClient socketClient)
        {
            Monitor.Enter(socketClientList);
            try
            {
                foreach (SocketClient socket in socketClientList)
                {
                    if (socket == socketClient)
                    {
                        socketClientList.Remove(socketClient);
                        break;
                    }
                }
            }
            catch
            {
            }
            Monitor.Exit(socketClientList);
        }

        /// <summary> 
        /// Function to start the SocketServer 
        /// </summary>
        /// <param name="ipAddress"> The IpAddress to listening on </param>
        /// <param name="port"> The Port to listen on </param>
        /// <param name="sizeOfRawBuffer"> Size of the Raw Buffer </param>
        /// <param name="userArg"> User supplied arguments </param>
        /// <param name="messageHandler"> Function pointer to the user MessageHandler function </param>
        /// <param name="acceptHandler"> Function pointer to the user AcceptHandler function </param>
        /// <param name="closeHandler"> Function pointer to the user CloseHandler function </param>
        /// <param name="errorHandler"> Function pointer to the user ErrorHandler function </param>
        public void Start(IPAddress ipAddress, int port, int sizeOfRawBuffer,
            MessageHandler messageHandler, AcceptHandler acceptHandler, CloseHandler closeHandler,
            ErrorHandler errorHandler)
        {
            // Is an AcceptThread currently running
            if (acceptThread == null)
            {
                // Set connection values
                IpAddress = ipAddress;
                Port = port;

                // Save the Handler Functions
                this.messageHandler = messageHandler;
                this.acceptHandler = acceptHandler;
                this.closeHandler = closeHandler;
                this.errorHandler = errorHandler;

                // Save the buffer size and user arguments
                SizeOfRawBuffer = sizeOfRawBuffer;

                // Start the listening thread if one is currently not running
                ThreadStart tsThread = new(AcceptThread);
                acceptThread = new Thread(tsThread)
                {
                    Name = "Notification.Accept"
                };
                acceptThread.Start();
            }
        }

        /// <summary> 
        /// Function to stop the SocketServer.  It can be restarted with Start 
        /// </summary>
        public void Stop()
        {
            // Abort the accept thread
            if (acceptThread != null)
            {
                tcpListener.Stop();
                acceptThread.Join();
                acceptThread = null;
            }

            // Dispose of all of the socket connections
            for (int iSocket = 0; iSocket < socketClientList.Count; ++iSocket)
            {
                SocketClient socket = (SocketClient)socketClientList[iSocket]!;
                socketClientList.Remove(socket);
                socket.Dispose();
            }

            // Wait for all of the socket client objects to be destroyed
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Clear the Handler Functions
            messageHandler = null!;
            acceptHandler = null!;
            closeHandler = null!;
            errorHandler = null!;

            // Clear the buffer size and user arguments
            SizeOfRawBuffer = 0;
        }


        /// <summary>
        /// The number of clients connected
        /// </summary>
        public int ConnectedClientCount
        {
            get
            {
                if (socketClientList == null)
                    return 0;

                return socketClientList.Count;
            }
        }

        /// <summary>
        /// Notifies connected clients of new alert from system
        /// </summary>
        /// <param name="data"></param>
        public int NotifyConnectedClients(string data)
        {
            int count = 0;
            ArrayList? ObjectsToRemove = null;

            for (int x = 0; x < socketClientList.Count; x++)
            {
                try
                {
                    SocketClient socket = (SocketClient)socketClientList[x]!;

                    if (socket.ClientSocket.Connected == true &&
                        socket.Send(data) == true)
                        count++;
                    else
                    {
                        ObjectsToRemove ??= [];

                        ObjectsToRemove.Add(socket);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:SocketServer: While in NotifyConnectedClients" +
                        e.Message);
                    System.Diagnostics.Debugger.Break();
                }
            }

            if (ObjectsToRemove != null)
            {
                foreach (SocketClient socket in ObjectsToRemove)
                {
                    socket.Disconnect();
                    socketClientList.Remove(socket);
                    socket.Dispose();
                }
            }


            return count;

        }
    }
}