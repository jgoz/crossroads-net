﻿namespace CrossroadsIO.Specifications.DeviceSpecs
{
    using System;

    using CrossroadsIO.Devices;
    using CrossroadsIO.Specifications;

    using Machine.Specifications;

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_single_message_in_blocking_mode : using_streamer_device
    {
        protected static Frame message;
        protected static SendStatus sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.SendFrame(Messages.SingleMessage);
            receiverAction = rep => message = rep.ReceiveFrame();
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageReceived> successfully_sent_single_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_single_message_with_an_ample_timeout : using_streamer_device
    {
        protected static Frame message;
        protected static SendStatus sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.SendFrame(Messages.SingleMessage, TimeSpan.FromMilliseconds(2000));
            receiverAction = rep => message = rep.ReceiveFrame(TimeSpan.FromMilliseconds(2000));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageReceived> successfully_sent_single_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_receive_a_single_message_with_insufficient_timeout : using_streamer_device
    {
        protected static Frame message;

        Establish context = () =>
        {
            receiverAction = rep => message = rep.ReceiveFrame(TimeSpan.FromMilliseconds(0));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageNotReceived> receiver_must_try_again;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_multipart_message_in_blocking_mode : using_streamer_device
    {
        protected static Message message;
        protected static SendStatus sendResult1;
        protected static SendStatus sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendFrame(Messages.MultiFirst);
                sendResult2 = req.SendFrame(Messages.MultiLast);
            };

            receiverAction = rep =>
            {
                message = rep.ReceiveMessage();
            };
        };

        Because of = StartThreads;

        Behaves_like<CompleteMessageReceived> successfully_sent_multi_part_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_multipart_message_with_an_ample_timeout : using_streamer_device
    {
        protected static Message message;
        protected static SendStatus sendResult1;
        protected static SendStatus sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendFrame(Messages.MultiFirst, TimeSpan.FromMilliseconds(2000));
                sendResult2 = req.SendFrame(Messages.MultiLast, TimeSpan.FromMilliseconds(2000));
            };

            receiverAction = rep =>
            {
                message = new Message(new[]
                {
                    rep.ReceiveFrame(TimeSpan.FromMilliseconds(2000)),
                    rep.ReceiveFrame(TimeSpan.FromMilliseconds(2000))
                });
            };
        };

        Because of = StartThreads;

        Behaves_like<CompleteMessageReceived> sends_multi_part_message_successfully;
    }

    abstract class using_streamer_device : using_threaded_device<StreamerDevice>
    {
        static using_streamer_device()
        {
            createSender = () => ctx.CreateSocket(SocketType.PUSH);
            createReceiver = () => ctx.CreateSocket(SocketType.PULL);
            createDevice = () => new StreamerDevice(ctx, FrontendAddr, BackendAddr, DeviceMode.Blocking);
        }
    }
}
