namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// A math expression
    /// </summary>
    public interface INumericExpression : IExpression
    {

        /// <summary>
        /// The type of math expression
        /// </summary>
        NumericExpressionTypes TypeOfNumericExpression { get; }
    }
}
