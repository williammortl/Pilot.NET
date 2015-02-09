namespace Pilot.NET.Lang.Enums
{

    /// <summary>
    /// Type of expression
    /// </summary>
    public enum NumericExpressionTypes
    {

        /// <summary>
        /// example: 4
        /// </summary>
        NumericLiteral,

        /// <summary>
        /// example: #var1
        /// </summary>
        NumericVariable,

        /// <summary>
        /// example: ?      <- this yields a random number
        /// </summary>
        RandomNumber,

        /// <summary>
        /// example: 3 * 3
        /// </summary>
        BinaryOperation
    }
}
