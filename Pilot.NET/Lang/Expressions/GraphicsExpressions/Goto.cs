namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;

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
        /// <param name="gotoX"></param>
        /// <param name="gotoY"></param>
        public Goto(INumericExpression gotoX, INumericExpression gotoY)
        {
            if ((gotoX == null) || (gotoY == null))
            {
                throw new InvalidSyntax("Cannot have a null coordiante in GOTO expression");
            }
            this.GotoX = gotoX;
            this.GotoY = gotoX;
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
