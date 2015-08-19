namespace Pilot.NET.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pilot.NET;
    using Pilot.NET.Lang;
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
            Assert.AreEqual(PILOTParser.UnwrapParentheses("((((((3 -+ 4) * #var1) + (7))))"), "((((((3 -+ 4) * #var1) + (7))))");
            Assert.AreEqual(PILOTParser.UnwrapParentheses(String.Empty), String.Empty);
            Assert.AreEqual(PILOTParser.UnwrapParentheses("        \r   \n           "), String.Empty);
            Assert.AreEqual(PILOTParser.UnwrapParentheses("(        \r   \n           )"), String.Empty);
            Assert.AreEqual(PILOTParser.UnwrapParentheses("((3 -+ 4) * #var1) + (7)"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(PILOTParser.UnwrapParentheses("(((3 -+ 4) * #var1) + (7))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(PILOTParser.UnwrapParentheses("(((((3 -+ 4) * #var1) + (7))))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(PILOTParser.UnwrapParentheses("((((((3 -+ 4) * #var1) + (7)))          ))"), "((3 -+ 4) * #var1) + (7)");
            Assert.AreEqual(PILOTParser.UnwrapParentheses("+(7) \\  ((3 -+ 4) * #var1)"), "+(7) \\  ((3 -+ 4) * #var1)");
            Assert.AreEqual(PILOTParser.UnwrapParentheses("(+(7) \\  ((3 -+ 4) * #var1))"), "+(7) \\  ((3 -+ 4) * #var1)");
        }

        /// <summary>
        /// Tests the parser's ability to find the correct operator to use for processing
        /// </summary>
        [TestMethod]
        public void TestBinaryNumericOperationOperatorFinding()
        {

            // strange tests
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("this shouldn't have any operators"), 0);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate(String.Empty), 0);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("        \r   \n           "), 0);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("((7+4)\\(7))"), 0);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("777.7"), 0);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("7"), 0);

            // should find an operator
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("1\\2/3*4+5+6-7"), 11);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("(#var1-7-9\\7+6)*-7"), 15);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("1\\2/3*4+5+-6-7"), 12);
            Assert.AreEqual(PILOTParser.NextBinaryOperatorToEvaluate("1\\2/3*4*+5/-6-7"), 13);
        }

        /// <summary>
        /// Tests by creating a line of PILOT, then ToString, then a line, then ToString, then compare
        /// </summary>
        [TestMethod]
        public void TestLineToStringThenToLineThenToStringThenToLineCompare()
        {

            // test weird scenario
            String PILOT = "*newlabel CY: 4 \\ (#var2 + ?)";
            Line l = PILOTParser.ParseLine(PILOT);
            String newPILOT = l.ToString();
            l = PILOTParser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());

            // basic line, text, with a label and match type
            PILOT = "*thisismylabel TN: this is test text";
            l = PILOTParser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = PILOTParser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());
 
            // compute, with a label and match type
            PILOT = "*newlabel CY:((((7 * #var1) + 7 - 4 \\ (#var2 + ?))))";
            l = PILOTParser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = PILOTParser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());

            // compute, with a label and match type
            PILOT = "*newlabel CY(((#var1 \\ 4) >= ((4 - #var5)))):((((7 * #var1) + 7 - 4 \\ (#var2 + ?))))";
            l = PILOTParser.ParseLine(PILOT);
            newPILOT = l.ToString();
            l = PILOTParser.ParseLine(newPILOT);
            Assert.AreEqual(newPILOT, l.ToString());
        }
    }
}
