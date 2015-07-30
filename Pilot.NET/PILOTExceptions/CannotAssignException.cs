namespace Pilot.NET.PILOTExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the exception thrown if you try to assign to anything other than a variable
    /// </summary>
    [Serializable]
    public sealed class CannotAssignException : ParserException
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        private CannotAssignException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CannotAssignException()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public CannotAssignException(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public CannotAssignException(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}
