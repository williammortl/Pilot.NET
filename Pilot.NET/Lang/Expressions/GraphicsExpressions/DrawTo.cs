namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// DrawTo graphics expression
    /// </summary>
    class DrawTo : IGraphicsExpression
    {

        /// <summary>
        /// The location to draw to, x coordinate
        /// </summary>
        public INumericExpression DrawToX { get; private set; }

        /// <summary>
        /// The location to draw to, y coordinate
        /// </summary>
        public INumericExpression DrawToY { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drawToX">x</param>
        /// <param name="drawToY">y</param>
        public DrawTo(INumericExpression drawToX, INumericExpression drawToY)
        {
            if ((drawToX == null) || (drawToY == null))
            {
                throw new InvalidSyntax("Cannot have a null coordiante in DRAWTO expression");
            }
            this.DrawToX = drawToX;
            this.DrawToY = drawToY;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.DRAWTO.ToString(), this.DrawToX.ToString(), this.DrawToY.ToString());
        }
    }
}
