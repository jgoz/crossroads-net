﻿namespace CrossroadsIO.Specifications.SocketSpecs
{
    using System;
    using System.Threading;

    using CrossroadsIO.Specifications;

    using Machine.Specifications;

    [Subject("Subscribe")]
    class when_subscribing_to_a_specific_prefix : using_threaded_pub_sub
    {
        protected static Frame message1;
        protected static Frame message2;
        protected static SendStatus sendResult1;
        protected static SendStatus sendResult2;

        Establish context = () =>
        {
            var signal = new ManualResetEvent(false);

            receiverInit = sub => sub.Subscribe(Messages.PubSubPrefix);

            receiverAction = sub =>
            {
                signal.Set();

                message1 = sub.ReceiveFrame();
                message2 = sub.ReceiveFrame(TimeSpan.FromMilliseconds(500));
            };

            senderInit = pub => signal.WaitOne(1000);

            senderAction = pub =>
            {
                sendResult1 = pub.SendFrame(Messages.PubSubFirst);
                sendResult2 = pub.SendFrame(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveFirst> successfully_received_first_message_and_filtered_out_second;
    }

    [Subject("Subscribe")]
    class when_subscribing_to_all_prefixes : using_threaded_pub_sub
    {
        protected static Frame message1;
        protected static Frame message2;
        protected static SendStatus sendResult1;
        protected static SendStatus sendResult2;

        Establish context = () =>
        {
            var signal = new ManualResetEvent(false);

            receiverInit = sub => sub.Subscribe(new byte[0]);

            receiverAction = sub =>
            {
                signal.Set();

                message1 = sub.ReceiveFrame();
                message2 = sub.ReceiveFrame(TimeSpan.FromMilliseconds(500));
            };

            senderInit = pub => signal.WaitOne(1000);

            senderAction = pub =>
            {
                sendResult1 = pub.SendFrame(Messages.PubSubFirst);
                sendResult2 = pub.SendFrame(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveAll> successfully_received_all_messages;
    }
}
