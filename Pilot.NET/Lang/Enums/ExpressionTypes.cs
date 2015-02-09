namespace Pilot.NET.Lang.Enums
{

    /// <summary>
    /// The types of expressions
    /// </summary>
    public enum ExpressionTypes
    {

        /// <summary>
        /// example: $var2 = hello $var3
        /// </summary>
        StringExpression,

        /// <summary>
        /// example: #var4 = 4 * 7
        /// </summary>
        NumericExpression,

        /// <summary>
        /// example: (4 + 3) = #var3
        /// </summary>
        BooleanConditionExpression
    }
}
