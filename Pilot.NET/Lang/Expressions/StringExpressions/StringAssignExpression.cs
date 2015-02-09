namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a string assignment expression
    /// </summary>
    public sealed class StringAssignExpression : IStringExpression
    {

        /// <summary>
        /// String expression right of the assignment
        /// </summary>
        private IStringExpression right;

        /// <summary>
        /// The type of expression
        /// </summary>
        public ExpressionTypes TypeOfExpression { get; private set; }

        /// <summary>
        /// The type of string expression
        /// </summary>
        public StringExpressionTypes TypeOfStringExpression { get; private set; }

        /// <summary>
        /// Variable left of the assignment
        /// </summary>
        public StringVariable Left { get; private set; }

        /// <summary>
        /// numeric expression right of the assignment
        /// </summary>
        public IStringExpression Right
        {
            get
            {
                return this.right;
            }
            private set
            {

                // check to make sure that we are not assigning to a StringAssignmentExpression
                if ((value == null) || (value is StringAssignExpression))
                {
                    throw new InvalidSyntax("Cannot assign a string variable to null or string assignment expression");
                }

                // assign
                this.right = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="left">string variable left of the assignment</param>
        /// <param name="right">string expression on the right of the assignment</param>
        public StringAssignExpression(StringVariable left, IStringExpression right)
        {
            this.TypeOfExpression = ExpressionTypes.StringExpression;
            this.TypeOfStringExpression = StringExpressionTypes.StringAssignment;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the string expression to duplicate</param>
        public StringAssignExpression(StringAssignExpression toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfStringExpression = toDup.TypeOfStringExpression;
            this.Left = (StringVariable)((toDup.Left == null) ? null : toDup.Left.Copy());
            this.Right = (IStringExpression)((toDup.Left == null) ? null : toDup.Left.Copy());
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT string expression</returns>
        public IExpression Copy()
        {
            return new StringAssignExpression(this);
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string expression</returns>
        public override string ToString()
        {
            return String.Format("{0} = {1}", this.Left.ToString(), this.Right.ToString());
        }
    }
}
