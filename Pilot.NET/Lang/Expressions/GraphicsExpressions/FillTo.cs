namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// FillTo graphics expression
    /// </summary>
    class FillTo : IGraphicsExpression
    {

        /// <summary>
        /// The location to fill to, x coordinate
        /// </summary>
        public INumericExpression FillToX { get; private set; }

        /// <summary>
        /// The location to fill to, y coordinate
        /// </summary>
        public INumericExpression FillToY { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fillToX">x</param>
        /// <param name="fillToY">y</param>
        public FillTo(INumericExpression fillToX, INumericExpression fillToY)
        {
            if ((fillToX == null) || (fillToY == null))
            {
                throw new InvalidSyntax("Cannot have a null coordiante in FILLTO expression");
            }
            this.FillToX = fillToX;
            this.FillToY = fillToY;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.FILLTO.ToString(), this.FillToX.ToString(), this.FillToY.ToString());
        }
    }
}
