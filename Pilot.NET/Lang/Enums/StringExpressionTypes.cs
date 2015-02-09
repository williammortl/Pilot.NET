namespace Pilot.NET.Lang.Enums
{

    /// <summary>
    /// The types of string expressions
    /// </summary>
    public enum StringExpressionTypes
    {

        /// <summary>
        /// example: this is a $var1 string #var2 literal
        /// </summary>
        StringLiteral,

        /// <summary>
        /// example: #var1
        /// </summary>
        StringVariable,

        /// <summary>
        /// example: $var1 = this is a literal
        /// </summary>
        StringAssignment
    }
}
