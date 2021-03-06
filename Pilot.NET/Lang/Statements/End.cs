﻿namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// An end statement, E
    /// </summary>
    internal sealed class End : IStatement
    {

        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matchType">the match type</param>
        /// <param name="ifCondition">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public End(MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the end</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(Keywords.E, this.MatchType, this.IfCondition, String.Empty);
        }
    }
}
