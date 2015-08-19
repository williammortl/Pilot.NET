namespace Pilot.NET.Lang
{
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This represents a label to jump to
    /// </summary>
    public sealed class Label
    {

        /// <summary>
        /// The label name
        /// </summary>
        private String labelName;

        /// <summary>
        /// The label name
        /// </summary>
        public String LabelName 
        {
            get
            {
                return this.labelName;
            }
            private set
            {

                // throw an error if string is empty
                this.labelName = value.Trim();
                if ((String.IsNullOrWhiteSpace(this.labelName) == true) || (this.labelName.StartsWith("*") == false))
                {
                    throw new InvalidSyntax("Cannot define a Label with an empty string");
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelName">the label name</param>
        internal Label(String labelName)
        {
            
            // var init
            this.LabelName = labelName;
        }

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns>the string representation of the label</returns>
        public override string ToString()
        {
            return this.LabelName;
        }
    }
}
