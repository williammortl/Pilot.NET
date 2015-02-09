namespace Pilot.NET.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Statements;
    using Pilot.NET.PILOTParser;
    using System;
    
    /// <summary>
    /// Tests for Pilot.NET.Lang
    /// </summary>
    [TestClass]
    public class Parser_Tests
    {

        /// <summary>
        /// Tests the parser's ability to unwrap parentheses
        /// </summary>
        [TestMethod]
        public void TestUnwrapParentheses()
        {
            Assert.AreEqual(Parser.UnwrapParentheses("((((((3 -+ 4) * #var1) + (7))))"), "((((((3 -+ 4) * #var1) + (7))))");
            Assert.AreEqual(Parser.UnwrapParentheses(String.Empty), String.Empty);
            Assert.AreEqual(Parser.UnwrapParentheses("        \r   \n           "), String.Empty);
            Assert.AreEqual(Parser.UnwrapParentheses("(        \r   \n           )"), String.Empty);
            Assert.AreEqual(Parser.UnwrapParentheses("((3 -+ 4) * #var1) + (7)"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(Parser.UnwrapParentheses("(((3 -+ 4) * #var1) + (7))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(Parser.UnwrapParentheses("(((((3 -+ 4) * #var1) + (7))))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(Parser.UnwrapParentheses("((((((3 -+ 4) * #var1) + (7)))          ))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(Parser.UnwrapParentheses("+(7) \\  ((3 -+ 4) * #var1)"), "+(7) \\  ((3 -+ 4) * #var1)");
            Assert.AreEqual(Parser.UnwrapParentheses("(+(7) \\  ((3 -+ 4) * #var1))"), "+(7) \\  ((3 -+ 4) * #var1)");
        }

        /// <summary>
        /// Tests the parser's ability to find the correct operator to use for processing
        /// </summary>
        [TestMethod]
        public void TestBinaryNumericOperationOperatorFinding()
        {

            // strange tests
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("this shouldn't have any operators"), 0);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate(String.Empty), 0);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("        \r   \n           "), 0);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("((7+4)\\(7))"), 0);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("777.7"), 0);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("7"), 0);

            // should find an operator
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("1\\2/3*4+5+6-7"), 11);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("(#var1-7-9\\7+6)*-7"), 15);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("1\\2/3*4+5+-6-7"), 12);
            Assert.AreEqual(Parser.NextBinaryOperatorToEvaluate("1\\2/3*4*+5/-6-7"), 13);
        }

        /// <summary>
        /// Tests by creating a line of PILOT, then ToString, then a line, then ToString, then compare
        /// </summary>
        [TestMethod]
        public void TestLineToStringThenToLineThenToStringThenToLineCompare()
        {

            // test weird scenario
            String PILOT = "20 *newlabel CY: 4 \\ (#var2 + ?)";
            Line l = Parser.ParseLine(PILOT);
            String newPILOT = l.ToString();
            l = Parser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());

            // basic line, text, with a label and match type
            PILOT = "10 *thisismylabel TN: this is test text";
            l = Parser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = Parser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());
 
            // compute, with a label and match type
            PILOT = "20 *newlabel CY:((((7 * #var1) + 7 - 4 \\ (#var2 + ?))))";
            l = Parser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = Parser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());

            // compute, with a label and match type
            PILOT = "20 *newlabel CY(((#var1 \\ 4) >= ((4 - #var5)))):((((7 * #var1) + 7 - 4 \\ (#var2 + ?))))";
            l = Parser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = Parser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());
        }
    }
}
