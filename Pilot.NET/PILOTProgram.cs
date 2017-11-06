namespace Pilot.NET.Lang
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a program written in PILOT
    /// </summary>
    public sealed class PILOTProgram
    {

        /// <summary>
        /// The lines of the program, the line number is the key
        /// </summary>
        private Dictionary<int, Line> programLines;

        /// <summary>
        /// Gets or sets the line by the line number 
        /// </summary>
        /// <param name="lineNumber">the line number</param>
        /// <returns>null if the line doesn't exist, otherwise the line</returns>
        public Line this[int lineNumber]
        {
            get
            {
                // var init
                Line retVal = null;

                // does this line exist?
                if (this.programLines.ContainsKey(lineNumber) == true)
                {
                    retVal = this.programLines[lineNumber];
                }

                return retVal;
            }
            set
            {

                // delete or add / replace line
                if (value == null)
                {
                    this.DeleteLine(lineNumber);
                }
                else
                {
                    this.AddLine(lineNumber, value);
                }
            }
        }

        /// <summary>
        /// Get a line by label
        /// </summary>
        /// <param name="label">the label to look for</param>
        /// <returns>the line with the label, otherwise null if the label doesn't exist</returns>
        public Line this[String label]
        {
            get
            {
                // var init
                Line retVal = null;

                // get the index of the label
                label = label.Trim().ToUpper();
                if (this.LineNumbers.Count > 0)
                {
                    foreach (var lineNumber in this.LineNumbers)
                    {
                        if ((this.programLines[lineNumber].LineLabel != null) &&
                            (this.programLines[lineNumber].LineLabel.LabelName.ToUpper() == label))
                        {
                            if (lineNumber > 0)
                            {
                                retVal = this[lineNumber];
                            }
                            break;
                        }
                    }
                }

                return retVal;
            }
        }

        /// <summary>
        /// A sorted list of line numbers for the program
        /// </summary>
        public List<int> LineNumbers
        {
            get
            {
                // get a sorted list of line numbers
                var retVal = new List<int>();
                if ((this.programLines != null) && (this.programLines.Count > 0))
                {
                    retVal = this.programLines.Keys.ToList<int>();
                    retVal.Sort();
                }

                return retVal;
            }
        }

        /// <summary>
        /// The state of the 
        /// </summary>
        public IPILOTState State
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PILOTProgram()
        {
            this.programLines = new Dictionary<int, Line>();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PILOTProgram()
        {
            this.programLines = new Dictionary<int, Line>();
        }

        /// <summary>
        /// Adds (or replaces) a line at the line number
        /// </summary>
        /// <param name="lineNumber">line number</param>
        /// <param name="line">line</param>
        public void AddLine(int lineNumber, Line line)
        {
            // does this line exist? if not, create
            if (this.programLines.ContainsKey(lineNumber) == false)
            {
                this.programLines.Add(lineNumber, null);
            }

            // set the value
            this.programLines[lineNumber] = line;
        }

        /// <summary>
        /// Delete a line by index, if that index doesn't exist, does nothing
        /// </summary>
        /// <param name="lineNumber">the line number to delete</param>
        /// <returns>true if successful</returns>
        public Boolean DeleteLine(int lineNumber)
        {
            // var init
            var retVal = false;

            // make sure that it is a valid line number
            if ((lineNumber >= 0) && (this.programLines.ContainsKey(lineNumber) == true))
            {
                this.programLines.Remove(lineNumber);
            }

            return retVal;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            // var init
            var retVal = String.Empty;

            // convert all lines to a string
            foreach (int lineNumber in this.LineNumbers)
            {
                var line = this[lineNumber];
                var lineString = (line == null) ? String.Empty : line.ToString();
                retVal += String.Format("{0} {1}\r\n", lineNumber.ToString(), lineString);
            }

            return retVal;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <param name="lineStart">what line number to start at</param>
        /// <param name="lineStop">what line number to stop at</param>
        /// <returns>the string representation</returns>
        public string ToString(int lineStart, int lineStop)
        {
            // var init
            var retVal = String.Empty;

            // convert all lines to a string
            foreach (int lineNumber in this.LineNumbers)
            {
                if ((lineNumber >= lineStart) && (lineNumber <= lineStop))
                {
                    Line line = this[lineNumber];
                    String lineString = (line == null) ? String.Empty : line.ToString();
                    retVal += lineString + "\r\n";
                }
            }

            return retVal;
        }

        /// <summary>
        /// Execute the PILOT program
        /// </summary>
        /// <param name="state">interpreter</param>
        public void Execute(IPILOTState state)
        {
            foreach (int lineNumber in this.LineNumbers)
            {
                var line = this[lineNumber];
                line.Execute(state);
            }
        }
    }
}
