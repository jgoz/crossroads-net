namespace CrossroadsIO.Devices
{
    using System;
    using System.Runtime.Serialization;

    using CrossroadsIO.Interop;

    /// <summary>
    /// The exception that is thrown when a Crossroads I/O device error occurs.
    /// </summary>
    [Serializable]
    public class XsDeviceException : XsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XsDeviceException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        public XsDeviceException(int errorCode)
            : base(errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsDeviceException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        /// <param name="message">The message that describes the error</param>
        public XsDeviceException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsDeviceException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        /// <param name="message">The message that describes the error</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public XsDeviceException(int errorCode, string message, Exception inner)
            : base(errorCode, message, inner)
        {
        }

        internal XsDeviceException(ErrorDetails errorDetails)
            : base(errorDetails)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsDeviceException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected XsDeviceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
