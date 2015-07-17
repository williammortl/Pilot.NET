namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// Class for shared statement methods
    /// </summary>
    internal class StatementMethods
    {

        /// <summary>
        /// Format a statement to a string
        /// </summary>
        /// <param name="keyword">the keyword for the statement</param>
        /// <param name="match">the match types</param>
        /// <param name="ifCondition">the boolean condition</param>
        /// <param name="parameters">the parameters for the command</param>
        /// <returns>the string representation</returns>
        public static String StatementToString(Keywords keyword, MatchTypes match, BooleanCondition ifCondition, String parameters)
        {

            // var init 
            String retVal = String.Empty;

            // format the output
            retVal = String.Format("{0}{1}{2}:{3}", keyword.ToString(),
                                                    ((match == MatchTypes.None) ? String.Empty : match.ToString()),
                                                    ((ifCondition == null) ? String.Empty : ifCondition.ToString()),
                                                    parameters).Trim();

            return retVal;
        }
    }
}
