namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A match statement, M
    /// </summary>
    internal sealed class PILOTMatch : IStatement
    {

        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// The strings to match
        /// </summary>
        public List<StringLiteral> Conditions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditions">the conditions to match</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public PILOTMatch(List<StringLiteral> conditions, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // var init
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.Conditions = conditions;

            // verify at least 1 condition is present
            if ((this.Conditions == null) || (this.Conditions.Count < 1))
            {
                throw new InvalidSyntax("Match needs to have at least 1 condition to match");
            }
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the match</returns>
        public override String ToString()
        {

            // loop through conditions
            String conditionsString = String.Empty;
            foreach (StringLiteral condition in this.Conditions)
            {
                conditionsString += " " + condition.ToString() + ",";
            }
            conditionsString = conditionsString.Substring(0, conditionsString.Length - 1);

            return StatementMethods.StatementToString(Keywords.M, this.MatchType, this.IfCondition, conditionsString);
        }
    }
}
