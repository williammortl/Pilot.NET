namespace Pilot.NET.Lang.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The keywords for the PILOT programming language
    /// </summary>
    public enum Keywords
    {

        /// <summary>
        /// input -> A: $stringval | A: #numval | A:
        /// </summary>
        [Description("ACCEPT INPUT")]
        A,

        /// <summary>
        /// compute -> C: $var1=$var2$var3 | C: #var1=#var1+1
        /// </summary>
        [Description("COMPUTE*")]
        C,

        /// <summary>
        /// end -> E:
        /// </summary>
        [Description("END (OR RETURN FROM SUBROUTINE)")]
        E,

        /// <summary>
        /// jump -> J: *MyLabel
        /// </summary>
        [Description("TURTLE GRAPHICS*")]
        GR,

        /// <summary>
        /// jump -> J: *MyLabel
        /// </summary>
        [Description("JUMP")]
        J,

        /// <summary>
        /// jump on match, should have the same numer of conditions as match -> JM: *label1, *label2, ...
        /// </summary>
        [Description("JUMP ON MATCH")]
        JM,

        /// <summary>
        /// match -> M: cond1, cond2, ...
        /// </summary>
        [Description("MATCH")]
        M,

        /// <summary>
        /// pause, unit of time is 1/60 th of a second, this pauses for 1 second -> PA: 60
        /// </summary>
        [Description("PAUSE TIME, IN UNITS OF 1/60TH OF A SECOND*")]
        PA,

        /// <summary>
        /// remark -> R: This is a comment
        /// </summary>
        [Description("REMARK*")]
        R,

        /// <summary>
        /// type something -> T: HELLO
        /// </summary>
        [Description("PRINT TEXT*")]
        T,

        /// <summary>
        /// use -> U: *myLabel
        /// </summary>
        [Description("JUMP TO LABEL (OR USE)")]
        U
    }
}
