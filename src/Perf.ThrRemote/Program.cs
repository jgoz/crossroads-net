﻿namespace CrossroadsIO.Perf.ThrRemote
{
    using System;
    using System.Threading;

    internal class Program
    {
        internal static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("usage: remote_thr <address> <message-size> <message-count>\n");
                return 1;
            }

            string address = args[0];
            int messageSize = Convert.ToInt32(args[1]);
            int messageCount = Convert.ToInt32(args[2]);

            if (messageSize <= 0 || messageCount <= 0)
            {
                Console.Error.WriteLine("message-size and message-count must be positive values.");
                return 1;
            }

            // Initialize XS infrastructure
            using (Context ctx = Context.Create())
            using (Socket skt = ctx.CreateSocket(SocketType.PUB))
            {
                skt.Connect(address);

                // Create a message to send.
                var msg = new byte[messageSize];

                // Start sending messages.
                for (int i = 0; i < messageCount; i++)
                {
                    skt.Send(msg);
                }

                Thread.Sleep(2000);
            }

            return 0;
        }
    }
}
