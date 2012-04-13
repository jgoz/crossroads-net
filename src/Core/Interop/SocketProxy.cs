namespace CrossroadsIO.Interop
{
    using System;
    using System.Runtime.InteropServices;

    internal class SocketProxy : IDisposable
    {
        // From options.hpp: unsigned char identity [256];
        private const int MaxBinaryOptionSize = 255;

        private IntPtr _message;
        private bool _disposed;

        public SocketProxy(IntPtr socketHandle)
        {
            if (socketHandle == IntPtr.Zero)
            {
                throw new ArgumentException("Socket handle must be a valid pointer.", "socketHandle");
            }

            SocketHandle = socketHandle;
            _message = Marshal.AllocHGlobal(LibXs.XsMsgTSize);
        }

        ~SocketProxy()
        {
            Dispose(false);
        }

        public IntPtr SocketHandle { get; private set; }

        public int Bind(string endpoint)
        {
            return LibXs.xs_bind(SocketHandle, endpoint);
        }

        public int Connect(string endpoint)
        {
            return LibXs.xs_connect(SocketHandle, endpoint);
        }

        public int Close()
        {
            // Allow Close to be called repeatedly without failure
            if (SocketHandle == IntPtr.Zero)
            {
                return 0;
            }

            int rc = LibXs.xs_close(SocketHandle);

            SocketHandle = IntPtr.Zero;

            return rc;
        }

        public int Receive(byte[] buffer, int flags)
        {
            if (LibXs.xs_msg_init_size(_message, buffer.Length) == -1)
            {
                return -1;
            }

            int bytesReceived = RetryIfInterrupted(() => LibXs.xs_recvmsg(SocketHandle, _message, flags));

            if (bytesReceived > 0)
            {
                Marshal.Copy(LibXs.xs_msg_data(_message), buffer, 0, bytesReceived);
            }

            if (LibXs.xs_msg_close(_message) == -1)
            {
                return -1;
            }

            return bytesReceived;
        }

        public byte[] Receive(byte[] buffer, int flags, out int size)
        {
            size = -1;

            if (LibXs.xs_msg_init(_message) == -1)
            {
                return buffer;
            }

            int bytesReceived = RetryIfInterrupted(() => LibXs.xs_recvmsg(SocketHandle, _message, flags));

            if (bytesReceived >= 0)
            {
                size = bytesReceived;

                if (buffer == null || size > buffer.Length)
                {
                    buffer = new byte[size];
                }

                Marshal.Copy(LibXs.xs_msg_data(_message), buffer, 0, size);
            }

            if (LibXs.xs_msg_close(_message) == -1)
            {
                size = -1;
            }

            return buffer;
        }

        public int Send(byte[] buffer, int size, int flags)
        {
            if (LibXs.xs_msg_init_size(_message, size) == -1)
            {
                return -1;
            }

            if (size > 0)
            {
                Marshal.Copy(buffer, 0, LibXs.xs_msg_data(_message), size);
            }

            int bytesSent = RetryIfInterrupted(() => LibXs.xs_sendmsg(SocketHandle, _message, flags));

            if (LibXs.xs_msg_close(_message) == -1)
            {
                return -1;
            }

            return bytesSent;
        }

        public int Forward(IntPtr destinationHandle)
        {
            if (LibXs.xs_msg_init(_message) == -1)
            {
                return -1;
            }

            int receiveMore;
            int bytesSent;

            do
            {
                if (LibXs.xs_recvmsg(SocketHandle, _message, 0) == -1)
                {
                    return -1;
                }

                if (GetReceiveMore(out receiveMore) == -1)
                {
                    return -1;
                }

                if ((bytesSent = LibXs.xs_sendmsg(destinationHandle, _message, receiveMore == 1 ? (int)SocketFlags.SendMore : 0)) == -1)
                {
                    return -1;
                }
            }
            while (receiveMore == 1);

            if (LibXs.xs_msg_close(_message) == -1)
            {
                return -1;
            }

            return bytesSent;
        }

        public int GetSocketOption(int option, out int value)
        {
            using (var optionLength = new DisposableIntPtr(IntPtr.Size))
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(int))))
            {
                Marshal.WriteInt32(optionLength, sizeof(int));

                int rc = RetryIfInterrupted(() => LibXs.xs_getsockopt(SocketHandle, option, optionValue, optionLength));
                value = Marshal.ReadInt32(optionValue);

                return rc;
            }
        }

        public int GetSocketOption(int option, out long value)
        {
            using (var optionLength = new DisposableIntPtr(IntPtr.Size))
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(long))))
            {
                Marshal.WriteInt32(optionLength, sizeof(long));

                int rc = RetryIfInterrupted(() => LibXs.xs_getsockopt(SocketHandle, option, optionValue, optionLength));
                value = Marshal.ReadInt64(optionValue);

                return rc;
            }
        }

        public int GetSocketOption(int option, out ulong value)
        {
            using (var optionLength = new DisposableIntPtr(IntPtr.Size))
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(ulong))))
            {
                Marshal.WriteInt32(optionLength, sizeof(ulong));

                int rc = RetryIfInterrupted(() => LibXs.xs_getsockopt(SocketHandle, option, optionValue, optionLength));
                value = unchecked(Convert.ToUInt64(Marshal.ReadInt64(optionValue)));

                return rc;
            }
        }

        public int GetSocketOption(int option, out byte[] value)
        {
            using (var optionLength = new DisposableIntPtr(IntPtr.Size))
            using (var optionValue = new DisposableIntPtr(MaxBinaryOptionSize))
            {
                Marshal.WriteInt32(optionLength, MaxBinaryOptionSize);

                int rc = RetryIfInterrupted(() => LibXs.xs_getsockopt(SocketHandle, option, optionValue, optionLength));

                value = new byte[Marshal.ReadInt32(optionLength)];
                Marshal.Copy(optionValue, value, 0, value.Length);

                return rc;
            }
        }

        public int SetSocketOption(int option, int value)
        {
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(int))))
            {
                Marshal.WriteInt32(optionValue, value);

                return RetryIfInterrupted(() => LibXs.xs_setsockopt(SocketHandle, option, optionValue, sizeof(int)));
            }
        }

        public int SetSocketOption(int option, long value)
        {
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(long))))
            {
                Marshal.WriteInt64(optionValue, value);

                return RetryIfInterrupted(() => LibXs.xs_setsockopt(SocketHandle, option, optionValue, sizeof(long)));
            }
        }

        public int SetSocketOption(int option, ulong value)
        {
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(ulong))))
            {
                Marshal.WriteInt64(optionValue, unchecked(Convert.ToInt64(value)));

                return RetryIfInterrupted(() => LibXs.xs_setsockopt(SocketHandle, option, optionValue, sizeof(ulong)));
            }
        }

        public int SetSocketOption(int option, byte[] value)
        {
            using (var optionValue = new DisposableIntPtr(value.Length))
            {
                Marshal.Copy(value, 0, optionValue, value.Length);

                return RetryIfInterrupted(() => LibXs.xs_setsockopt(SocketHandle, option, optionValue, value.Length));
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_message != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_message);
                    _message = IntPtr.Zero;
                }

                Close();
            }

            _disposed = true;
        }

        private static int RetryIfInterrupted(Func<int> func)
        {
            int rc;

            do
            {
                rc = func();
            }
            while (rc == -1 && LibXs.xs_errno() == ErrorCode.EINTR);

            return rc;
        }

        private int GetReceiveMore(out int receiveMore)
        {
            if (LibXs.MajorVersion >= 3)
            {
                return GetSocketOption((int)SocketOption.RCVMORE, out receiveMore);
            }

            long value;
            int rc = GetSocketOption((int)SocketOption.RCVMORE, out value);
            receiveMore = (int)value;

            return rc;
        }
    }
}
