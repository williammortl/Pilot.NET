namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using System;

    /// <summary>
    /// Clear the text screen
    /// </summary>
    internal sealed class TextClear : IStringExpression
    {

        /// <summary>
        /// The text which causes the screen to clear
        /// </summary>
        public const String CLEAR_TEXT = "{CLEAR}";

        /// <summary>
        /// Constructor
        /// </summary>
        public TextClear()
        {

            // do nothing
        }

        /// <summary>
        /// Convert this string variable to a string
        /// </summary>
        /// <returns>the PILOT string</returns>
        public override string ToString()
        {
            return TextClear.CLEAR_TEXT;
        }
    }
}
