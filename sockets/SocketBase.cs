using System.Net;

namespace Sockets
{
	/// <summary> 
	/// Called when a message is received 
	/// </summary>
	public delegate void MessageHandler(SocketBase socket, int iNumberOfBytes);

	/// <summary> 
	/// Called when a connection is closed
	///  </summary>
	public delegate void CloseHandler(SocketBase socket);

	/// <summary>
	///  Called when a socket error occurs 
	///  </summary>
	public delegate void ErrorHandler(SocketBase socket, Exception exception);

	/// <summary>
	/// Abstract class that defines base implementations
	/// </summary>
	public abstract class SocketBase : IDisposable
	{
		/// <summary>
		/// A reference to a user defined object
		/// </summary>
		// protected internal object? userArg;
		/// <summary>
		/// A reference to a user supplied function to be called when a socket message arrives 
		/// </summary>
		protected internal MessageHandler messageHandler = null!;
		/// <summary>
		/// A reference to a user supplied function to be called when a socket connection is closed 
		/// </summary>
		protected internal CloseHandler closeHandler = null!;
		/// <summary>
		/// A reference to a user supplied function to be called when a socket error occurs
		/// </summary>
		protected internal ErrorHandler errorHandler = null!;
		/// <summary>
		/// Flag to indicate if the class has been disposed
		/// </summary>
		protected internal bool disposed;
		/// <summary>
		/// The IpAddress the client is connect to
		/// </summary>
		protected internal IPAddress ipAddress = null!;
		/// <summary>
		/// The Port to either connect to or listen on
		/// </summary>
		protected internal int port;
		/// <summary>
		/// A raw buffer to capture data comming off the socket
		/// </summary>
		protected internal byte[] rawBuffer = null!;
		/// <summary>
		/// Size of the raw buffer for received socket data
		/// </summary>
		protected internal int sizeOfRawBuffer;

		public IPAddress IpAddress
		{
			get => ipAddress;
			set => ipAddress = value;
		}

		public int Port
		{
			get => port;
			set => port = value;
		}

		public byte[] RawBuffer
		{
			get => rawBuffer;
			set => rawBuffer = value;
		}

		public int SizeOfRawBuffer
		{
			get => sizeOfRawBuffer;
			set => sizeOfRawBuffer = value;
		}

		public virtual void Dispose() { }

	}
}