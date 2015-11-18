namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Prints text graphics expression
    /// </summary>
    class Print : IGraphicsExpression
    {

        /// <summary>
        /// The string variable to print
        /// </summary>
        public StringVariable TextToPrint { get; private set; }

        /// <summary>
        /// The size of the text
        /// </summary>
        public INumericExpression TextSize { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="textToPrint">the string variable to print</param>
        /// <param name="textSize">the size of the text</param>
        public Print(StringVariable textToPrint, INumericExpression textSize)
        {
            if ((textToPrint == null) || (textSize == null))
            {
                throw new InvalidSyntax("Cannot have null text or a null text size in a PRINT expression");
            }
            this.TextToPrint = textToPrint;
            this.TextSize = textSize;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.PRINT.ToString(), this.TextToPrint.ToString(), this.TextSize.ToString());
        }
    }
}
