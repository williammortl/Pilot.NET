namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Draw a box, and fills it graphics expression
    /// </summary>
    class BoxFill : IGraphicsExpression
    {

        /// <summary>
        /// The width of the box
        /// </summary>
        public INumericExpression BoxWidth { get; private set; }

        /// <summary>
        /// The height of the box
        /// </summary>
        public INumericExpression BoxHeight { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boxWidth">the width of the box</param>
        /// <param name="boxHeight">the height of the box</param>
        public BoxFill(INumericExpression boxWidth, INumericExpression boxHeight)
        {
            if ((boxWidth == null) || (boxHeight == null))
            {
                throw new InvalidSyntax("Cannot have a null width or height in a BOXFILL expression");
            }
            this.BoxWidth = boxWidth;
            this.BoxHeight = boxHeight;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.BOXFILL.ToString(), this.BoxWidth.ToString(), this.BoxHeight.ToString());
        }
    }
}
