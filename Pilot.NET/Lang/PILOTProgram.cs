﻿namespace Pilot.NET.Lang
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a program written in PILOT
    /// </summary>
    public sealed class PILOTProgram : IEnumerable<int>
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

                // does this line exist?
                if (this.programLines.ContainsKey(lineNumber) == false)
                {
                    this.programLines.Add(lineNumber, null);
                }

                // set the value
                this.programLines[lineNumber] = value;
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
                List<int> retVal = this.programLines.Keys.ToList<int>();
                retVal.Sort();

                return retVal;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        internal PILOTProgram()
        {
            this.programLines = new Dictionary<int, Line>();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">program to dup</param>
        internal PILOTProgram(PILOTProgram toDup)
        {

            // deep copy if not null
            this.programLines = new Dictionary<int, Line>();
            if (toDup != null)
            {
                foreach (int lineNumber in toDup.programLines.Keys)
                {
                    this.programLines.Add(lineNumber, toDup.programLines[lineNumber].Copy());
                }
            }
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
            label = label.Trim().ToLower();
            if (this.LineNumbers.Count > 0)
            {
                foreach (int lineNumber in this.LineNumbers)
                {
                    if (this.programLines[lineNumber].LineLabel.ToString().ToLower() == label)
                    {
                        retVal = lineNumber;
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>the copy</returns>
        public PILOTProgram Copy()
        {
            return new PILOTProgram(this);
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
            if (this.LineNumbers.Count > 0)
            {
                foreach (int lineNumber in this.LineNumbers)
                {
                    Line line = this.programLines[lineNumber];
                    String lineString = (line == null) ? String.Empty : line.ToString();
                    retVal += lineString + "\r\n";
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerator to the line numbers in the program
        /// </summary>
        /// <returns>null if no lines, otherwise an iterator</returns>
        public IEnumerator<int> GetEnumerator()
        {

            // var init
            IEnumerator<int> retVal = null;

            // test to make sure there are lines
            if (this.programLines.Keys.Count > 0)
            {
                retVal = this.LineNumbers.GetEnumerator();
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerator to the line numbers in the program
        /// </summary>
        /// <returns>null if no lines, otherwise an iterator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<int>)this).GetEnumerator();
        }
    }
}
