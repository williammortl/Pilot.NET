namespace iPilot
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    
    /// <summary>
    /// Window to display PILOT graphics
    /// </summary>
    internal partial class PilotInterfaceGraphicsForm : Form
    {

        /// <summary>
        /// the image containing the graphics, not responsible for the disposal of this
        /// </summary>
        public Image GraphicsImage;

        /// <summary>
        /// Private constructor, use ShowForm method instead
        /// </summary>
        /// <param name="graphicsImage">the image to draw</param>
        private PilotInterfaceGraphicsForm(Image graphicsImage)
        {
            this.GraphicsImage = graphicsImage;
            InitializeComponent();
        }

        /// <summary>
        /// Creates and shows a DefaultInterpreterInterfaceGraphicsForm form
        /// </summary>
        /// <param name="graphicsImage">the image to be drawn</param>
        /// <returns>a DefaultInterpreterInterfaceGraphicsForm variable</returns>
        public static PilotInterfaceGraphicsForm ShowForm(Image graphicsImage)
        {
            PilotInterfaceGraphicsForm frm = new PilotInterfaceGraphicsForm(graphicsImage);
            Thread t = new Thread(new ParameterizedThreadStart(PilotInterfaceGraphicsForm.MessagePump));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(frm);
            while (frm.IsHandleCreated == false)
            {
                Thread.Sleep(0);
            }
            return frm;
        }

        /// <summary>
        /// Repaint the graphics window
        /// </summary>
        public void RepaintGraphics()
        {
            this.graphicsBox.Invalidate();
        }

        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void DefaultInterpreterInterfaceGraphicsForm_Load(object sender, EventArgs e)
        {

            // form init
            this.Height = this.GraphicsImage.Height;
            this.Width = this.GraphicsImage.Width;
            this.graphicsBox.Height = this.GraphicsImage.Height;
            this.graphicsBox.Width = this.GraphicsImage.Width;
        }

        /// <summary>
        /// Picture box repaint event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void graphicsBox_Paint(object sender, PaintEventArgs e)
        {

            // mutex the image to make sure we don't have a race condition with a draw event
            lock (this.GraphicsImage)
            {
                e.Graphics.DrawImage(this.GraphicsImage, new Point(0, 0));
            } 
        }

        /// <summary>
        /// Form repaint event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void DefaultInterpreterInterfaceGraphicsForm_Paint(object sender, PaintEventArgs e)
        {
            this.graphicsBox.Invalidate();
        }

        /// <summary>
        /// Message pump method for the form
        /// </summary>
        /// <param name="obj">the DefaultInterpreterInterfaceGraphicsForm that this a pump for</param>
        private static void MessagePump(Object obj)
        {
            PilotInterfaceGraphicsForm frm = (PilotInterfaceGraphicsForm)obj;
            Application.Run(frm);
            frm.Dispose();
        }

        /// <summary>
        /// Saves the current image to a file
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void saveImage_Click(object sender, EventArgs e)
        {
            lock (this.GraphicsImage)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Bitmap Files (.bmp)|*.bmp|PNG Files (*.png)|*.png";
                    sfd.FilterIndex = 0;
                    sfd.InitialDirectory = Environment.CurrentDirectory;
                    sfd.FileName = "iPilotmage.bmp";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        String fileName = sfd.FileName;
                        ImageFormat format = (Path.GetExtension(fileName).Trim().Substring(1).ToLower().Contains("bmp") == true) ? ImageFormat.Bmp : ImageFormat.Png;
                        this.GraphicsImage.Save(fileName, format);
                    }
                }
            }
        }
    }
}
