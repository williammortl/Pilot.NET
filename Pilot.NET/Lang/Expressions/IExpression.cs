namespace Pilot.NET.Lang.Expressions
{

    /// <summary>
    /// A variable in PILOT
    /// </summary>
    internal interface IExpression
    {

        /// <summary>
        /// Returns a copy of the expression
        /// </summary>
        /// <returns>the copy</returns>
        IExpression Copy();
    }
}
