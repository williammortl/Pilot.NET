namespace Pilot.NET.Lang.Enums
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Binary operators (use 2 numbers) for numeric expressions, in ascending order of precedence
    /// </summary>
    public enum NumericBinaryOperators
    {
    
        /// <summary>
        /// Sets a numeric variable equal to a NumericExpression
        /// </summary>
        [Description("ASSIGN              - =")]
        Eq,

        /// <summary>
        /// Subtract right from left
        /// </summary>
        [Description("SUBTRACT            - -")]
        Sub,

        /// <summary>
        /// Add two expressions
        /// </summary>
        [Description("ADD                 - +")]
        Add,

        /// <summary>
        /// Remainder of left divided by right
        /// </summary>
        [Description("MODULO              - \\")]
        Mod,

        /// <summary>
        /// Divide left by right
        /// </summary>
        [Description("DIVISION            - /")]
        Div,

        /// <summary>
        /// Multiply two expressions
        /// </summary>
        [Description("MULTIPLY            - *")]
        Mult,
        
        /// <summary>
        /// Logarithm: a~b is log a to base b
        /// </summary>
        [Description("LOGARITHM           - ~")]
        Log,

        /// <summary>
        /// Exponent: a^b is a to the power of b 
        /// </summary>
        [Description("EXPONENT            - ^")]
        Exp
    }
}
