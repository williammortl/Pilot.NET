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
        public INumericExpression DrawDistance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drawDistance">distance</param>
        public Draw(INumericExpression drawDistance)
        {
            if (drawDistance == null)
            {
                throw new InvalidSyntax("Cannot have a null distance in DRAW expression");
            }
            this.DrawDistance = drawDistance;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.DRAW.ToString(), this.DrawDistance.ToString());
        }
    }
}
