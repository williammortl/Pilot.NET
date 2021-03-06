﻿namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// This is a math operator expression
    /// </summary>
    internal sealed class NumericBinaryOperation : INumericExpression
    {

        /// <summary>
        /// numeric expression left of the assignment
        /// </summary>
        private INumericExpression left;

        /// <summary>
        /// numeric expression right of the assignment
        /// </summary>
        private INumericExpression right;

        /// <summary>
        /// This is the operation being performed
        /// </summary>
        public NumericBinaryOperators Operator { get; private set; }

        /// <summary>
        /// numeric expression left of the assignment
        /// </summary>
        public INumericExpression Left
        {
            get
            {
                return this.left;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Cannot perform an operation on a null expression, on the left");
                }

                // make sure this is not an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Cannot perform any operation on an assignment expression, on the left");
                }

                // make sure that the left expression is a NumericVariable if the operator for this expression is an assign operator ('=')
                if (this.Operator == NumericBinaryOperators.Eq)
                {
                    if ((value is NumericVariable) == false)
                    {
                        throw new CannotAssignException("Cannot assign to anything other than a NumericVariable");
                    }
                    else if (((NumericVariable)value).VariableName.StartsWith("%") == true)
                    {
                        throw new InvalidSyntax("Cannot assign to read-only '%' variables");
                    }
                }

                // assign
                this.left = value;
            }
        }

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
        /// Constructor
        /// </summary>
        /// <param name="op">the operator to apply</param>
        /// <param name="left">expression left of the operator</param>
        /// <param name="right">expression right of the operator</param>
        public NumericBinaryOperation(NumericBinaryOperators op, INumericExpression left, INumericExpression right)
        {
            this.Operator = op;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return String.Format("({0} {1} {2})", this.Left.ToString(),
                                                  EnumMethods.NumericBinaryOperatorToString(this.Operator), 
                                                  this.Right.ToString());
        }
    }
}
