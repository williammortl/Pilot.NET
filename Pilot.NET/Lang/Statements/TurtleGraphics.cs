namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.GraphicsExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A turtle graphics statement, GR
    /// </summary>
    internal sealed class TurtleGraphics : IImmediateStatement
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
        /// The graphics expression for graphics to evaluates
        /// </summary>
        public List<IGraphicsExpression> GraphicsExpressions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphicsExpressions">the graphics expressions to evaluate</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifCondition">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public TurtleGraphics(List<IGraphicsExpression> graphicsExpressions, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // make sure not null or an empty list
            if ((graphicsExpressions == null) || (graphicsExpressions.Count < 1))
            {
                throw new InvalidSyntax("GR Cannot have a null Graphics Expression");
            }

            // set properties
            this.GraphicsExpressions = graphicsExpressions;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the accept</returns>
        public override String ToString()
        {
            String expression = String.Empty;
            foreach (IGraphicsExpression graphicsExp in this.GraphicsExpressions)
            {
                expression += String.Format("{0}; ", graphicsExp.ToString());
            }
            expression = expression.Substring(0, expression.Length - 2);
            return StatementMethods.StatementToString(Keywords.GR, this.MatchType, this.IfCondition, expression);
        }
    }
}
