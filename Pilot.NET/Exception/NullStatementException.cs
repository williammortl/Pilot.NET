namespace Pilot.NET.Exception
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the exception thrown if there is a null statement without the line containing a label
    /// </summary>
    [Serializable]
    public sealed class NullStatementException : ParserException
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        private NullStatementException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NullStatementException()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public NullStatementException(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public NullStatementException(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}
