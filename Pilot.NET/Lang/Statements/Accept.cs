namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// An accept statement, A
    /// </summary>
    internal sealed class Accept : IStatement
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
        /// The variable to receive the value, can be null
        /// </summary>
        public IVariable VariableToSet { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="variableToSet">the variable to set, can be null</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Accept(IVariable variableToSet, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.Keyword = Keywords.A;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.VariableToSet = variableToSet;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the accept</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, this.VariableToSet.ToString());
        }
    }
}
