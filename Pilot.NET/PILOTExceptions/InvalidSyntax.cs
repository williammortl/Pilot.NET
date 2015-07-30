namespace Pilot.NET.PILOTExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the exception thrown if invalid syntax is detected
    /// </summary>
    [Serializable]
    public sealed class InvalidSyntax : ParserException
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        private InvalidSyntax(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidSyntax()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public InvalidSyntax(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public InvalidSyntax(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}
