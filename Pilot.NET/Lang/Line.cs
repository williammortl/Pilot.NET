namespace Pilot.NET.Lang
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Statements;
    using System;
    
    /// <summary>
    /// Represents a line of the PILOT programming language
    /// </summary>
    public sealed class Line : IExecutable
    {

        /// <summary>
        /// The label for the line, null if no label
        /// </summary>
        public Label LineLabel { get; private set; }

        /// <summary>
        /// The PILOT statement for the line, can be null if the line is just a label
        /// </summary>
        internal IStatement LineStatement { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lineLabel">the label</param>
        /// <param name="lineStatement">what statement, can be null if this is just a label</param>
        internal Line(Label lineLabel, IStatement lineStatement)
        {

            // var init
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
                    retVal = String.Format("{0}", this.LineLabel);
                }
                else
                {
                    retVal = String.Format("{0} {1}", this.LineLabel, this.LineStatement.ToString());
                }
            }
            else
            {
                retVal = String.Format("{0}", this.LineStatement.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Executes an action upon the state using the interpreter,
        /// throws a PILOTException if an error occurs
        /// </summary>
        /// <param name="state">interpreter</param>
        public void Execute(IPILOTState state)
        {
            throw new NotImplementedException();
        }
    }
}
