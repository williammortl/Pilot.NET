namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Width graphics expression
    /// </summary>
    class Width : IGraphicsExpression
    {

        /// <summary>
        /// The pen width
        /// </summary>
        public INumericExpression PenWidth { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="penWidth">the width of the pen</param>
        public Width(INumericExpression penWidth)
        {
            if (penWidth == null)
            {
                throw new InvalidSyntax("Cannot have a null pen width in WIDTH expression");
            }
            this.PenWidth = penWidth;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.WIDTH.ToString(), this.PenWidth.ToString());
        }
    }
}
