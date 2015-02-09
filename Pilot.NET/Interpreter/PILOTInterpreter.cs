namespace Pilot.NET.Interpreter
{
    using Pilot.NET.Interpreter.InterpreterInterfaces;
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Executes a PILOTProgram
    /// </summary>
    public sealed class PILOTInterpreter : IDisposable
    {

        /// <summary>
        /// The interface to use for the PILOT translator to use for text IO and graphics output
        /// </summary>
        private IPILOTInterpreterInterface pilotInterface;

        /// <summary>
        /// String variables and their associated values
        /// </summary>
        private Dictionary<String, String> stringVariables;

        /// <summary>
        /// Numeric variables and their associated values
        /// </summary>
        private Dictionary<String, double> numericVariables;

        /// <summary>
        /// Constructor for the interpreter
        /// </summary>
        /// <param name="pilotInterface">the interface to use for the PILOT translator to use for text IO and graphics output</param>
        public PILOTInterpreter(IPILOTInterpreterInterface pilotInterface)
        {

            // check to make sure that interface is not null
            if (pilotInterface == null)
            {
                throw new PILOTException("Must provide a valid interface for the PILOT interpreter");
            }

            // attribute init
            this.pilotInterface = pilotInterface;
            this.stringVariables = new Dictionary<string, string>();
            this.numericVariables = new Dictionary<string, double>();
        }

        /// <summary>
        /// Clears the memory state with respect to variables
        /// </summary>
        public void ClearMemoryState()
        {
            this.stringVariables = new Dictionary<string, string>();
            this.numericVariables = new Dictionary<string, double>();
        }

        /// <summary>
        /// Creates or updates a string variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        public void SetStringVar(String varName, String val)
        {

            // adjust name if neccessary
            varName = varName.Trim().ToLower();
            varName = (varName.StartsWith("$") == true) ? varName.Substring(1) : varName;

            // add the variable if neccessary
            if (this.stringVariables.Keys.Contains(varName) == false)
            {
                this.stringVariables.Add(varName, String.Empty);
            }

            // update the value
            this.stringVariables[varName] = val;
        }

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        public String GetStringVar(String varName)
        {

            // var init
            String retVal = String.Empty;

            // adjust name if neccessary
            varName = varName.Trim().ToLower();
            varName = (varName.StartsWith("$") == true) ? varName.Substring(1) : varName;

            // look for variable
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = this.stringVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find string variable: ${0}", varName));
            }

            return retVal;
        }

        /// <summary>
        /// Creates or updates a numeric variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        public void SetNumericVar(String varName, Double val)
        {

            // adjust name if neccessary
            varName = varName.Trim().ToLower();
            varName = (varName.StartsWith("#") == true) ? varName.Substring(1) : varName;

            // add the variable if neccessary
            if (this.numericVariables.Keys.Contains(varName) == false)
            {
                this.numericVariables.Add(varName, 0);
            }

            // update the value
            this.numericVariables[varName] = val;
        }

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        public double GetNumericVar(String varName)
        {

            // var init
            double retVal = 0;

            // adjust name if neccessary
            varName = varName.Trim().ToLower();
            varName = (varName.StartsWith("#") == true) ? varName.Substring(1) : varName;

            // look for variable
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = this.numericVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find numeric variable: #{0}", varName));
            }

            return retVal;
        }

        /// <summary>
        /// Runs the PILOT program
        /// </summary>
        /// <param name="prog">the program to execute</param>
        public void Run(PILOTProgram prog)
        {

            // clear current memory first
            this.ClearMemoryState();

            // initialize a call stack
            Stack<int> CallStack = new Stack<int>();

        }

        /// <summary>
        /// Evaluate a numeric expression, can throw RunTimeException
        /// </summary>
        /// <param name="ne">the numeric expression to evaluate</param>
        /// <returns>the value of evaluating the expression(s)</returns>
        public double EvaluateNumericExpression(INumericExpression ne)
        {

            // short circuit on null
            if (ne == null)
            {
                throw new RunTimeException("Cannot evaluate a null numeric expression");
            }

            // var init
            double retVal = 0;

            // evaluate based upon what type of numeric expression
            try
            {
                if (ne is NumericLiteral)
                {
                    retVal = ((NumericLiteral)ne).Number;
                }
                else if (ne is RandomNumber)
                {
                    Random rnd = new Random();
                    double sign = ((rnd.NextDouble() * 2.0) > 1) ? -1 : 1;
                    double value = Math.Ceiling(rnd.NextDouble() * ((sign > 0) ? 32767 : 32768));
                    retVal = sign * value;
                }
                else if (ne is NumericVariable)
                {
                    NumericVariable numVar = (NumericVariable)ne;
                    retVal = this.GetNumericVar(numVar.ToString());
                }
                else if (ne is NumericBinaryOperation)
                {
                    NumericBinaryOperation opExpression = (NumericBinaryOperation)ne;
                    double rightResult = this.EvaluateNumericExpression(opExpression.Right);
                    if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Eq)
                    {
                        if (opExpression.Left is NumericVariable)
                        {

                            // store the new variable value in the state
                            this.SetNumericVar(opExpression.Left.ToString(), rightResult);
                        }
                        else
                        {
                            throw new CannotAssignException(String.Format("Cannot assign to non-numeric variable: {0}", ne.ToString()));
                        }
                    }
                    else if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Add)
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        retVal = leftResult + rightResult;
                    }
                    else if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Div)
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        retVal = leftResult / rightResult;
                    }
                    else if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Mod)
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        retVal = leftResult % rightResult;
                    }
                    else if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Mult)
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        retVal = leftResult * rightResult;
                    }
                    else if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Sub)
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        retVal = leftResult - rightResult;
                    }
                }
                else
                {
                    throw new RunTimeException(String.Format("Unknown numeric expression type: {0}", ne.ToString()));
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", ne.ToString()), e);
            }

            return retVal;
        }

        /// <summary>
        /// Evaluates a string expression, can throw RunTimeException
        /// </summary>
        /// <param name="se">the string expression to evaluate</param>
        /// <returns>a String value of the evaluation</returns>
        public String EvaluateStringExpression(IStringExpression se)
        {

            // short circuit on null
            if (se == null)
            {
                throw new RunTimeException("Cannot evaluate a null string expression");
            }

            // var init
            String retVal = String.Empty;

            // evaluate based upon what type of numeric expression
            try
            {
                if (se is StringVariable)
                {
                    retVal = this.GetStringVar(((StringVariable)se).VariableName);
                }
                else if (se is StringAssignExpression)
                {
                    StringAssignExpression sae = (StringAssignExpression)se;
                    this.SetStringVar(sae.Left.VariableName, this.EvaluateStringExpression(sae.Right));
                }
                else if (se is StringLiteral)
                {
                    StringLiteral sl = (StringLiteral)se;
                    String[] splitText = sl.StringText.Split(new char[] { ' ' });
                    retVal = String.Empty;

                    // check for embedded text, i.e. PRINT OUT VAR $VAR1 and #VAR2 BEFORE THESE WORDS
                    foreach (String s in splitText)
                    {
                        if (s.Trim() == String.Empty)
                        {
                            retVal += " ";
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    throw new RunTimeException(String.Format("Unknown string expression type: {0}", se.ToString()));
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", se.ToString()), e);
            }

            return retVal;
        }

        /// <summary>
        /// Evaluates a boolean condition, can throw RunTimeException
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        public Boolean EvaluateBooleanCondition(BooleanCondition bc)
        {

            // short circuit on null
            if (bc == null)
            {
                throw new RunTimeException("Cannot evaluate a null boolean condition");
            }

            // var init
            Boolean retVal = false;



            return retVal;
        }

        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
        
            // dispose the interface
            pilotInterface.Dispose();
        }
    }
}
