namespace Pilot.NET
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using Pilot.NET.Lang.Statements;
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
        /// Parses text into a label
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
                    LinkedList<Label> labels = new LinkedList<Label>();
                    if (String.IsNullOrWhiteSpace(parametersForKeyword) == false)
                    {
                        String[] labelsString = parametersForKeyword.Split(new char[] { ',' });
                        foreach (String labelString in labelsString)
                        {
                            Label newLabel = PILOTParser.ParseLabel(labelString);
                            if (newLabel != null)
                            {
                                labels.AddLast(newLabel);
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
                    LinkedList<StringLiteral> conditions = new LinkedList<StringLiteral>();
                    if (String.IsNullOrWhiteSpace(parametersForKeyword) == false)
                    {
                        String[] conditionsString = parametersForKeyword.Split(new char[] { ',' });
                        foreach (String conditionString in conditionsString)
                        {
                            if (String.IsNullOrWhiteSpace(conditionString) == false)
                            {
                                conditions.AddLast(new StringLiteral(conditionString));
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
                    statement = new Text(new StringLiteral(parametersForKeyword), match, ifCondition);
                    break;
                }
                case Keywords.U:
                {
                    statement = new Use(PILOTParser.ParseLabel(parametersForKeyword), match, ifCondition);
                    break;
                }
            }

            return statement;
        }

        /// <summary>
        /// Parse the string into an numeric expression, throws ParserException if there is a problem
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
                else if (text.StartsWith("#") == true)
                {

                    // numeric variable
                    retVal = new NumericVariable(text);
                }
                else
                {

                    // at this point, since it's not a variable, it should be ok to do a replace
                    //  for mathematical constants
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
                throw new ParserException(String.Format("Cannot parse the text as a numeric expression: {0}", text), e);
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
        /// Parses a string to a Boolean condition
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
            int equalPos = text.IndexOf('=');
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
                else if (i > 0)
                {

                    // binary operator cannot exist at position 0
                    // only if we are not parentheses wrapped search for an operator
                    // eval order is by low operator precedence and then left to right
                    if (parenthesesDepth == 0)
                    {
                        if ((c == '-') && (prevOperatorFound > (int)NumericBinaryOperators.Sub) && (EnumMethods.IsNumericBinaryOperator(prevC) == false) && (prevC != '('))
                        {
                            retVal = i;
                            break;
                        }
                        else if ((c == '+') && (prevOperatorFound > (int)NumericBinaryOperators.Add) && (EnumMethods.IsNumericBinaryOperator(prevC) == false) && (prevC != '('))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Add;
                            retVal = i;
                        }
                        else if ((c == '\\') && (prevOperatorFound > (int)NumericBinaryOperators.Mod))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Mod;
                            retVal = i;
                        }
                        else if ((c == '/') && (prevOperatorFound > (int)NumericBinaryOperators.Div))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Div;
                            retVal = i;
                        }
                        else if ((c == '*') && (prevOperatorFound > (int)NumericBinaryOperators.Mult))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Mult;
                            retVal = i;
                        }
                        else if ((c == ',') && (prevOperatorFound > (int)NumericBinaryOperators.Log))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Log;
                            retVal = i;
                        }
                        else if ((c == '^') && (prevOperatorFound > (int)NumericBinaryOperators.Exp))
                        {
                            prevOperatorFound = (int)NumericBinaryOperators.Exp;
                            retVal = i;
                        }
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
            retVal = text.IndexOf("<");
            if (retVal < 0)
            {
                retVal = text.IndexOf(">");
            }
            if (retVal < 0)
            {
                retVal = text.IndexOf("=");
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
            text = (text[0] == '+') ? text.Substring(1).Trim() : text;
            text = text.Replace("-+", "-");
            text = text.Replace("++", "+");
            text = text.Replace("\\+", "\\");
            text = text.Replace("/+", "/");
            text = text.Replace("*+", "*");
            text = text.Replace("=+", "=");
            text = text.Replace("(+", "(");

            return text;
        }
    }
}
