namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Pen graphics expression
    /// </summary>
    class PILOTPen : IGraphicsExpression
    {

        /// <summary>
        /// The pen color to draw with
        /// </summary>
        public PenColors PenColor { get; private set; }

        /// <summary>
        /// The expression for calculating the pen color
        /// </summary>
        public INumericExpression PenColorExpression { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="penColor">the pen color to draw with</param>
        public PILOTPen(PenColors penColor)
        {
            this.PenColor = penColor;
            this.PenColorExpression = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="penColorExpression">the numerical expression to set the pen color</param>
        public PILOTPen(INumericExpression penColorExpression)
        {

            // check to make sure not numm
            if (penColorExpression == null)
            {
                throw new InvalidSyntax("Pen color cannot be a null expression");
            }

            // set values
            this.PenColor = PenColors.ERASE;
            this.PenColorExpression = penColorExpression;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            String color = (this.PenColorExpression == null) ? this.PenColor.ToString() : this.PenColorExpression.ToString();
            return String.Format("{0} {1}", GraphicsExpressionKeywords.PEN.ToString(), color);
        }
    }
}
