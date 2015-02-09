namespace Pilot.NET.Lang
{
    using Pilot.NET.Lang.Exceptions;
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
        public Label(String labelName)
        {
            
            // var init
            this.LabelName = labelName;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the label to duplicate</param>
        public Label(Label toDup)
        {
            this.LabelName = toDup.LabelName;
        }

        /// <summary>
        /// Create a copy of this label
        /// </summary>
        /// <returns>the copy</returns>
        public Label Copy()
        {
            return new Label(this);
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
