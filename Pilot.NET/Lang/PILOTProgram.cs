﻿namespace Pilot.NET.Lang
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

                    // does this line exist? if not, create
                    if (this.programLines.ContainsKey(lineNumber) == false)
                    {
                        this.programLines.Add(lineNumber, null);
                    }

                    // set the value
                    this.programLines[lineNumber] = value;
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

                // does this label exist?
                int lineNumber = this.LabelToLineNumber(label);
                if (lineNumber > 0)
                {
                    retVal = this[lineNumber];
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
                List<int> retVal = new List<int>();
                if ((this.programLines != null) && (this.programLines.Count > 0))
                {
                    retVal = this.programLines.Keys.ToList<int>();
                    retVal.Sort();
                }

                return retVal;
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
        /// Gets the ordinal of a line number in LineNumbers
        /// </summary>
        /// <param name="lineNumberToFind">the line number to look for</param>
        /// <returns>-1 if not found, otherwise the ordinal of the line number in LineNumbers</returns>
        public int OrdinalOfLineNumber(int lineNumberToFind)
        {

            // var init
            int retVal = -1;
            List<int> listOfLineNumbers = this.LineNumbers;

            // look for the line number
            for (int i = 0; i < listOfLineNumbers.Count; i++)
            {
                if (listOfLineNumbers[i] == lineNumberToFind)
                {
                    retVal = i;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the ordinal of the line number for the label
        /// </summary>
        /// <param name="label">the label to get the index of</param>
        /// <returns>-1 if label doesn't exist, else the ordinal of the line number</returns>
        public int OrdinalOfLabel(String label)
        {

            // var init
            int retVal = -1;
            List<int> listOfLineNumbers = this.LineNumbers;

            // look for the line number
            label = label.Trim().ToUpper();
            for (int i = 0; i < listOfLineNumbers.Count; i++)
            {
                int lineNumber = listOfLineNumbers[i];
                if ((this[lineNumber] != null) &&
                    (this[lineNumber].LineLabel != null) &&
                    (this[lineNumber].LineLabel.LabelName.ToUpper() == label))
                {
                    retVal = i;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Delete a line by index, if that index doesn't exist, does nothing
        /// </summary>
        /// <param name="lineNumber">the line number to delete</param>
        /// <returns>true if successful</returns>
        public Boolean DeleteLine(int lineNumber)
        {
            
            // var init
            Boolean retVal = false;

            // make sure that it is a valid line number
            if ((lineNumber >= 0) && (this.programLines.ContainsKey(lineNumber) == true))
            {
                this.programLines.Remove(lineNumber);
            }

            return retVal;
        }

        /// <summary>
        /// Clears all PILOT lines in the program
        /// </summary>
        public void Clear()
        {
            this.programLines = new Dictionary<int, Line>();
        }

        /// <summary>
        /// Gets the line number for the label
        /// </summary>
        /// <param name="label">the label to get the index of</param>
        /// <returns>-1 if label doesn't exist, else line number</returns>
        public int LabelToLineNumber(String label)
        {

            // var init
            int retVal = -1;

            // get the index of the label
            label = label.Trim().ToUpper();
            if (this.LineNumbers.Count > 0)
            {
                foreach (int lineNumber in this.LineNumbers)
                {
                    if ((this.programLines[lineNumber].LineLabel != null) && 
                        (this.programLines[lineNumber].LineLabel.LabelName.ToUpper() == label))
                    {
                        retVal = lineNumber;
                        break;
                    }
                }
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
            String retVal = String.Empty;

            // convert all lines to a string
            foreach (int lineNumber in this.LineNumbers)
            {
                Line line = this[lineNumber];
                String lineString = (line == null) ? String.Empty : line.ToString();
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
            String retVal = String.Empty;

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
    }
}
