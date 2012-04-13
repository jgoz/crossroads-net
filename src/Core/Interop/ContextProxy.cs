namespace CrossroadsIO.Interop
{
    using System;
    using System.Runtime.InteropServices;

    internal class ContextProxy : IDisposable
    {
        private bool _disposed;

        ~ContextProxy()
        {
            Dispose(false);
        }

        public IntPtr ContextHandle { get; private set; }

        public int Initialize()
        {
            ContextHandle = LibXs.xs_init();

            return ContextHandle == IntPtr.Zero ? -1 : 0;
        }

        public IntPtr CreateSocket(int socketType)
        {
            return LibXs.xs_socket(ContextHandle, socketType);
        }

        public int SetContextOption(int option, int value)
        {
            using (var optionValue = new DisposableIntPtr(Marshal.SizeOf(typeof(int))))
            {
                Marshal.WriteInt32(optionValue, value);

                return LibXs.xs_setsockopt(ContextHandle, option, optionValue, sizeof(int));
            }
        }

        public void Terminate()
        {
            if (ContextHandle == IntPtr.Zero)
            {
                return;
            }

            while (LibXs.xs_term(ContextHandle) != 0)
            {
                int errorCode = ErrorProxy.GetErrorCode();

                // If xs_term fails, valid return codes are EFAULT or EINTR. If EINTR is set, termination
                // was interrupted by a signal and may be safely retried.
                if (errorCode == ErrorCode.EFAULT)
                {
                    // This indicates an invalid context was passed in. There's nothing we can do about it here.
                    // It's arguably not a fatal error, so throwing an exception would be bad seeing as this may
                    // run inside a finalizer.
                    break;
                }
            }

            ContextHandle = IntPtr.Zero;
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
                Terminate();
            }

            _disposed = true;
        }
    }
}
