namespace Pilot.NET.Lang
{
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Statements;
    using System;
    
    /// <summary>
    /// Represents a line of the PILOT programming language
    /// </summary>
    public sealed class Line
    {

        /// <summary>
        /// The line number
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// The label for the line, null if no label
        /// </summary>
        public Label LineLabel { get; private set; }

        /// <summary>
        /// The PILOT statement for the line, can be null if the line is just a label
        /// </summary>
        public IStatement LineStatement { get; private set; }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">line to duplicate</param>
        public Line(Line toDup)
        {

            // deep copy
            this.LineNumber = toDup.LineNumber;
            this.LineLabel = (toDup.LineLabel == null) ? null : toDup.LineLabel.Copy();
            this.LineStatement = (toDup.LineStatement == null) ? null : toDup.LineStatement.Copy();

            // check to ensure that both the label and the statement are not empty
            if ((this.LineLabel == null) && (this.LineStatement == null))
            {
                throw new ParserException("Line must contain either a label or statement (or both)");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lineNumber">what line number</param>
        /// <param name="lineLabel">the label</param>
        /// <param name="lineStatement">what statement, can be null if this is just a label</param>
        public Line(int lineNumber, Label lineLabel, IStatement lineStatement)
        {

            // var init
            this.LineNumber = lineNumber;
            this.LineLabel = lineLabel;
            this.LineStatement = lineStatement;
            
            // check to ensure that both the label and the statement are not empty
            if ((this.LineLabel == null) && (this.LineStatement == null))
            {
                throw new ParserException("Line must contain either a label or statement (or both)");
            }
        }

        /// <summary>
        /// Convert this line to a string
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {

            // var init
            String retVal = String.Empty;

            // convert to string
            if (this.LineLabel != null)
            {
                if (this.LineStatement == null)
                {
                    retVal = String.Format("{0} {1}", this.LineNumber.ToString(), this.LineLabel);
                }
                else
                {
                    retVal = String.Format("{0} {1} {2}", this.LineNumber.ToString(), this.LineLabel, this.LineStatement.ToString());
                }
            }
            else
            {
                retVal = String.Format("{0} {1}", this.LineNumber.ToString(), this.LineStatement.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>the copy</returns>
        public Line Copy()
        {
            return new Line(this);
        }
    }
}
