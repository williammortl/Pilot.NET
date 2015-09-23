namespace PILOTi
{
    partial class Splash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.splashImage = new System.Windows.Forms.PictureBox();
            this.closeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splashImage)).BeginInit();
            this.SuspendLayout();
            // 
            // splashImage
            // 
            this.splashImage.Image = ((System.Drawing.Image)(resources.GetObject("splashImage.Image")));
            this.splashImage.Location = new System.Drawing.Point(0, 0);
            this.splashImage.Name = "splashImage";
            this.splashImage.Size = new System.Drawing.Size(621, 530);
            this.splashImage.TabIndex = 0;
            this.splashImage.TabStop = false;
            // 
            // closeTimer
            // 
            this.closeTimer.Enabled = true;
            this.closeTimer.Interval = 3000;
            this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(618, 527);
            this.Controls.Add(this.splashImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            ((System.ComponentModel.ISupportInitialize)(this.splashImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox splashImage;
        private System.Windows.Forms.Timer closeTimer;

    }
}