namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// A use statement, U
    /// </summary>
    internal sealed class Use : IStatement
    {

        /// <summary>
        /// The label to use
        /// </summary>
        private Label labelToUse;

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
        /// The label to use
        /// </summary>
        public Label LabelToUse 
        {
            get
            {
                return this.labelToUse;
            }
            private set
            {

                // make sure label isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Use statement cannot have a null label");
                }

                this.labelToUse = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LabelToJumpTo">the label to use</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Use(Label labelToUse, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.Keyword = Keywords.U;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.LabelToUse = labelToUse;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the use</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, this.LabelToUse.ToString());
        }
    }
}
