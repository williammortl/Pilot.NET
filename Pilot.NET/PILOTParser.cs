﻿namespace Pilot.NET
{
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.GraphicsExpressions;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using Pilot.NET.Lang.Statements;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Parses strings into a PILOT AST
    /// </summary>
    public static class PILOTParser
    {

        /// <summary>
        /// Parse the file into a program, can throw ParserException
        /// </summary>
        /// <param name="file">the filename</param>
        /// <returns>the PILOT program</returns>
        public static PILOTProgram ParseProgram(FileInfo file)
        {

            // init vars
            PILOTProgram retVal = null;

            // parse file
            try
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    retVal = PILOTParser.ParseProgram(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                throw new PILOTException(String.Format("Could not find the file: {0}", file.FullName));
            }
            catch (IOException)
            {
                throw new PILOTException(String.Format("Could not access the file: {0}", file.FullName));
            }

            return retVal;
        }

        /// <summary>
        /// Parse the file into a program, can throw ParserException
        /// </summary>
        /// <param name="text">the program, lines seperated by \n</param>
        /// <returns>the PILOT program</returns>
        public static PILOTProgram ParseProgram(String text)
        {

            // init vars
            String[] lines = text.Split(new char[] { '\n' });
            PILOTProgram retVal = new PILOTProgram();

            // loop through each line, extract line number, create the line object
            foreach (String line in lines)
            {
                String trimmedLine = line.Trim();
                if (String.IsNullOrWhiteSpace(trimmedLine) == false)
                {

                    // short circuit
                    if (String.IsNullOrWhiteSpace(trimmedLine) == true)
                    {
                        throw new ParserException("Line must contain syntax");
                    }

                    // split the string by ' ', make sure the split contains at least 2 items
                    String[] split = trimmedLine.Split(new char[] { ' ' }, 2);
                    if (split.Length < 2)
                    {
                        throw new ParserException(String.Format("The line:\r\n{0}\r\ndoes not contain valid syntax", text));
                    }
                    String lineNumberText = split[0].Trim();
                    String lineText = split[1].Trim();

                    // get line number, begin the statement string extraction
                    int lineNumber = 0;
                    try
                    {
                        lineNumber = System.Convert.ToInt32(lineNumberText);
                    }
                    catch (Exception e)
                    {
                        throw new ParserException(String.Format("The line:\r\n{0}\r\ndoes not contain a valid line number.", text), e);
                    }

                    Line newLine = PILOTParser.ParseLine(lineText);
                    retVal[lineNumber] = newLine;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Parses the string equivalent of the line and returns a line object repsenting it,
        /// can throw ParserException
        /// </summary>
        /// <param name="text">the text representing the line</param>
        /// <returns>line object repsenting the text</returns>
        public static Line ParseLine(String text)
        {

            // check for a label, build the Label object, build a statement object
            text = text.Trim();
            String labelString = String.Empty;
            if (text.StartsWith("*") == true)
            {
                String[] splitStatementString = text.Split(new char[] { ' ' }, 2);
                labelString = splitStatementString[0].Trim();
                if (splitStatementString.Length > 1)
                {
                    text = splitStatementString[1].Trim();
                }
                else
                {
                    text = String.Empty;
                }
            }
            Label lineLabel = PILOTParser.ParseLabel(labelString);
            IStatement lineStatement = PILOTParser.ParseStatement(text);

            return new Line(lineLabel, lineStatement);
        }

        /// <summary>
        /// Parses text into a label, can throw ParserException
        /// </summary>
        /// <param name="text">the text</param>
        /// <returns>a new Label or null if the string is empty</returns>
        internal static Label ParseLabel(String text)
        {

            // var init
            Label retVal = null;

            // parse the text
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == false)
            {
                retVal = new Label(text);
            }

            return retVal;
        }

        /// <summary>
        /// Take the statement string, creates the appropriate statement object,
        /// can throw ParserException
        /// </summary>
        /// <param name="text">the statement string</param>
        /// <returns>a statement object, null if the string is empty</returns>
        internal static IStatement ParseStatement(String text)
        {

            // short circuit on String.Empty
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // split keyword/match/if condition and the parameters
            String[] splitStatementString = text.Split(new char[] { ':' }, 2);
            String keywordMatchIf = splitStatementString[0].Trim();
            String parametersForKeyword = (splitStatementString.Length <= 1) ? String.Empty : splitStatementString[1].Trim();

            // extract the if condition, create BooleanCondition
            String keywordMatch = String.Empty;
            String ifConditionString = String.Empty;
            int parensLoc = keywordMatchIf.IndexOf('(');
            if (parensLoc >= 0)
            {
                ifConditionString = keywordMatchIf.Substring(parensLoc + 1);
                ifConditionString = ifConditionString.Substring(0, ifConditionString.Length - 1).Trim();
                keywordMatch = keywordMatchIf.Substring(0, parensLoc).Trim();
            }
            else
            {
                keywordMatch = keywordMatchIf;
            }
            keywordMatch = keywordMatch.ToUpper();
            BooleanCondition ifCondition = PILOTParser.ParseBooleanCondition(ifConditionString);

            // split keyword and match condition
            MatchTypes match = MatchTypes.None;
            String keywordString = keywordMatch;
            if ((keywordMatch.Length > 1) && (Enum.IsDefined(typeof(MatchTypes), keywordMatch[keywordMatch.Length - 1].ToString()) == true))
            {
                keywordString = keywordMatch.Substring(0, keywordMatch.Length - 1);
                match = (MatchTypes)Enum.Parse(typeof(MatchTypes), keywordMatch[keywordMatch.Length - 1].ToString());
            }

            // attempt to parse the keyword, check to make sure it is in fact a keyword, if it isn't throw an error
            if (Enum.IsDefined(typeof(Keywords), keywordString) == false)
            {
                throw new ParserException(String.Format("{0} is not a valid keyword in PILOT", keywordString));
            }
            Keywords keyword = (Keywords)Enum.Parse(typeof(Keywords), keywordString);
  
            // switch based upon the keyword and create new statement object
            IStatement statement = null; 
            switch (keyword)
            {
                case Keywords.A:
                {
                    statement = new Accept(PILOTParser.ParseVariable(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.C:
                {
                    statement = new Compute(PILOTParser.ParseAssignmentExpression(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.E:
                {
                    statement = new End(match, ifCondition);
                    break;
                }
                case Keywords.J:
                {
                    statement = new Jump(PILOTParser.ParseLabel(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.JM:
                {

                    // create linked list of labels
                    List<Label> labels = new List<Label>();
                    if (String.IsNullOrWhiteSpace(parametersForKeyword) == false)
                    {
                        String[] labelsString = parametersForKeyword.Split(new char[] { ',' });
                        foreach (String labelString in labelsString)
                        {
                            Label newLabel = PILOTParser.ParseLabel(labelString);
                            if (newLabel != null)
                            {
                                labels.Add(newLabel);
                            }
                            else
                            {
                                throw new ParserException("Cannot have empty labels in jump on match statement");
                            }
                        }
                    }
                    if (labels.Count < 1)
                    {
                        throw new ParserException("Jump on match needs at least one label to jump to");
                    }

                    statement = new JumpOnMatch(labels, match, ifCondition);
                    break;
                }
                case Keywords.M:
                {

                    // create linked list of StringLiteral conditions
                    List<StringLiteral> conditions = new List<StringLiteral>();
                    if (String.IsNullOrWhiteSpace(parametersForKeyword) == false)
                    {
                        String[] conditionsString = parametersForKeyword.Split(new char[] { ',' });
                        foreach (String conditionString in conditionsString)
                        {
                            if (String.IsNullOrWhiteSpace(conditionString) == false)
                            {
                                conditions.Add(new StringLiteral(conditionString));
                            }
                            else
                            {
                                throw new ParserException("Cannot have empty conditions in a match statement");
                            }
                        }
                    }
                    if (conditions.Count < 1)
                    {
                        throw new ParserException("Match needs at least one condition to evaluate");
                    }

                    statement = new PILOTMatch(conditions, match, ifCondition);
                    break;
                }
                case Keywords.PA:
                {
                    statement = new Pause(PILOTParser.ParseNumericExpression(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.R:
                {
                    statement = new Remark(parametersForKeyword);
                    break;
                }
                case Keywords.T:
                {
                    statement = new Text(PILOTParser.ParseStringExpression(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.TC:
                {
                    statement = new TextClear(PILOTParser.ParseStringExpression(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.U:
                {
                    statement = new Use(PILOTParser.ParseLabel(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.GR:
                {
                    statement = new TurtleGraphics(PILOTParser.ParseGraphicsExpression(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.SO:
                {

                    // get the two numeric expressions for the sound keyword
                    INumericExpression Note = null;
                    INumericExpression Duration = null;
                    if (String.IsNullOrWhiteSpace(parametersForKeyword) == false)
                    {
                        String[] conditionsString = parametersForKeyword.Split(new char[] { ',' });
                        if (conditionsString.Length == 2)
                        {
                            Note = PILOTParser.ParseNumericExpression(conditionsString[0]);
                            Duration = PILOTParser.ParseNumericExpression(conditionsString[1]);
                        }
                        else
                        {
                            throw new ParserException("Sound requires two numeric expression seperated by a comma");
                        }
                    }
                    if ((Note == null) || (Duration == null))
                    {
                        throw new ParserException("Invalid sound keyword");
                    }

                    statement = new Sound(Note, Duration, match, ifCondition);
                    break;
                }
            }

            return statement;
        }

        /// <summary>
        /// Parse the string into an numeric expression, can throw ParserException
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns>the numeric expression object, null if empty string or a problem</returns>
        internal static INumericExpression ParseNumericExpression(String text)
        {

            // short circuit on String.Empty
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // throw exception on more than one assignment in the expression
            if (PILOTParser.CountOf("=", text) > 1)
            {
                throw new ParserException("Cannot have more than one assignment in a valid numeric expression");
            }

            // short circuit on malformed parentheses
            text = text.Trim();
            int leftParens = PILOTParser.CountOf("(", text);
            int rightParens = PILOTParser.CountOf(")", text);
            if (leftParens != rightParens)
            {
                throw new ParserException("Numeric expression parentheses are malformed");
            }

            // cleanup the string
            text = PILOTParser.CleanupNumericExpression(text).ToUpper();

            // unwrap unneccessary parentheses
            text = PILOTParser.UnwrapParentheses(text).Trim();

            // var init
            double literalNum = 0;
            INumericExpression retVal = null;
            int binaryOperatorPosition = PILOTParser.NextBinaryOperatorToEvaluate(text);

            // parse the text
            try
            {
                if (binaryOperatorPosition > 0)
                {

                    // parse the binary operator expression
                    NumericBinaryOperators binaryOperatorToUse = EnumMethods.StringToNumericBinaryOperator(text.Substring(binaryOperatorPosition));
                    int opStringLen = EnumMethods.NumericBinaryOperatorToString(binaryOperatorToUse).Length;
                    String leftExpression = text.Substring(0, binaryOperatorPosition).Trim();
                    String rightExpression = text.Substring(binaryOperatorPosition + opStringLen).Trim();
                    retVal = new NumericBinaryOperation(binaryOperatorToUse,
                                                        PILOTParser.ParseNumericExpression(leftExpression),
                                                        PILOTParser.ParseNumericExpression(rightExpression));
                }
                else if (text == "?")
                {

                    // random number
                    return new RandomNumber();
                }
                else if ((text.StartsWith("#") == true) || (text.StartsWith("%") == true))
                {

                    // numeric variable
                    retVal = new NumericVariable(text);
                }
                else if ((text[0] == '-') && ((text[1] == '#') || (text.StartsWith("%") == true)))
                {

                    // handle -#varname -> (-1 * #varname)
                    String varName = text.Substring(1);
                    retVal = PILOTParser.ParseNumericExpression(String.Format("(-1 * {0})", varName));
                }
                else
                {

                    // at this point, since it's not a variable, its a Numeric Literal, 
                    //  it should be ok to do a replace for mathematical constants
                    text = text.Replace("E", Math.E.ToString());
                    text = text.Replace("PI", Math.PI.ToString());

                    // attempt to parse into a numeric literal
                    if (double.TryParse(text, out literalNum) == true)
                    {

                        // numeric literal
                        retVal = new NumericLiteral(literalNum);
                    }
                    else
                    {
                        throw new ParserException(String.Format("Could not determine what type of numeric expression exists in text: {0}", text));
                    }
                }
            }
            catch (PILOTException e)
            {
                retVal = null;
                throw new ParserException(String.Format("Cannot parse the text as a valid numeric expression: {0}", text), e);
            }

            return retVal;
        }

        /// <summary>
        /// Parses a string to a string expression, can throw ParserException
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns>the string expression, null if empty string</returns>
        internal static IStringExpression ParseStringExpression(String text)
        {

            // short circuit if String.Empty
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // var init
            IStringExpression retVal = null;

            // parse string expression
            try
            {
                if (text.StartsWith("$") == true)
                {
                    if (PILOTParser.CountOf("=", text) == 1)
                    {
                        
                        // build assignment expression
                        int operatorPosition = text.IndexOf('=');
                        String leftExpression = text.Substring(0, operatorPosition).Trim();
                        String rightExpression = text.Substring(operatorPosition + 1).Trim();
                        retVal = new StringAssignExpression(new StringVariable(leftExpression),
                                                            PILOTParser.ParseStringExpression(rightExpression));
                    }
                    else if ((PILOTParser.CountOf("=", text) == 0) && (PILOTParser.CountOf(" ", text) == 0))
                    {
                        retVal = new StringVariable(text);
                    }
                    else
                    {
                        retVal = new StringLiteral(text);
                    }
                }
                else
                {
                    retVal = new StringLiteral(text);
                }
            }
            catch (PILOTException e)
            {
                throw new ParserException(String.Format("Could not parse the PILOT text to a string expression: {0}", text), e);
            }

            return retVal;
        }

        /// <summary>
        /// Parses a string to a graphics expression, can throw ParserException
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns>list of graphics expressions</returns>
        internal static List<IGraphicsExpression> ParseGraphicsExpression(String text)
        {

            // var init
            List<IGraphicsExpression> retVal = new List<IGraphicsExpression>();

            // if string is empty, then that is the same as a graphics clear
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                retVal.Add(new ClearGraphics());
                return retVal;
            }

            // split by ; to allow multiple expressions per line
            text = text.Trim().ToUpper();
            String[] graphicsExpressions = text.Split(new char[1] { ';' });
            foreach (String graphicsExpression in graphicsExpressions)
            {

                // split the expression keyword from the parameters
                String[] textSplit = graphicsExpression.Trim().Split(new char[1] { ' ' }, 2);
                String expressionParameters = (textSplit.Length > 1) ? textSplit[1].Trim() : String.Empty;

                // throw exception on more than one assignment in the expression
                if (PILOTParser.CountOf("=", expressionParameters) > 0)
                {
                    throw new ParserException("Cannot have any assignments in a valid graphical expression");
                }

                // short circuit on malformed parentheses
                int leftParens = PILOTParser.CountOf("(", expressionParameters);
                int rightParens = PILOTParser.CountOf(")", expressionParameters);
                if (leftParens != rightParens)
                {
                    throw new ParserException("Graphical expression parentheses are malformed");
                }

                // cleanup the string
                expressionParameters = PILOTParser.CleanupNumericExpression(expressionParameters).ToUpper();

                // switch based upon expression keyword
                if (Enum.IsDefined(typeof(GraphicsExpressionKeywords), textSplit[0]) == false)
                {
                    throw new ParserException(String.Format("{0} is not a valid graphics expression keyword in PILOT", textSplit[0]));
                }
                GraphicsExpressionKeywords keyword = (GraphicsExpressionKeywords)Enum.Parse(typeof(GraphicsExpressionKeywords), textSplit[0]);
                switch (keyword)
                {
                    case GraphicsExpressionKeywords.CLEAR:
                    {
                        retVal.Add(new ClearGraphics());
                        break;
                    }
                    case GraphicsExpressionKeywords.DRAW:
                    {
                        INumericExpression drawDistance = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new Draw(drawDistance));
                        break;
                    }
                    case GraphicsExpressionKeywords.DRAWTO:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression drawToX = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression drawToY = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new DrawTo(drawToX, drawToY));
                        break;
                    }
                    case GraphicsExpressionKeywords.FILL:
                    {
                        INumericExpression fillDistance = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new Fill(fillDistance));
                        break;
                    }
                    case GraphicsExpressionKeywords.FILLTO:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression fillToX = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression fillToY = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new FillTo(fillToX, fillToY));
                        break;
                    }
                    case GraphicsExpressionKeywords.GO:
                    {
                        INumericExpression goDistance = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new Go(goDistance));
                        break;
                    }
                    case GraphicsExpressionKeywords.GOTO:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression gotoX = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression gotoY = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new Goto(gotoX, gotoY));
                        break;
                    }
                    case GraphicsExpressionKeywords.PEN:
                    {

                        // set pen color
                        if (Enum.IsDefined(typeof(PenColors), expressionParameters) == true)
                        {
                            retVal.Add(new PILOTPen((PenColors)Enum.Parse(typeof(PenColors), expressionParameters)));
                        }
                        else
                        {
                            retVal.Add(new PILOTPen(PILOTParser.ParseNumericExpression(expressionParameters)));
                        }

                        break;
                    }
                    case GraphicsExpressionKeywords.QUIT:
                    {
                        retVal.Add(new QuitGraphics());
                        break;
                    }
                    case GraphicsExpressionKeywords.TURN:
                    {
                        INumericExpression turnAngle = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new Turn(turnAngle));
                        break;
                    }
                    case GraphicsExpressionKeywords.TURNTO:
                    {
                        INumericExpression turnToAngle = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new TurnTo(turnToAngle));
                        break;
                    }
                    case GraphicsExpressionKeywords.WIDTH:
                    {
                        INumericExpression penWidth = PILOTParser.ParseNumericExpression(expressionParameters);
                        retVal.Add(new Width(penWidth));
                        break;
                    }
                    case GraphicsExpressionKeywords.BOX:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression height = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression width = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new Box(height, width));
                        break;
                    }
                    case GraphicsExpressionKeywords.BOXFILL:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression height = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression width = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new BoxFill(height, width));
                        break;
                    }
                    case GraphicsExpressionKeywords.ELLIPSE:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression horizontalRadius = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression verticalRadius = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new Ellipse(horizontalRadius, verticalRadius));
                        break;
                    }
                    case GraphicsExpressionKeywords.ELLIPSEFILL:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        INumericExpression horizontalRadius = PILOTParser.ParseNumericExpression(split[0]);
                        INumericExpression verticalRadius = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new EllipseFill(horizontalRadius, verticalRadius));
                        break;
                    }
                    case GraphicsExpressionKeywords.PRINT:
                    {
                        String[] split = expressionParameters.Split(new char[] { ',' });
                        StringVariable textToPrint = (StringVariable)PILOTParser.ParseVariable(split[0]);
                        INumericExpression textSize = PILOTParser.ParseNumericExpression(split[1]);
                        retVal.Add(new Print(textToPrint, textSize));
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Parses a string to a Boolean condition, can throw ParserException
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns>the boolean condition, null if empty string</returns>
        internal static BooleanCondition ParseBooleanCondition(String text)
        {

            // short circuit on String.Empty
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // cannot have more than one operator
            if ((PILOTParser.CountOf("=", text) > 1) ||
                (PILOTParser.CountOf("<", text) > 1) ||
                (PILOTParser.CountOf(">", text) > 1))
            {
                throw new ParserException("Cannot have more than one operator in a valid boolean condition");
            }

            // short circuit on malformed parentheses
            text = text.Trim();
            int leftParens = PILOTParser.CountOf("(", text);
            int rightParens = PILOTParser.CountOf(")", text);
            if (leftParens != rightParens)
            {
                throw new ParserException("Boolean condition parentheses are malformed");
            }

            // var init
            BooleanCondition retVal = null;

            // cleanup the string
            text = PILOTParser.CleanupNumericExpression(text);

            // unwrap unneccessary parentheses
            text = PILOTParser.UnwrapParentheses(text);

            // get the operator position
            int opertatorPosition = PILOTParser.NextBooleanOperatorToEvaluate(text);
            if (opertatorPosition <= 0)
            {
                throw new ParserException("A boolean operator was not found");
            }

            // split the string and extract the operator
            try
            {
                BooleanConditionOperators op = EnumMethods.StringToBooleanOperator(text.Substring(opertatorPosition));
                int opStringLen = EnumMethods.BooleanOperatorToString(op).Length;
                String leftExpression = text.Substring(0, opertatorPosition).Trim();
                String rightExpression = text.Substring(opertatorPosition + opStringLen).Trim();
                retVal = new BooleanCondition(op,
                                              PILOTParser.ParseNumericExpression(leftExpression),
                                              PILOTParser.ParseNumericExpression(rightExpression));
            }
            catch (PILOTException e)
            {
                retVal = null;
                throw new ParserException(String.Format("Cannot parse the text as a boolean condition: {0}", text), e);
            }

            return retVal;
        }

        /// <summary>
        /// Parse the string into a string or numeric assignment 
        /// expression, can throw a ParserException
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns>the assignment expression object, null if empty string</returns>
        internal static IExpression ParseAssignmentExpression(String text)
        {

            // short circuit if String.Empty
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // var init
            IExpression retVal = null;

            // parse assignment expression
            try
            { 
                if ((text.StartsWith("$") == true) && (PILOTParser.CountOf("=", text) == 1))
                {
                    retVal = PILOTParser.ParseStringExpression(text);
                }
                else
                {
                    retVal = PILOTParser.ParseNumericExpression(text);
                }
            }
            catch (PILOTException e)
            {
                throw new ParserException(String.Format("Could not parse the PILOT text to an assignment expression: {0}", text), e);
            }

            return retVal;
        }

        /// <summary>
        /// Parses text into an IVariable, can throw a ParserException
        /// </summary>
        /// <param name="text">the text to parse into a variable</param>
        /// <returns>an IVariable, null if the string is String.Empty</returns>
        internal static IVariable ParseVariable(String text)
        {

            //short circuit
            text = text.Trim();
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return null;
            }

            // var init
            IVariable retVal = null;

            // parse to variable
            try
            {
                if (text.StartsWith("$") == true)
                {
                    retVal = new StringVariable(text);
                }
                else
                {
                    retVal = new NumericVariable(text);
                }
            }
            catch (PILOTException e)
            {
                retVal = null;
                throw new ParserException(String.Format("Could not parse the PILOT text to variable: {0}", text), e);
            }

            return retVal;
        }

        /// <summary>
        /// The starting position in the string of the first operator to evaluate, assumes that
        /// the text string has been checked for parenthesis integrity, has had enclosing
        /// parentheses removed, all whitespacehas been condensed, and is in perfect form.
        /// Yields low operator precendence operators first.
        /// </summary>
        /// <param name="text">the string expression to analyze, this string should already be cleansed (no whitespace, no wrapping parens)</param>
        /// <returns>the starting position of the operator, 0 if no binary operator was found or error</returns>
        internal static int NextBinaryOperatorToEvaluate(String text)
        {

            // short circuit on empty string
            if (String.IsNullOrWhiteSpace(text) == true)
            {
                return 0;
            }

            // short circuit if assignment operator is found
            int equalPos = text.IndexOf(EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Eq)[0]);
            if (equalPos > 0)
            {
                return equalPos;
            }

            // var init for search
            int retVal = 0;
            int parenthesesDepth = 0;
            int prevOperatorFound = int.MaxValue;

            // loop through string and find operator
            for (int i = 0; i < text.Length; i++)
            {

                // loop var init
                char c = text[i];
                char prevC = text[Math.Max(0, i - 1)];

                // analyze the character
                if (c == '(')
                {
                    parenthesesDepth += 1;
                }
                else if (c == ')')
                {
                    parenthesesDepth -= 1;
                }
                else if ((i > 0) && (parenthesesDepth == 0))
                {

                    // binary operator cannot exist at position 0
                    // only if we are not parentheses wrapped search for an operator
                    // eval order is by low operator precedence and then left to right
                    if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Sub)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Sub) && (EnumMethods.IsNumericBinaryOperator(prevC) == false) && (prevC != '('))
                    {
                        retVal = i;
                        break;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Add)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Add) && (EnumMethods.IsNumericBinaryOperator(prevC) == false) && (prevC != '('))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Add;
                        retVal = i;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Mod)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Mod))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Mod;
                        retVal = i;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Div)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Div))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Div;
                        retVal = i;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Mult)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Mult))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Mult;
                        retVal = i;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Log)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Log))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Log;
                        retVal = i;
                    }
                    else if ((c == EnumMethods.NumericBinaryOperatorToString(NumericBinaryOperators.Exp)[0]) && (prevOperatorFound > (int)NumericBinaryOperators.Exp))
                    {
                        prevOperatorFound = (int)NumericBinaryOperators.Exp;
                        retVal = i;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Finds the boolean condition operator to evaluate
        /// </summary>
        /// <param name="text">>the string expression to analyze, this string should already be cleansed (no whitespace, no wrapping parens)</param>
        /// <returns>the starting position of the operator, 0 if no binary operator was found or error</returns>
        internal static int NextBooleanOperatorToEvaluate(String text)
        {

            // var init
            int retVal = 0;

            // find the operator
            retVal = text.IndexOf(EnumMethods.BooleanOperatorToString(BooleanConditionOperators.LT));
            if (retVal < 0)
            {
                retVal = text.IndexOf(EnumMethods.BooleanOperatorToString(BooleanConditionOperators.GT));
            }
            if (retVal < 0)
            {
                retVal = text.IndexOf(EnumMethods.BooleanOperatorToString(BooleanConditionOperators.Eq));
            }

            return retVal;
        }

        /// <summary>
        /// Unwraps parentheses if neccessary
        /// </summary>
        /// <param name="text">the expression in string form to anaylyze, this string should already be checked for consistency</param>
        /// <returns>the unwrapped string</returns>
        internal static String UnwrapParentheses(String text)
        {

            // var init
            String retVal = text.Trim();

            // if '(' and ')' wrap the expression and are uneccessary then remove them
            if ((text.StartsWith("(") == true) && (text.EndsWith(")") == true))
            {

                // setup for loop
                String unwrappedText = text.Substring(1, text.Length - 2).Trim();
                int parenthesesDepth = 0;

                // loop through each char
                foreach (char c in unwrappedText)
                {

                    // adjust depth
                    if (c == '(')
                    {
                        parenthesesDepth += 1;
                    }
                    else if (c == ')')
                    {
                        parenthesesDepth -= 1;
                    }

                    // if depth is ever negative then we can't unwrap anymore
                    if (parenthesesDepth < 0)
                    {
                        break;
                    }
                }

                // if depth == 0 then recurse on the unwrapped text, otherwise skip and use the original text
                if (parenthesesDepth == 0)
                {
                    retVal = PILOTParser.UnwrapParentheses(unwrappedText);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns how many times a character appears in a string
        /// </summary>
        /// <param name="toLookFor">char to look for</param>
        /// <param name="text">text to search</param>
        /// <returns>the count</returns>
        internal static int CountOf(String toLookFor, string text)
        {

            // var init
            int retVal = 0;
            int lastFound = -1;

            // look for all occurences
            do
            {
                lastFound = text.IndexOf(toLookFor, lastFound + 1);
                if (lastFound >= 0)
                {
                    retVal++;
                }
            }
            while (lastFound >= 0);

            return retVal;
        }

        /// <summary>
        /// Cleans a string that is a numeric/boolean expression into a
        /// parsable format
        /// </summary>
        /// <param name="text">the string the cleanse</param>
        /// <returns>the cleansed string</returns>
        internal static String CleanupNumericExpression(String text)
        {

            // cleanup white space
            text = text.Replace("\r", String.Empty);
            text = text.Replace("\n", String.Empty);
            text = text.Replace(" ", String.Empty);
            text = text.Trim();

            // drop plus for positive integers
            if (String.IsNullOrWhiteSpace(text) == false)
            {
                text = (text[0] == '+') ? text.Substring(1).Trim() : text;
                text = text.Replace("-+", "-");
                text = text.Replace("+-", "-");
                text = text.Replace("++", "+");
                text = text.Replace("--", "+");
                text = text.Replace("\\+", "\\");
                text = text.Replace("/+", "/");
                text = text.Replace("*+", "*");
                text = text.Replace("=+", "=");
                text = text.Replace("(+", "(");
            }

            return text;
        }
    }
}
