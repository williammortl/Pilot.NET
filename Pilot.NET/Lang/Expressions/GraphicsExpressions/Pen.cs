namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using System;

    /// <summary>
    /// Pen graphics expression
    /// </summary>
    class Pen : IGraphicsExpression
    {

        /// <summary>
        /// The pen color to draw with
        /// </summary>
        public PenColors PenColor { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PenColor">the pen color to draw with</param>
        public Pen(PenColors PenColor)
        {
            this.PenColor = PenColor;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.PEN.ToString(), this.PenColor.ToString());
        }
    }
}
