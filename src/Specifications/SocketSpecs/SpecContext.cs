namespace CrossroadsIO.Specifications.SocketSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    abstract class using_req
    {
        protected static Socket socket;
        protected static Context ctx;
        protected static Exception exception;

        Establish context = () =>
        {
            ctx = Context.Create();
            socket = ctx.CreateSocket(SocketType.REQ);
        };

        Cleanup resources = () =>
        {
            socket.Dispose();
            ctx.Dispose();
        };
    }

    abstract class using_req_rep
    {
        protected static Socket req;
        protected static Socket rep;
        protected static Context ctx;
        protected static Exception exception;

        Establish context = () =>
        {
            ctx = Context.Create();
            req = ctx.CreateSocket(SocketType.REQ);
            rep = ctx.CreateSocket(SocketType.REP);
        };

        Cleanup resources = () =>
        {
            req.Dispose();
            rep.Dispose();
            ctx.Dispose();
        };
    }

    abstract class using_pub_sub
    {
        protected static Socket pub;
        protected static Socket sub;
        protected static Context ctx;
        protected static Exception exception;

        Establish context = () =>
        {
            ctx = Context.Create();
            pub = ctx.CreateSocket(SocketType.PUB);
            sub = ctx.CreateSocket(SocketType.SUB);
        };

        Cleanup resources = () =>
        {
            sub.Dispose();
            pub.Dispose();
            ctx.Dispose();
        };
    }

    abstract class using_threaded_req_rep : using_threaded_socket_pair
    {
        static using_threaded_req_rep()
        {
            createSender = () => ctx.CreateSocket(SocketType.REQ);
            createReceiver = () => ctx.CreateSocket(SocketType.REP);
        }
    }

    abstract class using_threaded_pub_sub : using_threaded_socket_pair
    {
        static using_threaded_pub_sub()
        {
            createSender = () => ctx.CreateSocket(SocketType.PUB);
            createReceiver = () => ctx.CreateSocket(SocketType.SUB);
        }
    }

    abstract class using_threaded_socket_pair
    {
        private static readonly ManualResetEvent receiverReady = new ManualResetEvent(false);

        protected static Func<Socket> createSender;
        protected static Func<Socket> createReceiver;

        protected static Socket sender;
        protected static Socket receiver;
        protected static Context ctx;

        protected static Action<Socket> senderInit;
        protected static Action<Socket> senderAction;
        protected static Action<Socket> receiverInit;
        protected static Action<Socket> receiverAction;

        private static Thread receiverThread;
        private static Thread senderThread;

        Establish context = () =>
        {
            ctx = Context.Create();
            sender = createSender();
            receiver = createReceiver();

            senderInit = sck => { };
            receiverInit = sck => { };
            senderAction = sck => { };
            receiverAction = sck => { };

            senderThread = new Thread(() =>
            {
                senderInit(sender);
                sender.SendHighWatermark = 1;
                receiverReady.WaitOne();
                sender.Connect("inproc://spec_context");
                senderAction(sender);
            });

            receiverThread = new Thread(() =>
            {
                receiverInit(receiver);
                receiver.SendHighWatermark = 1;
                receiver.Bind("inproc://spec_context");
                receiverReady.Set();
                receiverAction(receiver);
            });
        };

        Cleanup resources = () =>
        {
            sender.Dispose();
            receiver.Dispose();
            ctx.Dispose();
        };

        protected static void StartThreads()
        {
            receiverThread.Start();
            senderThread.Start();

            if (!receiverThread.Join(5000))
            {
                receiverThread.Abort();
            }

            if (!senderThread.Join(5000))
            {
                senderThread.Abort();
            }
        }
    }
}
