namespace CrossroadsIO.Interop
{
    internal class PollerProxy
    {
        public int Poll(PollItem[] pollItems, int timeoutMilliseconds)
        {
            return LibXs.xs_poll(pollItems, pollItems.Length, timeoutMilliseconds);
        }
    }
}
