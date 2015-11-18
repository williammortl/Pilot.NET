namespace Pilot.NET
{
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.GraphicsExpressions;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using Pilot.NET.Lang.Statements;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Executes a PILOTProgram
    /// </summary>
    public sealed class PILOTInterpreter : IDisposable
    {

        /// <summary>
        /// turtle graphics x coordinate
        /// </summary>
        private const String X_VAR = "%X";

        /// <summary>
        /// turtle graphics y coordinate
        /// </summary>
        private const String Y_VAR = "%Y";

        /// <summary>
        /// turtle graphics theta coordinate
        /// </summary>
        private const String THETA_VAR = "%A";

        /// <summary>
        /// turtle graphics color value
        /// </summary>
        private const String COLOR_VAR = "%Z";

        /// <summary>
        /// turtle graphics pen width value
        /// </summary>
        private const String WIDTH_VAR = "%W";

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
        /// The size of the graphics display area
        /// </summary>
        private Point graphicsSize;

        /// <summary>
        /// The turtle's position
        /// </summary>
        public Point TurtlePosition { get; private set; }

        /// <summary>
        /// List of the name of string variables
        /// </summary>
        public List<String> StringVariables
        {
            get
            {
                return this.stringVariables.Keys.ToList<String>();
            }
        }

        /// <summary>
        /// List of the name of numeric variables
        /// </summary>
        public List<String> NumericVariables
        {
            get
            {
                return this.numericVariables.Keys.ToList<String>();
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PILOTInterpreter()
        {

            // attribute init
            this.pilotInterface = new DefaultInterpreterInterface();
            this.graphicsSize = new Point(this.pilotInterface.GraphicsOutput.Width, this.pilotInterface.GraphicsOutput.Height);
            this.ClearMemoryState();
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
            this.graphicsSize = new Point(this.pilotInterface.GraphicsOutput.Width, this.pilotInterface.GraphicsOutput.Height);
            this.ClearMemoryState();
        }

        /// <summary>
        /// Clears the memory state with respect to variables
        /// </summary>
        public void ClearMemoryState()
        {
            this.stringVariables = new Dictionary<string, string>();
            this.numericVariables = new Dictionary<string, double>();

            // turtle graphics init
            this.TurtlePosition = new Point(0, 0);
            this.SetNumericVar(PILOTInterpreter.X_VAR, 0);
            this.SetNumericVar(PILOTInterpreter.Y_VAR, 0);
            this.SetNumericVar(PILOTInterpreter.THETA_VAR, 0);
            this.SetNumericVar(PILOTInterpreter.WIDTH_VAR, 1);
            this.SetNumericVar(PILOTInterpreter.COLOR_VAR, (double)PenColors.YELLOW);
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

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.numericVariables.Keys.Contains(varName) == true)
            {
                retVal = this.numericVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find numeric variable: {0}", varName));
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

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = this.stringVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find string variable: {0}", varName));
            }

            return retVal;
        }

        /// <summary>
        /// Checks to see if a variable exists
        /// </summary>
        /// <param name="varName">the name of the variable</param>
        /// <returns>true if it exists</returns>
        public Boolean VarExists(String varName)
        {

            // var init
            Boolean retVal = false;

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = true;
            }
            else if (this.numericVariables.Keys.Contains(varName) == true)
            {
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Runs a PILOT program, can throw PILOTException
        /// </summary>
        /// <param name="text">the program in a string, each line is sperated by \r\n</param>
        public void Run(String text)
        {
            this.Run(PILOTParser.ParseProgram(text));
        }

        /// <summary>
        /// Runs a PILOT program, can throw PILOTException
        /// </summary>
        /// <param name="file">the file name of the program to execute</param>
        public void Run(FileInfo file)
        {
            this.Run(PILOTParser.ParseProgram(file));
        }

        /// <summary>
        /// Runs a PILOT program, can throw RunTimeException
        /// </summary>
        /// <param name="program">the program to execute</param>
        public void Run(PILOTProgram program)
        {

            // clear current memory first
            this.ClearMemoryState();

            // initialize a call stack
            int executionPointer = 0;
            Stack<int> callStack = new Stack<int>();
            MatchTypes matchState = MatchTypes.None;
            int matchStateOrdinal = 0;
            String acceptBuffer = String.Empty;

            // execute until we cant
            while (executionPointer < program.LineNumbers.Count)
            {
                
                // execute statement
                Line line = program[program.LineNumbers[executionPointer]];
                IStatement statement = line.LineStatement;

                // check for match type to determine whether or not to execute
                if (statement != null)
                {
                    if ((matchState == MatchTypes.None) || (statement.MatchType == matchState) || (statement.MatchType == MatchTypes.None))
                    {

                        // evaluate either immediate statement or others
                        if (statement is IImmediateStatement)
                        {
                            this.EvaluateImmediateStatement((IImmediateStatement)statement);
                            executionPointer++;
                        }
                        else
                        {

                            // evaluate boolean condition to determine whether or not to execute
                            if (this.EvaluateBooleanCondition(line.LineStatement.IfCondition) == true)
                            {

                                // evaluate keyword
                                if (statement is IImmediateStatement)
                                {
                                    this.EvaluateImmediateStatement((IImmediateStatement)statement);
                                    executionPointer++;
                                }
                                else if (statement is Accept)
                                {
                                    acceptBuffer = this.pilotInterface.ReadTextLine().Trim();
                                    Accept a = (Accept)statement;
                                    if (a.VariableToSet != null)
                                    {
                                        if (a.VariableToSet.VariableName.StartsWith("#") == true)
                                        {
                                            try
                                            {
                                                this.SetNumericVar(a.VariableToSet.VariableName, Convert.ToDouble(acceptBuffer));
                                            }
                                            catch (Exception)
                                            {
                                                throw new RunTimeException(String.Format("Variable {0} requires numeric input", a.VariableToSet.VariableName));
                                            }
                                        }
                                        else
                                        {
                                            this.SetStringVar(a.VariableToSet.VariableName, acceptBuffer);
                                        }
                                    }
                                    executionPointer++;
                                }
                                else if (statement is Jump)
                                {
                                    Jump j = (Jump)statement;
                                    executionPointer = program.OrdinalOfLabel(j.LabelToJumpTo.LabelName);
                                    if (executionPointer < 0)
                                    {
                                        throw new RunTimeException(String.Format("Could not find label: {0}", j.LabelToJumpTo.LabelName));
                                    }
                                }
                                else if (statement is Use)
                                {
                                    Use u = (Use)statement;
                                    callStack.Push(executionPointer + 1);
                                    executionPointer = program.OrdinalOfLabel(u.LabelToUse.LabelName);
                                    if (executionPointer < 0)
                                    {
                                        throw new RunTimeException(String.Format("Could not find label: {0}", u.LabelToUse.LabelName));
                                    }
                                }
                                else if (statement is End)
                                {
                                    if (callStack.Count == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        executionPointer = callStack.Pop();
                                    }
                                }
                                else if (statement is PILOTMatch)
                                {
                                    PILOTMatch m = (PILOTMatch)statement;
                                    matchState = MatchTypes.N;
                                    matchStateOrdinal = -1;
                                    for (int i = 0; i < m.Conditions.Count; i++)
                                    {
                                        String evalCondition = this.EvaluateStringExpression(m.Conditions[i]).Trim().ToUpper();
                                        if (acceptBuffer.Trim().ToUpper() == evalCondition)
                                        {
                                            matchState = MatchTypes.Y;
                                            matchStateOrdinal = i;
                                            break;
                                        }
                                    }
                                    executionPointer++;
                                }
                                else if (statement is JumpOnMatch)
                                {
                                    JumpOnMatch jom = (JumpOnMatch)statement;
                                    if (matchState != MatchTypes.Y)
                                    {
                                        executionPointer++;
                                    }
                                    else
                                    {
                                        if (matchStateOrdinal > jom.LabelsToJumpTo.Count)
                                        {
                                            throw new RunTimeException("Jump on Match statement does not have enough labels");
                                        }
                                        else
                                        {
                                            executionPointer = program.OrdinalOfLabel(jom.LabelsToJumpTo[matchStateOrdinal].LabelName);
                                            if (executionPointer < 0)
                                            {
                                                throw new RunTimeException(String.Format("Could not find label: {0}", jom.LabelsToJumpTo[matchStateOrdinal].LabelName));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                executionPointer++;
                            }
                        }
                    }
                    else
                    {
                        executionPointer++;
                    }
                }
                else
                {
                    executionPointer++;
                }   
            }
        }

        /// <summary>
        /// Evaluates an immediate statement, can throw PILOTException
        /// </summary>
        /// <param name="statement">the statement to evaluate</param>
        public void EvaluateImmediateStatement(String statement)
        {

            // short circuit
            if (String.IsNullOrWhiteSpace(statement) == true)
            {
                throw new RunTimeException("Statement must contain instructions to execute");
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
            catch (InvalidCastException)
            {
                throw new RunTimeException(String.Format("The following statement is not allowed to immediately execute: {0}", statement));
            }
        }

        /// <summary>
        /// Evaluates an immediate statement, can throw RunTimeException
        /// </summary>
        /// <param name="statement">the statement to evaluate</param>
        internal void EvaluateImmediateStatement(IImmediateStatement statement)
        {
            if (this.EvaluateBooleanCondition(statement.IfCondition) == true)
            {
                if (statement is Compute)
                {
                    Compute cs = (Compute)statement;
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
                else if (statement is Text)
                {
                    Text ts = (Text)statement;
                    String textOut = this.EvaluateStringExpression(ts.TextToDisplay).Trim();
                    Boolean carriageReturn = true;
                    if (textOut.EndsWith("\\") == true)
                    {
                        carriageReturn = false;
                        textOut = textOut.Substring(0, textOut.Length - 1);
                    }
                    this.pilotInterface.WriteText(textOut, carriageReturn);
                }
                else if (statement is TextClear)
                {
                    TextClear tc = (TextClear)statement;
                    this.pilotInterface.ClearText();
                    String textOut = this.EvaluateStringExpression(tc.TextToDisplay).Trim();
                    Boolean carriageReturn = true;
                    if (textOut.EndsWith("\\") == true)
                    {
                        carriageReturn = false;
                        textOut = textOut.Substring(0, textOut.Length - 1);
                    }
                    this.pilotInterface.WriteText(textOut, carriageReturn);
                }
                else if (statement is TurtleGraphics)
                {
                    TurtleGraphics tg = (TurtleGraphics)statement;
                    foreach (IGraphicsExpression ge in tg.GraphicsExpressions)
                    {
                        this.EvaluateGraphicsExpression(ge);
                    }
                }
                else if (statement is Remark)
                {

                    // do nothing!
                }
                else if (statement is Pause)
                {
                    Pause pa = (Pause)statement;
                    double timeToPause = this.EvaluateNumericExpression(pa.TimeToPause);
                    Thread.Sleep(Convert.ToInt32(timeToPause * 1000 / 60));
                }
            }
        }

        /// <summary>
        /// Evaluate a numeric expression, can throw PILOTException
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
                    int sign = ((PILOTInterpreter.randomGenerator.NextDouble() * 2.0) > 1) ? -1 : 1;
                    double value = Math.Ceiling(PILOTInterpreter.randomGenerator.NextDouble() * ((sign > 0) ? 32767 : 32768));
                    retVal = sign * value;
                }
                else if (numericalExpression is NumericVariable)
                {
                    NumericVariable numVar = (NumericVariable)numericalExpression;
                    retVal = (this.VarExists(numVar.VariableName) == true) ? this.GetNumericVar(numVar.VariableName) : 0;
                }
                else if (numericalExpression is NumericBinaryOperation)
                {
                    NumericBinaryOperation opExpression = (NumericBinaryOperation)numericalExpression;
                    double rightResult = this.EvaluateNumericExpression(opExpression.Right);
                    if (opExpression.Operator == NumericBinaryOperators.Eq)
                    {
                        this.SetNumericVar(opExpression.Left.ToString(), rightResult);
                        retVal = rightResult;
                    }
                    else
                    {
                        double leftResult = this.EvaluateNumericExpression(opExpression.Left);
                        if (opExpression.Operator == NumericBinaryOperators.Add)
                        {
                            retVal = leftResult + rightResult;
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Div)
                        {
                            retVal = leftResult / rightResult;
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Mod)
                        {
                            retVal = Math.Abs(Convert.ToInt64(leftResult) % Convert.ToInt64(rightResult));
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Mult)
                        {
                            retVal = leftResult * rightResult;
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Sub)
                        {
                            retVal = leftResult - rightResult;
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Log)
                        {
                            retVal = Math.Log(leftResult, rightResult);
                        }
                        else if (opExpression.Operator == NumericBinaryOperators.Exp)
                        {
                            retVal = Math.Pow(leftResult, rightResult);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", numericalExpression.ToString()), e);
            }

            return Math.Round(retVal, 2);
        }

        /// <summary>
        /// Evaluates a string expression, can throw PILOTException
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
                return String.Empty;
            }

            // var init
            String retVal = String.Empty;

            // evaluate based upon what type of string expression
            try
            {
                if (stringExpression is StringVariable)
                {
                    StringVariable stringVar = (StringVariable)stringExpression;
                    retVal = (this.VarExists(stringVar.VariableName) == true) ? this.GetStringVar(stringVar.VariableName) : String.Empty;
                }
                else if (stringExpression is StringAssignExpression)
                {
                    StringAssignExpression sae = (StringAssignExpression)stringExpression;
                    retVal = this.EvaluateStringExpression(sae.Right);
                    this.SetStringVar(sae.Left.VariableName, retVal);
                }
                else if (stringExpression is StringLiteral)
                {
                    String[] split = ((StringLiteral)stringExpression).StringText.Split(new char[] { ' ' });
                    foreach (String s in split)
                    {
                        String splitPart = s.Trim();
                        if ((splitPart.Length > 1) && ((splitPart[0] == '#') || (splitPart[0] == '$') || (splitPart[0] == '%')))
                        {

                            // extract the variable name
                            int puncLoc = PILOTInterpreter.FirstPunctuation(splitPart.Substring(1)) + 1;
                            String afterPunc = String.Empty;
                            if (puncLoc > 1)
                            {
                                afterPunc = splitPart.Substring(puncLoc);
                                splitPart = splitPart.Substring(0, puncLoc);
                            }

                            // attempt to replace variable name
                            try
                            {
                                splitPart = ((splitPart[0] == '#') || (splitPart[0] == '%')) ? this.GetNumericVar(splitPart).ToString("0.00") : this.GetStringVar(splitPart);
                                splitPart += afterPunc;
                            }
                            catch (RunTimeException)
                            {
                                splitPart = s.Trim();
                            }
                        }
                        retVal += splitPart + " ";
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
        /// Evaluates a graphics expression
        /// </summary>
        /// <param name="graphicsExpression">the graphics expression to evaluate</param>
        internal void EvaluateGraphicsExpression(IGraphicsExpression graphicsExpression)
        {
            
            // short circuit on null
            if (graphicsExpression == null)
            {
                throw new RunTimeException("Cannot evaluate a null graphics expression");
            }

            // var init
            String retVal = String.Empty;

            // evaluate based upon what type of string expression
            try
            {
                if ((graphicsExpression is ClearGraphics) || (graphicsExpression is QuitGraphics))
                {
                    this.pilotInterface.ClearGraphics();
                    this.SetNumericVar(PILOTInterpreter.X_VAR, 0);
                    this.SetNumericVar(PILOTInterpreter.Y_VAR, 0);
                    this.SetNumericVar(PILOTInterpreter.THETA_VAR, 0);
                    this.SetNumericVar(PILOTInterpreter.WIDTH_VAR, 1);
                    this.SetNumericVar(PILOTInterpreter.COLOR_VAR, (double)PenColors.YELLOW);
                }
                else if (graphicsExpression is PILOTPen)
                {
                    PILOTPen pp = (PILOTPen)graphicsExpression;
                    if (pp.PenColorExpression == null)
                    {
                        this.SetNumericVar(PILOTInterpreter.COLOR_VAR, (double)pp.PenColor);
                    }
                    else
                    {
                        int colorVal = (int)this.EvaluateNumericExpression(pp.PenColorExpression);
                        if ((colorVal >= (int)PenColors.ERASE) && (colorVal <= (int)PenColors.WHITE))
                        {
                            this.SetNumericVar(PILOTInterpreter.COLOR_VAR, (double)colorVal);
                        }
                        else
                        {
                            this.SetNumericVar(PILOTInterpreter.COLOR_VAR, (double)PenColors.ERASE);
                        }
                    }
                }
                else if (graphicsExpression is Turn)
                {
                    Turn t = (Turn)graphicsExpression;
                    int turnAngle = (int)this.EvaluateNumericExpression(t.TurnAngle);
                    int currentAngle = (int)this.GetNumericVar(PILOTInterpreter.THETA_VAR);
                    this.SetNumericVar(PILOTInterpreter.THETA_VAR, Convert.ToInt32((currentAngle + turnAngle) % 360));
                }
                else if (graphicsExpression is TurnTo)
                {
                    TurnTo tt = (TurnTo)graphicsExpression;
                    int turnToAngle = (int)this.EvaluateNumericExpression(tt.TurnToAngle);
                    this.SetNumericVar(PILOTInterpreter.THETA_VAR, Convert.ToInt32(turnToAngle % 360));
                }
                else if (graphicsExpression is Width)
                {
                    Width w = (Width)graphicsExpression;
                    int penWidth = Math.Max(1, (int)this.EvaluateNumericExpression(w.PenWidth));
                    this.SetNumericVar(PILOTInterpreter.WIDTH_VAR, penWidth);
                }
                else
                {

                    // var init
                    Point currentPoint = new Point((int)this.GetNumericVar(PILOTInterpreter.X_VAR), (int)this.GetNumericVar(PILOTInterpreter.Y_VAR));
                    Point transStart = this.TranslatePoint(currentPoint);
                    Point endPoint = new Point(currentPoint.X, currentPoint.Y);
                    float currentLineWidth = (float)this.GetNumericVar(PILOTInterpreter.WIDTH_VAR);
                    PenColors currentColor = (PenColors)this.GetNumericVar(PILOTInterpreter.COLOR_VAR);
                    int currentAngle = (int)this.GetNumericVar(PILOTInterpreter.THETA_VAR);

                    // parse commands that actually draw
                    if (graphicsExpression is Draw)
                    {

                        // var init & calculations
                        Draw d = (Draw)graphicsExpression;
                        endPoint = PILOTInterpreter.DrawDistanceAngle(currentPoint, currentAngle, (int)this.EvaluateNumericExpression(d.DrawDistance));
                        Point transEnd = this.TranslatePoint(endPoint);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (Pen p = new Pen(EnumMethods.PenColorToColor(currentColor), currentLineWidth))
                                {
                                    g.DrawLine(p, transStart, transEnd);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is DrawTo)
                    {

                        // var init & calculations
                        DrawTo dt = (DrawTo)graphicsExpression;
                        endPoint = new Point((int)this.EvaluateNumericExpression(dt.DrawToX), (int)this.EvaluateNumericExpression(dt.DrawToY));
                        Point transEnd = this.TranslatePoint(endPoint);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (Pen p = new Pen(EnumMethods.PenColorToColor(currentColor), currentLineWidth))
                                {
                                    g.DrawLine(p, transStart, transEnd);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is Fill)
                    {
                    }
                    else if (graphicsExpression is FillTo)
                    {
                    }
                    else if (graphicsExpression is Go)
                    {

                        // var init & calculations
                        Go go = (Go)graphicsExpression;
                        endPoint = PILOTInterpreter.DrawDistanceAngle(currentPoint, currentAngle, (int)this.EvaluateNumericExpression(go.GoDistance));
                        Point transEnd = this.TranslatePoint(endPoint);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (SolidBrush b = new SolidBrush(EnumMethods.PenColorToColor(currentColor)))
                                {
                                    g.FillRectangle(b, (float)(transEnd.X - (.5 * currentLineWidth)), (float)(transEnd.Y - (.5 * currentLineWidth)), currentLineWidth, currentLineWidth);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is Goto)
                    {

                        // var init & calculations
                        Goto gt = (Goto)graphicsExpression;
                        endPoint = new Point((int)this.EvaluateNumericExpression(gt.GotoX), (int)this.EvaluateNumericExpression(gt.GotoY));
                        Point transEnd = this.TranslatePoint(endPoint);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (SolidBrush b = new SolidBrush(EnumMethods.PenColorToColor(currentColor)))
                                {
                                    g.FillRectangle(b, (float)(transEnd.X - (.5 * currentLineWidth)), (float)(transEnd.Y - (.5 * currentLineWidth)), currentLineWidth, currentLineWidth);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is Box)
                    {

                        // var init & calculations
                        Box b = (Box)graphicsExpression;
                        int width = (int)this.EvaluateNumericExpression(b.BoxWidth);
                        int height = (int)this.EvaluateNumericExpression(b.BoxHeight);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (Pen p = new Pen(EnumMethods.PenColorToColor(currentColor), currentLineWidth))
                                {
                                    Rectangle r = new Rectangle(transStart.X, transStart.Y, width, height);
                                    g.DrawRectangle(p, r);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is BoxFill)
                    {

                        // var init & calculations
                        BoxFill bf = (BoxFill)graphicsExpression;
                        int width = (int)this.EvaluateNumericExpression(bf.BoxWidth);
                        int height = (int)this.EvaluateNumericExpression(bf.BoxHeight);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (SolidBrush b = new SolidBrush(EnumMethods.PenColorToColor(currentColor)))
                                {
                                    Rectangle r = new Rectangle(transStart.X, transStart.Y, width, height);
                                    g.FillRectangle(b, r);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is Ellipse)
                    {

                        // var init & calculations
                        Ellipse e = (Ellipse)graphicsExpression;
                        int horizontalRadius = (int)this.EvaluateNumericExpression(e.HorizontalRadius);
                        int verticalRadius = (int)this.EvaluateNumericExpression(e.VerticalRadius);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (Pen p = new Pen(EnumMethods.PenColorToColor(currentColor), currentLineWidth))
                                {
                                    Rectangle r = new Rectangle(transStart.X - horizontalRadius, transStart.Y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
                                    g.DrawEllipse(p, r);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is EllipseFill)
                    {

                        // var init & calculations
                        EllipseFill ef = (EllipseFill)graphicsExpression;
                        int horizontalRadius = (int)this.EvaluateNumericExpression(ef.HorizontalRadius);
                        int verticalRadius = (int)this.EvaluateNumericExpression(ef.VerticalRadius);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (SolidBrush b = new SolidBrush(EnumMethods.PenColorToColor(currentColor)))
                                {
                                    Rectangle r = new Rectangle(transStart.X - horizontalRadius, transStart.Y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
                                    g.FillEllipse(b, r);
                                }
                            }
                        }
                    }
                    else if (graphicsExpression is Print)
                    {

                        // var init & calculations
                        Print p = (Print)graphicsExpression;
                        String text = this.EvaluateStringExpression(p.TextToPrint);
                        int size = (int)this.EvaluateNumericExpression(p.TextSize);

                        // plot
                        lock (this.pilotInterface.GraphicsOutput)
                        {
                            using (Graphics g = Graphics.FromImage(this.pilotInterface.GraphicsOutput))
                            {
                                using (Font f = new Font("System", size))
                                {
                                    using (SolidBrush b = new SolidBrush(EnumMethods.PenColorToColor(currentColor)))
                                    {
                                        g.DrawString(text, f, b, transStart);
                                    }
                                }
                            }
                        }
                    }

                    // update current point
                    this.SetNumericVar(PILOTInterpreter.X_VAR, endPoint.X);
                    this.SetNumericVar(PILOTInterpreter.Y_VAR, endPoint.Y);

                    // redraw
                    this.pilotInterface.RedrawGraphics();
                }
            }
            catch (Exception e)
            {
                throw new RunTimeException(String.Format("Could not evaluate the expression: {0}", graphicsExpression.ToString()), e);
            }
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

            // add the variable if neccessary
            varName = varName.Trim().ToUpper();
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

            // add the variable if neccessary
            varName = varName.Trim().ToUpper();
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
        /// Translates points from zero-centered to .NET image box
        /// </summary>
        /// <param name="p">zero centered point</param>
        /// <returns>.NET image style point</returns>
        internal Point TranslatePoint(Point p)
        {
            return new Point(Convert.ToInt32(.5 * this.graphicsSize.X) + p.X,
                             Convert.ToInt32(.5 * this.graphicsSize.Y) - p.Y);
        }

        /// <summary>
        /// Draw from a point a particular distance
        /// </summary>
        /// <param name="start">the start point</param>
        /// <param name="angle">the PILOT angle</param>
        /// <param name="distance">the distance</param>
        /// <returns>the destination point to draw to in PILOT coordinates</returns>
        internal static Point DrawDistanceAngle(Point start, int angle, int distance)
        {

            // var init
            Point retVal = new Point(start.X, start.Y);

            // calculate destination point in PILOT coordinates
            if ((angle >= 0) && (angle <= 360))
            {
                if ((angle == 0) || (angle == 360))
                {
                    retVal.Y += distance;
                }
                else if (angle == 90)
                {
                    retVal.X += distance;
                }
                else if (angle == 180)
                {
                    retVal.Y -= distance;
                }
                else if (angle == 270)
                {
                    retVal.X -= distance;
                }
                else if (angle < 90)
                {
                    retVal.X += (int)(Math.Sin(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                    retVal.Y += (int)(Math.Cos(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                }
                else if (angle < 180)
                {
                    angle -= 90;
                    retVal.X += (int)(Math.Cos(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                    retVal.Y -= (int)(Math.Sin(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                }
                else if (angle < 270)
                {
                    angle -= 180;
                    retVal.X -= (int)(Math.Sin(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                    retVal.Y -= (int)(Math.Cos(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                }
                else
                {
                    angle -= 270;
                    retVal.X -= (int)(Math.Cos(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                    retVal.Y += (int)(Math.Sin(PILOTInterpreter.DegreesToRadians(angle)) * distance);
                }
            }
            else if (angle > 360)
            {
                retVal = PILOTInterpreter.DrawDistanceAngle(start, angle % 360, distance);
            }
            else
            {
                retVal = PILOTInterpreter.DrawDistanceAngle(start, 360 + angle, distance);
            }

            return retVal;
        }

        /// <summary>
        /// Writes output to the interface, handles trailing slashes and new lines
        /// </summary>
        /// <param name="pilotOutput">the pilot output to write</param>
        private void WriteOutput(String pilotOutput)
        {
            Boolean newLine = true;
            if (pilotOutput[pilotOutput.Length - 1] == '\\')
            {
                newLine = false;
                pilotOutput = pilotOutput.Substring(0, pilotOutput.Length - 1);
            }
            this.pilotInterface.WriteText(pilotOutput, newLine);
        }

        private static void RecurseFill(Graphics g, Bitmap b, Point start, Point end, Point examine)
        {

        }

        /// <summary>
        /// Conversion from degrees to radians
        /// </summary>
        /// <param name="degrees">degrees</param>
        /// <returns>radians</returns>
        private static double DegreesToRadians(int degrees)
        {
            // var init
            double retVal = 0;

            if (degrees == 360)
            {
                retVal = 0;
            }
            else if ((degrees >= 0) && (degrees < 360))
            {
                retVal = (Convert.ToDouble(degrees) / 360.0) * (2 * Math.PI);
            }
            else if (degrees < 0)
            {
                retVal = PILOTInterpreter.DegreesToRadians(360 + degrees);
            }
            else
            {
                retVal = PILOTInterpreter.DegreesToRadians(degrees % 360);
            }

            return retVal;
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
