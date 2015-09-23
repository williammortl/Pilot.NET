namespace Pilot.NET.Lang.Enums
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Operators for boolean expressions
    /// </summary>
    public enum BooleanConditionOperators
    {

        /// <summary>
        /// Left less than right
        /// </summary>
        [Description("LESS THAN           - <")]
        LT,

        /// <summary>
        /// Left less than or equal to right
        /// </summary>
        [Description("LESS THAN EQUAL     - <=")]
        LTEq,

        /// <summary>
        /// Left greater than right
        /// </summary>
        [Description("GREATER THAN        - >")]
        GT,

        /// <summary>
        /// Left greater than or equal to right
        /// </summary>
        [Description("GREATER THAN EQUAL  - >=")]
        GTEq,

        /// <summary>
        /// Are expressions equal
        /// </summary>
        [Description("EQUAL               - =")]
        Eq,

        /// <summary>
        /// Are expressions not equal
        /// </summary>
        [Description("NOT EQUAL           - <>")]
        NotEq
    }
}
