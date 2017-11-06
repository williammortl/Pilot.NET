namespace Pilot.NET.ExternalInterfaces
{
    using Pilot.NET;
    using System;
    using System.Drawing;

    /// <summary>
    /// The default text only interface for PILOT
    /// </summary>
    public sealed class TextOnlyInterface : IPILOTExternalInterface
    {

        /// <summary>
        /// the image to draw the graphics in
        /// </summary>
        public Image GraphicsOutput { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextOnlyInterface()
        {
            this.GraphicsOutput = null;
        }

        /// <summary>
        /// Refreshes the graphics
        /// </summary>
        public void RedrawGraphics()
        {
            // do nothing
        }

        /// <summary>
        /// Called by the PILOT translator to write text to the screen
        /// </summary>
        /// <param name="text">the text to write</param>
        /// <param name="lineBreak">add a line break</param>
        public void WriteText(String text, Boolean lineBreak)
        {
            if (lineBreak == true)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }

        /// <summary>
        /// Reads a line of text from the console
        /// </summary>
        /// <returns></returns>
        public string ReadTextLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Clears the text screen
        /// </summary>
        public void ClearText()
        {
            Console.Clear();
        }

        /// <summary>
        /// Clears the graphics
        /// </summary>
        public void ClearGraphics()
        {
            // do nothing
        }

        /// <summary>
        /// Close the graphics windows
        /// </summary>
        public void CloseGraphicsWindow()
        {
            // do nothing
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="frequency">frequency of sound</param>
        /// <param name="playMilliseconds">sound duration</param>
        public void PlaySound(double frequency, int playMilliseconds)
        {
            // do nothing
        }

        /// <summary>
        /// Disposes the class
        /// </summary>
        public void Dispose()
        {
            // do nothing
        }
    }
}
