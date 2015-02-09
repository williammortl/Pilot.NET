namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// An end statement, E
    /// </summary>
    public sealed class End : IStatement
    {

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
        /// Constructor
        /// </summary>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public End(MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.Keyword = Keywords.E;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public End(End toDup)
        {
            this.Keyword = toDup.Keyword;
            this.MatchType = toDup.MatchType;
            this.IfCondition = (BooleanCondition)((toDup.IfCondition == null) ? null : toDup.IfCondition.Copy());
        }

        /// <summary>
        /// Returns a copy of the end
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new End(this);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the end</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, String.Empty);
        }
    }
}
