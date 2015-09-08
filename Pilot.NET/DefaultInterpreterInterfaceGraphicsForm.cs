namespace Pilot.NET
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
    internal partial class DefaultInterpreterInterfaceGraphicsForm : Form
    {

        /// <summary>
        /// The title for the graphics window
        /// </summary>
        private const String TITLE = "Pilot.NET - Graphics";

        /// <summary>
        /// the image containing the graphics, not responsible for the disposal of this
        /// </summary>
        public Image GraphicsImage;

        /// <summary>
        /// Private constructor, use ShowForm method instead
        /// </summary>
        /// <param name="graphicsImage">the image to draw</param>
        private DefaultInterpreterInterfaceGraphicsForm(Image graphicsImage)
        {
            this.GraphicsImage = graphicsImage;
            this.Text = DefaultInterpreterInterfaceGraphicsForm.TITLE;
            InitializeComponent();
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
            e.Graphics.DrawImage(this.GraphicsImage, new Point(0, 0));   
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
        /// Creates and shows a DefaultInterpreterInterfaceGraphicsForm form
        /// </summary>
        /// <param name="graphicsImage">the image to be drawn</param>
        /// <returns>a DefaultInterpreterInterfaceGraphicsForm variable</returns>
        public static DefaultInterpreterInterfaceGraphicsForm ShowForm(Image graphicsImage)
        {
            DefaultInterpreterInterfaceGraphicsForm frm = new DefaultInterpreterInterfaceGraphicsForm(graphicsImage);
            Thread t = new Thread(new ParameterizedThreadStart(DefaultInterpreterInterfaceGraphicsForm.MessagePump));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(frm);
            while (frm.IsHandleCreated == false)
            {
                Thread.Sleep(0);
            }
            return frm;
        }

        /// <summary>
        /// Message pump method for the form
        /// </summary>
        /// <param name="obj">the DefaultInterpreterInterfaceGraphicsForm that this a pump for</param>
        private static void MessagePump(Object obj)
        {
            DefaultInterpreterInterfaceGraphicsForm frm = (DefaultInterpreterInterfaceGraphicsForm)obj;
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
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Bitmap Files (.bmp)|*.bmp|PNG Files (*.png)|*.png";
                sfd.FilterIndex = 0;
                sfd.InitialDirectory = Environment.CurrentDirectory;
                sfd.FileName = "PilotImage.bmp";
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
