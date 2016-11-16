namespace PILOTi
{
    using Pilot.NET;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Media;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// The default interpreter interface
    /// </summary>
    internal sealed class PILOTiInterface : IPILOTInterpreterInterface
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
        /// The amplitude strength
        /// </summary>
        private const int AMPLITUDE = 1000;

        /// <summary>
        /// the graphics form window
        /// </summary>
        private PILOTiInterfaceGraphicsForm graphicsForm;

        /// <summary>
        /// The sound player for audio
        /// </summary>
        private SoundPlayer soundPlayer;

        /// <summary>
        /// the image to draw the graphics in
        /// </summary>
        public Image GraphicsOutput { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PILOTiInterface()
        {
            this.GraphicsOutput = new Bitmap(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            this.graphicsForm = null;
            this.soundPlayer = new SoundPlayer();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">the width of the window</param>
        /// <param name="height">the height of the window</param>
        public PILOTiInterface(int width, int height)
        {
            this.GraphicsOutput = new Bitmap(width, height);
            this.graphicsForm = null;
        }

        /// <summary>
        /// Refreshes the graphics
        /// </summary>
        public void RedrawGraphics()
        {
            this.CreateGraphicsFormIfNeeded();
            this.graphicsForm.GraphicsImage = this.GraphicsOutput;
            this.graphicsForm.Invoke(new Action(this.graphicsForm.RepaintGraphics));
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
            this.GraphicsOutput = new Bitmap(this.GraphicsOutput.Width, this.GraphicsOutput.Height);
            this.RedrawGraphics();
        }

        /// <summary>
        /// Close the graphics windows
        /// </summary>
        public void CloseGraphicsWindow()
        {

            // clear the graphics image
            this.ClearGraphics();

            // dispose the form if neccessary
            if ((this.graphicsForm != null) && (this.graphicsForm.IsDisposed == false))
            {
                this.graphicsForm.Invoke(new Action(this.graphicsForm.Close));
            }
            this.graphicsForm = null;
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="frequency">frequency of sound</param>
        /// <param name="playMilliseconds">sound duration</param>
        public void PlaySound(double frequency, int playMilliseconds)
        {

            // var init
            double actualAmplitude = ((PILOTiInterface.AMPLITUDE * (System.Math.Pow(2, 15))) / 1000) - 1;
            double deltaFT = 2 * Math.PI * frequency / 44100.0;
            int numSamples = 441 * playMilliseconds / 10;
            int numSamplesWhite = 441 * 20 / 10;
            int numBytes = numSamples * 4;

            // generate sound
            using (MemoryStream ms = new MemoryStream(44 + numBytes))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    WriteWavHeader(ms, false, 1, 16, 44100, 1 * (numSamples + numSamplesWhite));
                    for (int j = 0; j < numSamples; j++)
                    {
                        double t = (double)j / (double)44100;
                        short s = (short)(PILOTiInterface.AMPLITUDE * (Math.Sin(t * frequency * 2.0 * Math.PI)));
                        bw.Write(s);
                    }
                    for (int j = 0; j < numSamplesWhite; j++)
                    {
                        short sampleData = 0;
                        bw.Write(sampleData);
                    }
                    bw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    // actually play the sound
                    this.soundPlayer.Stream = ms;
                    this.soundPlayer.PlaySync();
                }
            }
        }

        private static void WriteWavHeader(MemoryStream stream, bool isFloatingPoint, ushort channelCount, ushort bitDepth, int sampleRate, int totalSampleCount)
        {
            stream.Position = 0;

            // RIFF header.
            // Chunk ID.
            stream.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);

            // Chunk size.
            stream.Write(BitConverter.GetBytes(((bitDepth / 8) * totalSampleCount) + 36), 0, 4);

            // Format.
            stream.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);



            // Sub-chunk 1.
            // Sub-chunk 1 ID.
            stream.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);

            // Sub-chunk 1 size.
            stream.Write(BitConverter.GetBytes(16), 0, 4);

            // Audio format (floating point (3) or PCM (1)). Any other format indicates compression.
            stream.Write(BitConverter.GetBytes((ushort)(isFloatingPoint ? 3 : 1)), 0, 2);

            // Channels.
            stream.Write(BitConverter.GetBytes(channelCount), 0, 2);

            // Sample rate.
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // Bytes rate.
            stream.Write(BitConverter.GetBytes(sampleRate * channelCount * (bitDepth / 8)), 0, 4);

            // Block align.
            stream.Write(BitConverter.GetBytes((ushort)channelCount * (bitDepth / 8)), 0, 2);

            // Bits per sample.
            stream.Write(BitConverter.GetBytes(bitDepth), 0, 2);



            // Sub-chunk 2.
            // Sub-chunk 2 ID.
            stream.Write(Encoding.ASCII.GetBytes("data"), 0, 4);

            // Sub-chunk 2 size.
            stream.Write(BitConverter.GetBytes((bitDepth / 8) * totalSampleCount), 0, 4);
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

            // dispose the sound player if neccessary
            if (this.soundPlayer != null)
            {
                this.soundPlayer.Dispose();
            }
        }

        /// <summary>
        /// Creates the graphics window if needed
        /// </summary>
        private void CreateGraphicsFormIfNeeded()
        {
            if ((this.graphicsForm != null) && (this.graphicsForm.Visible == false))
            {
                this.graphicsForm.Dispose();
                this.graphicsForm = null;
            }
            if (this.graphicsForm == null)
            {
                this.graphicsForm = PILOTiInterfaceGraphicsForm.ShowForm(this.GraphicsOutput);
            }
        }
    }
}
