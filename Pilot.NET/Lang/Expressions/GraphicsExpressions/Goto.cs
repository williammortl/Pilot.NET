namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Goto graphics expression
    /// </summary>
    class Goto : IGraphicsExpression
    {

        /// <summary>
        /// The location to go to, x coordinate
        /// </summary>
        public INumericExpression GotoX { get; private set; }

        /// <summary>
        /// The location to go to, y coordinate
        /// </summary>
        public INumericExpression GotoY { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gotoX">x</param>
        /// <param name="gotoY">y</param>
        public Goto(INumericExpression gotoX, INumericExpression gotoY)
        {
            if ((gotoX == null) || (gotoY == null))
            {
                throw new InvalidSyntax("Cannot have a null coordiante in GOTO expression");
            }
            this.GotoX = gotoX;
            this.GotoY = gotoY;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.GOTO.ToString(), this.GotoX.ToString(), this.GotoY.ToString());
        }
    }
}
