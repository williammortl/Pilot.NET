namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;

    /// <summary>
    /// This represents a PILOT statement
    /// </summary>
    public interface IStatement
    {

        /// <summary>
        /// The type of command
        /// </summary>
        Keywords Keyword { get; }
        
        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        MatchTypes MatchType { get; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        BooleanCondition IfCondition { get; }

        /// <summary>
        /// Returns a copy of the statement
        /// </summary>
        /// <returns>the copy</returns>
        IStatement Copy();
    }
}
