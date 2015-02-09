namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// A jump statement, J
    /// </summary>
    public sealed class Jump : IStatement
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
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public Jump(Jump toDup)
        {
            this.Keyword = toDup.Keyword;
            this.MatchType = toDup.MatchType;
            this.IfCondition = (BooleanCondition)((toDup.IfCondition == null) ? null : toDup.IfCondition.Copy());
            this.LabelToJumpTo = (Label)((toDup.LabelToJumpTo == null) ? null : toDup.LabelToJumpTo.Copy());
        }

        /// <summary>
        /// Returns a copy of the jump
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new Jump(this);
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
