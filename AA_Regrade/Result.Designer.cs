namespace AA_Regrade
{
    partial class FormResult
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelPreviousGrade = new System.Windows.Forms.Label();
            this.labelNextGrade = new System.Windows.Forms.Label();
            this.labelSteps = new System.Windows.Forms.Label();
            this.labelFailNarrative = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::AA_Regrade.Properties.Resources.result_confirm;
            this.pictureBox1.Location = new System.Drawing.Point(163, 247);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(93, 31);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.Color.Transparent;
            this.labelResult.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResult.ForeColor = System.Drawing.Color.LightCoral;
            this.labelResult.Location = new System.Drawing.Point(77, 32);
            this.labelResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(273, 37);
            this.labelResult.TabIndex = 1;
            this.labelResult.Text = "Failure";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelPreviousGrade
            // 
            this.labelPreviousGrade.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviousGrade.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPreviousGrade.Location = new System.Drawing.Point(69, 75);
            this.labelPreviousGrade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPreviousGrade.Name = "labelPreviousGrade";
            this.labelPreviousGrade.Size = new System.Drawing.Size(100, 20);
            this.labelPreviousGrade.TabIndex = 2;
            this.labelPreviousGrade.Text = "[Arcane]";
            this.labelPreviousGrade.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelNextGrade
            // 
            this.labelNextGrade.BackColor = System.Drawing.Color.Transparent;
            this.labelNextGrade.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNextGrade.Location = new System.Drawing.Point(247, 75);
            this.labelNextGrade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNextGrade.Name = "labelNextGrade";
            this.labelNextGrade.Size = new System.Drawing.Size(115, 20);
            this.labelNextGrade.TabIndex = 4;
            this.labelNextGrade.Text = "[Celestial]";
            // 
            // labelSteps
            // 
            this.labelSteps.BackColor = System.Drawing.Color.Transparent;
            this.labelSteps.Font = new System.Drawing.Font("GothicG", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteps.Location = new System.Drawing.Point(167, 75);
            this.labelSteps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSteps.Name = "labelSteps";
            this.labelSteps.Size = new System.Drawing.Size(85, 20);
            this.labelSteps.TabIndex = 5;
            this.labelSteps.Text = "←←←";
            this.labelSteps.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelFailNarrative
            // 
            this.labelFailNarrative.BackColor = System.Drawing.Color.Transparent;
            this.labelFailNarrative.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFailNarrative.ForeColor = System.Drawing.Color.LightCoral;
            this.labelFailNarrative.Location = new System.Drawing.Point(52, 107);
            this.labelFailNarrative.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFailNarrative.Name = "labelFailNarrative";
            this.labelFailNarrative.Size = new System.Drawing.Size(313, 20);
            this.labelFailNarrative.TabIndex = 6;
            this.labelFailNarrative.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AA_Regrade.Properties.Resources.gear;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Image = global::AA_Regrade.Properties.Resources.arcane;
            this.pictureBox2.Location = new System.Drawing.Point(175, 148);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(69, 64);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // FormResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.BackgroundImage = global::AA_Regrade.Properties.Resources.result;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(419, 302);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelFailNarrative);
            this.Controls.Add(this.labelSteps);
            this.Controls.Add(this.labelNextGrade);
            this.Controls.Add(this.labelPreviousGrade);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelPreviousGrade;
        private System.Windows.Forms.Label labelNextGrade;
        private System.Windows.Forms.Label labelSteps;
        private System.Windows.Forms.Label labelFailNarrative;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}