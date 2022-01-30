namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Enums;
    using PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A unary numeric expression
    /// </summary>
    class NumericUnaryOperation
    {

        /// <summary>
        /// numeric expression right of the assignment
        /// </summary>
        private INumericExpression right;

        /// <summary>
        /// This is the operation being performed
        /// </summary>
        public NumericUnaryOperators Operator { get; private set; }

        /// <summary>
        /// numeric expression right of the assignment
        /// </summary>
        public INumericExpression Right
        {
            get
            {
                return this.right;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Cannot perform an operation on a null expression, on the right");
                }

                // make sure this is not an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Cannot perform any operation on an assignment expression, on the right");
                }

                // assign
                this.right = value;
            }
        }


        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return String.Format("({0} {1})", this.Operator.ToString(), this.Right.ToString());
        }
    }
}
