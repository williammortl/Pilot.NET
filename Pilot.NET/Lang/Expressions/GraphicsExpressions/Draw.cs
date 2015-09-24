namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Draw graphics expression
    /// </summary>
    class Draw : IGraphicsExpression
    {

        /// <summary>
        /// The length to draw
        /// </summary>
        public INumericExpression DrawLength { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drawLength">x</param>
        public Draw(INumericExpression drawLength)
        {
            if (drawLength == null)
            {
                throw new InvalidSyntax("Cannot have a null draw length in DRAW expression");
            }
            this.DrawLength = drawLength;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.DRAW.ToString(), this.DrawLength.ToString());
        }
    }
}
