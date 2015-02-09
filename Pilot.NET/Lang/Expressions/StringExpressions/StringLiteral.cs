namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is a string expression literal, can contain string variables inside which are evaluated at run time
    /// </summary>
    public sealed class StringLiteral : IStringExpression
    {

        /// <summary>
        /// The type of expression
        /// </summary>
        public ExpressionTypes TypeOfExpression { get; private set; }

        /// <summary>
        /// The type of string expression
        /// </summary>
        public StringExpressionTypes TypeOfStringExpression { get; private set; }

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
            this.TypeOfExpression = ExpressionTypes.StringExpression;
            this.TypeOfStringExpression = StringExpressionTypes.StringLiteral;
            this.StringText = stringText.TrimEnd();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the string to duplicate</param>
        public StringLiteral(StringLiteral toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfStringExpression = toDup.TypeOfStringExpression;
            this.StringText = toDup.StringText.TrimEnd();
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT string</returns>
        public IExpression Copy()
        {
            return new StringLiteral(this);
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
