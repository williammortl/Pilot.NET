namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// TurnTo graphics expression
    /// </summary>
    class TurnTo : IGraphicsExpression
    {

        /// <summary>
        /// The angle to turn to
        /// </summary>
        public INumericExpression TurnToAngle { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="turnToAngle">angle</param>
        public TurnTo(INumericExpression turnToAngle)
        {
            if (turnToAngle == null)
            {
                throw new InvalidSyntax("Cannot have a null angle in TURNTO expression");
            }
            this.TurnToAngle = turnToAngle;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.TURNTO.ToString(), this.TurnToAngle.ToString());
        }
    }
}
