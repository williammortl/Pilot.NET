namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;

    /// <summary>
    /// A display text statement, T
    /// </summary>
    internal sealed class Text : IImmediateStatement
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
        /// The text to display
        /// </summary>
        public IStringExpression TextToDisplay { get; private set;  }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">the text of the parameters</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Text(IStringExpression text, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // attribute init
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.TextToDisplay = text;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the display text</returns>
        public override String ToString()
        {

            // var init
            String textToString = (this.TextToDisplay == null) ? String.Empty : this.TextToDisplay.ToString();

            return StatementMethods.StatementToString(Keywords.T, this.MatchType, this.IfCondition, textToString);
        }
    }
}
