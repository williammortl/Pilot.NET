namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.GraphicsExpressions;
    using System;

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
        public IGraphicsExpression GraphicsExpression { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphicsExpression">the graphics expression to evaluate</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifCondition">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public TurtleGraphics(IGraphicsExpression graphicsExpression, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.GraphicsExpression = graphicsExpression;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the accept</returns>
        public override String ToString()
        {
            String expression = (this.GraphicsExpression == null) ? String.Empty : this.GraphicsExpression.ToString();
            return StatementMethods.StatementToString(Keywords.GR, this.MatchType, this.IfCondition, expression);
        }
    }
}
