namespace CrossroadsIO.Specifications.DeviceSpecs
{
    using System;
    using System.Threading;

    using CrossroadsIO.Devices;

    using Machine.Specifications;

    abstract class using_threaded_device<TDevice> where TDevice : Device
    {
        protected const string FrontendAddr = "inproc://dev_frontend";
        protected const string BackendAddr = "inproc://dev_backend";

        protected static Func<TDevice> createDevice;
        protected static Func<Socket> createSender;
        protected static Func<Socket> createReceiver;

        protected static Socket sender;
        protected static Socket receiver;
        protected static TDevice device;
        protected static Context zmqContext;

        protected static Action<TDevice> deviceInit;
        protected static Action<Socket> senderInit;
        protected static Action<Socket> senderAction;
        protected static Action<Socket> receiverInit;
        protected static Action<Socket> receiverAction;

        private static Thread deviceThread;
        private static Thread receiverThread;
        private static Thread senderThread;

        private static ManualResetEvent deviceReady;
        private static ManualResetEvent receiverReady;
        private static ManualResetEvent receiverDone;

        Establish context = () =>
        {
            zmqContext = Context.Create();
            device = createDevice();
            sender = createSender();
            receiver = createReceiver();

            deviceInit = dev => { };
            senderInit = sck => { };
            receiverInit = sck => { };
            senderAction = sck => { };
            receiverAction = sck => { };

            deviceReady = new ManualResetEvent(false);
            receiverReady = new ManualResetEvent(false);
            receiverDone = new ManualResetEvent(false);

            deviceThread = new Thread(() =>
            {
                deviceInit(device);
                device.Initialize();

                deviceReady.Set();

                device.Start();
            });

            receiverThread = new Thread(() =>
            {
                deviceReady.WaitOne();

                receiverInit(receiver);
                receiver.ReceiveHighWatermark = 1;
                receiver.Linger = TimeSpan.Zero;
                receiver.Connect(BackendAddr);

                receiverReady.Set();

                receiverAction(receiver);

                receiverDone.Set();
            });

            senderThread = new Thread(() =>
            {
                receiverReady.WaitOne();

                senderInit(sender);
                sender.SendHighWatermark = 1;
                sender.Linger = TimeSpan.Zero;
                sender.Connect(FrontendAddr);

                device.PollerPulse.WaitOne();

                senderAction(sender);
            });
        };

        Cleanup resources = () =>
        {
            receiverDone.WaitOne();

            deviceReady.Dispose();
            receiverReady.Dispose();
            receiverDone.Dispose();

            if (sender != null)
            {
                sender.Dispose();
            }

            if (receiver != null)
            {
                receiver.Dispose();
            }

            if (device != null)
            {
                device.Dispose();
            }

            if (zmqContext != null)
            {
                zmqContext.Dispose();
            }
        };

        protected static void StartThreads()
        {
            deviceThread.Start();
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

            device.Stop();

            if (!deviceThread.Join(5000))
            {
                deviceThread.Abort();
            }
        }
    }
}
