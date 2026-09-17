namespace fpdungeonCrawler
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbLayer3 = new PictureBox();
            pbLayer2 = new PictureBox();
            pbLayer1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbLayer3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLayer2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLayer1).BeginInit();
            SuspendLayout();
            // 
            // pbLayer3
            // 
            pbLayer3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbLayer3.BackgroundImage = Properties.Resources.Layer3_Hall;
            pbLayer3.BackgroundImageLayout = ImageLayout.Stretch;
            pbLayer3.Location = new Point(-5, -11);
            pbLayer3.Margin = new Padding(2);
            pbLayer3.Name = "pbLayer3";
            pbLayer3.Size = new Size(820, 691);
            pbLayer3.TabIndex = 2;
            pbLayer3.TabStop = false;
            // 
            // pbLayer2
            // 
            pbLayer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbLayer2.BackColor = SystemColors.ActiveBorder;
            pbLayer2.BackgroundImage = Properties.Resources.Layer2_HallPic;
            pbLayer2.BackgroundImageLayout = ImageLayout.Stretch;
            pbLayer2.Location = new Point(164, 102);
            pbLayer2.Margin = new Padding(2);
            pbLayer2.Name = "pbLayer2";
            pbLayer2.Size = new Size(494, 441);
            pbLayer2.TabIndex = 3;
            pbLayer2.TabStop = false;
            // 
            // pbLayer1
            // 
            pbLayer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbLayer1.BackColor = SystemColors.ControlDarkDark;
            pbLayer1.BackgroundImage = Properties.Resources.Layer1_HallPic;
            pbLayer1.BackgroundImageLayout = ImageLayout.Stretch;
            pbLayer1.Enabled = false;
            pbLayer1.Location = new Point(308, 243);
            pbLayer1.Margin = new Padding(2);
            pbLayer1.Name = "pbLayer1";
            pbLayer1.Size = new Size(207, 168);
            pbLayer1.TabIndex = 4;
            pbLayer1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(826, 668);
            ControlBox = false;
            Controls.Add(pbLayer1);
            Controls.Add(pbLayer2);
            Controls.Add(pbLayer3);
            DoubleBuffered = true;
            Enabled = false;
            Margin = new Padding(2);
            Name = "Form1";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pbLayer3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLayer2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLayer1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pbLayer3;
        private PictureBox pbLayer2;
        private PictureBox pbLayer1;
    }
}
