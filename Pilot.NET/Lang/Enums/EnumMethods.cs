namespace Pilot.NET.Lang.Enums
{
    using Pilot.NET.Lang.Exceptions;
    using System;

    /// <summary>
    /// Class with enum conversion functions
    /// </summary>
    public static class EnumMethods
    {

        /// <summary>
        /// Converts a NumericBinaryOperator to string
        /// </summary>
        /// <param name="op">the operator</param>
        /// <returns>the PILOT representation</returns>
        public static String NumericBinaryOperatorToString(NumericBinaryOperators op)
        {

            // var init
            string retVal = "+";

            // what op
            switch (op)
            {
                case NumericBinaryOperators.Div:
                {
                    retVal = "/";
                    break;
                }
                case NumericBinaryOperators.Mod:
                {
                    retVal = "\\";
                    break;
                }
                case NumericBinaryOperators.Mult:
                {
                    retVal = "*";
                    break;
                }
                case NumericBinaryOperators.Sub:
                {
                    retVal = "-";
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Converts a string to a NumericBinaryOperator, throws a InvalidSyntax if no match
        /// </summary>
        /// <param name="op">the string</param>
        /// <returns>the operator</returns>
        public static NumericBinaryOperators StringToNumericBinaryOperator(String op)
        {

            // var init
            NumericBinaryOperators retVal = NumericBinaryOperators.Eq;

            // check string value, return correct value
            op = op.Trim();
            if (op.StartsWith("=") == true)
            {
                retVal = NumericBinaryOperators.Eq;
            }
            else if (op.StartsWith("+") == true)
            {
                retVal = NumericBinaryOperators.Add;
            }
            else if (op.StartsWith("/") == true)
            {
                retVal = NumericBinaryOperators.Div;
            }
            else if (op.StartsWith("\\") == true)
            {
                retVal = NumericBinaryOperators.Mod;
            }
            else if (op.StartsWith("*") == true)
            {
                retVal = NumericBinaryOperators.Mult;
            }
            else if (op.StartsWith("-") == true)
            {
                retVal = NumericBinaryOperators.Sub;
            }
            else
            {
                throw new InvalidSyntax("Incorrect numeric operator");
            }

            return retVal;
        }

        /// <summary>
        /// Checks to see if the char is a NumericBinaryOperator
        /// </summary>
        /// <param name="c">the char to check</param>
        /// <returns>true if it is an operator</returns>
        public static Boolean IsNumericBinaryOperator(char c)
        {

            // var init
            Boolean retVal = false;

            // check char value
            if ((c == '=') || 
                (c == '+') || 
                (c == '/') || 
                (c == '\\') || 
                (c == '*') ||
                (c == '-'))
            {
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Converts an boolean operator type to string
        /// </summary>
        /// <param name="op">the operator</param>
        /// <returns>the PILOT representation</returns>
        public static String BooleanOperatorToString(BooleanConditionOperators op)
        {

            // var init
            string retVal = "=";

            // what op
            switch (op)
            {
                case BooleanConditionOperators.GT:
                {
                    retVal = ">";
                    break;
                }
                case BooleanConditionOperators.GTEq:
                {
                    retVal = ">=";
                    break;
                }
                case BooleanConditionOperators.LT:
                {
                    retVal = "<";
                    break;
                }
                case BooleanConditionOperators.LTEq:
                {
                    retVal = "<=";
                    break;
                }
                case BooleanConditionOperators.NotEq:
                {
                    retVal = "<>";
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Converts a string to a BooleanOperator, throws a InvalidSyntax exception if no match
        /// </summary>
        /// <param name="op">the string</param>
        /// <returns>the operator</returns>
        public static BooleanConditionOperators StringToBooleanOperator(String op)
        {

            // var init
            BooleanConditionOperators retVal = BooleanConditionOperators.Eq;

            // check string value, return correct value
            op = op.Trim();
            if (op.StartsWith(">=") == true)
            {
                retVal = BooleanConditionOperators.GTEq;
            }
            else if (op.StartsWith(">") == true)
            {
                retVal = BooleanConditionOperators.GT;
            }
            else if (op.StartsWith("<=") == true)
            {
                retVal = BooleanConditionOperators.LTEq;
            }
            else if (op.StartsWith("<>") == true)
            {
                retVal = BooleanConditionOperators.NotEq;
            }
            else if (op.StartsWith("<") == true)
            {
                retVal = BooleanConditionOperators.LT;
            }
            else if (op.StartsWith("=") == true)
            {
                retVal = BooleanConditionOperators.Eq;
            }
            else
            {
                throw new InvalidSyntax("Incorrect boolean operator");
            }

            return retVal;
        }
    }
}
