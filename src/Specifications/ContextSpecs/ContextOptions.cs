namespace CrossroadsIO.Specifications.ContextSpecs
{
    using System;

    using Machine.Specifications;

    [Subject("Context options")]
    class when_setting_the_max_sockets_context_option : using_ctx
    {
        Because of = () =>
            exception = Catch.Exception(() => ctx.MaxSockets = 2);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            ctx.MaxSockets.ShouldEqual(2);
    }

    [Subject("Context options")]
    class when_setting_the_thread_pool_size_context_option : using_ctx
    {
        Because of = () =>
            exception = Catch.Exception(() => ctx.ThreadPoolSize = 2);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_return_the_given_value = () =>
            ctx.ThreadPoolSize.ShouldEqual(2);
    }

    abstract class using_ctx
    {
        protected static Context ctx;
        protected static Exception exception;

        Establish context = () =>
        {
            ctx = Context.Create();
        };

        Cleanup resources = () => ctx.Dispose();
    }
}
