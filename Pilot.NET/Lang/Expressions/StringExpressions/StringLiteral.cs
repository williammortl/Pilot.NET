namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a string expression literal, can contain string variables inside which are evaluated at run time
    /// </summary>
    internal sealed class StringLiteral : IStringExpression
    {

        /// <summary>
        /// The string text
        /// </summary>
        public String StringText { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stringText">the string text</param>
        public StringLiteral(String stringText)
        {
            this.StringText = stringText.TrimEnd();
        }

        /// <summary>
        /// Convert this string variable to a string
        /// </summary>
        /// <returns>the PILOT string</returns>
        public override string ToString()
        {
            return this.StringText;
        }
    }
}
