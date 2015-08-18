namespace Pilot.NET.Lang.Enums
{

    /// <summary>
    /// Binary operators (use 2 numbers) for numeric expressions, in ascending order of precedence
    /// </summary>
    internal enum NumericBinaryOperators
    {
    
        /// <summary>
        /// Sets a numeric variable equal to a NumericExpression
        /// </summary>
        Eq,

        /// <summary>
        /// Subtract right from left
        /// </summary>
        Sub,

        /// <summary>
        /// Add two expressions
        /// </summary>
        Add,

        /// <summary>
        /// Remainder of left divided by right
        /// </summary>
        Mod,

        /// <summary>
        /// Divide left by right
        /// </summary>
        Div,
        
        /// <summary>
        /// Multiply two expressions
        /// </summary>
        Mult,

        /// <summary>
        /// Logarithm: a,b is log a to base b
        /// </summary>
        Log,

        /// <summary>
        /// Exponent: a^b is a to the power of b 
        /// </summary>
        Exp
    }
}
