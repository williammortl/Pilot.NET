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
    /// General tests for Pilot.NET
    /// </summary>
    [TestClass]
    public class General_Tests
    {

        /// <summary>
        /// Tests the ability to add/delete/edit lines
        /// </summary>
        [TestMethod]
        public void CheckProgramConstruction()
        {

            // setup program
            PILOTProgram prog = new PILOTProgram();
            prog[3] = Parser.ParseLine("3 J:*testlabel");
            prog.DeleteLine(0);
            prog[2] = Parser.ParseLine("2 T:GOODBYE");
            prog[1] = Parser.ParseLine("1 *testlabel T:HELLO");
            prog[2] = Parser.ParseLine("2 T:WORLD");
            prog[4] = Parser.ParseLine("4 E:");
            prog.DeleteLine(3);
            int[] programLineNumbers = prog.LineNumbers.ToArray();

            // check the constructions
            Assert.AreEqual(prog["*testlabel"].LineNumber, 1);
            Assert.AreEqual("2 T:WORLD", prog[2].ToString());
            Assert.AreEqual(prog[1].LineLabel.ToString(), "*testlabel");
            Assert.AreEqual(prog[2].LineStatement.Keyword, Keywords.T);
            Assert.AreEqual(((Text)prog[2].LineStatement).TextToDisplay.ToString(), "WORLD");
            Assert.AreEqual(programLineNumbers.Length, 3);
            Assert.AreEqual(programLineNumbers[0], 1);
            Assert.AreEqual(programLineNumbers[1], 2);
            Assert.AreEqual(programLineNumbers[2], 4);
        }
    }
}
