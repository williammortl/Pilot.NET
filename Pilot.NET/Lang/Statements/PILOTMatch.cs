namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A match statement, M
    /// </summary>
    public sealed class PILOTMatch : IStatement
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
        /// The strings to match
        /// </summary>
        public LinkedList<StringLiteral> Conditions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditions">the conditions to match</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public PILOTMatch(LinkedList<StringLiteral> conditions, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // var init
            this.Keyword = Keywords.M;
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
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public PILOTMatch(PILOTMatch toDup)
        {

            // deep copy
            this.Keyword = toDup.Keyword;
            this.MatchType = toDup.MatchType;
            this.IfCondition = (BooleanCondition)((toDup.IfCondition == null) ? null : toDup.IfCondition.Copy());
            this.Conditions = new LinkedList<StringLiteral>();

            // verify at least 1 condition is present
            if ((toDup.Conditions == null) || (toDup.Conditions.Count < 1))
            {
                throw new InvalidSyntax("Match needs to have at least 1 condition to match");
            }

            // deep copy conditions
            foreach (StringLiteral condition in toDup.Conditions)
            {
                this.Conditions.AddLast((StringLiteral)condition.Copy());
            }
        }

        /// <summary>
        /// Returns a copy of the match
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new PILOTMatch(this);
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

            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, conditionsString);
        }
    }
}
