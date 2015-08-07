namespace Pilot.NET
{
    using System;
    using System.Drawing;

    /// <summary>
    /// The default interpreter interface
    /// </summary>
    internal sealed class DefaultInterpreterInterface : IPILOTInterpreterInterface
    {

        /// <summary>
        /// the default width for the graphics window
        /// </summary>
        private const int DEFAULT_WIDTH = 1024;

        /// <summary>
        /// the default height for the graphics window
        /// </summary>
        private const int DEFAULT_HEIGHT = 768;

        /// <summary>
        /// the graphics form window
        /// </summary>
        private DefaultInterpreterInterfaceGraphicsForm graphicsForm;

        /// <summary>
        /// the image to draw the graphics in
        /// </summary>
        public Image GraphicsOutput { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultInterpreterInterface()
        {
            this.GraphicsOutput = new Bitmap(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            this.graphicsForm = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">the width of the window</param>
        /// <param name="height">the height of the window</param>
        public DefaultInterpreterInterface(int width, int height)
        {
            this.GraphicsOutput = new Bitmap(width, height);
            this.graphicsForm = null;
        }

        /// <summary>
        /// Refreshes the graphics
        /// </summary>
        public void RedrawGraphics()
        {

            // create the form if it hasn't been created
            if (this.graphicsForm.Visible == false)
            {
                this.graphicsForm.Invoke(new Action(this.graphicsForm.Close));
            }
            this.graphicsForm = DefaultInterpreterInterfaceGraphicsForm.ShowForm(this.GraphicsOutput);

            // refresh the graphics
            this.graphicsForm.Invoke(new Action(this.graphicsForm.Invalidate));
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
        /// Disposes the class
        /// </summary>
        public void Dispose()
        {

            // dispose the form if neccessary
            if ((this.graphicsForm != null) && (this.graphicsForm.IsDisposed == false))
            {
                this.graphicsForm.Invoke(new Action(this.graphicsForm.Close));
            }
            this.graphicsForm = null;

            // dispose the image if neccessary
            if (this.GraphicsOutput != null)
            {
                this.GraphicsOutput.Dispose();
            }
        }
    }
}
