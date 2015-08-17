namespace Pilot.NET.Test
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
            PILOTProgram prog = new PILOTProgram();
            prog[3] = PILOTParser.ParseLine("3 J:*testlabel");
            prog.DeleteLine(0);
            prog[2] = PILOTParser.ParseLine("2 T:GOODBYE");
            prog[1] = PILOTParser.ParseLine("1 *testlabel T:HELLO");
            prog[2] = PILOTParser.ParseLine("2 T:WORLD");
            prog[4] = PILOTParser.ParseLine("4 E:");
            prog.DeleteLine(3);
            int[] programLineNumbers = prog.LineNumbers.ToArray();

            // check the constructions
            Assert.AreEqual(prog["*testlabel"].LineNumber, 1);
            Assert.AreEqual("2 T:WORLD", prog[2].ToString());
            Assert.AreEqual(prog[1].LineLabel.ToString(), "*testlabel");
            Assert.IsTrue(prog[2].LineStatement is Text);
            Assert.AreEqual(((Text)prog[2].LineStatement).TextToDisplay.ToString(), "WORLD");
            Assert.AreEqual(programLineNumbers.Length, 3);
            Assert.AreEqual(programLineNumbers[0], 1);
            Assert.AreEqual(programLineNumbers[1], 2);
            Assert.AreEqual(programLineNumbers[2], 4);
        }

        /// <summary>
        /// Tests DefaultInterpreterInterfaceGraphicsForm
        /// </summary>
        [TestMethod]
        public void TestDefaultInterpreterInterfaceGraphicsForm()
        {
            Image i = new Bitmap(300, 300);
            DefaultInterpreterInterfaceGraphicsForm f = DefaultInterpreterInterfaceGraphicsForm.ShowForm(i);
            Graphics g = Graphics.FromImage(i);
            g.DrawLine(new Pen(Color.Blue, 10), new Point(0, 0), new Point(200, 200));
            g.DrawLine(new Pen(Color.Red, 10), new Point(0, 200), new Point(200, 0));
            f.Invoke(new Action(f.Invalidate));
            Thread.Sleep(3000);
            f.Invoke(new Action(f.Close));
            i.Dispose();
        }
    }
}
