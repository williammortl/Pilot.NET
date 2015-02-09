namespace Pilot.NET.Lang.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This is the master exception that all PILOT exceptions derive from
    /// </summary>
    [Serializable]
    public class PILOTException : Exception
    {

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="sc">StreamingContext</param>
        protected PILOTException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

            // do nothing
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PILOTException()
            : base()
        {

            // do nothing
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="theException">exception text</param>
        public PILOTException(String theException)
            : base(theException)
        {

            // do nothing
        }

        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="theException">the exception text</param>
        /// <param name="innerException">inner exception</param>
        public PILOTException(String theException, Exception innerException)
            : base(theException, innerException)
        {

            // do nothing
        }
    }
}