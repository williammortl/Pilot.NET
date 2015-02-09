namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;

    /// <summary>
    /// A string expression
    /// </summary>
    public interface IStringExpression : IExpression
    {

        /// <summary>
        /// The type of string expression
        /// </summary>
        StringExpressionTypes TypeOfStringExpression { get; }
    }
}

