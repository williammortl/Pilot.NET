namespace Pilot.NET.Lang.Expressions.Boolean
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using System;

    /// <summary>
    /// A boolean condition expression
    /// </summary>
    public sealed class BooleanCondition : IExpression
    {

        /// <summary>
        /// expression left of the assignment
        /// </summary>
        private INumericExpression left;

        /// <summary>
        /// expression right of the assignment
        /// </summary>
        private INumericExpression right;

        /// <summary>
        /// What kind of expression
        /// </summary>
        public ExpressionTypes TypeOfExpression { get; private set; }

        /// <summary>
        /// This is the boolean operation
        /// </summary>
        public BooleanConditionOperators Operator { get; private set; }

        /// <summary>
        /// expression left of the assignment
        /// </summary>
        public INumericExpression Left
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
                    throw new InvalidSyntax("Cannot perform a boolean compare on a null expression, on the left");
                }

                // make sure this is not an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Cannot perform a boolean compare on an assignment expression, on the left");
                }

                // assign
                this.left = value;
            }
        }

        /// <summary>
        /// expression right of the assignment
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
                    throw new InvalidSyntax("Cannot perform a boolean compare on a null expression, on the right");
                }

                // make sure this is not an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Cannot perform a boolean compare on an assignment expression, on the right");
                }

                // assign
                this.right = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="op">the boolean operator to apply</param>
        /// <param name="left">expression left of the operator</param>
        /// <param name="right">expression right of the operator</param>
        public BooleanCondition(BooleanConditionOperators op, INumericExpression left, INumericExpression right)
        {
            this.TypeOfExpression = ExpressionTypes.BooleanConditionExpression;
            this.Operator = op;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the boolean condition to duplicate</param>
        public BooleanCondition(BooleanCondition toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.Operator = toDup.Operator;
            this.Left = (INumericExpression)((toDup == null) ? null : toDup.Left.Copy());
            this.Right = (INumericExpression)((toDup == null) ? null : toDup.Right.Copy());
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this boolean condition</returns>
        public IExpression Copy()
        {
            return new BooleanCondition(this);
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0} {1} {2})", this.Left.ToString(), EnumMethods.BooleanOperatorToString(this.Operator), this.Right.ToString());
        }
    }
}
