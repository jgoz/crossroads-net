namespace CrossroadsIO
{
    using System;

    using CrossroadsIO.Interop;

    internal class SendSocket : Socket
    {
        internal SendSocket(SocketProxy socketProxy, SocketType socketType)
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

        internal override int Receive(byte[] buffer, SocketFlags flags)
        {
            throw new NotSupportedException();
        }

        internal override byte[] Receive(byte[] frame, SocketFlags flags, out int size)
        {
            throw new NotSupportedException();
        }
    }
}
