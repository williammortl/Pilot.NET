namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Go graphics expression
    /// </summary>
    class Go : IGraphicsExpression
    {

        /// <summary>
        /// The distance to go
        /// </summary>
        public INumericExpression GoDistance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="goDistance">distance</param>
        public Go(INumericExpression goDistance)
        {
            if (goDistance == null)
            {
                throw new InvalidSyntax("Cannot have a null distance in GO expression");
            }
            this.GoDistance = goDistance;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.GO.ToString(), this.GoDistance.ToString());
        }
    }
}
