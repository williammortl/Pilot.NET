namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Fill graphics expression
    /// </summary>
    class Fill : IGraphicsExpression
    {

        /// <summary>
        /// The length to draw
        /// </summary>
        public INumericExpression FillDistance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fillDistance">distance</param>
        public Fill(INumericExpression fillDistance)
        {
            if (fillDistance == null)
            {
                throw new InvalidSyntax("Cannot have a null distance in FILL expression");
            }
            this.FillDistance = fillDistance;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.FILL.ToString(), this.FillDistance.ToString());
        }
    }
}
