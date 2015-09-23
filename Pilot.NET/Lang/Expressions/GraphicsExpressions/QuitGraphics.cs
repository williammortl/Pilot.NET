namespace Pilot.NET.Lang.Expressions.GraphicsExpressions
{
    using Pilot.NET.Lang.Enums;
    using System;

    /// <summary>
    /// Quit graphics expression
    /// </summary>
    class QuitGraphics : IGraphicsExpression
    {

        /// <summary>
        /// overrides ToString
        /// </summary>
        /// <returns>the graphics expression as a string</returns>
        public override string ToString()
        {
            return String.Format("{0}", GraphicsExpressionKeywords.QUIT.ToString());
        }
    }
}
