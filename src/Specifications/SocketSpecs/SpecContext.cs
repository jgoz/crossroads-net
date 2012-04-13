namespace CrossroadsIO.Specifications.SocketSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    abstract class using_req
    {
        protected static Socket socket;
        protected static Context zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = Context.Create();
            socket = zmqContext.CreateSocket(SocketType.REQ);
        };

        Cleanup resources = () =>
        {
            socket.Dispose();
            zmqContext.Dispose();
        };
    }

    abstract class using_req_rep
    {
        protected static Socket req;
        protected static Socket rep;
        protected static Context zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = Context.Create();
            req = zmqContext.CreateSocket(SocketType.REQ);
            rep = zmqContext.CreateSocket(SocketType.REP);
        };

        Cleanup resources = () =>
        {
            req.Dispose();
            rep.Dispose();
            zmqContext.Dispose();
        };
    }

    abstract class using_pub_sub
    {
        protected static Socket pub;
        protected static Socket sub;
        protected static Context zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = Context.Create();
            pub = zmqContext.CreateSocket(SocketType.PUB);
            sub = zmqContext.CreateSocket(SocketType.SUB);
        };

        Cleanup resources = () =>
        {
            sub.Dispose();
            pub.Dispose();
            zmqContext.Dispose();
        };
    }

    abstract class using_threaded_req_rep : using_threaded_socket_pair
    {
        static using_threaded_req_rep()
        {
            createSender = () => zmqContext.CreateSocket(SocketType.REQ);
            createReceiver = () => zmqContext.CreateSocket(SocketType.REP);
        }
    }

    abstract class using_threaded_pub_sub : using_threaded_socket_pair
    {
        static using_threaded_pub_sub()
        {
            createSender = () => zmqContext.CreateSocket(SocketType.PUB);
            createReceiver = () => zmqContext.CreateSocket(SocketType.SUB);
        }
    }

    abstract class using_threaded_socket_pair
    {
        private static readonly ManualResetEvent receiverReady = new ManualResetEvent(false);

        protected static Func<Socket> createSender;
        protected static Func<Socket> createReceiver;

        protected static Socket sender;
        protected static Socket receiver;
        protected static Context zmqContext;

        protected static Action<Socket> senderInit;
        protected static Action<Socket> senderAction;
        protected static Action<Socket> receiverInit;
        protected static Action<Socket> receiverAction;

        private static Thread receiverThread;
        private static Thread senderThread;

        Establish context = () =>
        {
            zmqContext = Context.Create();
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
            zmqContext.Dispose();
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
