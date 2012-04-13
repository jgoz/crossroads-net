namespace CrossroadsIO
{
    using System;
    using System.Text;

    using CrossroadsIO.Interop;

    /// <summary>
    /// Creates Crossroads I/O <see cref="Socket"/> instances within a process boundary.
    /// </summary>
    /// <remarks>
    /// The <see cref="Context"/> object is a container for all sockets in a single process,
    /// and acts as the transport for inproc sockets. <see cref="Context"/> is thread safe.
    /// A <see cref="Context"/> must not be terminated until all spawned sockets have been
    /// successfully closed.
    /// </remarks>
    public class Context : IDisposable
    {
        private readonly ContextProxy _contextProxy;

        private int _threadPoolSize = 1;
        private int _maxSockets = 512;
        private bool _disposed;

        static Context()
        {
            DefaultEncoding = Encoding.UTF8;
        }

        internal Context(ContextProxy contextProxy)
        {
            if (contextProxy == null)
            {
                throw new ArgumentNullException("contextProxy");
            }

            _contextProxy = contextProxy;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Context"/> class.
        /// </summary>
        ~Context()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the default encoding for all sockets in the current process.
        /// </summary>
        public static Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of sockets can be simultaneously active in the current context. (Default = 512).
        /// </summary>
        public int MaxSockets
        {
            get { return _maxSockets; }
            set { SetContextOption(ContextOption.MAX_SOCKETS, (_maxSockets = value)); }
        }

        /// <summary>
        /// Gets or sets the size of the thread pool for the current context. (Default = 1)
        /// </summary>
        public int ThreadPoolSize
        {
            get { return _threadPoolSize; }
            set { SetContextOption(ContextOption.IO_THREADS, (_threadPoolSize = value)); }
        }

        /// <summary>
        /// Create a <see cref="Context"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Context"/> instance.</returns>
        public static Context Create()
        {
            var contextProxy = new ContextProxy();

            if (contextProxy.Initialize() == -1)
            {
                throw new XsException(ErrorProxy.GetLastError());
            }

            return new Context(contextProxy);
        }

        /// <summary>
        /// Create a socket with the current context and the specified socket type.
        /// </summary>
        /// <param name="socketType">A <see cref="SocketType"/> value for the socket.</param>
        /// <returns>A <see cref="Socket"/> instance with the current context and the specified socket type.</returns>
        public Socket CreateSocket(SocketType socketType)
        {
            switch (socketType)
            {
                case SocketType.REQ:
                case SocketType.REP:
                case SocketType.XREQ:
                case SocketType.XREP:
                case SocketType.XPUB:
                case SocketType.PAIR:
                    return CreateSocket(sp => new DuplexSocket(sp, socketType), socketType);

                case SocketType.PUSH:
                case SocketType.PUB:
                    return CreateSocket(sp => new SendSocket(sp, socketType), socketType);

                case SocketType.PULL:
                    return CreateSocket(sp => new ReceiveSocket(sp, socketType), socketType);

                case SocketType.SUB:
                    return CreateSocket(sp => new SubscribeSocket(sp, socketType), socketType);

                case SocketType.XSUB:
                    return CreateSocket(sp => new SubscribeExtSocket(sp, socketType), socketType);
            }

            throw new InvalidOperationException("Invalid socket type specified: " + socketType);
        }

        /// <summary>
        /// Terminate the Crossroads I/O context.
        /// </summary>
        /// <remarks>
        /// Context termination is performed in the following steps:
        /// <ul>
        ///   <li>
        ///     Any blocking operations currently in progress on sockets open within context shall return immediately
        ///     with an error code of ETERM. With the exception of <see cref="Socket.Close"/>, any further operations
        ///     on sockets open within the context shall fail with a <see cref="XsSocketException"/>.
        ///   </li>
        ///   <li>
        ///     After interrupting all blocking calls, <see cref="Terminate"/> shall block until the following conditions
        ///     are met:
        ///     <ul>
        ///       <li>
        ///         All sockets open within the context have been closed with <see cref="Socket.Close"/>.
        ///       </li>
        ///       <li>
        ///         For each socket within the context, all messages sent by the application  have either been
        ///         physically transferred to a network peer, or the socket's linger period set with the
        ///         <see cref="Socket.Linger"/> socket option has expired.
        ///       </li>
        ///     </ul>
        ///   </li>
        /// </ul>
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">The <see cref="Context"/> has already been disposed.</exception>
        /// <exception cref="XsException">An error occurred creating the socket.</exception>
        public void Terminate()
        {
            EnsureNotDisposed();

            _contextProxy.Terminate();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Context"/> class.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Context"/>, and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _contextProxy.Dispose();
                }
            }

            _disposed = true;
        }

        private TSocket CreateSocket<TSocket>(Func<SocketProxy, TSocket> constructor, SocketType socketType)
        {
            EnsureNotDisposed();

            IntPtr socketHandle = _contextProxy.CreateSocket((int)socketType);

            if (socketHandle == IntPtr.Zero)
            {
                throw new XsException(ErrorProxy.GetLastError());
            }

            return constructor(new SocketProxy(socketHandle));
        }

        private void SetContextOption(ContextOption option, int value)
        {
            EnsureNotDisposed();

            if (_contextProxy.SetContextOption((int)option, value) == -1)
            {
                throw new XsSocketException(ErrorProxy.GetLastError());
            }
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
