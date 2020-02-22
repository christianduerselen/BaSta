using System;

namespace BaSta.TimeSync
{
    /// <summary>
    /// Represents errors that are represented with a return code for the application execution.
    /// </summary>
    /// <seealso cref="Exception" />
    public class ReturnCodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnCodeException"/> class.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        public ReturnCodeException(int returnCode)
        {
            ReturnCode = returnCode;
        }

        /// <summary>
        /// Gets the return code.
        /// </summary>
        /// <value>The return code.</value>
        public int ReturnCode { get; }
    }
}