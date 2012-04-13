﻿namespace CrossroadsIO
{
    using System;

    using CrossroadsIO.Interop;

    /// <summary>
    /// Provides data for <see cref="CrossroadsIO.Socket.ReceiveReady"/> and <see cref="CrossroadsIO.Socket.SendReady"/> events.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        internal SocketEventArgs(Socket socket, PollEvents readyEvents)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            Socket = socket;
            ReceiveReady = readyEvents.HasFlag(PollEvents.PollIn);
            SendReady = readyEvents.HasFlag(PollEvents.PollOut);
        }

        /// <summary>
        /// Gets the socket that may be used to receive or send at least one message without blocking.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets a value indicating whether at least one message may be received by the socket without blocking.
        /// </summary>
        public bool ReceiveReady { get; private set; }

        /// <summary>
        /// Gets a value indicating whether at least one message may be sent by the socket without blocking.
        /// </summary>
        public bool SendReady { get; private set; }
    }
}
