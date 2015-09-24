namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Turn graphics expression
    /// </summary>
    class Turn : IGraphicsExpression
    {

        /// <summary>
        /// The angle to turn to
        /// </summary>
        public INumericExpression TurnAngle { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="turnAngle">x</param>
        public Turn(INumericExpression turnAngle)
        {
            if (turnAngle == null)
            {
                throw new InvalidSyntax("Cannot have a null turn angle in TURN expression");
            }
            this.TurnAngle = turnAngle;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", GraphicsExpressionKeywords.TURN.ToString(), this.TurnAngle.ToString());
        }
    }
}
