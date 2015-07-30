namespace Pilot.NET.PILOTExceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the exception thrown if an error occurs at run time
    /// </summary>
    [Serializable]
    public class RunTimeException : PILOTException
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        protected RunTimeException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RunTimeException()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public RunTimeException(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public RunTimeException(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}
