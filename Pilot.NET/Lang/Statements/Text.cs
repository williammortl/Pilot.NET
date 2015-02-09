namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;

    /// <summary>
    /// A display text statement, T
    /// </summary>
    public sealed class Text : IStatement
    {

        /// <summary>
        /// The text to display
        /// </summary>
        private StringLiteral textToDisplay;

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
        /// Add a carriage return after printing the text
        /// </summary>
        public Boolean CarriageReturn { get; private set; }

        /// <summary>
        /// The text to display
        /// </summary>
        public StringLiteral TextToDisplay 
        {
            get
            {
                return this.textToDisplay;
            }
            private set
            {

                // throw error if null
                if (value == null)
                {
                    throw new InvalidSyntax("Cannot pass null string literal to Text statement");
                }

                // check for carriage return
                if (value.StringText.EndsWith("\\") == true)
                {
                    String newText = value.StringText;
                    newText = newText.Substring(0, newText.Length - 1);
                    this.textToDisplay = new StringLiteral(newText);
                    this.CarriageReturn = true;
                }
                else
                {
                    this.textToDisplay = value;
                    this.CarriageReturn = false;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">the text of the parameters</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Text(StringLiteral text, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // attribute init
            this.Keyword = Keywords.T;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.TextToDisplay = text;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public Text(Text toDup)
        {
            this.Keyword = toDup.Keyword;
            this.MatchType = toDup.MatchType;
            this.IfCondition = (BooleanCondition)((toDup.IfCondition == null) ? null : toDup.IfCondition.Copy());
            this.TextToDisplay = (StringLiteral)((toDup.TextToDisplay == null) ? null : toDup.TextToDisplay.Copy());
        }

        /// <summary>
        /// Returns a copy of the text display
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new Text(this);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the display text</returns>
        public override String ToString()
        {

            // var init
            String textToString = this.TextToDisplay.ToString() + ((this.CarriageReturn == true) ? " \\" : String.Empty);

            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, textToString);
        }
    }
}
