using System.Net;

namespace Sockets
{

    public delegate void MessageHandler(SocketBase socket, int numberOFBytes);

    public delegate void CloseHandler(SocketBase socket);

    public delegate void ErrorHandler(SocketBase socket, Exception exception);

    public abstract class SocketBase(IPAddress iPAddress, int port, MessageHandler messageHandler, CloseHandler closeHandler, ErrorHandler errorHandler) : IDisposable
    {
        protected internal MessageHandler messageHandler = messageHandler;

        protected internal CloseHandler closeHandler = closeHandler;

        protected internal ErrorHandler errorHandler = errorHandler;

        protected internal IPAddress ipAdress = iPAddress;

        protected internal int port = port;

        // protected internal bool isDisposed = false;

        protected internal byte[] rawBuffer = [];

        protected internal int bufferSize = 0;

        public IPAddress IpAdress
        {
            get => ipAdress;
            set
            {
                ipAdress = value;
            }
        }

        public int Port
        {
            get => port;
            set
            {
                port = value;
            }
        }

        public byte[] RawBuffer
        {
            get => rawBuffer;
            set
            {
                rawBuffer = value;
            }
        }

        public int BufferSize
        {
            get => bufferSize;
            set
            {
                bufferSize = value;
            }
        }

        public abstract void Dispose();

    }
}