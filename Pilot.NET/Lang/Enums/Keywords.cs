namespace Pilot.NET.Lang.Enums
{

    /// <summary>
    /// The keywords for the PILOT programming language
    /// </summary>
    internal enum Keywords
    {

        /// <summary>
        /// remark -> R: This is a comment
        /// </summary>
        R,

        /// <summary>
        /// type something -> T: HELLO
        /// </summary>
        T,

        /// <summary>
        /// input -> A: $stringval | A: #numval | A:
        /// </summary>
        A,

        /// <summary>
        /// jump -> J: *MyLabel
        /// </summary>
        J,

        /// <summary>
        /// match -> M: cond1, cond2, ...
        /// </summary>
        M,

        /// <summary>
        /// compute -> C: $var1=$var2$var3 | C: #var1=#var1+1
        /// </summary>
        C,

        /// <summary>
        /// use -> U: *myLabel
        /// </summary>
        U,

        /// <summary>
        /// end -> E:
        /// </summary>
        E,

        /// <summary>
        /// pause, unit of time is 1/60 th of a second, this pauses for 1 second -> PA: 60
        /// </summary>
        PA,

        /// <summary>
        /// jump on match, should have the same numer of conditions as match -> JM: *label1, *label2, ...
        /// </summary>
        JM
    }
}
