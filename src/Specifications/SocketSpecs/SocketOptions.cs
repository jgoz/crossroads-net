﻿namespace CrossroadsIO.Specifications.SocketSpecs
{
    using System;

    using CrossroadsIO.Specifications;

    using Machine.Specifications;

    [Subject("Socket options")]
    class when_setting_the_affinity_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.Affinity = 0x03ul);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.Affinity.ShouldEqual(0x03ul);
    }

    [Subject("Socket options")]
    class when_setting_the_backlog_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.Backlog = 6);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.Backlog.ShouldEqual(6);
    }

    [Subject("Socket options")]
    class when_setting_the_identity_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.Identity = Messages.Identity);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.Identity.ShouldEqual(Messages.Identity);
    }

    [Subject("Socket options")]
    class when_setting_the_keepalive_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.KeepAlive = true);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.KeepAlive.ShouldBeTrue();
    }

    [Subject("Socket options")]
    class when_setting_the_linger_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.Linger = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.Linger.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_max_message_size_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.MaxMessageSize = 60000L);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.MaxMessageSize.ShouldEqual(60000L);
    }

    [Subject("Socket options")]
    class when_setting_the_multicast_hops_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.MulticastHops = 6);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.MulticastHops.ShouldEqual(6);
    }

    [Subject("Socket options")]
    class when_setting_the_multicast_rate_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.MulticastRate = 60);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.MulticastRate.ShouldEqual(60);
    }

    [Subject("Socket options")]
    class when_setting_the_multicast_recovery_interval_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.MulticastRecoveryInterval = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.MulticastRecoveryInterval.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_receive_buffer_size_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.ReceiveBufferSize = 10000);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.ReceiveBufferSize.ShouldEqual(10000);
    }

    [Subject("Socket options")]
    class when_setting_the_receive_high_watermark_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.ReceiveHighWatermark = 100);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.ReceiveHighWatermark.ShouldEqual(100);
    }

    [Subject("Socket options")]
    class when_setting_the_receive_timeout_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.ReceiveTimeout = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.ReceiveTimeout.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_reconnect_interval_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.ReconnectInterval = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.ReconnectInterval.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_reconnect_interval_max_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.ReconnectIntervalMax = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.ReconnectIntervalMax.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_send_buffer_size_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.SendBufferSize = 10000);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.SendBufferSize.ShouldEqual(10000);
    }

    [Subject("Socket options")]
    class when_setting_the_send_high_watermark_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.SendHighWatermark = 100);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.SendHighWatermark.ShouldEqual(100);
    }

    [Subject("Socket options")]
    class when_setting_the_send_timeout_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.SendTimeout = TimeSpan.FromMilliseconds(333));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.SendTimeout.ShouldEqual(TimeSpan.FromMilliseconds(333));
    }

    [Subject("Socket options")]
    class when_setting_the_supported_protocol_socket_option : using_req
    {
        Because of = () =>
            exception = Catch.Exception(() => socket.SupportedProtocol = ProtocolType.Both);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            socket.SupportedProtocol.ShouldEqual(ProtocolType.Both);
    }
}
