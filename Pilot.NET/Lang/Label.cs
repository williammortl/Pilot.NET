namespace Pilot.NET.Lang
{
    using Pilot.NET.Exception;
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
                if (String.IsNullOrWhiteSpace(this.labelName) == true)
                {
                    throw new InvalidSyntax("Cannot define a Label with an empty string");
                }

                // check for leading *
                if (this.labelName.StartsWith("*") == true)
                {
                    this.labelName = this.labelName.Substring(1);
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
            return "*" + this.LabelName;
        }
    }
}
