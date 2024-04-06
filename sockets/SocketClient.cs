using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets
{
    /// <summary> 
    /// This class abstracts a socket 
    /// </summary>
    public class SocketClient : SocketBase
    {
        private readonly Queue messageQueue = new();
        private byte[]? theRest;
        private bool m_Connected = false;

        #region Private Properties
        /// <summary>
        /// A network stream object 
        /// </summary>
        private NetworkStream networkStream = null!;
        /// <summary>
        /// A TcpClient object for socket connection 
        /// </summary>
        private TcpClient? tcpClient;
        /// <summary>
        /// A callback object for processing recieved socket data 
        /// </summary>
        private readonly AsyncCallback callbackReadMethod;
        /// <summary>
        /// A callback object for processing send socket data
        /// </summary>
        private readonly AsyncCallback callbackWriteMethod;
        /// <summary>
        /// Size of the receive buffer. Defaults to 1048576
        /// </summary>
        private readonly int receiveBufferSize = 1048576;
        /// <summary>
        /// Size of the send buffer. Defaults to 1048576
        /// </summary>
        private readonly int sendBufferSize = 1048576;
        /// <summary>
        /// The SocketServer for this socket object
        /// </summary>
        private readonly SocketServer socketServer = null!;

        public SocketServer SocketServer
        {
            get { return socketServer; }
        }

        /// <summary>
        /// The socket for the client connection 
        /// </summary>
        private Socket? clientSocket;

        #endregion Private Properties

        #region Constructor & Destructor

        /// <summary> 
        /// Constructor for client support
        /// </summary>
        /// <param name="sizeOfRawBuffer"> The size of the raw buffer </param>
        /// <param name="userArg"> A Reference to the Users arguments </param>
        /// <param name="messageHandler">  Reference to the user defined message handler method </param>
        /// <param name="closeHandler">  Reference to the user defined close handler method </param>
        /// <param name="errorHandler">  Reference to the user defined error handler method </param>
        public SocketClient(int sizeOfRawBuffer,
            MessageHandler messageHandler, CloseHandler closeHandler,
            ErrorHandler errorHandler)
        {
            // Create the raw buffer
            SizeOfRawBuffer = sizeOfRawBuffer;
            RawBuffer = new byte[SizeOfRawBuffer];

            // Save the user argument
            // this.userArg = userArg;

            // Set the handler methods
            this.messageHandler = messageHandler;
            this.closeHandler = closeHandler;
            this.errorHandler = errorHandler;

            // Set the async socket method handlers
            callbackReadMethod = new AsyncCallback(ReceiveComplete);
            callbackWriteMethod = new AsyncCallback(SendComplete);

            m_Connected = true;
            // Init the dispose flag
            disposed = false;
        }

        /// <summary> Constructor for SocketServer Suppport </summary>
        /// <param name="socketServer"> A Reference to the parent SocketServer </param>
        /// <param name="clientSocket"> The Socket object we are encapsulating </param>
        /// <param name="socketListArray"> The index of the SocketServer Socket List Array </param>
        /// <param name="ipAddress"> The IpAddress of the remote server </param>
        /// <param name="port"> The Port of the remote server </param>
        /// <param name="messageHandler"> Reference to the user defined message handler function </param>
        /// <param name="closeHandler"> Reference to the user defined close handler function </param>
        /// <param name="errorHandler"> Reference to the user defined error handler function </param>
        /// <param name="sizeOfRawBuffer"> The size of the raw buffer </param>
        /// <param name="userArg"> A Reference to the Users arguments </param>
        public SocketClient(SocketServer socketServer, Socket clientSocket,
            IPAddress ipAddress, int port, int sizeOfRawBuffer,
            MessageHandler messageHandler, CloseHandler closeHandler,
            ErrorHandler errorHandler)
            : this(sizeOfRawBuffer, messageHandler, closeHandler, errorHandler)
        {

            // Set reference to SocketServer
            this.socketServer = socketServer;

            // Init the socket references
            this.clientSocket = clientSocket;

            // Set the Ipaddress and Port
            IpAddress = ipAddress;
            Port = port;

            // Init the NetworkStream reference
            networkStream = new NetworkStream(this.clientSocket);

            // Set these socket options
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket,
                SocketOptionName.ReceiveBuffer, receiveBufferSize);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket,
                SocketOptionName.SendBuffer, sendBufferSize);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket,
                SocketOptionName.DontLinger, 1);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Tcp,
                SocketOptionName.NoDelay, 1);

            // Wait for a message
            Receive();
        }

        /// <summary> 
        /// Overloaded constructor for client support
        /// </summary>
        /// <param name="sendBufferSize"></param>
        /// <param name="receiveBufferSize"></param>
        /// <param name="sizeOfRawBuffer"> The size of the raw buffer </param>
        /// <param name="userArg"> A Reference to the Users arguments </param>
        /// <param name="messageHandler">  Reference to the user defined message handler method </param>
        /// <param name="closeHandler">  Reference to the user defined close handler method </param>
        /// <param name="errorHandler">  Reference to the user defined error handler method </param>
        public SocketClient(int sendBufferSize, int receiveBufferSize,
            int sizeOfRawBuffer,
            MessageHandler messageHandler,
            CloseHandler closeHandler,
            ErrorHandler errorHandler
            ) : this(sizeOfRawBuffer, messageHandler, closeHandler, errorHandler)
        {
            //Set the size of the send/receive buffers
            this.sendBufferSize = sendBufferSize;
            this.receiveBufferSize = receiveBufferSize;
        }

        /// <summary> 
        /// Finialize 
        /// </summary>

        ~SocketClient()
        {
            if (!disposed)
                Dispose();
        }

        /// <summary>
        /// Disposes of internal objects
        /// </summary>
        public override void Dispose()
        {
            try
            {
                // Flag that dispose has been called
                disposed = true;
                // Disconnect the client from the server
                Disconnect();
            }
            catch
            {
            }
            // Remove the socket from the list
            socketServer?.RemoveSocket(this);

            base.Dispose();
        }


        #endregion Constructor & Destructor

        private void SaveTheRestofTheStream(int ptr, int TotalLen)
        {
            theRest = new byte[TotalLen - ptr];
            for (int i = 0; ptr < TotalLen; i++)
                theRest[i] = RawBuffer[ptr++];

            return;
        }
        private void ParseMessage(int TotalLength)
        {
            int ptr = 0;

            try
            {
                while (ptr < TotalLength)
                {
                    if ((TotalLength - ptr) <= 2)
                    {
                        SaveTheRestofTheStream(ptr, TotalLength);
                        return;
                    }

                    byte[] blen = RawBuffer[ptr..(ptr + 2)];
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(blen);
                    ushort len = BitConverter.ToUInt16(blen);

                    byte[] msg;

                    if (len + 2 > (TotalLength - ptr))
                    {
                        SaveTheRestofTheStream(ptr, TotalLength);
                        return;
                    }

                    ptr += 2;

                    msg = new byte[len];
                    for (int i = 0; i < len; i++)
                        msg[i] = RawBuffer[ptr++];

                    messageQueue.Enqueue(msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:SocketClient: Got Exception while ParseMessage");
                Console.WriteLine(e);
            }
        }

        /// <summary> 
        /// Called when a message arrives
        ///  </summary>
        /// <param name="ar"> An async result interface </param>
        private void ReceiveComplete(IAsyncResult ar)
        {
            try
            {
                // Is the Network Stream object valid
                if (networkStream.CanRead)
                {
                    // Read the current bytes from the stream buffer
                    int bytesRecieved = networkStream.EndRead(ar);
                    // If there are bytes to process else the connection is lost
                    if (bytesRecieved > 0)
                    {
                        try
                        {
                            if (theRest != null)
                            {
                                int i;
                                byte[] tmp = new byte[bytesRecieved + theRest.Length];

                                for (i = 0; i < theRest.Length; i++)
                                    tmp[i] = theRest[i];

                                for (int j = 0; j < bytesRecieved; j++)
                                    tmp[i++] = RawBuffer[j];

                                RawBuffer = tmp;

                                bytesRecieved += theRest.Length;

                                theRest = null;
                            }

                            ParseMessage(bytesRecieved);

                            if (messageQueue.Count > 0)
                            {
                                RawBuffer = (byte[])messageQueue.Dequeue()!;
                                // A message came in send it to the MessageHandler
                                messageHandler(this, RawBuffer.Length);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        // Wait for a new message

                        if (m_Connected == true)
                            Receive();
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error:SocketClient: Got Exception while ReceiveComplete");

                try
                {
                    System.Diagnostics.Debugger.Break();
                    // The connection must have dropped call the CloseHandler
                    closeHandler(this);
                }
                catch
                {
                }
            }
        }

        /// <summary> 
        /// Called when a message is sent
        ///  </summary>
        /// <param name="ar"> An async result interface </param>
        private void SendComplete(IAsyncResult ar)
        {
            try
            {
                // Is the Network Stream object valid
                if (networkStream.CanWrite)
                    networkStream.EndWrite(ar);
            }
            catch (Exception)
            {
                Console.WriteLine("Error:SocketClient: Got Exception while SendComplete");
            }
        }

        /// <summary> 
        /// The socket for the client connection 
        /// </summary>
        public Socket ClientSocket
        {
            get
            {
                return clientSocket!;
            }
            set
            {
                clientSocket = value;
            }
        }

        /// <summary> 
        /// Function used to connect to a server 
        /// </summary>
        /// <param name="IpAddress"> The address to connect to </param>
        /// <param name="Port"> The Port to connect to </param>
        public void Connect(IPAddress ipAddress, int port)
        {
            try
            {
                if (networkStream == null)
                {
                    // Set the Ipaddress and Port
                    IpAddress = ipAddress;
                    Port = port;

                    // Attempt to establish a connection
                    tcpClient = new TcpClient(ipAddress.ToString(), port);
                    networkStream = tcpClient.GetStream();

                    // Set these socket options
                    tcpClient.ReceiveBufferSize = receiveBufferSize;
                    tcpClient.SendBufferSize = sendBufferSize;
                    tcpClient.NoDelay = true;
                    tcpClient.LingerState = new LingerOption(false, 0);

                    m_Connected = true;
                    // Start to receive messages
                    Receive();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error:SocketClient: Got Exception while Connect:" + e.Message);
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary> 
        /// Function used to disconnect from the server 
        /// </summary>
        public virtual void Disconnect()
        {
            if (m_Connected == true)
            {
                // Close down the connection
                networkStream?.Close();
                tcpClient?.Close();
                clientSocket?.Close();

                // Clean up the connection state
                networkStream = null!;
                tcpClient = null;
                clientSocket = null;

                m_Connected = false;
            }
        }

        /// <summary>
        ///  Function to send a string to the server 
        ///  </summary>
        /// <param name="message"> A string to send </param>
        // public bool Send(string message)
        // {
        //     byte[] ushortBytes = BitConverter.GetBytes((ushort)message.Length);
        //     if (BitConverter.IsLittleEndian)
        //         Array.Reverse(ushortBytes);
        //     string len = Encoding.UTF8.GetString(ushortBytes);
        //     byte[] rawBuffer = Encoding.UTF8.GetBytes(len + message);
        //     return Send(rawBuffer);
        // }
        /// <summary> 
        /// Function to send a raw buffer to the server 
        /// </summary>
        /// <param name="rawBuffer"> A Raw buffer of bytes to send </param>
        public bool Send(byte[] message)
        {
            byte[] ushortBytes = BitConverter.GetBytes((ushort)message.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ushortBytes);
            string len = Encoding.UTF8.GetString(ushortBytes);
            byte[] rawBuffer = Encoding.UTF8.GetBytes(len + Encoding.UTF8.GetString(message));
            if ((networkStream != null) && networkStream.CanWrite)
            //&& 
            //(clientSocket != null) && (this.clientSocket.Connected == true))
            {
                // Issue an asynchronus write
                networkStream.BeginWrite(rawBuffer, 0, rawBuffer.GetLength(0), callbackWriteMethod, null);
                return true;
            }
            else
                return false;
        }

        /// <summary> 
        /// Wait for a message to arrive
        /// </summary>
        public void Receive()
        {
            while (messageQueue.Count != 0)
            {
                RawBuffer = (byte[])messageQueue.Dequeue()!;

                messageHandler(this, RawBuffer.Length);
            }

            if ((networkStream != null) && networkStream.CanRead)
            {
                RawBuffer = new byte[SizeOfRawBuffer];
                // Issue an asynchronous read
                networkStream.BeginRead(RawBuffer, 0, SizeOfRawBuffer, callbackReadMethod, null);
            }
            else
                throw new Exception("Socket Closed");
        }

    }
}