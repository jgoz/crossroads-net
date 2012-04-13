namespace CrossroadsIO
{
    using System;

    using CrossroadsIO.Interop;

    internal class DuplexSocket : Socket
    {
        internal DuplexSocket(SocketProxy socketProxy, SocketType socketType)
            : base(socketProxy, socketType)
        {
        }

        public override void Subscribe(byte[] prefix)
        {
            throw new NotSupportedException();
        }

        public override void Unsubscribe(byte[] prefix)
        {
            throw new NotSupportedException();
        }
    }
}
