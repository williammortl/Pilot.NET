namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Exception;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a string assignment expression
    /// </summary>
    internal sealed class StringAssignExpression : IStringExpression
    {

        /// <summary>
        /// String expression right of the assignment
        /// </summary>
        private IStringExpression right;

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
            this.Left = left;
            this.Right = right;
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
