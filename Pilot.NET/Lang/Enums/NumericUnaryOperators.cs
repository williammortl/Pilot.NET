namespace Pilot.NET.Lang.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Numeric unary operators
    /// </summary>
    public enum NumericUnaryOperators
    {

        /// <summary>
        /// Sin
        /// </summary>
        [Description("SINE                - SIN")]
        SIN,

        /// <summary>
        /// Cos
        /// </summary>
        [Description("COSINE              - COS")]
        COS,

        /// <summary>
        /// Tan
        /// </summary>
        [Description("TANGENT             - TAN")]
        TAN,

        /// <summary>
        /// Arc sin
        /// </summary>
        [Description("ARC SINE            - ASIN")]
        ASIN,

        /// <summary>
        /// Arc cos
        /// </summary>
        [Description("ARC COSINE          - ACOS")]
        ACOS,

        /// <summary>
        /// Arc tan
        /// </summary>
        [Description("ARC TANGENT         - ATAN")]
        ATAN,

        /// <summary>
        /// Round
        /// </summary>
        [Description("ROUND               - RND")]
        RND,

        /// <summary>
        /// Floor function
        /// </summary>
        [Description("FLOOR FUNC.         - FLOOR")]
        FLOOR,

        /// <summary>
        /// Ceiling function
        /// </summary>
        [Description("CEILING FUNC.       - CEIL")]
        CEIL,

        /// <summary>
        /// Convert $A -> #A
        /// </summary>
        [Description("CONVERT $ TO #      - CONV")]
        CONV,

        /// <summary>
        /// Absolute value
        /// </summary>
        [Description("ABSOLUTE VAL        - ABS")]
        ABS
    }
}
