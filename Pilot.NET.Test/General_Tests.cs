﻿namespace Pilot.NET.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pilot.NET;
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Statements;
    using System;
    using System.Drawing;
    using System.Threading;

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
            String pilotString = "0 R:to be deleted\r\n3 J:*testlabel\r\n2 T:GOODBYE\r\n1 *testlabel T:HELLO\r\n2 T:WORLD\r\n4 E:";
            PILOTProgram prog = PILOTParser.ParseProgram(pilotString);
            prog.DeleteLine(0);
            prog.DeleteLine(3);
            int[] programLineNumbers = prog.LineNumbers.ToArray();

            // check the constructions
            Assert.AreEqual(prog.LabelToLineNumber("*testlabel"), 1);
            Assert.AreEqual("T:WORLD", prog[2].ToString());
            Assert.AreEqual(prog[1].LineLabel.ToString(), "*testlabel");
            Assert.IsTrue(prog[2].LineStatement is Text);
            Assert.AreEqual(((Text)prog[2].LineStatement).TextToDisplay.ToString(), "WORLD");
            Assert.AreEqual(programLineNumbers.Length, 3);
            Assert.AreEqual(programLineNumbers[0], 1);
            Assert.AreEqual(programLineNumbers[1], 2);
            Assert.AreEqual(programLineNumbers[2], 4);
        }
    }
}
