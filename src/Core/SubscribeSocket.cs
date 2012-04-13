namespace CrossroadsIO
{
    using System;

    using CrossroadsIO.Interop;

    internal class SubscribeSocket : Socket
    {
        internal SubscribeSocket(SocketProxy socketProxy, SocketType socketType)
            : base(socketProxy, socketType)
        {
        }

        public override int Send(byte[] buffer, int size, SocketFlags flags)
        {
            throw new NotSupportedException();
        }
    }
}
