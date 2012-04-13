namespace CrossroadsIO
{
    /// <summary>
    /// Specifies the protocols that a <see cref="Socket"/> supports.
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// Both Internet Protocol versions 4 and 6.
        /// </summary>
        Both = 0,

        /// <summary>
        /// Internet Protocol version 4.
        /// </summary>
        Ipv4Only = 1
    }
}
