namespace Pilot.NET
{
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using Pilot.NET.Lang.Statements;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Executes a PILOTProgram
    /// </summary>
    public sealed class PILOTInterpreter : IDisposable
    {

        /// <summary>
        /// For the random number generator
        /// </summary>
        private static Random randomGenerator = new Random();

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
        /// Default constructor
        /// </summary>
        public PILOTInterpreter()
        {

            // attribute init
            this.pilotInterface = new DefaultInterpreterInterface();
            this.stringVariables = new Dictionary<string, string>();
            this.numericVariables = new Dictionary<string, double>();
        }

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
            if (this.numericVariables.Keys.Contains(varName) == true)
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
        /// Runs a PILOT program
        /// </summary>
        /// <param name="prog">the program to execute</param>
        public void Run(PILOTProgram prog)
        {

            // clear current memory first
            this.ClearMemoryState();

            // initialize a call stack
            int executionPointer = 0;
            Stack<int> CallStack = new Stack<int>();
            MatchTypes currentMatch = MatchTypes.None;
            String acceptBuffer = String.Empty;

            // execute until we cant
            while (executionPointer < prog.LineNumbers.Count)
            {
                
                // execute statement
                Line currentLine = prog[prog.LineNumbers[executionPointer]];
                IStatement currentStatement = currentLine.LineStatement;

                // check for match type to determine whether or not to execute
                if ((currentMatch == MatchTypes.None) || (currentStatement.MatchType == currentMatch))
                {

                    // evaluate boolean condition to determine whether or not to execute
                    if ((currentLine.LineStatement.IfCondition == null) || (this.EvaluateBooleanCondition(currentLine.LineStatement.IfCondition) == true))
                    {

                        // evaluate keyword
                        if (currentStatement is IImmediateStatement)
                        {
                            this.EvaluateImmediateStatement((IImmediateStatement)currentStatement);
                            executionPointer++;
                        }
                        else if (currentStatement is Accept)
                        {
                            acceptBuffer = this.pilotInterface.ReadTextLine();
                            Accept a = (Accept)currentStatement;
                            if (a.VariableToSet != null)
                            {
                                if (a.VariableToSet.VariableName.StartsWith("#") == true)
                                {
                                    try
                                    {
                                        this.SetNumericVar(a.VariableToSet.VariableName, Convert.ToDouble(acceptBuffer));
                                    }
                                    catch
                                    {
                                        this.pilotInterface.WriteText("Expected numeric input", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    this.SetStringVar(a.VariableToSet.VariableName, acceptBuffer);
                                }
                            }
                            executionPointer++;
                        }
                        else if (currentStatement is Pause)
                        {
                            Pause pa = (Pause)currentStatement;
                            double timeToPause = this.EvaluateNumericExpression(pa.TimeToPause);
                            Thread.Sleep(Convert.ToInt32(timeToPause * 1000 / 60));
                            executionPointer++;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Runs a PILOT program
        /// </summary>
        /// <param name="text">the program in a string, each line is sperated by \r\n</param>
        public void Run(String text)
        {
            this.Run(PILOTParser.ParseProgram(text));
        }

        /// <summary>
        /// Runs a PILOT program
        /// </summary>
        /// <param name="file">the file name of the program to execute</param>
        public void Run(FileInfo file)
        {
            this.Run(PILOTParser.ParseProgram(file));
        }

        /// <summary>
        /// Evaluates an immediate statement
        /// </summary>
        /// <param name="statement">the statement to evaluate</param>
        public void EvaluateImmediateStatement(String statement)
        {

            // short circuit
            if (String.IsNullOrWhiteSpace(statement) == true)
            {
                this.pilotInterface.WriteText("Cannot evaluate an empty string", true);
            }

            // var init
            IImmediateStatement iis = null;

            // try to parse the statement and cast as an immediate statement
            try
            {

                // execute the statement
                iis = (IImmediateStatement)PILOTParser.ParseStatement(statement);
                this.EvaluateImmediateStatement(iis);
            }
            catch (PILOTException pe)
            {
                this.pilotInterface.WriteText(pe.Message, true);
            }
            catch (InvalidCastException)
            {
                this.pilotInterface.WriteText(String.Format("The following statement is not allowed to immediately execute: {0}", statement), true);
            }
        }

        /// <summary>
        /// Evaluates an immediate statement
        /// </summary>
        /// <param name="statement">the statement to evaluate</param>
        internal void EvaluateImmediateStatement(IImmediateStatement statement)
        {
            try
            {
                if (statement is Compute)
                {
                    Compute cs = (Compute)statement;
                    if (this.EvaluateBooleanCondition(cs.IfCondition) == true)
                    {
                        if (cs.ExpressionToCompute is INumericExpression)
                        {
                            INumericExpression ne = (INumericExpression)cs.ExpressionToCompute;
                            this.EvaluateNumericExpression(ne).ToString();
                        }
                        else if (cs.ExpressionToCompute is IStringExpression)
                        {
                            IStringExpression se = (IStringExpression)cs.ExpressionToCompute;
                            this.EvaluateStringExpression(se);
                        }
                    }
                }
                else if (statement is Text)
                {
                    Text ts = (Text)statement;
                    if (this.EvaluateBooleanCondition(ts.IfCondition) == true)
                    {
                        this.WriteOutput(this.EvaluateStringExpression(ts.TextToDisplay));
                    }
                }
            }
            catch (PILOTException pe)
            {
                this.pilotInterface.WriteText(pe.Message, true);
            }
            catch (Exception)
            {
                this.pilotInterface.WriteText(String.Format("An error occured executing the statement: {0}", statement.ToString()), true);
            }
        }

        /// <summary>
        /// Evaluate a numeric expression, can throw RunTimeException
        /// </summary>
        /// <param name="numericalExpression">the numeric expression to evaluate</param>
        /// <returns>the value of evaluating the expression(s)</returns>
        public double EvaluateNumericExpression(String numericalExpression)
        {

            // short circuit
            if (String.IsNullOrWhiteSpace(numericalExpression) == true)
            {
                throw new RunTimeException("Cannot evaluate an empty string");
            }

            // var init
            double retVal = 0;
            INumericExpression ne = null;

            // evaluate the numerical expression
            ne = PILOTParser.ParseNumericExpression(numericalExpression);
            retVal = this.EvaluateNumericExpression(ne);

            return retVal;
        }

        /// <summary>
        /// Evaluate a numeric expression, can throw RunTimeException
        /// </summary>
        /// <param name="numericalExpression">the numeric expression to evaluate</param>
        /// <returns>the value of evaluating the expression(s)</returns>
        internal double EvaluateNumericExpression(INumericExpression numericalExpression)
        {

            // short circuit on null
            if (numericalExpression == null)
            {
                throw new RunTimeException("Cannot evaluate a null numeric expression");
            }

            // var init
            double retVal = 0;

            // evaluate based upon what type of numeric expression
            try
            {
                if (numericalExpression is NumericLiteral)
                {
                    retVal = ((NumericLiteral)numericalExpression).Number;
                }
                else if (numericalExpression is RandomNumber)
                {

                    double sign = ((PILOTInterpreter.randomGenerator.NextDouble() * 2.0) > 1) ? -1 : 1;
                    double value = Math.Ceiling(PILOTInterpreter.randomGenerator.NextDouble() * ((sign > 0) ? 32767 : 32768));
                    retVal = sign * value;
                }
                else if (numericalExpression is NumericVariable)
                {
                    NumericVariable numVar = (NumericVariable)numericalExpression;
                    retVal = this.GetNumericVar(numVar.ToString());
                }
                else if (numericalExpression is NumericBinaryOperation)
                {
                    NumericBinaryOperation opExpression = (NumericBinaryOperation)numericalExpression;
                    double rightResult = this.EvaluateNumericExpression(opExpression.Right);
                    if (opExpression.Operator == Lang.Enums.NumericBinaryOperators.Eq)
                    {
                        this.SetNumericVar(opExpression.Left.ToString(), rightResult);
                        retVal = rightResult;
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
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", numericalExpression.ToString()), e);
            }

            return retVal;
        }

        /// <summary>
        /// Evaluates a string expression, can throw RunTimeException
        /// </summary>
        /// <param name="stringExpression">the string expression to evaluate</param>
        /// <returns>a String value of the evaluation</returns>
        public String EvaluateStringExpression(String stringExpression)
        {

            // var init
            String retVal = String.Empty;
            IStringExpression se = null;

            // evaluate the numerical expression
            se = PILOTParser.ParseStringExpression(stringExpression);
            retVal = this.EvaluateStringExpression(se);

            return retVal;
        }

        /// <summary>
        /// Evaluates a string expression, can throw RunTimeException
        /// </summary>
        /// <param name="stringExpression">the string expression to evaluate</param>
        /// <returns>a String value of the evaluation</returns>
        internal String EvaluateStringExpression(IStringExpression stringExpression)
        {

            // short circuit on null
            if (stringExpression == null)
            {
                throw new RunTimeException("Cannot evaluate a null string expression");
            }

            // var init
            String retVal = String.Empty;

            // evaluate based upon what type of string expression
            try
            {
                if (stringExpression is StringVariable)
                {
                    retVal = this.GetStringVar(((StringVariable)stringExpression).VariableName);
                }
                else if (stringExpression is StringAssignExpression)
                {
                    StringAssignExpression sae = (StringAssignExpression)stringExpression;
                    retVal = this.EvaluateStringExpression(sae.Right);
                    this.SetStringVar(sae.Left.VariableName, retVal);
                }
                else if (stringExpression is StringLiteral)
                {
                    String[] splitText = ((StringLiteral)stringExpression).StringText.Split(new char[] { ' ' });
                    foreach (String s in splitText)
                    {
                        String word = s.Trim();
                        if ((word.Length > 1) && ((word[0] == '#') || (word[0] == '$')))
                        {
                            int puncLoc = PILOTInterpreter.FirstPunctuation(word.Substring(1)) + 1;
                            if (puncLoc > 1)
                            {
                                String afterPunc = word.Substring(puncLoc);
                                word = word.Substring(0, puncLoc);
                                word = (word[0] == '#') ? this.GetNumericVar(word).ToString() : this.GetStringVar(word);
                                word += afterPunc;
                            }
                            else
                            {
                                word = (word[0] == '#') ? this.GetNumericVar(word).ToString() : this.GetStringVar(word);
                            }
                        }
                        retVal += word + " ";
                    }
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", stringExpression.ToString()), e);
            }

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

        /// <summary>
        /// Creates or updates a numeric variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        internal void SetNumericVar(String varName, Double val)
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
        /// Creates or updates a string variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        internal void SetStringVar(String varName, String val)
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
        /// Evaluates a boolean condition, can throw RunTimeException
        /// </summary>
        /// <param name="bc">the boolean condition to evaluate, ok to be null</param>
        /// <returns>the evaluation of the boolean condition, true if null</returns>
        internal Boolean EvaluateBooleanCondition(BooleanCondition bc)
        {

            // short circuit on null
            if (bc == null)
            {
                return true;
            }

            // var init
            Boolean retVal = false;

            // evaluate the boolean condition
            try
            {
                double left = this.EvaluateNumericExpression(bc.Left);
                double right = this.EvaluateNumericExpression(bc.Right);
                if (bc.Operator == BooleanConditionOperators.Eq)
                {
                    retVal = (left == right);
                }
                else if (bc.Operator == BooleanConditionOperators.GT)
                {
                    retVal = (left > right);
                }
                else if (bc.Operator == BooleanConditionOperators.GTEq)
                {
                    retVal = (left >= right);
                }
                else if (bc.Operator == BooleanConditionOperators.LT)
                {
                    retVal = (left < right);
                }
                else if (bc.Operator == BooleanConditionOperators.LTEq)
                {
                    retVal = (left <= right);
                }
                else if (bc.Operator == BooleanConditionOperators.NotEq)
                {
                    retVal = (left != right);
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the boolean condition: {0}", bc.ToString()), e);
            }

            return retVal;
        }

        /// <summary>
        /// Writes output to the interface, handles trailing slashes and new lines
        /// </summary>
        /// <param name="pilotOutput">the pilot output to write</param>
        private void WriteOutput(String pilotOutput)
        {
            if (String.IsNullOrWhiteSpace(pilotOutput) == false)
            {
                Boolean newLine = true;
                if (pilotOutput[pilotOutput.Length - 1] == '\\')
                {
                    newLine = false;
                    pilotOutput = pilotOutput.Substring(0, pilotOutput.Length - 1);
                }
                this.pilotInterface.WriteText(pilotOutput, newLine);
            }
        }

        /// <summary>
        /// Gives you the location of the first punctuation
        /// </summary>
        /// <param name="stringToCheck">the string to check for punctuation</param>
        /// <returns>the location of the first punctuation, -1 no punctuation</returns>
        private static int FirstPunctuation(String stringToCheck)
        {

            // var init
            int retVal = -1;

            // loop through string, look for punctutation
            for (int i = 0; i < stringToCheck.Length; i++)
            {
                if (Char.IsPunctuation(stringToCheck[i]) == true)
                {
                    retVal = i;
                    break;
                }
            }

            return retVal;
        }
    }
}
