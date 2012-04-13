namespace CrossroadsIO
{
    using System;
    using System.Runtime.Serialization;

    using CrossroadsIO.Interop;

    /// <summary>
    /// An exception thrown by the result of a Crossroads I/O library call.
    /// </summary>
    [Serializable]
    public class XsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        public XsException(int errorCode)
        {
            ErrorCode = errorCode;
            ErrorName = GetErrorName(errorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        /// <param name="message">The message that describes the error</param>
        public XsException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorName = GetErrorName(errorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the Crossroads I/O library call.</param>
        /// <param name="message">The message that describes the error</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public XsException(int errorCode, string message, Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
            ErrorName = GetErrorName(errorCode);
        }

        internal XsException(ErrorDetails errorDetails)
            : this(errorDetails.ErrorCode, errorDetails.Message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected XsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the error code returned by the Crossroads I/O library call.
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Gets the string representation of the error code, as found in the Crossroads I/O docs.
        /// </summary>
        public string ErrorName { get; private set; }

        private static string GetErrorName(int errorCode)
        {
            return CrossroadsIO.ErrorCode.ErrorNames.ContainsKey(errorCode)
                       ? CrossroadsIO.ErrorCode.ErrorNames[errorCode]
                       : "Error " + errorCode;
        }
    }
}
