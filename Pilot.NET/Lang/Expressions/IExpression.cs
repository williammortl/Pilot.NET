namespace Pilot.NET.Lang.Expressions
{
    using Pilot.NET.Lang.Enums;
    using System;

    /// <summary>
    /// A variable in PILOT
    /// </summary>
    public interface IExpression
    {

        /// <summary>
        /// What kind of expression
        /// </summary>
        ExpressionTypes TypeOfExpression { get; }

        /// <summary>
        /// Returns a copy of the expression
        /// </summary>
        /// <returns>the copy</returns>
        IExpression Copy();
    }
}
