namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Exception;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// A jump statement, J
    /// </summary>
    internal sealed class Jump : IStatement
    {

        /// <summary>
        /// The label to jump to
        /// </summary>
        private Label labelToJumpTo;

        /// <summary>
        /// The type of command
        /// </summary>
        public Keywords Keyword { get; private set; }

        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// The label to jump to
        /// </summary>
        public Label LabelToJumpTo 
        {
            get
            {
                return this.labelToJumpTo;
            }
            private set
            {

                // check to make sure that the label is not null
                if (value == null)
                {
                    throw new InvalidSyntax("Jump statemnt must contain a label");
                }

                // assign
                this.labelToJumpTo = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelToJumpTo">the label to jump to</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Jump(Label labelToJumpTo, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.Keyword = Keywords.J;
            this.MatchType = matchType;
            this.IfCondition = ifCondition; 
            this.LabelToJumpTo = labelToJumpTo;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the jump</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, this.LabelToJumpTo.ToString());
        }
    }
}
