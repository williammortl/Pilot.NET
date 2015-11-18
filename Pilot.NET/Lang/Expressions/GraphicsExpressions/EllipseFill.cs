namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// Draws an ellipse, and fills it graphics expression
    /// </summary>
    class EllipseFill : IGraphicsExpression
    {

        /// <summary>
        /// The horizontal radius of the ellipse
        /// </summary>
        public INumericExpression HorizontalRadius { get; private set; }

        /// <summary>
        /// The vertical radius of the ellipse
        /// </summary>
        public INumericExpression VerticalRadius { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="horizontalRadius">the horizontal radius</param>
        /// <param name="verticalRadius">the vertical radius</param>
        public EllipseFill(INumericExpression horizontalRadius, INumericExpression verticalRadius)
        {
            if ((horizontalRadius == null) || (verticalRadius == null))
            {
                throw new InvalidSyntax("Cannot have a null radius in an ELLIPSEFILL expression");
            }
            this.HorizontalRadius = horizontalRadius;
            this.VerticalRadius = verticalRadius;
        }

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", GraphicsExpressionKeywords.ELLIPSEFILL.ToString(), this.HorizontalRadius.ToString(), this.VerticalRadius.ToString());
        }
    }
}
