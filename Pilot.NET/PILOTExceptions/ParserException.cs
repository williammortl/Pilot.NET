namespace Pilot.NET.PILOTExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the exception thrown if there is an error parsing the file
    /// </summary>
    [Serializable]
    public class ParserException : PILOTException
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        protected ParserException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParserException()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public ParserException(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public ParserException(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}
